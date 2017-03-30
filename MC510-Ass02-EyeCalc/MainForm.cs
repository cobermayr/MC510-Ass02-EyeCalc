using System;
using System.Windows.Forms;

namespace MC510_Ass02_EyeCalc
{
    public partial class MainForm : Form
    {
        CalcCore calcCore;

        public MainForm()
        {
            InitializeComponent();

            calcCore = new CalcCore();
            calcCore.updateText += new CalcCore.UpdateText(onUpdateText);

            calcControl.elementSelected += new CalcControl.ElementSelected(onElementSelected);

            TestAdapter testAdapter = new TestAdapter(calcControl);
        }

        private void onElementSelected(String s)
        {
            calcCore.onElementSelected(s, calcControl.getText());
        }

        private void onUpdateText(String s)
        {
            calcControl.updateText(s);
        }
    }
}
