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
            command = new MySqlCommand("select room from classrooms", connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView_room.DataSource = table;
            GridView_room.DataBind();
            


            // public schedule
            
            command = new MySqlCommand("select department from department", connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView_departments.DataSource = table;
            GridView_departments.DataBind();
            connection.Close();


            if (!IsPostBack)
            {
                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM department", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                CheckBoxList_majors.DataSource = reader;
                                CheckBoxList_majors.DataValueField = "department";
                                CheckBoxList_majors.DataTextField = "department";
                                CheckBoxList_majors.DataBind();
                            }
                        }
                    }


                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }
            }
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

        protected void Button_addRoom_Click(object sender, EventArgs e)
        {
            connection.Open();

            string tickFinder = "select count(*) from classrooms";

            MySqlCommand comm = new MySqlCommand(tickFinder, connection);
            int temp = Convert.ToInt32(comm.ExecuteScalar().ToString());


            string insertQuery = "insert into classrooms (classroomsid,room) values (@id,@room)";

            MySqlCommand com = new MySqlCommand(insertQuery, connection);
            String id = temp.ToString();
            com.Parameters.AddWithValue("@id", id);
            com.Parameters.AddWithValue("@room", TextBox_Room.Text);


            com.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("settings.aspx");
           
        }

        protected void GridView_room_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        

        protected void Button_addClass_Click1(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                string insertQuery = "insert into classes (name,M1,M2,M3,M4) values (@name,@M1,@M2,@M3,@M4)";
                command = new MySqlCommand(insertQuery, connection);

                string[] arr = new string[4];
                int j = 0;
                for (int i = 0; i < CheckBoxList_majors.Items.Count; i++)
                {
                    if (CheckBoxList_majors.Items[i].Selected)
                    {
                        arr[j] = CheckBoxList_majors.Items[i].Value;
                        j++;
                    }
                          
                }
                


                //string instructor = Session["New"].ToString();
                command.Parameters.AddWithValue("@name", TextBox_className.Text);
                command.Parameters.AddWithValue("@M1", arr[0]);
                command.Parameters.AddWithValue("@M2", arr[1]);
                command.Parameters.AddWithValue("@M3", arr[2]);
                command.Parameters.AddWithValue("@M4", arr[3]);

                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                //GridView1.DataSource = table;
                //GridView1.DataBind();
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Add Class Success!')" +
                         "</script>");



            }
            catch (Exception ex)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Add Class Error')" +
                         "</script>");
                Response.Write(ex);
            }
            finally { connection.Close(); }
        }
    }
}