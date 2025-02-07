using System;
using Systems.Gamemodes.Base;
using Systems.Gamemodes.PersonMode;
using Systems.Sounds.Radio;
using UnityEngine;

namespace Systems.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static IGamemode CurrentGamemode { get; private set; }
        public static event Action<IGamemode> GamemodeChanged;
        
        public static GameManager Instance { get; private set; }
        
        // [SerializeField] private GameObject playerPrefab;
        // [SerializeField] private Transform spawnPoint;

        private void Awake()
        {
            Instance = this;
            InitializeData();

            CurrentGamemode = new PersonGamemode();
        }

        private void InitializeData()
        {
            RadioStationsManager.Initialize();
        }

        private void Start()
        {
            CurrentGamemode.EnterMode();
        }

        public static void ChangeGamemode(IGamemode gamemode)
        {
            CurrentGamemode.ExitMode();
            CurrentGamemode = gamemode;
            CurrentGamemode.EnterMode();
            GamemodeChanged?.Invoke(gamemode);
        }
    }
}