using System.Collections.Generic;
using HomeTheater.Helper;

namespace HomeTheater.Serial.data
{
    internal class video
    {
        public string file, galabel, id, subtitle, title, vars;
        public List<video> folder;

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