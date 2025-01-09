using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tmds.DBus;
using UnityEngine;
using UnityEngine.UIElements;

namespace Plugins.MediaControl
{
    [DBusInterface("org.mpris.MediaPlayer2.Player")]
    interface IMediaPlayer : IDBusObject
    {
        // Task<IDictionary<string, object>> MetadataAsync { get; }
        Task<object> GetAsync(string propertyName);

        
        Task PlayPauseAsync();
        Task NextAsync();
        Task PreviousAsync();
    }
    
    [DBusInterface("org.freedesktop.DBus.Properties")]
    interface IProperties : IDBusObject
    {
        Task<object> GetAsync(string propertyName);
        Task<IDictionary<string, object>> GetAllAsync();
        Task SetAsync(string propertyName, object value);
    }
    
    public class LinuxMediaControl
    {
        private Connection _connection;
        private IMediaPlayer _player;
        private IProperties _properties;

        public async void Initialize()
        {
            _connection = new(Address.Session);
            await _connection.ConnectAsync();
            
            _player = _connection.CreateProxy<IMediaPlayer>("org.mpris.MediaPlayer2.spotify", "/org/mpris/MediaPlayer2");
            // _properties =
            //     _connection.CreateProxy<IProperties>("org.mpris.MediaPlayer2.spotify", "/org/mpris/MediaPlayer2");
        }

        public async void PlayPause()
        {
            Debug.Log("Play/Pause");
            try
            {
                await _player.PlayPauseAsync();
                Debug.Log("Toggled Play/Pause");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private string _oldUrl;

        public async Task GetCurrentTrackInfo(VisualElement image)
        {
            if (_player == null)
                return;
            
            try
            {
                // var m = await _properties.GetAsync("Metadata");
                var m = await _player.GetAsync("Metadata");
                var metadata = (IDictionary<string, object>)m;

                var position = (long)(await _player.GetAsync("Position"));

                // Pobieranie kluczowych informacji z metadanych
                string title = metadata.TryGetValue("xesam:title", out var value) ? value.ToString() : "Unknown Title";
                // string artist = metadata.ContainsKey("xesam:artist") ? string.Join(", ", (string[])) : "Unknown Artist";
                string artist = "Unknown Artist";

                if (metadata.TryGetValue("mpris:artUrl", out var artUrl))
                {
                    var artLink = artUrl.ToString();
                    
                    if (_oldUrl == artLink) return;

                    SetTexture(image, artLink);
                }
                
                if (metadata.TryGetValue("xesam:artist", out var artists))
                {
                    var casted = (string[])artists;
                    artist = string.Join(", ", casted);
                }
                
                string album = metadata.ContainsKey("xesam:album") ? metadata["xesam:album"].ToString() : "Unknown Album";
                var duration = metadata.ContainsKey("mpris:length") ? (ulong)metadata["mpris:length"] : 0;  // Duration in seconds
                
                Debug.Log("Current Track Info: " + title + ", " + artist + ", " + album);
                Debug.Log($"Player Info: {position} / {duration}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private async void SetTexture(VisualElement div, string url)
        {
            try
            {
                div.style.backgroundImage = await DownloadImage(url);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            // div.style.backgroundImage 
        }

        private async Task<Texture2D> DownloadImage(string url)
        {
            using var client = new HttpClient();
            byte[] bytes = await client.GetByteArrayAsync(url);
                
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
                
            return texture;
        }
    }
}