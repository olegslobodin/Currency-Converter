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
            ListInit init = new ListInit();
            for (int i = 0; i < init.Codes.Count; i++)
            {
                string line = string.Format("{0} - {1}", init.Codes.ElementAt(i), init.Descriptions.ElementAt(i));

                ListViewItem item = new ListViewItem(line);
                item.Tag = init.Codes.ElementAt(i);
                listView1.Items.Add(item);

                item = new ListViewItem(line);
                item.Tag = init.Codes.ElementAt(i);
                listView2.Items.Add(item);
            }
        }

        static bool documentReady = true;
        static bool converting = false;

        double requestExchange(string from, string to, double value, string date)
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
            return value * Convert.ToDouble(number.Replace('.', ','));
        }

        static bool isNum(char s)
        {
            return ((s >= '0' && s <= '9') || s == '.');
        }

        void convert(int sourceFieldNumber)
        {
            if (converting)
                return;
            converting = true;
            TextBox sourceField = ((sourceFieldNumber == 1) ? textBox1 : textBox2);
            TextBox destField = ((sourceFieldNumber == 2) ? textBox1 : textBox2);
            sourceField.Enabled = false;
            try
            {

                string sourceCode = ((sourceFieldNumber == 1) ? listView1.SelectedItems[0].Tag.ToString() : listView2.SelectedItems[0].Tag.ToString());
                string destCode = ((sourceFieldNumber == 2) ? listView1.SelectedItems[0].Tag.ToString() : listView2.SelectedItems[0].Tag.ToString());
                double value = Convert.ToDouble(sourceField.Text.Replace('.', ','));
                string date = dateTimePicker1.Value.ToString("yyyy_MM_dd");
                destField.Text = requestExchange(sourceCode, destCode, value, date).ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            sourceField.Enabled = true;
            converting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            requestExchange("JPY", "CHF", 666, dateTimePicker1.Value.ToString("yyyy_MM_dd"));
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                convert(1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                convert(2);
        }
    }
}
