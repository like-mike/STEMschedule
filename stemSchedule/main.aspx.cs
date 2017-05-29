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
        
        //select *,TIME_FORMAT(StartTime, '%h:%i %p')startTime,TIME_FORMAT(EndTime, '%h:%i %p')EndTime from schedule
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public string PUBLIC_SCHEDULE = "Select * FROM SCHEDULE WHERE PUBLIC = 1";
        public string PRIVATE_SCHEDULE = "SELECT * FROM SCHEDULE WHERE PUBLIC = 0";
       
        public bool G1Selected = false;
        

        // global variables
        public static MySqlConnection connection = new MySqlConnection(DB_CREDENTIALS);
        public static MySqlCommand command;
        public static DataTable table;
        public static MySqlDataAdapter data;
        public bool editing = false;

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





        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button3.UniqueID;
            if (!IsPostBack)
            {
                resetCheck();
                if (Session["New"] != null)
                {
                    //label_welcome.Text += Session["New"].ToString();
                    this.Form.DefaultButton = this.Button3.UniqueID;
                }
                else
                {
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_Login').openModal({dismissible: false });", true);
                }
                
                try
                { // public schedule
                    connection.Open();
                    command = new MySqlCommand("SELECT * FROM SCHEDULE WHERE Public = 1", connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();

                }
                catch (Exception ex) { }
                finally { connection.Close(); }


                try
                { // hidden schedule
                    connection.Open();
                    command = new MySqlCommand("SELECT * FROM SCHEDULE", connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1_Hidden.DataSource = table;
                    GridView1_Hidden.DataBind();

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
                    using (var cmd = new MySqlCommand("SELECT * FROM major order by major", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DropDownList_ShowDept.DataSource = reader;
                                DropDownList_ShowDept.DataValueField = "major";
                                DropDownList_ShowDept.DataTextField = "major";
                                DropDownList_ShowDept.DataBind();
                            }
                        }
                    }
                    //Add blank item at index 0.
                    DropDownList_ShowDept.Items.Insert(0, new ListItem("", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }



                






            }
            //string update = "Select * from SCHEDULE WHERE PUBLIC = 1";
            


        }

        private void Initialize()
        {

        }

        protected void Button_Push_Click(object sender, EventArgs e)
        {
            if (GridView2.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PRIVATE Class Selected')" +
                        "</script>");
            }
            else
            {
                foreach (GridViewRow row in GridView2.Rows)
                {
                    if (row.RowIndex == GridView2.SelectedIndex)
                    {
                        sendSqlCommand("UPDATE schedule SET public = 1 WHERE CRN =" + row.Cells[1].Text + ";");



                        Response.Redirect("main.aspx");
                    }
                }
            }

            
        }


        public void checkConflict(bool changeConflict, String CRN, String Faculty, String ClassNum, String Days, String StartTime, String EndTime, String Term, String Room, String M1, String M2, String M3, String M4, String Credits, String M, String T, String W, String Th, String F, String Sa, String Su, String Fr, String So, String Ju, String Se, String Year)
        {
            resetCheck();
            if (!changeConflict)
                connection.Open();
            //select * from schedule WHERE '9:30:00' >= startTime AND endTime >= '21:20:00' 
            string checkNum = "select count(*) from schedule WHERE '" + StartTime + "' >= (startTime)  AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "')";
            string roomQuery = "";
            string roomUpdate = "";

            //room conflict
            checkNum += " AND (room) = '" + Room + "'";
            MySqlCommand comm = new MySqlCommand(checkNum, connection);
            int roomConflicts = Convert.ToInt32(comm.ExecuteScalar().ToString());

            if (roomConflicts > 1)
            {
                roomQuery = "select * from schedule WHERE '" + StartTime + "' >= (startTime)  AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "') AND (Room) ='" + Room +"'";
                roomUpdate = "update schedule set conflict = 'YES', Rcon = '1' WHERE '" + StartTime + "' >= (startTime)  AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "') AND (Room) ='" + Room + "'";
                comm = new MySqlCommand(roomUpdate, connection);
                comm.ExecuteNonQuery();
            }
            



            //major conflict
            string majorQuery = "select * FROM SCHEDULE WHERE '" + StartTime + "' >= (startTime) AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "') AND TERM = '" + Term + "'";
            checkNum = "select count(*) from schedule WHERE '" + StartTime + "' >= (startTime)  AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "')";
            string updateQuery = "update schedule set conflict = 'YES', mCon = '1' WHERE '" + StartTime + "' >= (startTime) AND (endTime) >= '" + EndTime + "' AND (Public) = 1 AND ((M) = '" + M + "' OR (T) = '" + T + "' OR (W) = '" + W + "' OR (Th) = '" + Th + "' OR (F) = '" + F + "') AND TERM = '" + Term + "'";
            int[] majors = new int[4];
            string[] Mquery = new string[4];
            string countQuery = "select count(*) FROM SCHEDULE WHERE '" + StartTime + "' >= (startTime)AND(endTime) >= '" + EndTime + "' AND(Public) = 1 AND((M) = '" + M + "' OR(T) = '" + T + "' OR(W) = '" + W + "' OR(Th) = '" + Th + "' OR(F) = '" + F + "') AND TERM = '" + Term + "'";



            string checkMajor = majorQuery;
            //M1
            majors[0] = 0;
            Mquery[0] = " AND ((M1) = '" + M1 + "' OR (M2) = '" + M1 + "' OR (M3) = '" + M1 + "' OR (M4) = '" + M1 + "')";
            string checkM1 = checkNum + Mquery[0];
            comm = new MySqlCommand(checkM1, connection);
            majors[0] = Convert.ToInt32(comm.ExecuteScalar().ToString());

            //M2
            majors[1] = 0;
            Mquery[1] = " AND ((M1) = '" + M2 + "' OR (M2) = '" + M2 + "' OR (M3) = '" + M2 + "' OR (M4) = '" + M2 + "')";
            string checkM2 = checkNum + Mquery[1];
            comm = new MySqlCommand(checkM2, connection);
            majors[1] = Convert.ToInt32(comm.ExecuteScalar().ToString());

            //M3
            majors[2] = 0;
            Mquery[2] = " AND ((M1) = '" + M3 + "' OR (M2) = '" + M3 + "' OR (M3) = '" + M3 + "' OR (M4) = '" + M3 + "')";
            string checkM3 = checkNum + Mquery[2];
            comm = new MySqlCommand(checkM3, connection);
            majors[2] = Convert.ToInt32(comm.ExecuteScalar().ToString());

            //M4
            majors[3] = 0;
            Mquery[3] = " AND ((M1) = '" + M4 + "' OR (M2) = '" + M4 + "' OR (M3) = '" + M4 + "' OR (M4) = '" + M4 + "')";
            string checkM4 = checkNum + Mquery[3];
            comm = new MySqlCommand(checkM4, connection);
            majors[3] = Convert.ToInt32(comm.ExecuteScalar().ToString());

            int majorConflictNum = 0;
            
            for(int i = 0; i < 4; i++)
            {
                if (majors[i] != 0)
                {
                    checkMajor += Mquery[i];
                    updateQuery += Mquery[i];
                    countQuery += Mquery[i];

                    majorConflictNum += majors[i];
                    //count++;
                }
                    

            }

            comm = new MySqlCommand(countQuery, connection);
            int count = Convert.ToInt32(comm.ExecuteScalar().ToString());

            if (count>1){
                command = new MySqlCommand(updateQuery, connection);
                command.ExecuteNonQuery();
            }



            bool conflicts = true;
            string finalQuery = "";
            if (roomConflicts > 1 && count > 1)
                finalQuery = roomQuery + " UNION " + checkMajor;
            else if (roomConflicts > 1 && count <2)
                finalQuery = roomQuery;
            else if (roomConflicts < 1 && count > 1)
                finalQuery = majorQuery;
            else
            {
                conflicts = false;
                finalQuery = "Select * from schedule WHERE PUBLIC = 1";//Ithink
            }
                


            command = new MySqlCommand(finalQuery, connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView1.DataSource = table;
            GridView1.DataBind();
            //GridView2.SelectedRow.Cells[24].Text;

            if (conflicts)
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.Cells[30].Text == "1" && row.Cells[31].Text == "1")
                    {
                        row.BackColor = ColorTranslator.FromHtml("#42cbf4");
                        row.ToolTip = "Room & Major Conflicts";
                    }
                    else if (row.Cells[30].Text == "1" && row.Cells[31].Text != "1")
                    {
                        row.BackColor = ColorTranslator.FromHtml("#f44165");
                        row.ToolTip = "Room Conflict";
                    }
                    else if (row.Cells[30].Text != "1" && row.Cells[31].Text == "1")
                    {
                        row.BackColor = ColorTranslator.FromHtml("#f4d641");
                        row.ToolTip = "Major Conflict";
                    }
                }
            }
            



                string conflictFound = "";
            



            




            
            Response.Write(finalQuery);


            Label_showSearch.Text = conflictFound;
            Label_showSearch.Visible = true;









            connection.Close();
        }



        protected void Button_AddClass_Click1(object sender, EventArgs e)
        {
            



            string CRN = CRN_Text.Value.ToString();
            string enrollment = Enrollment_Text.Value.ToString();
            string credits = Credits_Text.Value.ToString();
            string calYear = Year_Text.Value.ToString();


            try
            {
                string command = "DELETE from SCHEDULE WHERE CRN = " + GridView2.SelectedRow.Cells[1].Text;
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); editing = false; }

            try
            {
                connection.Open();
                //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Conflict,Public,M,T,W,Th,F,Sa,Su,Fr,So,Ju,Se,CalYear,User) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Conflict,@Public,@M,@T,@W,@Th,@F,@Sa,@Su,@Fr,@So,@Jr,@Se,@CalYear,@User)";
                command = new MySqlCommand(insertQuery, connection);
                //21
                //string instructor = Session["New"].ToString();
                string sclass = DropDownList_class.SelectedValue.ToString();

                string[] arr = new string[7];
                int j = 0;
                for (int i = 0; i < CheckBoxList_days.Items.Count; i++)
                {
                    if (CheckBoxList_days.Items[i].Selected)
                    {
                        arr[j] = CheckBoxList_days.Items[i].Value;
                        j++;
                    }

                }
                //Array.Sort(arr);
                string days = "";
                int x = 0;
                string M = "", T = "", W = "", Th = "", F = "", Sa = "", Su = "";
                for (int i = 0; i < j; i++)
                {
                    if (arr[i] == "1")
                    {
                        days += "M";
                        M = "1";
                    }
                    else if (arr[i] == "2")
                    {
                        days += "T";
                        T = "1";
                    }
                    else if (arr[i] == "3")
                    {
                        days += "W";
                        W = "1";
                    }
                    else if (arr[i] == "4")
                    {
                        days += "Th";
                        Th = "1";
                    }
                    else if (arr[i] == "5")
                    {
                        days += "F";
                        F = "1";
                    }
                    else if (arr[i] == "6")
                    {
                        days += "Sa";
                        Sa = "1";
                    }
                    else if (arr[i] == "7")
                    {
                        days += "Su";
                        Su = "1";
                    }
                }


                command.Parameters.AddWithValue("@M", M);
                command.Parameters.AddWithValue("@T", T);
                command.Parameters.AddWithValue("@W", W);
                command.Parameters.AddWithValue("@Th", Th);
                command.Parameters.AddWithValue("@F", F);
                command.Parameters.AddWithValue("@Sa", Sa);
                command.Parameters.AddWithValue("@Su", Su);

                //command.Parameters.AddWithValue("@PK", TextBox_CRN.Text);
                command.Parameters.AddWithValue("@CRN", CRN);
                command.Parameters.AddWithValue("@Faculty", DropDownList_instructor.SelectedValue);
                command.Parameters.AddWithValue("@ClassNum", DropDownList_class.SelectedValue);
                command.Parameters.AddWithValue("@Days", days);
                command.Parameters.AddWithValue("@CalYear", calYear);
                command.Parameters.AddWithValue("@User", Session["New"].ToString());
                
                command.Parameters.AddWithValue("@StartTime", TextBox_StartTime.Text);
                command.Parameters.AddWithValue("@EndTime", TextBox_EndTime.Text);
                command.Parameters.AddWithValue("@Term", DropDownList_term.SelectedValue);
                command.Parameters.AddWithValue("@Room", DropDownList_Classroom.SelectedValue);
                command.Parameters.AddWithValue("@EnrollNum", enrollment);




                string Fr_com = "select Fr from classes where name='" + DropDownList_class.SelectedValue + "'";
                MySqlCommand com = new MySqlCommand(Fr_com, connection);
                string fr = com.ExecuteScalar().ToString();

                string So_com = "select So from classes where name='" + DropDownList_class.SelectedValue + "'";
                com = new MySqlCommand(So_com, connection);
                string so = com.ExecuteScalar().ToString();

                string jr_com = "select Jr from classes where name='" + DropDownList_class.SelectedValue + "'";
                com = new MySqlCommand(jr_com, connection);
                string ju = com.ExecuteScalar().ToString();

                string sr_com = "select Sr from classes where name='" + DropDownList_class.SelectedValue + "'";
                com = new MySqlCommand(sr_com, connection);
                string se = com.ExecuteScalar().ToString();


                command.Parameters.AddWithValue("@Fr", fr);
                command.Parameters.AddWithValue("@So", so);
                command.Parameters.AddWithValue("@Jr", ju);
                command.Parameters.AddWithValue("@Se", se);
                string year = "";
                if (fr == "1")
                    year += "Fr";
                if (so == "1")
                    year += "So";
                if (ju == "1")
                    year += "Jr";
                if (se == "1")
                    year += "Sr";
                command.Parameters.AddWithValue("@Year", year);



                string M1_com = "select M1 from classes where name='" + sclass + "'";
                com = new MySqlCommand(M1_com, connection);
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

                command.Parameters.AddWithValue("@Credits", credits);
                command.Parameters.AddWithValue("@Conflict", null);//conflicts
                command.Parameters.AddWithValue("@Public", 0);//conflicts
                                                              //command.ExecuteNonQuery();
                                                              //Response.Redirect("main.aspx");
                                                              //Response.Write("Add Class Success");

                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();


                command = new MySqlCommand(PRIVATE_SCHEDULE, connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView2.DataSource = table;
                GridView2.DataBind();


                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Add Class Success!')" +
    "</script>"
  );
                connection.Close();
                checkConflict(false, CRN, DropDownList_instructor.SelectedValue, DropDownList_class.SelectedValue, days, TextBox_StartTime.Text, TextBox_EndTime.Text, DropDownList_term.SelectedValue, DropDownList_Classroom.SelectedValue, M1, M2, M3, M4, credits, M, T, W, Th, F, Sa, Su, fr, so, ju, se,"2017");


            }
            catch (Exception ex)
            {
                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Input Error!')" +
    "</script>"
  );
                Response.Write(ex);

            }
            finally
            {
                connection.Close();

            }
            Label_AddClassDesc.Visible = false;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView2.Rows)
            {
                GridView2.SelectedIndex = -1;
                row.BackColor = ColorTranslator.FromHtml("");
            }

            foreach (GridViewRow row in GridView1.Rows)
            {
                row.BackColor = ColorTranslator.FromHtml("");
            }

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

            foreach (GridViewRow row in GridView1.Rows)
            {
                GridView1.SelectedIndex = -1;
                row.BackColor = ColorTranslator.FromHtml("");
            }

            foreach (GridViewRow row in GridView2.Rows)
            {
                row.BackColor = ColorTranslator.FromHtml("");
            }

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
            if (GridView1.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PUBLIC Class Selected')" +
                        "</script>");
            }
            else
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

            
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("main.aspx");
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
            string FileName = "STEMschedule" + DateTime.Now + ".xls";
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
            if (GridView2.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PRIVATE Class Selected')" +
                        "</script>");
            }
            else {
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

            

        }

        protected void Button_ShowAll_Click(object sender, EventArgs e)
        {
            string update = "Select * from SCHEDULE WHERE PUBLIC = 1";
            try
            { // public schedule

                connection.Open();
                command = new MySqlCommand(update, connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();
                Label_showSearch.Visible = false;
              


            }
            catch (Exception ex)
            {
                Response.Write(ex);

            }
            finally { connection.Close(); }
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
                if (!where)
                {
                    if (DropDownList_ShowYear.SelectedValue.ToString() == "1")
                    {
                        update += " WHERE FR = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "2")
                    {
                        update += " WHERE SO = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "3")
                    {
                        update += " WHERE JU = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "4")
                    {
                        update += " WHERE SE = '1'";
                    }
                    where = true;
                }
                else {
                    if (DropDownList_ShowYear.SelectedValue.ToString() == "1")
                    {
                        update += " AND FR = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "2")
                    {
                        update += " AND SO = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "3")
                    {
                        update += " AND JU = '1'";
                    }
                    else if (DropDownList_ShowYear.SelectedValue.ToString() == "4")
                    {
                        update += " AND SE = '1'";
                    }
                    
                }
                    

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

                PUBLIC_SCHEDULE = update;

            }
            catch (Exception ex) {
                Response.Write(ex);
                
            }
            finally { connection.Close(); }
            //Response.Write(update);

        }

        

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        public void resetCheck()
        {
            try
            {
                connection.Open();
                string updateQuery = "update schedule set Conflict = '',RCon='',MCon='' WHERE NOT CRN =''";

                command = new MySqlCommand(updateQuery, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex) { }
            finally
            {
                connection.Close();
            }
            String CRN, Faculty, ClassNum, startTime, endTime, term, room, M1, M2, M3, M4, M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se;
            //int i = 1;
            
            
            //checkConflict(false, CRN, Faculty, ClassNum, "", startTime, endTime, term, room, M1, M2, M3, M4, "", M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se, "2017");
            //Response.Write("test" + i);
        }

        protected void checkSpecific(object sender, EventArgs e)
        {
            if (GridView1.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PUBLIC Class Selected')" +
                        "</script>");
            }
            else
            {
                String CRN = GridView1.SelectedRow.Cells[1].Text;
                String Faculty = GridView1.SelectedRow.Cells[2].Text;
                String ClassNum = GridView1.SelectedRow.Cells[3].Text;
                String startTime = GridView1.SelectedRow.Cells[5].Text;
                String endTime = GridView1.SelectedRow.Cells[6].Text;
                String term = GridView1.SelectedRow.Cells[7].Text;
                String room = GridView1.SelectedRow.Cells[8].Text;
                String M1 = GridView1.SelectedRow.Cells[9].Text;
                String M2 = GridView1.SelectedRow.Cells[10].Text;
                String M3 = GridView1.SelectedRow.Cells[11].Text;
                String M4 = GridView1.SelectedRow.Cells[12].Text;
                String M = GridView1.SelectedRow.Cells[14].Text;
                String T = GridView1.SelectedRow.Cells[15].Text;
                String W = GridView1.SelectedRow.Cells[16].Text;
                String Th = GridView1.SelectedRow.Cells[17].Text;
                String F = GridView1.SelectedRow.Cells[18].Text;
                String Sa = GridView1.SelectedRow.Cells[19].Text;
                String Su = GridView1.SelectedRow.Cells[20].Text;
                String Fr = GridView1.SelectedRow.Cells[21].Text;
                String So = GridView1.SelectedRow.Cells[22].Text;
                String Jr = GridView1.SelectedRow.Cells[23].Text;
                String Se = GridView1.SelectedRow.Cells[24].Text;


                checkConflict(false, CRN, Faculty, ClassNum, "", startTime, endTime, term, room, M1, M2, M3, M4, "", M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se,"2017");
            }
            


        }


        protected void checkSpecificPrivate(object sender, EventArgs e)
        {
            if (GridView2.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PRIVATE Class Selected')" +
                        "</script>");
            }
            else
            {
                String CRN = GridView2.SelectedRow.Cells[1].Text;
                String Faculty = GridView2.SelectedRow.Cells[2].Text;
                String ClassNum = GridView2.SelectedRow.Cells[3].Text;
                String startTime = GridView2.SelectedRow.Cells[5].Text;
                String endTime = GridView2.SelectedRow.Cells[6].Text;
                String term = GridView2.SelectedRow.Cells[7].Text;
                String room = GridView2.SelectedRow.Cells[8].Text;
                String M1 = GridView2.SelectedRow.Cells[9].Text;
                String M2 = GridView2.SelectedRow.Cells[10].Text;
                String M3 = GridView2.SelectedRow.Cells[11].Text;
                String M4 = GridView2.SelectedRow.Cells[12].Text;
                String M = GridView2.SelectedRow.Cells[14].Text;
                String T = GridView2.SelectedRow.Cells[15].Text;
                String W = GridView2.SelectedRow.Cells[16].Text;
                String Th = GridView2.SelectedRow.Cells[17].Text;
                String F = GridView2.SelectedRow.Cells[18].Text;
                String Sa = GridView2.SelectedRow.Cells[19].Text;
                String Su = GridView2.SelectedRow.Cells[20].Text;
                String Fr = GridView2.SelectedRow.Cells[21].Text;
                String So = GridView2.SelectedRow.Cells[22].Text;
                String Jr = GridView2.SelectedRow.Cells[23].Text;
                String Se = GridView2.SelectedRow.Cells[24].Text;


                checkConflict(false, CRN, Faculty, ClassNum, "", startTime, endTime, term, room, M1, M2, M3, M4, "", M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se,"2017");
            }



        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
            for(int i = 13; i <= 24; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            for (int i = 13; i <= 24; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
        }

        protected void Button_AddUserShow_Click(object sender, EventArgs e)
        {
            //divControl.Attributes("sty") = "height:200px; color:Red"
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            
        }

        protected void Button_DeleteUserShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM UserData ORDER BY UserName ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteUser.DataSource = reader;
                            DropDownList_deleteUser.DataValueField = "UserName";
                            DropDownList_deleteUser.DataTextField = "UserName";
                            DropDownList_deleteUser.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteUser.Items.Insert(0, new ListItem("Select User Name", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteUser').openModal({ });", true);
        }

        protected void Button_ChangePwShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM UserData ORDER BY UserName ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_chgPass.DataSource = reader;
                            DropDownList_chgPass.DataValueField = "UserName";
                            DropDownList_chgPass.DataTextField = "UserName";
                            DropDownList_chgPass.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_chgPass.Items.Insert(0, new ListItem("Select User Name", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_chgPass').openModal({ });", true);
        }

        protected void Button_editSessionShow_Click(object sender, EventArgs e)
        {
            if (GridView2.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PRIVATE Class Selected')" +
                        "</script>");
            }
            else
            {
                String CRN = GridView2.SelectedRow.Cells[1].Text;
                String Faculty = GridView2.SelectedRow.Cells[2].Text;
                String ClassNum = GridView2.SelectedRow.Cells[3].Text;


                String startTime = GridView2.SelectedRow.Cells[5].Text;
                String endTime = GridView2.SelectedRow.Cells[6].Text;
                String term = GridView2.SelectedRow.Cells[7].Text;
                String room = GridView2.SelectedRow.Cells[8].Text;
                String enrollment = GridView2.SelectedRow.Cells[28].Text;
                String credits = GridView2.SelectedRow.Cells[27].Text;
                String calYear = GridView2.SelectedRow.Cells[32].Text;





                String M = GridView2.SelectedRow.Cells[14].Text;
                String T = GridView2.SelectedRow.Cells[15].Text;
                String W = GridView2.SelectedRow.Cells[16].Text;
                String Th = GridView2.SelectedRow.Cells[17].Text;
                String F = GridView2.SelectedRow.Cells[18].Text;
                String Sa = GridView2.SelectedRow.Cells[19].Text;
                String Su = GridView2.SelectedRow.Cells[20].Text;

                CRN_Text.Value = CRN;


                DropDownList_class.SelectedValue = ClassNum;
                if (DropDownList_class.SelectedValue != ClassNum)
                    Response.Write(
                           "<script type=\"text/javascript\">" +
                           "alert('" + ClassNum + " no longer exists. Please add class in Settings if you wish to use')" +
                           "</script>");


                DropDownList_term.SelectedValue = term;

                DropDownList_Classroom.SelectedValue = room;
                if (DropDownList_Classroom.SelectedValue != room)
                    Response.Write(
                           "<script type=\"text/javascript\">" +
                           "alert('" + room + " no longer exists. Please add Room in Settings if you wish to use')" +
                           "</script>");
                Enrollment_Text.Value = enrollment;
                Credits_Text.Value = credits;
                TextBox_StartTime.Text = startTime;
                TextBox_EndTime.Text = endTime;

                DropDownList_instructor.SelectedValue = Faculty;
                if (DropDownList_instructor.SelectedValue != Faculty)
                    Response.Write(
                           "<script type=\"text/javascript\">" +
                           "alert('" + Faculty + " no longer exists. Please add instructor in Settings if you wish to use')" +
                           "</script>");
                Year_Text.Value = calYear;

                if (M == "1")
                    SelectCheckBoxList("Monday");
                if (T == "1")
                    SelectCheckBoxList("Tuesday");
                if (W == "1")
                    SelectCheckBoxList("Wednesday");
                if (Th == "1")
                    SelectCheckBoxList("Thursday");
                if (F == "1")
                    SelectCheckBoxList("Friday");
                if (Sa == "1")
                    SelectCheckBoxList("Saturday");
                if (Su == "1")
                    SelectCheckBoxList("Sunday");






                editing = true;
                Label_AddClassDesc.Visible = true;



                /*foreach (GridViewRow row in GridView2.Rows)
                {
                    if (row.RowIndex == GridView2.SelectedIndex)
                    {
                        sendSqlCommand("UPDATE schedule SET public = 1 WHERE CRN =" + row.Cells[1].Text + ";");




                    }
                }*/




                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM classes ORDER BY name ASC", connection))
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
                    DropDownList_class.Items.Insert(0, new ListItem("Select Class", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }



                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM classrooms ORDER BY room ASC", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DropDownList_Classroom.DataSource = reader;
                                DropDownList_Classroom.DataValueField = "room";
                                DropDownList_Classroom.DataTextField = "room";
                                DropDownList_Classroom.DataBind();
                            }
                        }
                    }
                    //Add blank item at index 0.
                    DropDownList_Classroom.Items.Insert(0, new ListItem("Select Room", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }

                try
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand("SELECT * FROM instructor ORDER BY instructor ASC", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DropDownList_instructor.DataSource = reader;
                                DropDownList_instructor.DataValueField = "instructor";
                                DropDownList_instructor.DataTextField = "instructor";
                                DropDownList_instructor.DataBind();
                            }
                        }
                    }
                    //Add blank item at index 0.
                    DropDownList_instructor.Items.Insert(0, new ListItem("Select Instructor", ""));
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }




                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal1').openModal({ });", true);
            }
        }

        private void SelectCheckBoxList(string valueToSelect)
        {
            ListItem listItem = this.CheckBoxList_days.Items.FindByText(valueToSelect);

            if (listItem != null) listItem.Selected = true;
        }



        protected void Button_addSessionShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM classes ORDER BY name ASC", connection))
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
                DropDownList_class.Items.Insert(0, new ListItem("Select Class", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM classrooms ORDER BY room ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_Classroom.DataSource = reader;
                            DropDownList_Classroom.DataValueField = "room";
                            DropDownList_Classroom.DataTextField = "room";
                            DropDownList_Classroom.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_Classroom.Items.Insert(0, new ListItem("Select Room", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM instructor ORDER BY instructor ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_instructor.DataSource = reader;
                            DropDownList_instructor.DataValueField = "instructor";
                            DropDownList_instructor.DataTextField = "instructor";
                            DropDownList_instructor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_instructor.Items.Insert(0, new ListItem("Select Instructor", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }



            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal1').openModal({ });", true);
        }

        protected void Button_UserAdd_Click(object sender, EventArgs e)
        {
            string newUser = newUser_Text.Value.ToString();
            string confirmUser = confirmUser_Text.Value.ToString();
            string pass = newUPass_Text.Value.ToString();
            string confirmPass = confirmNewUPass_Text.Value.ToString();

            if(newUser != confirmUser)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('User Names Do Not Match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
            }
            else if(newUser == "")
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('User Name is Empty')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
            }
            else
            {
                if (pass != confirmPass)
                {
                    Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Passwords Do Not Match')" +
                        "</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string check = "Select count(*) from UserData where UserName = '" + newUser + "'";
                        MySqlCommand comm = new MySqlCommand(check, connection);
                        int existingUser = Convert.ToInt32(comm.ExecuteScalar().ToString());



                        if (existingUser != 0)
                        {
                            Response.Write(
                                    "<script type=\"text/javascript\">" +
                                    "alert('Existing User. Please Enter a New User Name')" +
                                    "</script>");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
                        }
                        else
                        {
                            
                            string insertQuery = "insert into userdata (userName,Password) values (@user,@pass)";

                            command = new MySqlCommand(insertQuery, connection);
                            command.Parameters.AddWithValue("@user", newUser);
                            command.Parameters.AddWithValue("@pass", pass);
                            command.ExecuteNonQuery();

                            Response.Write(
                                    "<script type=\"text/javascript\">" +
                                    "alert('Successfully Added New User!')" +
                                    "</script>");
                        }

                        
                    }
                    catch(Exception ex)
                    {
                        Response.Write(
                                    "<script type=\"text/javascript\">" +
                                    "alert('Error Adding New User. Please make sure valid username/password')" + 
                                    "</script>");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
                        Response.Write(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }

                    
                }



            }

            
        }

        protected void Button_deleteUser_Click(object sender, EventArgs e)
        {
            string userName = DropDownList_deleteUser.SelectedValue.ToString();
            string confirmUserName = confirmUserNameDelete_Text.Value.ToString();

            if(DropDownList_deleteUser.SelectedIndex == 0)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No user name selected')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteUser').openModal({ });", true);
            }

            if(userName != confirmUserName)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('User Names Do Not Match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteUser').openModal({ });", true);
            }
            else
            {
                if (DropDownList_deleteUser.SelectedIndex != 0)
                {
                    try
                    {
                        string command = "DELETE from UserData WHERE UserName = '" + userName + "'";
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery();

                        Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('User Successfully Deleted')" +
                            "</script>");
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        connection.Close();
                    }
                }
                
            }
        }

        protected void Button_chgPw_Click(object sender, EventArgs e)
        {
            string password = chgPass_Text.Value.ToString();
            string confirmPass = confirmChgPass_Text.Value.ToString();

            if(DropDownList_chgPass.SelectedIndex == 0)
            {
                Response.Write(
                       "<script type=\"text/javascript\">" +
                       "alert('No User Name Selected')" +
                       "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_chgPass').openModal({ });", true);
            }

            if (password != confirmPass)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Passwords do not match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_chgPass').openModal({ });", true);
            }
            else
            {
                try
                {
                    connection.Open();
                    string update = "UPDATE UserData SET Password = '" + password + "' WHERE UserName = '" + DropDownList_chgPass.SelectedValue.ToString() + "'";
                    MySqlCommand cmd = new MySqlCommand(update, connection);
                    cmd.ExecuteNonQuery();

                    if (DropDownList_chgPass.SelectedIndex != 0)
                    {
                        Response.Write(
                                            "<script type=\"text/javascript\">" +
                                                "alert('Password Successfully Changed')" +
                                                "</script>");
                    }
                        
                }
                catch (Exception ex){}
                finally { connection.Close(); }
            }


        }

        protected void Button_Login_Click(object sender, EventArgs e)
        {
            int temp = 0;

            

            string formUserName = UserName.Value.ToString();
            string formPassword = Password.Value.ToString();
            try
            {
                //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                connection.Open();
                string checkuser = "select count(*) from UserData where UserName='" + formUserName + "'";
                MySqlCommand com = new MySqlCommand(checkuser, connection);
                temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                connection.Close();
            }
            catch { }


            if (temp == 1)
            {
                try
                {
                    connection.Open();
                    string checkPasswordQuery = "select Password from UserData where UserName='" + formUserName + "'";
                    MySqlCommand passComm = new MySqlCommand(checkPasswordQuery, connection);
                    string pass = passComm.ExecuteScalar().ToString().Replace(" ", "");//remove whitespace
                                                                                       //verify password
                    if (pass == formPassword)
                    {
                        Session["New"] = formUserName;
                        //Response.Write("Password is correct");
                        //Response.Redirect("main.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_Login').openModal({dismissible: false });", true);
                        Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Password is not correct')" +
                        "</script>");
                    }
                    connection.Close();
                }
                catch { }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_Login').openModal({dismissible: false });", true);
                Response.Write(
                "<script type=\"text/javascript\">" +
                "alert('User Name is not correct')" +
                "</script>");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_Login').openModal({dismissible: false });", true);
        }

        protected void Button_AddMajorShow_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addMajor').openModal({ });", true);
        }

        protected void Button_DeleteInstructorShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Instructor ORDER BY Instructor ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteInstructor.DataSource = reader;
                            DropDownList_deleteInstructor.DataValueField = "Instructor";
                            DropDownList_deleteInstructor.DataTextField = "Instructor";
                            DropDownList_deleteInstructor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteInstructor.Items.Insert(0, new ListItem("Select Instructor", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }
            

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteInstructor').openModal({ });", true);
        }

        protected void Button_DeleteRoomShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM classrooms ORDER BY room ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteRoom.DataSource = reader;
                            DropDownList_deleteRoom.DataValueField = "room";
                            DropDownList_deleteRoom.DataTextField = "room";
                            DropDownList_deleteRoom.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteRoom.Items.Insert(0, new ListItem("Select Room", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteRoom').openModal({ });", true);
        }

        protected void Button_DeleteClassShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Classes ORDER BY name ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteClass.DataSource = reader;
                            DropDownList_deleteClass.DataValueField = "name";
                            DropDownList_deleteClass.DataTextField = "name";
                            DropDownList_deleteClass.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteClass.Items.Insert(0, new ListItem("Select Class", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClass').openModal({ });", true);
        }

        protected void Button_AddInstructorShow_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addInstructor').openModal({ });", true);
        }

        protected void Button_AddClassShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM major ORDER BY major ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CheckBoxList_majors.DataSource = reader;
                            CheckBoxList_majors.DataValueField = "major";
                            CheckBoxList_majors.DataTextField = "major";
                            CheckBoxList_majors.DataBind();
                        }
                    }
                }


            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addClass').openModal({ });", true);
        }

        protected void Button_AddRoomShow_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addRoom').openModal({ });", true);
        }

        protected void Button_DeleteMajorShow_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Major ORDER BY Major ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteMajor.DataSource = reader;
                            DropDownList_deleteMajor.DataValueField = "Major";
                            DropDownList_deleteMajor.DataTextField = "Major";
                            DropDownList_deleteMajor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteMajor.Items.Insert(0, new ListItem("Select Major", ""));
            }
            catch (Exception ex) { Response.Write(ex); }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteMajor').openModal({ });", true);
        }

        protected void Button_AddMajor_Click(object sender, EventArgs e)
        {
            string major = addDepartment_Text.Value.ToString();
            string confirmMajor = confirmAddDepartment_Text.Value.ToString();
            int existing = 0;

            if (major != confirmMajor)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addMajor').openModal({  });", true);
                Response.Write(
                "<script type=\"text/javascript\">" +
                "alert('Major names do not match')" +
                "</script>");
            }
            else
            {
                try
                {
                    connection.Open();
                    string existingQuery = "Select count(*) from major where major = '" + major + "'";
                    MySqlCommand comm = new MySqlCommand(existingQuery, connection);
                    existing = Convert.ToInt32(comm.ExecuteScalar().ToString());
                }
                catch(Exception ex) { }
                finally { connection.Close(); }
                if (existing != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addMajor').openModal({  });", true);
                    Response.Write(
                    "<script type=\"text/javascript\">" +
                    "alert('Major already exists')" +
                    "</script>");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string addMajor = "insert into major (major) value ('" + major + "')";
                        MySqlCommand comm = new MySqlCommand(addMajor, connection);
                        comm.ExecuteNonQuery();
                        Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Successfully added major')" +
                        "</script>");
                    }
                    catch (Exception ex){ }
                    finally { connection.Close(); }
                }

            }

        }

        protected void Button_AddInstructor_Click(object sender, EventArgs e)
        {
            string instructor = addInstructor_Text.Value.ToString();
            string confirmInstructor = confirmAddInstructor_Text.Value.ToString();
            int existing = 0;

            if (instructor != confirmInstructor)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addInstructor').openModal({  });", true);
                Response.Write(
                "<script type=\"text/javascript\">" +
                "alert('Names do not match')" +
                "</script>");
            }
            else
            {
                try
                {
                    connection.Open();
                    string existingQuery = "Select count(*) from instructor where instructor = '" + instructor + "'";
                    MySqlCommand comm = new MySqlCommand(existingQuery, connection);
                    existing = Convert.ToInt32(comm.ExecuteScalar().ToString());
                }
                catch (Exception ex) { }
                finally { connection.Close(); }
                if (existing != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addInstructor').openModal({  });", true);
                    Response.Write(
                    "<script type=\"text/javascript\">" +
                    "alert('Instructor already exists')" +
                    "</script>");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string addMajor = "insert into instructor (instructor) value ('" + instructor + "')";
                        MySqlCommand comm = new MySqlCommand(addMajor, connection);
                        comm.ExecuteNonQuery();
                        Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Successfully added instructor')" +
                        "</script>");
                    }
                    catch (Exception ex) { }
                    finally { connection.Close(); }
                }

            }

        }

        protected void Button_AddRoom_Click(object sender, EventArgs e)
        {
            string room = addRoom_Text.Value.ToString();
            string confirmRoom = confirmAddRoom_Text.Value.ToString();
            int existing = 0;

            if (room != confirmRoom)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addRoom').openModal({  });", true);
                Response.Write(
                "<script type=\"text/javascript\">" +
                "alert('Room names do not match')" +
                "</script>");
            }
            else
            {
                try
                {
                    connection.Open();
                    string existingQuery = "Select count(*) from classrooms where room = '" + room + "'";
                    MySqlCommand comm = new MySqlCommand(existingQuery, connection);
                    existing = Convert.ToInt32(comm.ExecuteScalar().ToString());
                }
                catch (Exception ex) { Response.Write(ex); }
                finally { connection.Close(); }
                if (existing != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addRoom').openModal({  });", true);
                    Response.Write(
                    "<script type=\"text/javascript\">" +
                    "alert('Room already exists')" +
                    "</script>");
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string addMajor = "insert into classrooms (room) value ('" + room + "')";
                        MySqlCommand comm = new MySqlCommand(addMajor, connection);
                        comm.ExecuteNonQuery();
                        Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Successfully added room')" +
                        "</script>");
                    }
                    catch (Exception ex) { Response.Write(ex); }
                    finally { connection.Close(); }
                }

            }

        }
        protected void Button_addClass_Click(object sender, EventArgs e)
        {
            string className = addClass_Text.Value.ToString();
            string confirmClass = confirmAddClass_Text.Value.ToString();
            int existing = 0;

            if(className != confirmClass)
            {
                Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Class names do not match')" +
                             "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addClass').openModal({ });", true);
            }
            else
            {
                try
                {
                    connection.Open();
                    string existingQuery = "Select count(*) from classes where name = '" + className + "'";
                    MySqlCommand comm = new MySqlCommand(existingQuery, connection);
                    existing = Convert.ToInt32(comm.ExecuteScalar().ToString());
                }
                catch(Exception ex) { }
                finally { connection.Close(); }
                if(existing != 0)
                {
                    Response.Write(
                           "<script type=\"text/javascript\">" +
                           "alert('Class already exists')" +
                            "</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addClass').openModal({ });", true);
                }
                else
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
                        
                        command.Parameters.AddWithValue("@name", className);
                        command.Parameters.AddWithValue("@M1", arr[0]);
                        command.Parameters.AddWithValue("@M2", arr[1]);
                        command.Parameters.AddWithValue("@M3", arr[2]);
                        command.Parameters.AddWithValue("@M4", arr[3]);

                        command.ExecuteNonQuery();
                        Response.Write(
                                "<script type=\"text/javascript\">" +
                                "alert('Add Class Success')" +
                                 "</script>");

                    }
                    catch (Exception ex) { }                        
                    finally { connection.Close(); }
                }
            }


                
        }
        protected void Button_deleteMajor_Click(object sender, EventArgs e)
        {
            string major = DropDownList_deleteMajor.SelectedValue.ToString();
            string confirmMajor = confirmMajorDelete_Text.Value.ToString();

            if(DropDownList_deleteMajor.SelectedIndex == 0)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No major selected')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteMajor').openModal({ });", true);
            }

            if (major != confirmMajor)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Major names to not match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteMajor').openModal({ });", true);
            }
            else
            {

                if (DropDownList_deleteMajor.SelectedIndex != 0)
                {
                    try
                    {
                        string command = "DELETE from major WHERE major = '" + major + "'";
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery();

                        Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Major Successfully Deleted')" +
                            "</script>");
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        connection.Close();
                    }
                }
                
            }



        }

        protected void Button_deleteInstructor_Click(object sender, EventArgs e)
        {
            string instructor = DropDownList_deleteInstructor.SelectedValue.ToString();
            string confirmInstructor = confirmInstructorDelete_Text.Value.ToString();

            if (DropDownList_deleteMajor.SelectedIndex == 0)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No instructor selected')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteInstructor').openModal({ });", true);
            }

            if (instructor != confirmInstructor)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Instructor names to not match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteInstructor').openModal({ });", true);
            }
            else
            {

                if (DropDownList_deleteInstructor.SelectedIndex != 0)
                {
                    try
                    {
                        string command = "DELETE from instructor WHERE instructor = '" + instructor + "'";
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery();

                        Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Instructor Successfully Deleted')" +
                            "</script>");
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            }

        protected void Button_deleteRoom_Click(object sender, EventArgs e)
        {
            string Room = DropDownList_deleteRoom.SelectedValue.ToString();
            string confirmRoom = confirmRoomDelete_Text.Value.ToString();

            if (DropDownList_deleteRoom.SelectedIndex == 0)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No room selected')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteRoom').openModal({ });", true);
            }

            if (Room != confirmRoom)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Room names to not match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteRoom').openModal({ });", true);
            }
            else
            {

                if (DropDownList_deleteRoom.SelectedIndex != 0)
                {
                    try
                    {
                        string command = "DELETE from classrooms WHERE room = '" + Room + "'";
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery();

                        Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Room Successfully Deleted')" +
                            "</script>");
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        protected void Button_deleteClass_Click(object sender, EventArgs e)
        {
            string delClass = DropDownList_deleteClass.SelectedValue.ToString();
            string confirmClass = confirmClassDelete_Text.Value.ToString();

            if (DropDownList_deleteClass.SelectedIndex == 0)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No class selected')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClass').openModal({ });", true);
            }

            if (delClass != confirmClass)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('Class names to not match')" +
                        "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClass').openModal({ });", true);
            }
            else
            {

                if (DropDownList_deleteRoom.SelectedIndex != 0)
                {
                    try
                    {
                        string command = "DELETE from classes WHERE name = '" + delClass + "'";
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery();

                        Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Class Successfully Deleted')" +
                            "</script>");
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
    }
    
}