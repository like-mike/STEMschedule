using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;//
using System.Configuration;//
using MySql.Data.MySqlClient;
using System.Data;

namespace stemSchedule
{
    public partial class Registration : System.Web.UI.Page
    {
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                string server = "cs.spu.edu";
                string database = "stemschedule";
                string uid = "stemschedule";
                string password = "stemschedule.stemschedule";

                string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

                MySqlConnection conn = new MySqlConnection(connectionString);


                conn.Open();
                string checkuser = "select count(*) from userdata where UserName='" + UNTextBox.Text + "'";
                MySqlCommand com = new MySqlCommand(checkuser,conn);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                if(temp==1)
                {
                    Response.Write("User already exists");
                }

                conn.Close();
            }
        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Guid newGUID = Guid.NewGuid();//unique global id
                string server = "cs.spu.edu";
                string database = "stemschedule";
                string uid = "stemschedule";
                string password = "stemschedule.stemschedule";

                string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                        database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

                MySqlConnection conn = new MySqlConnection(connectionString);
                //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string insertQuery = "insert into userdata (Id,UserName,Email,Password) values (@id,@Uname,@email,@password)";
                MySqlCommand com = new MySqlCommand(insertQuery, conn);
                //need to create GUID, id is primary key in table
                com.Parameters.AddWithValue("@id",newGUID.ToString());
                com.Parameters.AddWithValue("@Uname", UNTextBox.Text);
                com.Parameters.AddWithValue("@email", emailTextBox.Text);
                com.Parameters.AddWithValue("@password", passTextBox.Text);
                
                com.ExecuteNonQuery();
                //Response.Redirect("Manager.aspx");
                Response.Write("Registration Successful");

                conn.Close();
                
            }
            catch(Exception ex)
            {
                Response.Write("Error:"+ex.ToString());
            }

            //Response.Write("Success");
        }
    }
}