using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;//
using System.Configuration;//

namespace stemSchedule
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string checkuser = "select count(*) from userdata where UserName='" + UNTextBox.Text + "'";
                SqlCommand com = new SqlCommand(checkuser,conn);
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

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string insertQuery = "insert into userdata (Id,UserName,Email,Password) values (@id,@Uname,@email,@password)";
                SqlCommand com = new SqlCommand(insertQuery, conn);
                //need to create GUID, id is primary key in table
                com.Parameters.AddWithValue("@id",newGUID.ToString());
                com.Parameters.AddWithValue("@Uname", UNTextBox.Text);
                com.Parameters.AddWithValue("@email", emailTextBox.Text);
                com.Parameters.AddWithValue("@password", passTextBox.Text);
                
                com.ExecuteNonQuery();
                Response.Redirect("Manager.aspx");
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