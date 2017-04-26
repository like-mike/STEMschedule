using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;

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
        public const int TERM_COLUMN = 7;
        public const int ROOM_COLUMN = 8;
        public const int YEAR_COLUMN = 10;
        public const int M1_COLUMN = 11;
        public const int M2_COLUMN = 12;
        public const int M3_COLUMN = 13;
        public const int M4_COLUMN = 14;
        public const int CONFLICT_COLUMN = 16;

        // global variables
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;

        enum timeConflict { None, Major_4, Major_3, Major_2, Major_1, Year, Faculty, Room };

        enum term { Autumn, Winter, Spring, Summer };

        enum yearTypicallyTaken { Freshman, Sophomore, Junior, Senior, Multiple };

        void sendSqlCommand(string sqlCommand)
        {
            try
            {
                connection.Open();
                command = new MySqlCommand(sqlCommand, connection);
                int numRowsUpdated = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                int errorcode = ex.Number;
            }
            finally
            {
                connection.Close();
            }
        }

        DateTime getTime(string textTime)
        {
            int hour = Int32.Parse(textTime.Substring(0, 2));
            int min = Int32.Parse(textTime.Substring(3, 2));
            if (textTime.Substring(6, 2).ToUpper() == "PM")
                hour += 12;
            var time = new DateTime(1988, 10, 13, hour, min, 0);
            return time;
        }

        void detectTimeConflict(int possibleConflict, GridViewRow newRow, GridViewRow oldRow)
        {
            int oldConflict = 0;
            int newConflict = 0;
            var newStartTime = getTime(newRow.Cells[START_COLUMN].Text);
            var newEndTime = getTime(newRow.Cells[END_COLUMN].Text);
            var oldStartTime = getTime(oldRow.Cells[START_COLUMN].Text);
            var oldEndTime = getTime(oldRow.Cells[END_COLUMN].Text);
            if (newStartTime < oldEndTime || newEndTime < oldStartTime)
            {
                int.TryParse(newRow.Cells[CONFLICT_COLUMN].Text, out newConflict);
                int.TryParse(oldRow.Cells[CONFLICT_COLUMN].Text, out oldConflict);
                if (possibleConflict > newConflict)
                    sendSqlCommand("UPDATE schedule SET conflict = " + possibleConflict + " WHERE CRN =" + newRow.Cells[CRN_COLUMN].Text + ";");
                if (possibleConflict > oldConflict)
                    sendSqlCommand("UPDATE schedule SET conflict = " + possibleConflict + " WHERE CRN =" + oldRow.Cells[CRN_COLUMN].Text + ";");
            }
        }

        void detectConflict(GridViewRow newRow)
        {
            foreach (GridViewRow oldRow in GridView1.Rows)
            {
                if (newRow.Cells[TERM_COLUMN].Text == oldRow.Cells[TERM_COLUMN].Text)
                {
                    if (newRow.Cells[ROOM_COLUMN].Text == oldRow.Cells[ROOM_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Room, newRow, oldRow);
                    else if (newRow.Cells[FACUTLY_COLUMN].Text == oldRow.Cells[FACUTLY_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Faculty, newRow, oldRow);
                    else if (newRow.Cells[YEAR_COLUMN].Text == oldRow.Cells[YEAR_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Year, newRow, oldRow);
                    else if (newRow.Cells[M1_COLUMN].Text == oldRow.Cells[M1_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Major_1, newRow, oldRow);
                    else if (newRow.Cells[M2_COLUMN].Text == oldRow.Cells[M2_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Major_2, newRow, oldRow);
                    else if (newRow.Cells[M3_COLUMN].Text == oldRow.Cells[M3_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Major_3, newRow, oldRow);
                    else if (newRow.Cells[M4_COLUMN].Text == oldRow.Cells[M4_COLUMN].Text)
                        detectTimeConflict((int)timeConflict.Major_4, newRow, oldRow);
                }
            }
            Response.Redirect("main.aspx");
        }

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
            foreach (GridViewRow privateRow in GridView2.Rows)
            {
                if (privateRow.RowIndex == GridView2.SelectedIndex)
                {
                    detectConflict(privateRow);
                    sendSqlCommand("UPDATE schedule SET public = 1 WHERE CRN =" + privateRow.Cells[CRN_COLUMN].Text + ";");
                    Response.Redirect("main.aspx");
                }
            }
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