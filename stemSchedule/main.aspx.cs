using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;

namespace stemSchedule
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string server = "cs.spu.edu";
            string database = "stemschedule";
            string uid = "stemschedule";
            string password = "stemschedule.stemschedule";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from schedule WHERE Public=1";
        
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();

            //private data
            MySqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from schedule WHERE Public=0";

            cmd2.ExecuteNonQuery();
            DataTable dt_private = new DataTable();
            MySqlDataAdapter da_private = new MySqlDataAdapter(cmd2);
            da_private.Fill(dt_private);
            GridView2.DataSource = dt_private;
            GridView2.DataBind();


            con.Close();
           


        }
        private void Initialize()
        {
            
        }
        

        
        

        protected void Button_Push_Click(object sender, EventArgs e)
        {
            string server = "cs.spu.edu";
            string database = "stemschedule";
            string uid = "stemschedule";
            string password = "stemschedule.stemschedule";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection cn = new MySqlConnection(connectionString);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "UPDATE schedule SET public = 1 WHERE public = 0";
            cn.Open();
            int numRowsUpdated = cmd.ExecuteNonQuery();
            cmd.Dispose();
            Response.Redirect("main.aspx");

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Response.Write("test");
            if (e.Row.Cells[0].Text.Equals("1234"))
            {
                e.Row.BackColor = System.Drawing.Color.DarkRed;
                e.Row.ForeColor = System.Drawing.Color.White;
                e.Row.ToolTip = "this is a fun tip!";
            }
            
            
            
        }

        protected void Button_AddClass_Click1(object sender, EventArgs e)
        {
            string server = "cs.spu.edu";
            string database = "stemschedule";
            string uid = "stemschedule";
            string password = "stemschedule.stemschedule";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            MySqlConnection conn = new MySqlConnection(connectionString);


            //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Conflict,Public) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Conflict,@Public)";
            MySqlCommand com = new MySqlCommand(insertQuery, conn);

            //string instructor = Session["New"].ToString();

            com.Parameters.AddWithValue("@CRN", TextBox_CRN.Text);
            com.Parameters.AddWithValue("@Faculty", TextBox_Faculty.Text);
            com.Parameters.AddWithValue("@ClassNum", TextBox_Class.Text);
            com.Parameters.AddWithValue("@Days", TextBox_Days.Text);
            com.Parameters.AddWithValue("@StartTime", TextBox_StartTime.Text);
            com.Parameters.AddWithValue("@EndTime", TextBox_EndTime.Text);
            com.Parameters.AddWithValue("@Term", TextBox_Term.Text);
            com.Parameters.AddWithValue("@Room", TextBox_Classroom.Text);
            com.Parameters.AddWithValue("@EnrollNum", TextBox_Enrollment.Text);
            com.Parameters.AddWithValue("@Year", TextBox_YearTaken.Text);
            com.Parameters.AddWithValue("@M1", TextBox_M1.Text);
            com.Parameters.AddWithValue("@M2", TextBox_Major2.Text);
            com.Parameters.AddWithValue("@M3", TextBox_Major3.Text);
            com.Parameters.AddWithValue("@M4", TextBox_Major4.Text);
            com.Parameters.AddWithValue("@Credits", TextBox_Credits.Text);
            com.Parameters.AddWithValue("@Conflict", 0);//conflicts
            com.Parameters.AddWithValue("@Public", 0);//conflicts
            com.ExecuteNonQuery();
            Response.Redirect("main.aspx");
            Response.Write("Add Class Success");


            conn.Close();
        }
    }
}