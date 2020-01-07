using System.Collections.Specialized;

namespace HomeTheater.Serial
{
    class APIServerMarks
    {
        static APIServerMarks _i;
        public static APIServerMarks Instance
        {
            get
            {
                if (_i == null)
                {
                    Load();
                }
                return _i;
            }
        }

        public static void Load()
        {
            _i = new APIServerMarks();
        }

        private string doMarks(NameValueCollection postData = null)
        {
            return APIServer.Instance.doMarks(postData);
        }

        private bool setMarks(NameValueCollection postData = null)
        {
            string result = doMarks(postData);
            dynamic resultjson = SimpleJson.SimpleJson.DeserializeObject<dynamic>(result);
            string msg = null;
            bool auth = false;
            foreach (var id in resultjson)
            {
                switch (id.Key)
                {
                    case "msg":
                        msg = id.Value.ToString();
                        break;
                    case "auth":
                        auth = id.Value;
                        break;
                }
            }
            return auth && "success" == msg;
        }

        public bool setWantToSee(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", "-1" }, { "wanttosee", "true" }, { "minute", "0" }, { "second", "0" } });
        }

        public bool setDelSee(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "delId", ID.ToString() } });
        }

        public bool setWatched(int ID)
        {
            if (0 >= ID)
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", "-2" }, { "watched", "true" }, { "minute", "0" }, { "second", "0" } });
        }

        public bool setPauseAdd(int ID, string Series = "")
        {
            if (0 >= ID || string.IsNullOrWhiteSpace(Series))
                return false;
            return this.setMarks(new NameValueCollection { { "id", ID.ToString() }, { "seria", Series }, { "pauseadd", "true" }, { "minute", "0" }, { "second", "0" }, { "tran", "0" } });
        }
    }
}
