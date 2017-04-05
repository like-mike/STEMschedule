using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace stemSchedule
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string checkuser = "select count(*) from UserData where UserName='" + UNTextBox.Text + "'";
            SqlCommand com = new SqlCommand(checkuser, conn);
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            conn.Close();

            if (temp == 1)
            {
                conn.Open();
                string checkPasswordQuery = "select password from UserData where UserName='" + UNTextBox.Text + "'";
                SqlCommand passComm = new SqlCommand(checkuser, conn);
                string password = passComm.ExecuteScalar().ToString();
                //verify password
                if(password==passTextBox.Text)
                {
                    Session["New"] = UNTextBox.Text;
                    Response.Write("Password is correct");
                }
                else
                {
                    Response.Write("Password is not correct");
                }

                
            }
            else
            {
                Response.Write("User name is not correct");
            }
            
        }
    }
}