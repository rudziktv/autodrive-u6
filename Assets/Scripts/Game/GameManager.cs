using System;
using System.Collections;
using Development.MediaControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private UIDocument mediaControlUI;
        
        private LinuxMediaControl _mediaControl;
        private Coroutine _mediaControlCoroutine;
        
        private VisualElement _visualElement;

        private void Start()
        {
            if (playerPrefab != null)
                Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            // _mediaControl = new LinuxMediaControl();
            // _mediaControl.Initialize();
            //
            // mediaControlUI.rootVisualElement.Q<Button>("play-pause").clicked += OnPlayPauseButton;
            // _visualElement = mediaControlUI.rootVisualElement.Q<VisualElement>("thumbnail");
            // _mediaControlCoroutine = StartCoroutine(MediaControlCoroutine());
        }

        private void OnPlayPauseButton()
        {
            Debug.Log("OnPlayPauseButton");
            _mediaControl.PlayPause();
        }

        private IEnumerator MediaControlCoroutine()
        {
            while (true)
            {
                _mediaControl.GetCurrentTrackInfo(_visualElement);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}