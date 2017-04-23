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
        // "#defines"
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public const string PUBLIC_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits, Conflict FROM schedule WHERE Public = 1";
        public const string PRIVATE_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits, Conflict FROM schedule WHERE Public = 0";

        public const int CRN_COLUMN = 1;
        public const int FACUTLY_COLUMN = 2;
        public const int START_COLUMN = 5;
        public const int END_COLUMN = 6;
        public const int TERM_COLUMN = 6;
        public const int ROOM_COLUMN = 8;
        public const int YEAR_COLUMN = 10;
        public const int M1_COLUMN = 11;
        public const int M2_COLUMN = 12;
        public const int M3_COLUMN = 13;
        public const int M4_COLUMN = 14;

        // global variables
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;

        enum timeConflict { None, Room, Faculty, Year, Major_1, Major_2, Major_3, Major_4 };

        enum yearTypicallyTaken { Freshman, Sophomore, Junior, Senior, Multiple };

        protected void Page_Load(object sender, EventArgs e)
        {
            // public schedule
            connection.Open();
            command = new MySqlCommand(PUBLIC_SCHEDULE, connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView1.DataSource = table;
            GridView1.DataBind();

            //private schedule
            command = new MySqlCommand(PRIVATE_SCHEDULE, connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView2.DataSource = table;
            GridView2.DataBind();
            connection.Close();
        }

        private void Initialize()
        {

        }

        protected void Button_Push_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells)
            //{
            //    if (oneCell.Selected)
            //    {
            //        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
            //        int loannumber = dataGridView1.Rows[oneCell.RowIndex].Cells['index of loannumber column in datagridview'].Value; // assuming loannmber is integer
            //        string username = dataGridView1.Rows[oneCell.RowIndex].Cells['index of username column in datagridview'].Value; // assuming username is string
            /* Now create an object of MySqlConnection and MySqlCommand
             * and the execute following query
             */
            //        string query = string.Format("DELETE FROM table_name WHERE loannumber = {0} AND username = '{1}'", loannumber, username);
            //    }
            //}
            connection.Open();
            foreach (GridViewRow privateRow in GridView2.Rows)
            {
                if (privateRow.RowIndex == GridView2.SelectedIndex)
                {
                    GridViewRow row = GridView2.SelectedRow;
                    command = new MySqlCommand("UPDATE schedule SET public = 1 WHERE CRN =" + row.Cells[CRN_COLUMN].Text + ";", connection);
                    int numRowsUpdated = command.ExecuteNonQuery();
                    command = new MySqlCommand("UPDATE schedule AS schedule INNER JOIN schedule AS s1 ON schedule.CRN <> s1.CRN set schedule.conflict = CASE"
                        + " WHEN schedule.Room = s1.Room THEN " + (int)timeConflict.Room
                        + " WHEN schedule.Faculty = s1.Faculty THEN " + (int)timeConflict.Faculty
                        + " WHEN schedule.Year = s1.Year THEN " + (int)timeConflict.Year
                        + " WHEN schedule.M1 = s1.M1 THEN " + (int)timeConflict.Major_1
                        + " WHEN schedule.M2 = s1.M2 THEN " + (int)timeConflict.Major_2
                        + " WHEN schedule.M3 = s1.M3 THEN " + (int)timeConflict.Major_3
                        + " WHEN schedule.M4 = s1.M4 THEN " + (int)timeConflict.Major_4
                        + " ELSE " + (int)timeConflict.None
                        + " END"
                        + " WHERE (schedule.StartTime <= s1.EndTime AND schedule.EndTime >= s1.StartTime) AND (schedule.Public = 1 AND s1.Public = 1)", connection);
                    numRowsUpdated = command.ExecuteNonQuery();
                }
            }
            connection.Close();
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
            connection.Open();
            //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Conflict,Public) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Conflict,@Public)";
            command = new MySqlCommand(insertQuery, connection);

            //string instructor = Session["New"].ToString();

            command.Parameters.AddWithValue("@CRN", TextBox_CRN.Text);
            command.Parameters.AddWithValue("@Faculty", TextBox_Faculty.Text);
            command.Parameters.AddWithValue("@ClassNum", TextBox_Class.Text);
            command.Parameters.AddWithValue("@Days", TextBox_Days.Text);
            command.Parameters.AddWithValue("@StartTime", TextBox_StartTime.Text);
            command.Parameters.AddWithValue("@EndTime", TextBox_EndTime.Text);
            command.Parameters.AddWithValue("@Term", TextBox_Term.Text);
            command.Parameters.AddWithValue("@Room", TextBox_Classroom.Text);
            command.Parameters.AddWithValue("@EnrollNum", TextBox_Enrollment.Text);
            command.Parameters.AddWithValue("@Year", TextBox_YearTaken.Text);
            command.Parameters.AddWithValue("@M1", TextBox_M1.Text);
            command.Parameters.AddWithValue("@M2", TextBox_Major2.Text);
            command.Parameters.AddWithValue("@M3", TextBox_Major3.Text);
            command.Parameters.AddWithValue("@M4", TextBox_Major4.Text);
            command.Parameters.AddWithValue("@Credits", TextBox_Credits.Text);
            command.Parameters.AddWithValue("@Conflict", 0);//conflicts
            command.Parameters.AddWithValue("@Public", 0);//conflicts
            command.ExecuteNonQuery();
            Response.Redirect("main.aspx");
            Response.Write("Add Class Success");

            connection.Close();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowIndex == GridView1.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (row.RowIndex == GridView2.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }
    }
}