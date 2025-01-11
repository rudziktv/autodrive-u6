using System;
using System.Collections.Generic;
using System.IO;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Systems.Managers
{
    public class FMODBankManager : MonoBehaviour
    {
        public static readonly string BanksPath = Path.Combine(Application.streamingAssetsPath, "Banks");
        
        private readonly Dictionary<string, Bank> _loadedBanks = new();
        
        public static FMODBankManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            try
            {
                if (!Directory.Exists(BanksPath))
                    Directory.CreateDirectory(BanksPath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private Bank GetBank(string bankName)
        {
            if (!_loadedBanks.TryGetValue(bankName, out var bank))
                throw new Exception($"Bank {bankName} not loaded!");
            return bank;
        }

        public Bank LoadBank(string bankName)
        {
            var path = Path.Combine(BanksPath, bankName);
            if (_loadedBanks.ContainsKey(bankName))
                return GetBank(bankName);

            // MAYBE TODO
            // if (RuntimeManager.HasBankLoaded(bankName))

            var result = RuntimeManager.StudioSystem.loadBankFile(path, LOAD_BANK_FLAGS.NORMAL, out var bank);
            if (result == RESULT.OK)
            {
                _loadedBanks.Add(bankName, bank);
                return bank;
            }
            throw new Exception($"Failed to load bank {bankName}. Error: {result}");
        }

        private void UnloadBank(string bankName)
        {
            _loadedBanks.Remove(bankName, out var bank);
            
            // RuntimeManager.UnloadBank(bankName);
        }
    }
}