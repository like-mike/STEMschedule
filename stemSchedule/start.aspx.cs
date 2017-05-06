using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace stemSchedule
{
    public partial class start : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string formUserName = UserName.Value.ToString();
            string formPassword = Password.Value.ToString();

            string server = "cs.spu.edu";
            string database = "stemschedule";
            string uid = "stemschedule";
            string password = "stemschedule.stemschedule";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            int temp = 0;
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string checkuser = "select count(*) from UserData where UserName='" + formUserName + "'";
                MySqlCommand com = new MySqlCommand(checkuser, conn);
                temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                conn.Close();
            }
            catch { }
            

            if (temp == 1)
            {
                try
                {
                    conn.Open();
                    string checkPasswordQuery = "select Password from UserData where UserName='" + formUserName + "'";
                    MySqlCommand passComm = new MySqlCommand(checkPasswordQuery, conn);
                    string pass = passComm.ExecuteScalar().ToString().Replace(" ", "");//remove whitespace
                                                                                       //verify password
                    if (pass == formPassword)
                    {
                        Session["New"] = formUserName;
                        Response.Write("Password is correct");
                        Response.Redirect("main.aspx");
                    }
                    else
                    {
                        Response.Write("Password is not correct");
                    }
                    conn.Close();
                }
                catch { }

            }
            else
            {
                Response.Write("User name is not correct");
            }

        }

    
    }
}