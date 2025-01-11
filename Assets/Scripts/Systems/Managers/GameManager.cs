using System;
using Systems.Sounds.Radio;
using UnityEngine;

namespace Systems.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        // [SerializeField] private GameObject playerPrefab;
        // [SerializeField] private Transform spawnPoint;

        private void Awake()
        {
            Instance = this;
            InitializeData();
        }

        private void InitializeData()
        {
            RadioStationsManager.Initialize();
        }

        private void Start()
        {
            // if (playerPrefab != null)
            //     Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}