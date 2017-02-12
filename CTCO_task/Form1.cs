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
        
        DataTable table = new DataTable();
        DataTable table1 = new DataTable();
        bool changes;

        decimal total=0;

        public Form1()
        {
            InitializeComponent();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("service", typeof(string));
            table.Columns.Add("money", typeof(int));
            dataGridView1.DataSource = table;

            table1.Columns.Add("name", typeof(string));
            table1.Columns.Add("money", typeof(int));
            dataGridView2.DataSource = table1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            total += numericUpDown1.Value;

            table.Rows.Add(textBox1.Text, textBox2.Text, numericUpDown1.Value);                     

            label4.Text = "Toatl: "+Convert.ToString(total);

            

            if (table1.Rows.Count==0)
                table1.Rows.Add(textBox1.Text, numericUpDown1.Value);
            else
            {
                changes = true;
                foreach (DataRow row in table1.Rows)
                {
                    if (row["name"].ToString() == textBox1.Text)
                    {
                        row["money"] = Convert.ToString(Convert.ToDecimal(row["money"].ToString()) + numericUpDown1.Value);
                        changes = false;
                        break;
                    }
                }
                if(changes)
                table1.Rows.Add(textBox1.Text, numericUpDown1.Value);
            }

            label5.Text = "Average: " +( total/table1.Rows.Count);
            
        }
    }
}
