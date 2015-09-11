using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;


namespace rendomgen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {

            if(txtFrom.Text=="" || txtTo.Text=="")
            {
                MessageBox.Show("please fill from and to");
                return;
            }
            OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\random\\db.accdb;Persist Security Info=False;");
            
            oleDbConnection.Open();

            OleDbCommand cmd1 = new OleDbCommand();

            cmd1.Connection = oleDbConnection;

            cmd1.CommandText = "DELETE from series";

            cmd1.ExecuteNonQuery();

            for (int i =Convert.ToInt32(txtFrom.Text); i <= Convert.ToInt32(txtTo.Text); i++)
            {
                OleDbCommand cmd = new OleDbCommand();

                cmd.Connection = oleDbConnection;

                cmd.CommandText = "INSERT INTO series([Number]) VALUES (@No)";

                cmd.Parameters.Add("@No", OleDbType.Numeric).Value = i;

                cmd.ExecuteNonQuery();
            }


            oleDbConnection.Close();
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\random\\db.accdb;Persist Security Info=False;";

            string queryString = "SELECT number FROM [series] where given=0 order by number";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    connection.Open();

                    OleDbCommand cmd = new OleDbCommand("select count(*) FROM [series] where given=false", connection);

                    int total = (Int32)cmd.ExecuteScalar();
  

                    OleDbDataReader reader = command.ExecuteReader();

             
                    Random r = new Random();
                    int rInt = r.Next(1, total); //for ints
                    int i = 1;
                    while (reader.Read())
                    {
                        Int32 randomno = Convert.ToInt32(reader.GetValue(0));
                       // lblNo.Text = randomno.ToString();
                        if (i == rInt)
                        {
                            lblNo.Text = randomno.ToString();
                            cmd.CommandText = "Update series set given=false where [Number]=@No";

                            cmd.Parameters.Add("@No", OleDbType.Numeric).Value = randomno;

                            cmd.ExecuteNonQuery();
                            break;
                        }
                        i++;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\random\\db.accdb;Persist Security Info=False;";

            string queryString = "SELECT number FROM [series] where given=true order by number";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    connection.Open();

                    OleDbCommand cmd = new OleDbCommand("select count(*) FROM [series] where given=true", connection);

                    int total = (Int32)cmd.ExecuteScalar();


                    OleDbDataReader reader = command.ExecuteReader();


                    Random r = new Random();
                    int rInt = r.Next(1, total); //for ints
                    int i = 1;
                    while (reader.Read())
                    {
                        Int32 randomno = Convert.ToInt32(reader.GetValue(0));
                        // lblNo.Text = randomno.ToString();
                        if (i == rInt)
                        {
                            //lblNo.Text = randomno.ToString();
                            MessageBox.Show("Winner is " + randomno.ToString());
                            //cmd.CommandText = "Update series set given=1 where [Number]=@No";

                            //cmd.Parameters.Add("@No", OleDbType.Numeric).Value = randomno;

                            //cmd.ExecuteNonQuery();
                            break;
                        }
                        i++;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source");
            }
        }
    }
}
