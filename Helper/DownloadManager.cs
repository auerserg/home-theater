using System.Collections.Generic;
using HomeTheater.API.Serial;

namespace HomeTheater.Helper
{
    internal class DownloadManager
    {
        private static DownloadManager _i;

        private readonly Dictionary<string, DonwoloadWorker> Workers =
            new Dictionary<string, DonwoloadWorker>();

        public bool isFirstRun { get; set; }

        public static DownloadManager Instance
        {
            get
            {
                Load();
                return _i;
            }
        }

        public static void Load()
        {
            if (_i == null)
                _i = new DownloadManager();
        }

        private List<string> getNotActual(string MarkCurrent, List<string> OrderVideos)
        {
            var result = new List<string>();
            foreach (var item in OrderVideos)
            {
                result.Add(item);
                if (item == MarkCurrent)
                    break;
            }

            return result;
        }

        public void Add(Season season)
        {
            switch (season.Type)
            {
                case "want":
                    foreach (var itemVideo in season.Videos)
                        AddVideo(itemVideo.Value);
                    break;
                case "new":
                    var videoIds = getNotActual(season.MarkCurrent, season.OrderVideos);
                    foreach (var itemVideo in season.Videos)
                        AddVideo(itemVideo.Value, videoIds.Contains(itemVideo.Key));
                    break;
                case "nonew":
                case "watched":
                    foreach (var itemVideo in season.Videos)
                        AddVideo(itemVideo.Value, isFirstRun);
                    break;
            }
        }

        public void Add(Video video, bool isDownloaded = false)
        {
            var id = video.ID.ToString();
            AddVideo(id, video, isDownloaded);
        }

        private void AddVideo(Video video, bool isDownloaded = false)
        {
            var id = string.Format("{0}_{1}", video.SeasonID, video.VideoID);
            AddVideo(id, video, isDownloaded);
        }

        private void AddVideo(string id, Video video, bool isDownloaded = false)
        {
            if (Workers.ContainsKey(id))
                Workers[id].Add(video);
            else
                Workers[id] = new DonwoloadWorker(video, isDownloaded);
        }
    }
}