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
            webExtractor1 = new WebExtractor(webBrowser1);
            view1 = new View(textBox1, textBox2, listView1, listView2, dateTimePicker1, webExtractor1);
            webExtractor1.addObserver(view1);

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

        public WebExtractor webExtractor1;
        View view1;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                view1.updateFields(1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                view1.updateFields(2);
        }
    }
}
