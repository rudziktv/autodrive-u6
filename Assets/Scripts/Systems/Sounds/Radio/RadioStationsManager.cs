using System.IO;
using System.Linq;
using Systems.AppStorage;
using UnityEngine;

namespace Systems.Sounds.Radio
{
    public static class RadioStationsManager
    {
        public readonly static string RadioStationsPath = Path.Combine(Storage.AppDataPath, "user", "radio_stations.json");
        public readonly static RadioStation[] DefaultStations =
        {
            new("ESKA", "https://waw.ic.smcdn.pl/2330-1.mp3"),
            new("Radio ZET", "https://playerservices.streamtheworld.com/api/livestream-redirect/RADIO_ZET.mp3")
        };

        public static RadioStation[] userStations;

        public static void Initialize()
        {
            userStations = Storage.LoadData(RadioStationsPath, DefaultStations);
            Storage.SaveData(RadioStationsPath, userStations);
        }

        public static void AddStation(RadioStation station, int index = -1)
        {
            var list = userStations.ToList();

            if (index < 0)
                list.Add(station);
            else
                list.Insert(index, station);
            
            userStations = list.ToArray();
            Storage.SaveData(RadioStationsPath, userStations);
        }

        public static void RemoveStation(RadioStation station)
        {
            var list = userStations.ToList();
            list.Remove(station);
            userStations = list.ToArray();
            Storage.SaveData(RadioStationsPath, userStations);
        }
    }
}