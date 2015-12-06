using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurrencyConverter
{
    class View : Observer
    {
        public View(TextBox tb1, TextBox tb2, ListView lw1, ListView lw2, DateTimePicker dt1, WebExtractor we1)
        {
            textBox1 = tb1;
            textBox2 = tb2;
            listView1 = lw1;
            listView2 = lw2;
            dateTimePicker1 = dt1;
            webExtractor1 = we1;
        }

        TextBox textBox1, textBox2, sourceField, destField;
        ListView listView1, listView2;
        DateTimePicker dateTimePicker1;
        WebExtractor webExtractor1;
        bool converting = false;

        override public void update(Object data)
        {
            if (!converting)
                return;
            destField.Text = ((Double)data).ToString();
            sourceField.Enabled = true;
            converting = false;
        }

        public void updateFields(int sourceFieldNumber)
        {
            if (converting)
                return;
            converting = true;
            sourceField = ((sourceFieldNumber == 1) ? textBox1 : textBox2);
            destField = ((sourceFieldNumber == 2) ? textBox1 : textBox2);
            sourceField.Enabled = false;
            try
            {

                string sourceCode = ((sourceFieldNumber == 1) ? listView1.SelectedItems[0].Tag.ToString() : listView2.SelectedItems[0].Tag.ToString());
                string destCode = ((sourceFieldNumber == 2) ? listView1.SelectedItems[0].Tag.ToString() : listView2.SelectedItems[0].Tag.ToString());
                double value = Convert.ToDouble(sourceField.Text.Replace('.', ','));
                string date = dateTimePicker1.Value.ToString("yyyy_MM_dd");
                webExtractor1.requestExchange(sourceCode, destCode, value, date);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
