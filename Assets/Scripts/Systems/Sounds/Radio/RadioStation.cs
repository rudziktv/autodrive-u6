using System;

namespace Systems.Sounds.Radio
{
    [Serializable]
    public class RadioStation
    {
        public string name;
        public string url;

        public RadioStation() { }

        public RadioStation(string name, string url)
        {
            this.name = name;
            this.url = url;
        }
    }
}