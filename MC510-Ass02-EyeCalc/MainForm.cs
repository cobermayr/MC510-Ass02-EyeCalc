using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MC510_Ass02_EyeCalc
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            calcControl.elementSelected += new CalcControl.ElementSelected(onElementSelected);
            TestAdapter testAdapter = new TestAdapter(calcControl);
        }

        private void onElementSelected(String s)
        {
            Console.WriteLine(s);
        }
    }
}
