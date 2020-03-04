using System.Collections.Generic;
using HomeTheater.API.Serial;

namespace HomeTheater.Helper
{
    internal class DonwoloadWorker
    {
        public DonwoloadWorkerStatus Status;
        private Video Video;

        public Dictionary<int, Video> Videos = new Dictionary<int, Video>();

        public DonwoloadWorker(Video video, bool isDownloaded = false)
        {
            Add(video);
        }

        public void Add(Video video)
        {
            if (null == Video)
            {
                Video = video;
            }
            else
            {
                //UNDONE Устанавливать видео или обновлять элементы
            }
        }
    }

    public enum DonwoloadWorkerStatus
    {
        Queued,
        Running,
        Paused,
        Completed,
        Downloaded,
        Canceled,
        DownloadError
    }
}