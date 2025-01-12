using System;

namespace Systems.Sounds.Radio
{
    [Serializable]
    public class RadioStation
    {
        public string name;
        public string url;
        public string details;

        public RadioStation() { }

        public RadioStation(string name, string url, string details = "")
        {
            this.name = name;
            this.url = url;
            this.details = details;
        }
    }
}