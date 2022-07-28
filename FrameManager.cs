using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Hologram
{
    public class FrameManager
    {
        public FrameManager()
        {
            sw.Start();
            FrameTime = (float)1000 / FPS;
        }

        private static int FPS = 120;
        private static float FrameTime;

        private Stopwatch sw = new Stopwatch();

        private double startFrame;
        private double endFrame;

        private double lastFrame;
        private double drawTime;

        private const int deltaSize = 500;
        private double[] deltas = new double[deltaSize];
        private int offset = 0;
        private bool cycledDeltas = false;

        public int avgFPS { get; private set; }

        public void StartingDraw()
        {
            startFrame = sw.Elapsed.TotalMilliseconds;
        }

        public void FinishedDraw()
        {
            endFrame = sw.Elapsed.TotalMilliseconds;

            drawTime = endFrame - startFrame;

            double _deltaTime = endFrame - lastFrame;
            deltas[offset] = _deltaTime;

            offset++;
            if (offset == deltaSize)
            {
                cycledDeltas = true;
                offset = 0;
            }

            double totalTime = 0;
            int sampleTo = cycledDeltas ? deltaSize : offset;
            for (int i = 0; i < sampleTo; i++)
            {
                totalTime += deltas[i];
            }

            avgFPS = (int)Math.Round(sampleTo / (totalTime / 1000));

            lastFrame = sw.Elapsed.TotalMilliseconds;
        }

        public void WaitUntilNextFrame()
        {
            double sleepyTime = FrameTime - drawTime;
            double nextFrame = lastFrame + sleepyTime;
            if (sleepyTime > 0)
            {
                Thread.Sleep((int)sleepyTime - 1);
            }
            while (sw.Elapsed.TotalMilliseconds < nextFrame) { }
        }
    }
}
