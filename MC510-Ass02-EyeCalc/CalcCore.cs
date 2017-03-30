using System;
using System.Globalization;

namespace MC510_Ass02_EyeCalc
{
    class CalcCore
    {
        public delegate void UpdateText(String s);
        public UpdateText updateText;

        float a, c, d;
        char b;

        public void onElementSelected(String s, String currentText)
        {
            switch (s)
            {
                case "C":
                    a = 0;
                    c = 0;
                    d = 0;
                    b = '+';
                    updateText("");
                    break;
                case "CE":
                    updateText("");
                    break;
                case "-":
                    handleOperator(s, currentText);
                    break;
                case "1":
                    handleNumber(s, currentText);
                    break;
                case "2":
                    handleNumber(s, currentText);
                    break;
                case "3":
                    handleNumber(s, currentText);
                    break;
                case "+":
                    handleOperator(s, currentText);
                    break;
                case "4":
                    handleNumber(s, currentText);
                    break;
                case "5":
                    handleNumber(s, currentText);
                    break;
                case "6":
                    handleNumber(s, currentText);
                    break;
                case "*":
                    handleOperator(s, currentText);
                    break;
                case "7":
                    handleNumber(s, currentText);
                    break;
                case "8":
                    handleNumber(s, currentText);
                    break;
                case "9":
                    handleNumber(s, currentText);
                    break;
                case "/":
                    handleOperator(s, currentText);
                    break;
                case "0":
                    handleNumber(s, currentText);
                    break;
                case "=":
                    if (currentText != "")
                    {
                        c = float.Parse(currentText, CultureInfo.InvariantCulture.NumberFormat);
                        if (b == '/')
                        {
                            d = a / c;
                            a = d;
                        }
                        else if (b == '+')
                        {
                            d = a + c;
                            a = d;
                        }
                        else if (b == '-')
                        {
                            d = a - c;
                            a = d;
                        }
                        else
                        {
                            d = a * c;
                            a = d;
                        }
                        updateText(d.ToString("0.0", CultureInfo.InvariantCulture));
                    }
                    break;
            }
        }

        private void handleNumber(String s, String currentText)
        {
            if ((currentText == "+") || (currentText == "-") || (currentText == "*") || (currentText == "/")){
                updateText(s);
            }
            else
                updateText(currentText + s);
        }

        private void handleOperator(String s, String currentText) {
            if(currentText != "+" && currentText != "-" && currentText != "*" && currentText != "/" && currentText != "")
            a = float.Parse(currentText, CultureInfo.InvariantCulture.NumberFormat);
            b = s.ToCharArray()[0];
            updateText(s);
        }
    }
}
