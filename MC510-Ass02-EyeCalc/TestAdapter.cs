using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                while(true) {
                    if(1 == 1) {
                        int x = (Cursor.Position.X - calcControl.Parent.Left) - 8;
                        int y = (Cursor.Position.Y - calcControl.Parent.Top) - 30;
                        calcControl.updateGaze(((float)x / ((float)calcControl.Width)), ((float)y) / ((float)calcControl.Height));
                        Thread.Sleep(50);
                    }
                }
            });

            backgroundThread.IsBackground = true; // <- Important! You don't want this thread to keep the application open.
            backgroundThread.Start();
        }
    }
}
