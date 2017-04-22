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

            //activecell.EntireRow.select();
            //row.RowIndex == GridView2.SelectedIndex
            connection.Open();
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (row.RowIndex == GridView2.SelectedIndex)
                {
                   // string CRN = row.Rows[GridView2.SelectedIndex].Cells['index of loannumber column in datagridview'].Value;

                }
            }
                command = new MySqlCommand("UPDATE schedule SET public = 1 WHERE public = 0", connection);
            int numRowsUpdated = command.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("main.aspx");
            /*SELECT*
           FROM mytable a
           JOIN mytable b on a.starttime <= b.endtime
           and a.endtime >= b.starttime
           and a.name != b.name;*/

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

            /*create trigger mytable_no_overlap
            before insert on mytable
            for each row
            begin
                if exists(select * from mytable
                        where starttime <= new.endtime
                        and endtime >= new.starttime) then
                    signal sqlstate '45000' SET MESSAGE_TEXT = 'Overlaps with existing data';
                end if;
            end;*/
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