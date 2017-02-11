using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTCO_task
{
    public partial class Form1 : Form
    {
        public class record
        {
            public string name { get; set; }
            public string service { get; set; }
            public decimal money { get; set; }
        }

        //List<record> data = new List<record>();
        BindingList<record> data = new BindingList<record>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            record rec = new record();
            rec.name = textBox1.Text;
            rec.service = textBox2.Text;
            rec.money = numericUpDown1.Value;
            data.Add(rec);
            BindingSource source = new BindingSource(data, null);
            dataGridView1.DataSource = source;

        }
    }
}
