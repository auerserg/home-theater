using System.Collections.Generic;
using HomeTheater.Helper;

namespace HomeTheater.API.Response
{
    internal class Video
    {
#pragma warning disable 0649
        public string file;
        public string galabel;
        public string id;
        public string subtitle;
        public string title;
        public string vars;
        public List<Video> folder;
#pragma warning restore 0649

        public string fileReal
        {
            get
            {
                if (!string.IsNullOrEmpty(file))
                    return Decoder.File(file);
                return "";
            }
        }
    }
}