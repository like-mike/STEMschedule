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
    public partial class settings : System.Web.UI.Page
    {
        // "#defines"
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public const string PUBLIC_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits FROM schedule WHERE Public = 1";
        public const string PRIVATE_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits FROM schedule WHERE Public = 0";

        // global variables
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;

        enum timeConflict { None, Room, Faculty, Year, Major_1, Major_2, Major_3, Major_4 };

        enum yearTypicallyTaken { Freshman, Sophomore, Junior, Senior, Multiple };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["New"] == null)
            {
                Response.Redirect("start.aspx");
            }

            // public schedule
            connection.Open();
            command = new MySqlCommand("select department from department", connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView_departments.DataSource = table;
            GridView_departments.DataBind();
            connection.Close();
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("start.aspx");
        }


        protected void Button_addDepartment_Click1(object sender, EventArgs e)
        {
            connection.Open();
      
            string tickFinder = "select count(*) from department";
            
            MySqlCommand comm = new MySqlCommand(tickFinder, connection);
            int temp = Convert.ToInt32(comm.ExecuteScalar().ToString());
            
            
            string insertQuery = "insert into department (iddepartment,department) values (@id,@dept)";

            MySqlCommand com = new MySqlCommand(insertQuery, connection);
            String id = temp.ToString();
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@dept", "test");
            

            com.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("settings.aspx");
            Response.Write("Add Class Success");
        }
    }
}