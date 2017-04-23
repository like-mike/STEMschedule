using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Configuration;


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
        GridView GridView_hidden = new GridView();


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


            command = new MySqlCommand("SELECT * from schedule where public = 1", connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView_hidden.DataSource = table;
            GridView_hidden.DataBind();
            connection.Close();

            connection.Open();
            using (var cmd = new MySqlCommand("SELECT * FROM department", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        DropDownList_ShowDept.DataSource = reader;
                        DropDownList_ShowDept.DataValueField = "department";
                        DropDownList_ShowDept.DataTextField = "department";
                        DropDownList_ShowDept.DataBind();
                    }
                }
            }
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

        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                             .ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                              .ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            //Bind Data to GridView
            GridView3.Caption = Path.GetFileName(FilePath);
            GridView3.DataSource = dt;
            GridView3.DataBind();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
            }
        }

        protected void button_Save_Click(object sender, EventArgs e)
        {
            int rows = GridView1.Rows.Count;


            string CRN;
            string Faculty;
            string ClassNum;
            string Days;
            string StartTime;
            string EndTime;
            string Term;
            string Room;
            string EnrollNum;
            string Year, Credits;
            string M1, M2, M3, M4;
            for (int i = 0; i < rows; i++)
            {

                try
                {
                    CRN = GridView3.Rows[i].Cells[0].Text;
                    Faculty = GridView3.Rows[i].Cells[1].Text;
                    ClassNum = GridView3.Rows[i].Cells[2].Text;
                    Days = GridView3.Rows[i].Cells[3].Text;
                    StartTime = GridView3.Rows[i].Cells[4].Text;
                    EndTime = GridView3.Rows[i].Cells[5].Text;
                    Term = GridView3.Rows[i].Cells[6].Text;
                    Room = GridView3.Rows[i].Cells[7].Text;
                    EnrollNum = GridView3.Rows[i].Cells[8].Text;
                    Year = GridView3.Rows[i].Cells[9].Text;
                    M1 = GridView3.Rows[i].Cells[10].Text;
                    M2 = GridView3.Rows[i].Cells[11].Text;
                    M3 = GridView3.Rows[i].Cells[12].Text;
                    M4 = GridView3.Rows[i].Cells[13].Text;
                    Credits = GridView3.Rows[i].Cells[14].Text;


                    connection.Open();
                    //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                    string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Conflict,Public) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Conflict,@Public)";
                    command = new MySqlCommand(insertQuery, connection);

                    //string instructor = Session["New"].ToString();

                    command.Parameters.AddWithValue("@CRN", CRN);
                    command.Parameters.AddWithValue("@Faculty", Faculty);
                    command.Parameters.AddWithValue("@ClassNum", ClassNum);
                    command.Parameters.AddWithValue("@Days", Days);
                    command.Parameters.AddWithValue("@StartTime", StartTime);
                    command.Parameters.AddWithValue("@EndTime", EndTime);
                    command.Parameters.AddWithValue("@Term", Term);
                    command.Parameters.AddWithValue("@Room", Room);
                    command.Parameters.AddWithValue("@EnrollNum", EnrollNum);
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@M1", M1);
                    command.Parameters.AddWithValue("@M2", M2);
                    command.Parameters.AddWithValue("@M3", M3);
                    command.Parameters.AddWithValue("@M4", M4);
                    command.Parameters.AddWithValue("@Credits", Credits);
                    command.Parameters.AddWithValue("@Conflict", 0);//conflicts
                    command.Parameters.AddWithValue("@Public", 0);//conflicts
                    command.ExecuteNonQuery();
                    //Response.Redirect("main.aspx");
                    //Response.Write("Add Class Success");
                }
                catch
                {

                }


            }
        }

        protected void Button_changePrivate_Click(object sender, EventArgs e)
        {
            string command = "UPDATE schedule SET PUBLIC = 0 WHERE CRN = " + GridView1.SelectedRow.Cells[1].Text;
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(command, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("main.aspx");
        }

        protected void DropDownList_ShowDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            String department = DropDownList_ShowDept.SelectedValue.ToString();
            connection.Open();
            command = new MySqlCommand("SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits FROM schedule", connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView1.DataSource = table;
            GridView1.DataBind();
            connection.Close();
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "STEMschedule" + DateTime.Now + ".xlsx";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GridView_hidden.GridLines = GridLines.Both;
            GridView_hidden.HeaderStyle.Font.Bold = true;
            GridView_hidden.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void Button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "DELETE from SCHEDULE WHERE CRN = " + GridView2.SelectedRow.Cells[1].Text;
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                Response.Redirect("main.aspx");
            }
            catch { }
        }
    }
}