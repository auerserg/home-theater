using System;
using System.Windows.Forms;

namespace HomeTheater.Helper
{
    public class CountDownTimer : IDisposable
    {
        private readonly DateTime _minTime = new DateTime(1, 1, 1, 0, 0, 0);

        private readonly Timer timer = new Timer();
        private DateTime _maxTime = new DateTime(1, 1, 1, 0, 30, 0);
        public Action CountDownFinished;
        public Action TimeChanged;

        public CountDownTimer(int min, int sec)
        {
            SetTime(min, sec);
            Init();
        }

        public CountDownTimer(DateTime dt)
        {
            SetTime(dt);
            Init();
        }

        public CountDownTimer()
        {
            Init();
        }

        public bool IsRunnign => timer.Enabled;

        public int StepMs
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }

        public DateTime TimeLeft { get; private set; }
        private long TimeLeftMs => TimeLeft.Ticks / TimeSpan.TicksPerMillisecond;

        public string TimeLeftStr => TimeLeft.ToString(0 < TimeLeft.Hour ? "hh:mm:ss" : "mm:ss");

        public string TimeLeftMsStr => TimeLeft.ToString("mm:ss.fff");
#pragma warning disable CA1816
        public void Dispose()
#pragma warning restore CA1816

        {
            timer.Dispose();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (TimeLeftMs > timer.Interval)
            {
                TimeLeft = TimeLeft.AddMilliseconds(-timer.Interval);
                TimeChanged?.Invoke();
            }
            else
            {
                Stop();
                TimeLeft = _minTime;

                TimeChanged?.Invoke();
                CountDownFinished?.Invoke();
            }
        }

        private void Init()
        {
            TimeLeft = _maxTime;

            StepMs = 1000;
            timer.Tick += TimerTick;
        }

        public void SetTime(DateTime dt)
        {
            TimeLeft = _maxTime = dt;
            TimeChanged?.Invoke();
        }

        public void SetTime(int min, int sec = 0)
        {
            var H = Convert.ToInt32(Math.Floor((double) (min / 60)));
            var M = min % 60;
            SetTime(H, M, sec);
        }

        public void SetTime(int hour, int min, int sec = 0)
        {
            SetTime(new DateTime(1, 1, 1, hour, min, sec));
        }

        public void Start()
        {
            timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Reset()
        {
            TimeLeft = _maxTime;
        }

        public void Restart()
        {
            Reset();
            Start();
        }
    }
}