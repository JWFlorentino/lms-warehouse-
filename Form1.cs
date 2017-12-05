using System;//test
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Data.SqlClient;
using System.IO;

namespace FTPupload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {




            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection _DBCon = new SqlConnection("Data Source=12.124.18.34,121;Initial Catalog=test;User Id=polo;Password=toets4;");
                SqlCommand _DBCommand = new SqlCommand("[dbo].[repCustDelStatusPAGorder]", _DBCon);
                _DBCommand.CommandType = CommandType.StoredProcedure; _DBCommand.CommandTimeout = 600000;
                _DBCon.Open(); SqlDataAdapter da = new SqlDataAdapter(_DBCommand); DataTable dt = new DataTable(); da.Fill(dt); _DBCon.Close();

                StringBuilder sb = new StringBuilder();

                string[] columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText(@"C:\temp\CLBPL_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv", sb.ToString());
                //CLBPL_YYYYMMDD_HHSS.csv
                

                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("City Logistics", "uB6VPTWb");
                    client.UploadFile("ftp://ftp.bpl.za.com/CLBPL_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv", "STOR", @"C:\temp\CLBPL_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
		//check
            }
        }
    }
}
