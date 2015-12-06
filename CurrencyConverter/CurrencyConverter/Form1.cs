using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurrencyConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static double lastExchange = 0;
        static bool documentReady = true;

        void requestExchange(string from, string to, double value, string date)
        {
            string link = String.Format("http://{0}.fxexchangerate.com/{1}-{2}-exchange-rates-history.html", from, to, date);
            documentReady = false;
            webBrowser1.Navigate(link);
            webBrowser1.DocumentCompleted += delegate
            {
                documentReady = true;
            };
            while (!documentReady)
                Application.DoEvents();
            HtmlElementCollection collection = webBrowser1.Document.All;
            List<string> contents = new List<string>();

            /*
             * Adds all inner-text of a tag, including inner-text of sub-tags
             * ie. <html><body><a>test</a><b>test 2</b></body></html> would do:
             * "test test 2" when collection[i] == <html>
             * "test test 2" when collection[i] == <body>
             * "test" when collection[i] == <a>
             * "test 2" when collection[i] == <b>
             */
            for (int i = 0; i < collection.Count; i++)
            {
                if (!string.IsNullOrEmpty(collection[i].InnerText))
                {
                    contents.Add(collection[i].InnerText);
                }
            }

            /*
             * <html><body><a>test</a><b>test 2</b></body></html>
             * outputs: test test 2|test test 2|test|test 2
             */
            string contentString = string.Join("|", contents.ToArray());
            int position = contentString.IndexOf(string.Format("1 {0} =", from));
            for (++position; position < contentString.Length && !isNum(contentString[position]); position++) ;
            string number = "";
            for (; position < contentString.Length && isNum(contentString[position]); position++)
                number += contentString[position];
            lastExchange = Convert.ToDouble(number.Replace('.', ','));
            MessageBox.Show(lastExchange.ToString());
        }

        static bool isNum(char s)
        {
            return ((s >= '0' && s <= '9') || s == '.');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            requestExchange("JPY", "CHF", 666, dateTimePicker1.Value.ToString("yyyy_MM_dd"));
        }

    }
}
