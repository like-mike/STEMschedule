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
using System.Globalization;

namespace stemSchedule
{
    public partial class main : System.Web.UI.Page
    {
        // "#defines"
        
    
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public const string PUBLIC_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits, Conflict, ConflictCRN FROM schedule WHERE Public = 1";
        //public const string PUBLIC_SCHEDULE = "SELECT * FROM schedule WHERE Public = 1";
        public const string PRIVATE_SCHEDULE = "SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits, Conflict, ConflictCRN FROM schedule WHERE Public = 0";
        public bool G1Selected = false;
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
        public const int CREDITS_COLUMN = 15;
        public const int CONFLICT_COLUMN = 16;
        public const int CONFLICT_CRN_COLUMN = 17;

        // global variables
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;

        enum timeConflict { None, Major_4, Major_3, Major_2, Major_1, Year, Faculty, Room };

        enum term { Autumn, Winter, Spring, Summer };

        enum yearTypicallyTaken { Freshman, Sophomore, Junior, Senior, Multiple };
        GridView GridView_hidden = new GridView();

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
                Response.Write(ex);
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
                {
                    sendSqlCommand("UPDATE schedule SET conflict = " + possibleConflict + " WHERE CRN =" + newRow.Cells[CRN_COLUMN].Text + ";");
                    sendSqlCommand("UPDATE schedule SET ConflictCRN = " + oldRow.Cells[CRN_COLUMN].Text + " WHERE CRN = " + newRow.Cells[CRN_COLUMN].Text + "; ");
                }
                if (possibleConflict > oldConflict)
                {
                    sendSqlCommand("UPDATE schedule SET conflict = " + possibleConflict + " WHERE CRN =" + oldRow.Cells[CRN_COLUMN].Text + ";");
                    sendSqlCommand("UPDATE schedule SET ConflictCRN = " + newRow.Cells[CRN_COLUMN].Text + " WHERE CRN = " + oldRow.Cells[CRN_COLUMN].Text + "; ");
                }
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
            if (!IsPostBack)
            {
                if (Session["New"] != null)
                {
                    //label_welcome.Text += Session["New"].ToString();

                }
                else
                    Response.Redirect("Start.aspx");
                try
                { // public schedule
                    connection.Open();
                    command = new MySqlCommand(PUBLIC_SCHEDULE, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();

                }
                catch (Exception ex) { }
                finally { connection.Close(); }

                try
                { //private schedule
                    connection.Open();
                    command = new MySqlCommand(PRIVATE_SCHEDULE, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView2.DataSource = table;
                    GridView2.DataBind();
                    connection.Close();
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }
                try
                {
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
                    //Add blank item at index 0.
                    DropDownList_ShowDept.Items.Insert(0, new ListItem("", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }

                

                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM classes", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DropDownList_class.DataSource = reader;
                                DropDownList_class.DataValueField = "name";
                                DropDownList_class.DataTextField = "name";
                                DropDownList_class.DataBind();
                            }
                        }
                    }
                    //Add blank item at index 0.
                    //DropDownList_M1.Items.Insert(0, new ListItem("", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }

                
                

                

            }
            
        }

        private void Initialize()
        {

        }

        protected void Button_Push_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (row.RowIndex == GridView2.SelectedIndex)
                {
                    sendSqlCommand("UPDATE schedule SET public = 1 WHERE CRN =" + row.Cells[CRN_COLUMN].Text + ";");

                    detectConflict(row);

                    Response.Redirect("main.aspx");
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string conflictCRN;
            e.Row.Cells[CRN_COLUMN].Visible = false;
            e.Row.Cells[CONFLICT_COLUMN].Visible = false;
            e.Row.Cells[CONFLICT_CRN_COLUMN].Visible = false;

            if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Room)
            {


            }

            if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Room)
            {
                e.Row.BackColor = System.Drawing.Color.LightPink;
                e.Row.ToolTip = timeConflict.Room.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Faculty)
            {
                e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                e.Row.ToolTip = timeConflict.Faculty.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Year)
            {
                e.Row.BackColor = System.Drawing.Color.LightYellow;
                e.Row.ToolTip = timeConflict.Year.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Major_1)
            {
                e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
                e.Row.ToolTip = timeConflict.Major_1.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Major_2)
            {
                e.Row.BackColor = System.Drawing.Color.LightSkyBlue;
                e.Row.ToolTip = timeConflict.Major_2.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Major_3)
            {
                e.Row.BackColor = System.Drawing.Color.LightCyan;
                e.Row.ToolTip = timeConflict.Major_3.ToString() + " Conflict";
            }
            else if (Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "Conflict")) == (int)timeConflict.Major_3)
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.ToolTip = timeConflict.Major_4.ToString() + " Conflict";
            }
        }

        protected void Button_AddClass_Click1(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Conflict,Public) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Conflict,@Public)";
                command = new MySqlCommand(insertQuery, connection);

                //string instructor = Session["New"].ToString();
                string sclass = DropDownList_class.SelectedValue.ToString();
                command.Parameters.AddWithValue("@CRN", TextBox_CRN.Text);
                command.Parameters.AddWithValue("@Faculty", TextBox_Faculty.Text);
                command.Parameters.AddWithValue("@ClassNum", sclass);
                command.Parameters.AddWithValue("@Days", TextBox_Days.Text);
                command.Parameters.AddWithValue("@StartTime", TextBox_StartTime.Text);
                command.Parameters.AddWithValue("@EndTime", TextBox_EndTime.Text);
                command.Parameters.AddWithValue("@Term", DropDownList_term.SelectedValue);
                command.Parameters.AddWithValue("@Room", TextBox_Classroom.Text);
                command.Parameters.AddWithValue("@EnrollNum", TextBox_Enrollment.Text);
                command.Parameters.AddWithValue("@Year", DropDownList_year.SelectedValue);

                
                string M1_com = "select M1 from classes where name='" + sclass + "'";
                MySqlCommand com = new MySqlCommand(M1_com, connection);
                string M1 = com.ExecuteScalar().ToString();

                string M2_com = "select M2 from classes where name='" + sclass + "'";
                com = new MySqlCommand(M2_com, connection);
                string M2 = com.ExecuteScalar().ToString();

                string M3_com = "select M3 from classes where name='" + sclass + "'";
                com = new MySqlCommand(M3_com, connection);
                string M3 = com.ExecuteScalar().ToString();

                string M4_com = "select M4 from classes where name='" + sclass + "'";
                com = new MySqlCommand(M4_com, connection);
                string M4 = com.ExecuteScalar().ToString();


                command.Parameters.AddWithValue("@M1", M1);
                command.Parameters.AddWithValue("@M2", M2);
                command.Parameters.AddWithValue("@M3", M3);
                command.Parameters.AddWithValue("@M4", M4);

                command.Parameters.AddWithValue("@Credits", TextBox_Credits.Text);
                command.Parameters.AddWithValue("@Conflict", 0);//conflicts
                command.Parameters.AddWithValue("@Public", 0);//conflicts
                //command.ExecuteNonQuery();
                //Response.Redirect("main.aspx");
                //Response.Write("Add Class Success");
                
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();
                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Add Class Success!')" +
    "</script>"
  );



            }
            catch (Exception ex) {
                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Input Error!')" +
    "</script>"
  );
                Response.Write(ex);
            }
            finally { connection.Close(); }
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
            /*if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = Server.MapPath(FolderPath + FileName);
                FileUpload1.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension, rbHDR.SelectedItem.Text);
            }*/
            HttpPostedFile file = Request.Files["myFile"];

            //check file was submitted
            if (file != null && file.ContentLength > 0)
            {
                string fname = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", fname)));
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
                catch (Exception ex) {
                    
                }
                finally
                {
                    connection.Close();
                }


            }
        }

        protected void Button_changePrivate_Click(object sender, EventArgs e)
        {
            string command = "UPDATE schedule SET PUBLIC = 0 WHERE CRN = " + GridView1.SelectedRow.Cells[1].Text;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex) { Response.Write(ex); }
            finally
            {
                connection.Close();
                Response.Redirect("main.aspx");
            }
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("start.aspx");
        }

        protected void DropDownList_ShowDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            String department = DropDownList_ShowDept.SelectedValue.ToString();
            try
            {
                connection.Open();
                command = new MySqlCommand("SELECT CRN, Faculty, ClassNum, Days, TIME_FORMAT(StartTime, '%h:%i %p') StartTime, TIME_FORMAT(EndTime, '%h:%i %p') EndTime, Term, Room, EnrollNum, Year, M1, M2, M3, M4, Credits FROM schedule", connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();

            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }
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

            }
            catch (Exception ex) { Response.Write(ex); }
            finally
            {
                connection.Close();
                Response.Redirect("main.aspx");
            }
        }

        




        protected void Button_ApplySort_Click(object sender, EventArgs e)
        {
            string update = "Select * from SCHEDULE";
            string dept = "";
            bool where = false;
            string showing = "Showing: ";

           


            if(TextBox_searchCRN.Text != "")
            {
                string CRN = TextBox_searchCRN.Text;
                update += " WHERE CRN = " + CRN;
                where = true;
                showing += "CRN = " + CRN + " ";
            }

            if (RadioButtonList_ShowClasses.SelectedIndex == 1 && TextBox_searchCRN.Text == "")
            {

                if (!where)
                {
                    update += " WHERE User = '" + Session["New"].ToString() + "'";
                    where = true;
                }
                else
                {
                    update += "AND User = '" + Session["New"].ToString() + "'";
                }
                showing += "Only My Classes ";
            }
            
            else if (RadioButtonList_ShowClasses.SelectedIndex == 2 && TextBox_searchCRN.Text == "")
            {
                update += " WHERE Conflict != '0'";
                where = true;
                showing += "All Classes with Conflicts ";
            }

            else if(RadioButtonList_ShowClasses.SelectedIndex == 0)
            {
                Label_showSearch.Visible = false;
            }

            if (DropDownList_ShowDept.SelectedIndex != 0 && TextBox_searchCRN.Text == "")
            {
                dept = DropDownList_ShowDept.SelectedValue.ToString();
                if (!where)
                {
                    update += " WHERE (M1 = '" + dept + "' OR M2 = '" + dept + "' OR M3 = '" + dept + "' OR M4 = '" + dept + "')";
                    where = true;
                }
                else
                {
                    update += " AND (M1 = '" + dept + "' OR M2 = '" + dept + "' OR M3 = '" + dept + "' OR M4 = '" + dept + "')";
                }
                
                showing += "Major = " + dept + " ";
            }

            if (DropDownList_ShowYear.SelectedIndex != 0 && TextBox_searchCRN.Text == "")
            {
                if(!where)
                {
                    update += " WHERE YEAR = '" + DropDownList_ShowYear.SelectedValue.ToString() + "'";
                    where = true;
                }
                else
                    update += " AND YEAR = '" + DropDownList_ShowYear.SelectedValue.ToString() + "'";

                showing += "Year = " + DropDownList_ShowYear.SelectedItem.ToString() + " ";
            }

            try
            { // public schedule
                
                connection.Open();
                command = new MySqlCommand(update, connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();
                Label_showSearch.Text = showing;
                if(where)
                    Label_showSearch.Visible = true;
                else
                    Label_showSearch.Visible = false;



            }
            catch (Exception ex) {
                Response.Write(ex);
                
            }
            finally { connection.Close(); }
            //Response.Write(update);

        }

        

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[CRN_COLUMN].Visible = false;
            e.Row.Cells[CONFLICT_COLUMN].Visible = false;
            e.Row.Cells[CONFLICT_CRN_COLUMN].Visible = false;
        }
    }
}