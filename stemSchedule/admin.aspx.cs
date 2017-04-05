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
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_deleteError.Visible = false;
            Panel_delete.Visible = false;
            if (Session["New"] != null)
            {
                //label_welcome.Text += Session["New"].ToString();

            }
            else
                Response.Redirect("Login.aspx");

            if (IsPostBack)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string checkuser = "select count(*) from UserData where UserName='" + UNTextBox.Text + "'";
                SqlCommand com = new SqlCommand(checkuser, conn);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                if (temp == 1)
                {
                    Response.Write("User already exists");
                }

                conn.Close();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Guid newGUID = Guid.NewGuid();//unique global id

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string insertQuery = "insert into UserData (Id,UserName,Email,Password) values (@id,@Uname,@email,@password)";
                SqlCommand com = new SqlCommand(insertQuery, conn);
                //need to create GUID, id is primary key in table
                com.Parameters.AddWithValue("@id", newGUID.ToString());
                com.Parameters.AddWithValue("@Uname", UNTextBox.Text);
                com.Parameters.AddWithValue("@email", emailTextBox.Text);
                com.Parameters.AddWithValue("@password", passTextBox.Text);

                com.ExecuteNonQuery();
                Response.Redirect("admin.aspx");
                Response.Write("Registration Successful");

                conn.Close();

            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.ToString());
            }
        }

        protected void Button_showAdd_Click(object sender, EventArgs e)
        {
            Panel_addUser.Visible = true;
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("Login.aspx");
        }

        protected void Button_Settings_Click(object sender, EventArgs e)
        {
            Response.Redirect("schedule.aspx");
        }

        protected void Button_buttonHide_Click(object sender, EventArgs e)
        {
            Panel_addUser.Visible = false;
        }

        protected void Button_delete_Click(object sender, EventArgs e)
        {
            SqlCommand com;
            SqlCommand comm;
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string deleteQuery = "delete from UserData where userName = '" + TextBox_delete1.Text + "'";
            com = new SqlCommand(deleteQuery, conn);
            com.ExecuteNonQuery();
            Response.Redirect("admin.aspx");
            conn.Close();
        }

        protected void Button_hide_Click(object sender, EventArgs e)
        {
            Panel_delete.Visible = false;
        }

        protected void Button_showDelete_Click(object sender, EventArgs e)
        {
            Panel_delete.Visible = true;
        }

        protected void Button_showDelete_Click1(object sender, EventArgs e)
        {
            Panel_delete.Visible = true;
        }

        protected void Button_delete_Click1(object sender, EventArgs e)
        {
            SqlCommand com;
            SqlCommand comm;
            SqlConnection conn;
            /*conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            int check = 0;
            string tickFinder = "select count(*) from schedule where Instructor'" + TextBox_delete1.Text + "'";
            comm = new SqlCommand(tickFinder, conn);
            int temp = Convert.ToInt32(comm.ExecuteScalar().ToString());

            if (temp == 1)
            {
                Label_deleteError.Text = "Error: User currently has classes";
            }
            else
                check = 1;*/
            int check = 1;

            if (check == 1)
            {

                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string deleteQuery = "delete from UserData where userName = '" + TextBox_delete1.Text + "'";
                com = new SqlCommand(deleteQuery, conn);
                com.ExecuteNonQuery();
                Response.Redirect("admin.aspx");
                conn.Close();
            }
        }
    }
}