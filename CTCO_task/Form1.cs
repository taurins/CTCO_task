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
        DataTable inputData = new DataTable();
        DataTable expenses = new DataTable();
        DataTable transactions;
        DataTable transChecker;

        bool expensesCheck;

        decimal total=0;
        decimal average=0;

        public Form1()
        {
            InitializeComponent();

            tableColumns();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Expenses();
            TotalAverage();
            transAdd();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            transCalc();
        }


        private void tableColumns()
        {
            inputData.Columns.Add("Name", typeof(string));
            inputData.Columns.Add("Service", typeof(string));
            inputData.Columns.Add("Amount", typeof(decimal));
            dataGridView1.DataSource = inputData;

            expenses.Columns.Add("Name", typeof(string));
            expenses.Columns.Add("Amount", typeof(decimal));
            dataGridView2.DataSource = expenses;            
        }

        private void TotalAverage()
        {
            total += numericUpDown1.Value;
            average = Math.Round((total / expenses.Rows.Count), 2);
            inputData.Rows.Add(textBox1.Text, textBox2.Text, numericUpDown1.Value);
            label4.Text = "Total: " + Convert.ToString(total);
            label5.Text = "Average: " + Convert.ToString(average);
        }

        private void Expenses()
        {
            if (expenses.Rows.Count == 0)
                expenses.Rows.Add(textBox1.Text, numericUpDown1.Value);
            else
            {
                expensesCheck = true;
                foreach (DataRow row in expenses.Rows)
                {
                    if (row["Name"].ToString() == textBox1.Text)
                    {
                        row["Amount"] = Convert.ToString(Convert.ToDecimal(row["Amount"].ToString()) + numericUpDown1.Value);
                        expensesCheck = false;
                        break;
                    }
                }
                if (expensesCheck)
                    expenses.Rows.Add(textBox1.Text, numericUpDown1.Value);
            }
        }

        private void transAdd()
        {

            transChecker = new DataTable();

            transChecker.Columns.Add("Name", typeof(string));
            transChecker.Columns.Add("Amount", typeof(decimal));

            
            foreach (DataRow row in expenses.Rows)
            {
                transChecker.Rows.Add(row["Name"].ToString(), (Convert.ToDecimal(row["Amount"].ToString())-average) );
            }
            transChecker.Select("","Amount asc");
            dataGridView3.DataSource = transChecker;
        }

        private void transCalc()
        {
            transactions = new DataTable();
            transactions.Columns.Add("From", typeof(string));
            transactions.Columns.Add("To", typeof(string));
            transactions.Columns.Add("Amount", typeof(decimal));

            if(transChecker.Rows.Count>0)
            for (int i = transChecker.Rows.Count-1; i >=0; i--)
            {
                DataRow rowi = transChecker.Rows[i];
                if (Convert.ToDecimal(rowi["Amount"].ToString())<0)
                    for (int j = transChecker.Rows.Count - 1; j >= 0; j--)
                    {
                        DataRow rowj = transChecker.Rows[j];
                            if ((Convert.ToDecimal(rowi["Amount"].ToString()) == (Convert.ToDecimal(rowj["Amount"].ToString()) * (-1))))
                            {
                                transactions.Rows.Add(rowi["Name"].ToString(), rowj["name"].ToString(), rowj["Amount"].ToString());
                                if (i > j)
                                {
                                    transChecker.Rows[i].Delete();
                                    transChecker.Rows[j].Delete();
                                }
                                else
                                {
                                    transChecker.Rows[j].Delete();
                                    transChecker.Rows[i].Delete();
                                }
                                break;
                            }                            
                    }
                    
            }

            dataGridView4.DataSource = transactions;
        }

        
    }
}
