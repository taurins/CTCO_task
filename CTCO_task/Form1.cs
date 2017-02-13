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

        decimal total = 0;
        decimal average = 0;

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
            //Calculates expenses for each person
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
            //checks whitch person has to pay and which persons get paid
            transChecker = new DataTable();

            transChecker.Columns.Add("Name", typeof(string));
            transChecker.Columns.Add("Amount", typeof(decimal));

            foreach (DataRow row in expenses.Rows)
            {
                transChecker.Rows.Add(row["Name"].ToString(), (Convert.ToDecimal(row["Amount"].ToString()) - average));
            }
            transChecker.Select("", "Amount asc");
        }

        private void transCalc()
        {
            transactions = new DataTable();
            transactions.Columns.Add("From", typeof(string));
            transactions.Columns.Add("To", typeof(string));
            transactions.Columns.Add("Amount", typeof(decimal));
            DataRow rowi;
            DataRow rowj;

            for (int i = transChecker.Rows.Count - 1; i >= 0; i--)
            {
                rowi = transChecker.Rows[i];
                //for equal transactions
                if (Convert.ToDecimal(rowi["Amount"].ToString()) < 0)
                    for (int j = transChecker.Rows.Count - 1; j >= 0; j--)
                    {
                        rowj = transChecker.Rows[j];
                        if (rowi["Name"]!= rowj["Name"])
                            if (Convert.ToDecimal(rowi["Amount"].ToString()) == (Convert.ToDecimal(rowj["Amount"].ToString()) * (-1)))
                            {
                                transactions.Rows.Add(rowi["Name"].ToString(), rowj["name"].ToString(), rowj["Amount"].ToString());
                                rowi["Amount"] = 0;
                                rowj["Amount"] = 0;

                            }
                    }
            }

            for (int i = transChecker.Rows.Count - 1; i >= 0; i--)
            {
                rowi = transChecker.Rows[i];
                //for smaler minuses
                if (Convert.ToDecimal(rowi["Amount"].ToString()) < 0)               
                    for (int j = transChecker.Rows.Count - 1; j >= 0; j--)
                    {
                        rowj = transChecker.Rows[j];
                        if (rowi["Name"] != rowj["Name"])
                            if ((Convert.ToDecimal(rowi["Amount"].ToString())) >= (Convert.ToDecimal(rowj["Amount"].ToString()) * (-1)))
                        {
                            if (Convert.ToDecimal(rowi["Amount"].ToString()) < 0)
                            {
                                transactions.Rows.Add(rowi["Name"].ToString(), rowj["name"].ToString(), rowi["Amount"].ToString());
                                rowj["Amount"] = Convert.ToString(Convert.ToDecimal(rowj["Amount"]) - (Convert.ToDecimal(rowi["Amount"])*(-1)));
                                rowi["Amount"] = 0;

                            }
                        }
                    }
            }


            for (int i = transChecker.Rows.Count - 1; i >= 0; i--)
            {
                rowi = transChecker.Rows[i];
                //for larger minuses
                    for (int j = transChecker.Rows.Count - 1; j >= 0; j--)
                    {
                        rowj = transChecker.Rows[j];
                    if (rowi["Name"] != rowj["Name"])
                        if ((Convert.ToDecimal(rowi["Amount"].ToString()) <= (Convert.ToDecimal(rowj["Amount"].ToString()) * (-1))))
                        {
                            if (Convert.ToDecimal(rowi["Amount"].ToString()) < 0)
                            {
                                transactions.Rows.Add(rowi["Name"].ToString(), rowj["name"].ToString(), rowj["Amount"].ToString());
                                rowi["Amount"] = Convert.ToString(Convert.ToDecimal(rowi["Amount"]) + Convert.ToDecimal(rowj["Amount"]));
                                rowj["Amount"] = 0;
                            }

                        }
                    }

            }
            dataGridView3.DataSource = transactions;
        }
    }
}
