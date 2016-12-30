using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;

namespace MonoPiWolf2
{
    public class Pcm
    {
        private readonly ConnectedPin _pin;

        private const int Hertz = 200;
        private byte _frequency;
        private TimeSpan _onTime;
        private TimeSpan _offTime;

        public Pcm(ConnectedPin pin)
        {
            _pin = pin;
            _frequency = 1;
        }

        public async Task Go(CancellationToken token)
        {
            await Task.Yield();
            try
            {
                var sw = new Stopwatch();
                var waitTime = TimeSpan.FromMilliseconds(0.001);
                while (!token.IsCancellationRequested)
                {
                    sw.Restart();
                    _pin.Enabled = true;
                    while (sw.Elapsed < _onTime)
                    {
                        Thread.Sleep(waitTime);
                    }

                    sw.Restart();
                    _pin.Enabled = false;
                    while (sw.Elapsed < _offTime)
                    {
                        Thread.Sleep(waitTime);
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException)
            {
            }
        }

        public void ModulateUp()
        {
            Modulate(Convert.ToByte(_frequency + 1));
        }

        public void ModulateDown()
        {
            Modulate(Convert.ToByte(_frequency - 1));
        }

        public void Modulate(byte frequency)
        {
            _frequency = frequency;
            var onPercentage = (double)frequency / byte.MaxValue;

            const double cycleMilliseconds = 1000.0 / Hertz;

            var cycleTimeInTicks = TimeSpan.TicksPerMillisecond * (long)cycleMilliseconds;

            var onTimeInTicks = cycleTimeInTicks * onPercentage;
            var offTimeInTicks = cycleTimeInTicks - onTimeInTicks;

            _onTime = TimeSpan.FromTicks((long)onTimeInTicks);
            _offTime = TimeSpan.FromTicks((long)offTimeInTicks);

        }
    }
}