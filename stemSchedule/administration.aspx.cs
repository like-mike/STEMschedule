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
    public partial class administation : System.Web.UI.Page
    {
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["New"] != null)
            {
                //label_welcome.Text += Session["New"].ToString();

            }
            else
                Response.Redirect("Start.aspx");




            try
            { // user data
                connection.Open();
                command = new MySqlCommand("SELECT UserName FROM USERDATA", connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView_users.DataSource = table;
                GridView_users.DataBind();

            }
            catch (Exception ex) { }
            finally { connection.Close(); }
        }

        protected void submitButton_Click(object sender, EventArgs e)
        {

        }

        protected void Button_ShowChgPass(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            Panel3.Visible = true;
        }

        protected void Button_buttonHide_Click(object sender, EventArgs e)
        {
            Panel_addUser.Visible = false;
        }
        

        protected void Button_ShowUser(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel1.Visible = false;
        }

        protected void Button_showDeleteUser(object sender, EventArgs e)
        {
            
        }


        protected void Button_showAddUser(object sender, EventArgs e)
        {
            Panel_addUser.Visible = true;
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("start.aspx");
        }

        protected void Button_chgPassHide_Click(object sender, EventArgs e)
        {
            Panel3.Visible = false;
        }
    }
}