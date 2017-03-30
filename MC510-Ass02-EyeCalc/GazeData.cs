using System;

namespace MC510_Ass02_EyeCalc
{
    class GazeData
    {
        int row = -1;
        int col = -1;
        long timestamp;

        public GazeData(int row, int column)
        {
            this.row = row;
            this.col = column;
            this.timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public GazeData(int row, int col, long timestamp)
        {
            this.row = row;
            this.col = col;
            this.timestamp = timestamp;
        }

        public int getRow()
        {
            return row;
        }

        public int getColumn()
        {
            return col;
        }

        public long getTimestamp()
        {
            return timestamp;
        }
    }
}
