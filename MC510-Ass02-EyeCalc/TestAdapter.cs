using EyeXFramework;
using System;
using System.Threading;
using System.Windows.Forms;
using Tobii.EyeX.Framework;

namespace MC510_Ass02_EyeCalc
{
    class TestAdapter
    {
        private CalcControl calcControl;
        private Random rnd = new Random();

        public TestAdapter(CalcControl calcControl)
        {
            this.calcControl = calcControl;

            // start thread
            Thread backgroundThread = new Thread(argument =>
            {
                using (var eyeXHost = new EyeXHost())
                {
                    // create a data stream: lightly filtered gaze point data
                    using (var lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
                    {
                        eyeXHost.Start();

                        while (true)
                        {
                            lightlyFilteredGazeDataStream.Next += (s, e) => calcControl.updateGaze(((float)(e.X - calcControl.Parent.Left - 8) / calcControl.Width), ((float)(e.Y - calcControl.Parent.Top - 30)) / calcControl.Height);
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                }
            });

            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
    }
}
