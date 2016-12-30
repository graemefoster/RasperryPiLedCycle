using System;
using System.Collections.Generic;
using System.Threading;
using Raspberry.IO.GeneralPurpose;

namespace MonoPiWolf2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pinR = ConnectorPin.P1Pin16.Output();
            var pinG = ConnectorPin.P1Pin18.Output();
            var pinB = ConnectorPin.P1Pin22.Output();

            using (var connection = new GpioConnection(pinR, pinG, pinB))
            {
                var pcmRed = new Pcm(connection.Pins[pinR]);
                var pcmGreen = new Pcm(connection.Pins[pinG]);
                var pcmBlue = new Pcm(connection.Pins[pinB]);

                var ct = new CancellationTokenSource();

                var taskR = pcmRed.Go(ct.Token);
                var taskG = pcmGreen.Go(ct.Token);
                var taskB = pcmBlue.Go(ct.Token);

                foreach(var rainbow in MakeColorGradient(0.01, 0.01, 0.01, 0, 2, 4, len:10000))
                {
                    pcmRed.Modulate(rainbow.Item1);
                    pcmGreen.Modulate(rainbow.Item2);
                    pcmBlue.Modulate(rainbow.Item3);
                    Thread.Sleep(TimeSpan.FromMilliseconds(5));
                }
            }
        }

        public static IEnumerable<Tuple<byte, byte, byte>> MakeColorGradient(
            double frequency1, double frequency2, double frequency3,
            double phase1, double phase2, double phase3,
            double center = 128, double width = 127, double len = 1000)
        {
            for (var i = 0; i < len; ++i)
            {
                var red = (byte)(Math.Sin(frequency1 * i + phase1) * width + center);
                var grn = (byte)(Math.Sin(frequency2 * i + phase2) * width + center);
                var blu = (byte)(Math.Sin(frequency3 * i + phase3) * width + center);
                yield return new Tuple<byte, byte, byte>(red, grn, blu);
            }
        }
    }
}