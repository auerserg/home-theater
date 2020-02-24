namespace HomeTheater.API.Response
{
    internal class updateCheck
    {
        public bool NewPage, NewPlayer, NewPlaylist;
        public bool NonewPage, NonewPlayer, NonewPlaylist;
        public bool WantPage, WantPlayer, WantPlaylist;
        public bool WatchedPage, WatchedPlayer, WatchedPlaylist;

        public updateCheck(string str = "")
        {
            var data = str.Split(',');
            for (var i = 0; i < data.Length; i++)
                this[i] = data[i];
        }


        public bool Page => NewPage || NonewPage || WantPage || WatchedPage;
        public bool Player => NewPlayer || NonewPlayer || WantPlayer || WatchedPlayer;
        public bool Playlist => NewPlaylist || NonewPlaylist || WantPlaylist || WatchedPlaylist;

        public bool New => NewPage && NewPlayer && NewPlaylist;
        public bool Nonew => NonewPage && NonewPlayer && NonewPlaylist;
        public bool Want => WantPage && WantPlayer && WantPlaylist;
        public bool Watched => WatchedPage && WatchedPlayer && WatchedPlaylist;

        public bool this[string index, string index2]
        {
            get
            {
                switch (index)
                {
                    case "new":
                        switch (index2)
                        {
                            case "page": return NewPage;
                            case "player": return NewPlayer;
                            case "playlist": return NewPlaylist;
                        }

                        break;
                    case "nonew":
                        switch (index2)
                        {
                            case "page": return NonewPage;
                            case "player": return NonewPlayer;
                            case "playlist": return NonewPlaylist;
                        }

                        break;
                    case "want":
                        switch (index2)
                        {
                            case "page": return WantPage;
                            case "player": return WantPlayer;
                            case "playlist": return WantPlaylist;
                        }

                        break;
                    case "watched":
                        switch (index2)
                        {
                            case "page": return WatchedPage;
                            case "player": return WatchedPlayer;
                            case "playlist": return WatchedPlaylist;
                        }

                        break;
                }

                return false;
            }
            set
            {
                switch (index)
                {
                    case "new":
                        switch (index2)
                        {
                            case "page":
                                NewPage = value;
                                break;
                            case "player":
                                NewPlayer = value;
                                break;
                            case "playlist":
                                NewPlaylist = value;
                                break;
                        }

                        break;
                    case "nonew":
                        switch (index2)
                        {
                            case "page":
                                NonewPage = value;
                                break;
                            case "player":
                                NonewPlayer = value;
                                break;
                            case "playlist":
                                NonewPlaylist = value;
                                break;
                        }

                        break;
                    case "want":
                        switch (index2)
                        {
                            case "page":
                                WantPage = value;
                                break;
                            case "player":
                                WantPlayer = value;
                                break;
                            case "playlist":
                                WantPlaylist = value;
                                break;
                        }

                        break;
                    case "watched":
                        switch (index2)
                        {
                            case "page":
                                WatchedPage = value;
                                break;
                            case "player":
                                WatchedPlayer = value;
                                break;
                            case "playlist":
                                WatchedPlaylist = value;
                                break;
                        }

                        break;
                }
            }
        }

        public bool this[int index, int index2]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        switch (index2)
                        {
                            case 0: return NewPage;
                            case 1: return NewPlayer;
                            case 2: return NewPlaylist;
                        }

                        break;
                    case 1:
                        switch (index2)
                        {
                            case 0: return WantPage;
                            case 1: return WantPlayer;
                            case 2: return WantPlaylist;
                        }

                        break;
                    case 2:
                        switch (index2)
                        {
                            case 0: return NonewPage;
                            case 1: return NonewPlayer;
                            case 2: return NonewPlaylist;
                        }

                        break;
                    case 3:
                        switch (index2)
                        {
                            case 0: return WatchedPage;
                            case 1: return WatchedPlayer;
                            case 2: return WatchedPlaylist;
                        }

                        break;
                }

                return false;
            }
            set
            {
                switch (index)
                {
                    case 0:
                        switch (index2)
                        {
                            case 0:
                                NewPage = value;
                                break;
                            case 1:
                                NewPlayer = value;
                                break;
                            case 2:
                                NewPlaylist = value;
                                break;
                        }

                        break;
                    case 1:
                        switch (index2)
                        {
                            case 0:
                                WantPage = value;
                                break;
                            case 1:
                                WantPlayer = value;
                                break;
                            case 2:
                                WantPlaylist = value;
                                break;
                        }

                        break;
                        ;
                    case 2:
                        switch (index2)
                        {
                            case 0:
                                NonewPage = value;
                                break;
                            case 1:
                                NonewPlayer = value;
                                break;
                            case 2:
                                NonewPlaylist = value;
                                break;
                        }

                        break;
                    case 3:
                        switch (index2)
                        {
                            case 0:
                                WatchedPage = value;
                                break;
                            case 1:
                                WatchedPlayer = value;
                                break;
                            case 2:
                                WatchedPlaylist = value;
                                break;
                        }

                        break;
                }
            }
        }

        public bool this[string index]
        {
            get
            {
                switch (index)
                {
                    case "new":
                        return NewPage || NewPlayer || NewPlaylist;
                    case "nonew":
                        return NonewPage || NonewPlayer || NonewPlaylist;
                    case "want":
                        return WantPage || WantPlayer || WantPlaylist;
                    case "watched":
                        return WatchedPage || WatchedPlayer || WatchedPlaylist;
                }

                return false;
            }
        }

        private string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return NewPage ? "1" : "0";
                    case 1:
                        return NewPlayer ? "1" : "0";
                    case 2:
                        return NewPlaylist ? "1" : "0";
                    case 3: return NonewPage ? "1" : "0";
                    case 4: return NonewPlayer ? "1" : "0";
                    case 5: return NonewPlaylist ? "1" : "0";
                    case 6: return WantPage ? "1" : "0";
                    case 7: return WantPlayer ? "1" : "0";
                    case 8: return WantPlaylist ? "1" : "0";
                    case 9: return WatchedPage ? "1" : "0";
                    case 10: return WatchedPlayer ? "1" : "0";
                    case 11: return WatchedPlaylist ? "1" : "0";
                }

                return "0";
            }
            set
            {
                switch (index)
                {
                    case 0:
                        NewPage = "1" == value;
                        break;
                    case 1:
                        NewPlayer = "1" == value;
                        break;
                    case 2:
                        NewPlaylist = "1" == value;
                        break;
                    case 3:
                        NonewPage = "1" == value;
                        break;
                    case 4:
                        NonewPlayer = "1" == value;
                        break;
                    case 5:
                        NonewPlaylist = "1" == value;
                        break;
                    case 6:
                        WantPage = "1" == value;
                        break;
                    case 7:
                        WantPlayer = "1" == value;
                        break;
                    case 8:
                        WantPlaylist = "1" == value;
                        break;
                    case 9:
                        WatchedPage = "1" == value;
                        break;
                    case 10:
                        WatchedPlayer = "1" == value;
                        break;
                    case 11:
                        WatchedPlaylist = "1" == value;
                        break;
                }
            }
        }

        public override string ToString()
        {
            var data = new string[12];
            for (var i = 0; i < data.Length; i++)
                data[i] = this[i];

            return string.Join(",", data);
        }
    }
}