using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MC510_Ass02_EyeCalc
{
    public partial class CalcControl : UserControl
    {
        private static int KEYBOARD_UPDATE_FREQ = 60; // in Hz
        private static int KEYBOARD_DWELL_TIME = 1000; // in ms
        private static int KEYBOARD_TIME_BEFORE_LOCK = 1000; // in ms
        private static int KEYBOARD_MAX_SIGNAL_PAUSE = 1000; // in ms
        private static float KEYBOARD_X_FACTOR_TOLERANCE = 0.5f;
        private static float KEYBOARD_Y_FACTOR_TOLERANCE = 0.5f;
        
        private string[,] BASE_KEYBOARD = new string[4,4] { { "7", "8", "9", "/" }, { "4", "5", "6", "*" }, { "1", "2", "3", "-" }, { "0", "CE", "C", "+" } };
        private string[,] LOCKED_KEYBOARD = new string[4, 4] { { "-", "-", "-", "-" }, { "-", "-", "-", "-" }, { "-", "-", "-", "-" }, { "-", "-", "-", "UNLOCK" } };
        private string[,] currentKeyboard;

        public delegate void ElementSelected(String s);
        public ElementSelected elementSelected;

        private float? xFactor, yFactor;
        private GazeData startPoint, latestPoint;

        private double holdProgress = 0.0;
        private int col = -1;
        private int row = -1;

        private long lastEventTime;

        public CalcControl()
        {
            InitializeComponent();
            currentKeyboard = BASE_KEYBOARD;

            Thread backgroundThread = new Thread(argument =>
            {
                while(true) {
                        if(latestPoint != null) {
                            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;;

                            if(latestPoint.getRow() >= 0 && latestPoint.getColumn() >= 0) {
                                // gaze is on a cell
                                if (startPoint == null || startPoint.getColumn() != latestPoint.getColumn() || startPoint.getRow() != latestPoint.getRow()
                                        || (now - startPoint.getTimestamp()) > KEYBOARD_MAX_SIGNAL_PAUSE) {
                                    // then startPoint is either null or not the latest point
                                    startPoint = latestPoint;
                                    col = -1;
                                    row = -1;
                                    holdProgress = 0.0;
                                } else {
                                    // the gaze is on the same cell -> update ui
                                    col = latestPoint.getColumn();
                                    row = latestPoint.getRow();
                                    holdProgress = ((double) (now - startPoint.getTimestamp())) / KEYBOARD_DWELL_TIME;
                                }
                            } else {
                                col = -1;
                                row = -1;
                                holdProgress = 0.0;

                                // try to lock screen
                                if(startPoint == null || (startPoint.getRow() >= 0 && startPoint.getColumn() >= 0)) {
                                    // if gaze switches from onscreen to offscreen
                                    startPoint = latestPoint;
                                }

                                if((now - startPoint.getTimestamp()) >= KEYBOARD_TIME_BEFORE_LOCK) {
                                    currentKeyboard = LOCKED_KEYBOARD;
                                }
                            }
                            // always draw - either clear selection or update holdProgress
                            Invalidate();
                            
                            // check if the click is done
                            if(holdProgress >= 0.95) {
                                tryPerformSelection(col, row);
                                startPoint = null;
                            }
                        }

                        Thread.Sleep(1000 / KEYBOARD_UPDATE_FREQ);
                    }
            });

            backgroundThread.IsBackground = true; // <- Important! You don't want this thread to keep the application open.
            backgroundThread.Start();
        }

        private void tryPerformSelection(int selectedColumn, int selectedRow)
        {
            // allow only 1 execution of the following code in dwellTime/2 to avoid multiple selection
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if ((now - lastEventTime) >= (KEYBOARD_DWELL_TIME / 2))
            {
                for (int row = 0; row < currentKeyboard.GetLength(0); row++)
                {
                    for (int col = 0; col < currentKeyboard.GetLength(1); col++)
                    {
                        if (selectedColumn == col && selectedRow == row)
                        {
                            performSelection(row, col);
                        }
                    }
                }

                lastEventTime = now;
            }
        }

        /**
         * performs the actual element-selection
         * @param row
         * @param col
         */
        private void performSelection(int row, int col) {
            if (currentKeyboard.GetLength(0) > row && currentKeyboard.GetLength(1) > col)
            {
                // cell exists

                Object cell = currentKeyboard[row, col];
                if(currentKeyboard == LOCKED_KEYBOARD) {
                    // maybe unlock
                    if(((String)cell) == "UNLOCK") {
                        currentKeyboard = BASE_KEYBOARD;
                    }
                } else {
                    if (elementSelected != null)
                    {
                        elementSelected((String)cell);
                    }
                    currentKeyboard = BASE_KEYBOARD;
                }
            }
        }

        private void CalcControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // draw raster
            for (int row = 0; row < currentKeyboard.GetLength(0); row++)
            {
                float y = ((float)Height) / currentKeyboard.GetLength(0) * row;
                g.DrawLine(Pens.Gray, new Point(0, (int)y), new Point(Width, (int)y));
            }
            for (int col = 1; col < currentKeyboard.GetLength(1); col++)
            {
                float x = ((float)Width) / currentKeyboard.GetLength(1) * col;
                g.DrawLine(Pens.Gray, new Point((int)x, 0), new Point((int)x, Height));
            }

            // draw characters
            for (int row = 0; row < currentKeyboard.GetLength(0); row++)
            {
                for (int col = 0; col < currentKeyboard.GetLength(1); col++)
                {
                    if (currentKeyboard.GetLength(0) > row && currentKeyboard.GetLength(1) > col)
                    {
                        // cell exists

                        String drawString = "";
                        String cell = currentKeyboard[row, col];
                        String element = (String)cell;
                        drawString = element.Trim().Length == 0 ? "_" : element;

                        // set textSize based on holdProgress
                        Brush brush = Brushes.Gray;
                        Font font;
                        if (this.row == row && this.col == col)
                        {
                            font = new Font(DefaultFont.FontFamily, (40f + (float)(80f * holdProgress)));
                        }
                        else
                        {
                            font = new Font(DefaultFont.FontFamily, 40f);
                        }

                        float textWidth = g.MeasureString(drawString, font).Width;
                        float textHeight = g.MeasureString(drawString, font).Height;

                        float xOffset = (0.5f + col) * ((float)Width) / currentKeyboard.GetLength(1) - textWidth/2;
                        float yOffset = (0.5f + row) * ((float)Height) / currentKeyboard.GetLength(0) - textHeight / 2;

                        g.DrawString(drawString, font, brush, new Point((int)xOffset, (int)yOffset));
                    }
                }
            }

            // draw gaze-dot
            if (xFactor.HasValue && yFactor.HasValue)
            {
                g.FillEllipse(Brushes.Blue, ((float)Width) * xFactor.Value - 15, ((float)Height) * yFactor.Value - 15, 30, 30);
            }
        }

        public void updateGaze(float xFactor, float yFactor) {
            int col = -1;
            int row = -1;

            if(xFactor >= (0f - KEYBOARD_X_FACTOR_TOLERANCE) && xFactor <= (1f + KEYBOARD_X_FACTOR_TOLERANCE)) {
                this.xFactor = Math.Max(0.0f, Math.Min(1.0f, xFactor));
                col = (int)(((float)Width) * this.xFactor / (((float)Width) / currentKeyboard.GetLength(1)));
                col = Math.Min(col, BASE_KEYBOARD.GetLength(1)-1);
            } else {
                this.xFactor = xFactor;
            }
            if(yFactor >= (0f - KEYBOARD_Y_FACTOR_TOLERANCE) && yFactor <= (1f + KEYBOARD_Y_FACTOR_TOLERANCE)) {
                this.yFactor = Math.Max(0.0f, Math.Min(1.0f, yFactor));
                row = (int)(((float)Height) * this.yFactor / (((float)Height) / currentKeyboard.GetLength(0)));
                row = Math.Min(row, BASE_KEYBOARD.GetLength(0)-1);
            } else {
                this.yFactor = yFactor;
            }

            latestPoint = new GazeData(row, col);
        }

        private void CalcControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
