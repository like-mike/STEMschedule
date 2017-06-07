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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace stemSchedule
{
    public partial class main : System.Web.UI.Page
    {
        // "#defines"
        int rows = 0;
        //select *,TIME_FORMAT(StartTime, '%h:%i %p')startTime,TIME_FORMAT(EndTime, '%h:%i %p')EndTime from schedule
        public const string DB_CREDENTIALS = "SERVER = cs.spu.edu; DATABASE = stemschedule; UID = stemschedule; PASSWORD = stemschedule.stemschedule";
        public string PUBLIC_SCHEDULE = "Select * FROM SCHEDULE WHERE PUBLIC = 1";
        
       
        public bool G1Selected = false;

        public static string recentQuery = "";
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
        GridView test = new GridView();
        

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

        string militaryTime(string civTime)
        {
            string[] result = civTime.Split(':');
            int hour = Int32.Parse(result[0]);
            string min = result[1].Substring(0, 2);
            if (result[1].Substring(3, 2).ToUpper() == "PM")
                hour += 12;
            string newHour = "";
            if (hour < 10)
                newHour = "0" + hour.ToString();
            else
                newHour = hour.ToString();
            var milTime = newHour + ":" + min + ":00";
            return milTime;
        }

        string civilianTime(string textTime)
        {
            string civTime = "";
            int hour = Int32.Parse(textTime.Substring(0, 2));
            string min = textTime.Substring(3, 2);
            if (hour > 12)
            {
                hour -= 12;
                civTime = hour.ToString() + ":" + min + " PM";
            }
            else
            {
                civTime = hour.ToString() + ":" + min + " AM";
            }
            return civTime;
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            checkAll();
            Page.MaintainScrollPositionOnPostBack = true;
            rows = 0;
            this.Form.DefaultButton = this.Button_returnLogin.UniqueID;
            if (!IsPostBack)
            {
                
                if (Session["New"] != null)
                {
                    //label_welcome.Text += Session["New"].ToString();
                    this.Form.DefaultButton = this.Button_default.UniqueID;

                    string query;

                    try
                    { // user schedule
                        connection.Open();
                        MySqlCommand findQuery = new MySqlCommand("Select query from userdata where username = '" + Session["New"] + "'");
                        query = findQuery.ExecuteScalar().ToString();
                        

                        
                        command = new MySqlCommand(query, connection);
                        table = new DataTable();
                        data = new MySqlDataAdapter(command);
                        data.Fill(table);
                        GridView1.DataSource = table;
                        GridView1.DataBind();

                    }
                    catch (Exception ex) { }
                    finally { connection.Close(); }
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
                { //private schedule
                    connection.Open();
                    command = new MySqlCommand("Select * from schedule where public = 0 AND user = '" +Session["New"]+ "'", connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView2.DataSource = table;
                    GridView2.DataBind();
                    connection.Close();
                }
                catch (Exception ex) {  }
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
                string CRN = GridView2.SelectedRow.Cells[1].Text;
                string c = "UPDATE schedule SET PUBLIC = 1 WHERE CRN = " + CRN;
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(c, connection);
                    cmd.ExecuteNonQuery();
                    

                    
                    cmd = new MySqlCommand("SELECT lastquery from userdata where username = '" + Session["New"] + "'", connection);
                    string recentQuery = cmd.ExecuteScalar().ToString();



                    checkAll();
                    command = new MySqlCommand(recentQuery, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();


                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        if (row.Cells[1].Text == CRN)
                        {
                            row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
                            //row.ToolTip = "Room & Major Conflicts";
                        }

                    }

                    

                    string query = "Select * from schedule where public = 0 AND user = '" + Session["New"] + "'";

                    //Response.Write(query);

                    command = new MySqlCommand(query, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView2.DataSource = table;
                    GridView2.DataBind();

                }
                catch (Exception ex) { }
                finally
                {
                    connection.Close();
                    GridView2.SelectedIndex = -1;

                }


            }
        }


        public void checkConflict(bool output, String CRN, String Faculty, String ClassNum, String Days, String StartTime, String EndTime, String Term, String Room, String M1, String M2, String M3, String M4, String Credits, String M, String T, String W, String Th, String F, String Sa, String Su, String Fr, String So, String Ju, String Se, String Year)
        {
            //resetCheck();
            string check = "";
            string time = " where((startTime >= '" + StartTime + "' and endTime <= '" + EndTime + "') OR (endTime >= '" + StartTime + "' AND endTime <= '" + EndTime + "') OR (startTime <= '" + EndTime + "' AND startTime >= '" + StartTime + "')) AND Public = 1 AND CalYear = '" + Year + "' AND (M = '" + M + "' AND T = '" + T + "' AND W = '"+ W + "' AND Th = '" + Th + "' AND Fr = '" + Fr + "')";
            string roomQuery = time + " AND Room = '" + Room + "'";
            
            bool roomConflicts = false, majorConflicts = false;
            string finalQuery = "select * from schedule where public = 3";
            //command.ExecuteScalar().ToString();
            int countRoom = 0, countMajor = 0;



           

            try
            {
                connection.Open();
                check = "select count(*) from schedule" + roomQuery;
                MySqlCommand com = new MySqlCommand(check, connection);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                if(temp > 1)
                {
                    roomConflicts = true;
                    finalQuery += " union select * from schedule" + roomQuery;
                    countRoom = temp -1;

                    string update = "update schedule set Rcon = 1, conflicts = 1" + roomQuery;
                    com = new MySqlCommand(update, connection);
                    com.ExecuteNonQuery();
                }

                //Response.Write(temp.ToString());
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }


            //M1
            string M1query = time + " AND (M1 = '" + M1 + "' OR M2 = '" + M1 + "' OR M3 = '" + M1 + "' OR M4 = '" + M1 + "')";
            try
            {
                connection.Open();
                check = "select count(*) from schedule" + M1query;
                MySqlCommand com = new MySqlCommand(check, connection);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                if (temp > 1)
                {

                    majorConflicts = true;
                    finalQuery += " union select * from schedule" + M1query;
                    
                    countMajor += temp -1;

                    string update = "update schedule set Mcon = 1, conflicts = 1" + M1query;
                    com = new MySqlCommand(update, connection);
                    com.ExecuteNonQuery();
                }

                //Response.Write(temp.ToString());
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }

            //M2
            string M2query = time + " AND (M1 = '" + M2 + "' OR M2 = '" + M2 + "' OR M3 = '" + M2 + "' OR M4 = '" + M2 + "')";
            try
            {
                connection.Open();
                check = "select count(*) from schedule" + M2query;
                MySqlCommand com = new MySqlCommand(check, connection);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                if (temp > 1)
                {
                    majorConflicts = true;
                    finalQuery += " union select * from schedule" + M2query;
                    
                    countMajor += temp - 1;

                    string update = "update schedule set Mcon = 1, conflicts = 1" + M2query;
                    com = new MySqlCommand(update, connection);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }

            //M3
            string M3query = time + " AND (M1 = '" + M3 + "' OR M2 = '" + M3 + "' OR M3 = '" + M3 + "' OR M4 = '" + M3 + "')";
            try
            {
                connection.Open();
                check = "select count(*) from schedule" + M3query;
                MySqlCommand com = new MySqlCommand(check, connection);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                if (temp > 1)
                {
                    majorConflicts = true;
                    finalQuery += " union select * from schedule" + M3query;
                    
                    countMajor += temp - 1;

                    string update = "update schedule set Mcon = 1, Conflicts = 1" + M3query;
                    com = new MySqlCommand(update, connection);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }

            //M4
            string M4query = time + " AND (M1 = '" + M4 + "' OR M2 = '" + M4 + "' OR M3 = '" + M4 + "' OR M4 = '" + M4 + "')";
            try
            {
                connection.Open();
                check = "select count(*) from schedule" + M4query;
                MySqlCommand com = new MySqlCommand(check, connection);
                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                if (temp > 1)
                {
                    majorConflicts = true;
                    finalQuery += " union select * from schedule" + M4query;

                    countMajor += temp - 1;

                    string update = "update schedule set Mcon = 1, Conflicts = 1" + M4query;
                    com = new MySqlCommand(update, connection);
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }

            


            if (output)
            {
                if (countRoom > 0 && countMajor == 0)
                    Response.Write(
                   "<script type=\"text/javascript\">" +
                   "alert('(" + countRoom + ") Room Conflict(s) Found')" +
                    "</script>");
                else if (countMajor > 0 && countRoom == 0)
                    Response.Write(
                  "<script type=\"text/javascript\">" +
                  "alert('(" + countMajor + ") Major Conflict(s) Found')" +
                   "</script>");
                else if (countRoom > 0 && countMajor > 0)
                    Response.Write(
                  "<script type=\"text/javascript\">" +
                  "alert('(" + countMajor + ") Major Conflict(s) Found AND (" + countRoom + ") Room Conflicts Found')" +
                   "</script>");






                check = "select * from schedule" + roomQuery;
                try
                { // public schedule
                    connection.Open();
                    command = new MySqlCommand(finalQuery, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();

                }
                catch (Exception ex) {  }
                finally { connection.Close(); }





                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.Cells[30].Text == "1" && row.Cells[31].Text == "1")
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFBBBB");//red
                        row.ToolTip = "Room & Major Conflicts";
                    }
                    else if (row.Cells[30].Text == "1" && row.Cells[31].Text != "1")//blue
                    {
                        row.BackColor = ColorTranslator.FromHtml("#BBBBFF");
                        row.ToolTip = "Room Conflict";
                    }
                    else if (row.Cells[30].Text != "1" && row.Cells[31].Text == "1")
                    {
                        row.BackColor = ColorTranslator.FromHtml("#FFFFC8");//yellow
                        row.ToolTip = "Major Conflict";
                    }
                }


                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.Cells[1].Text == CRN)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
                        //row.ToolTip = "Room & Major Conflicts";
                    }

                }
            }

            

        }



        protected void Button_AddClass_Click1(object sender, EventArgs e)
        {




            string CRN = "";
            string enrollment = Enrollment_Text.Value.ToString();
            string credits = Credits_Text.Value.ToString();
            string calYear = Year_Text.Value.ToString();


            if (CRN_Text.Value.ToString() == "")
            {
                int match = 1;
                Random random = new Random();
                int CRN_random = 0;
                while (match != 0)
                {
                    CRN_random = random.Next(0, 1000);
                    try
                    {
                        connection.Open();
                        string check = "select count(*) from schedule where CRN='" + CRN_random + "'";
                        MySqlCommand com = new MySqlCommand(check, connection);
                        int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                        if (temp == 0)
                            match = 0;
                    }
                    catch (Exception ex) {  }
                    finally { connection.Close(); }
                }
                CRN = CRN_random.ToString();
            }
            else
                CRN = CRN_Text.Value.ToString();


            try
            {
                string command = "DELETE from SCHEDULE WHERE CRN = " + GridView2.SelectedRow.Cells[1].Text;
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {  }
            finally { connection.Close(); editing = false; }

            try
            {
                connection.Open();
                //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                string insertQuery = "insert into schedule (CRN,Faculty,ClassNum,Days,StartTime,EndTime,Term,Room,EnrollNum,Year,M1,M2,M3,M4,Credits,Public,M,T,W,Th,F,Sa,Su,Fr,So,Ju,Se,CalYear,User) values (@CRN,@Faculty,@ClassNum,@Days,@StartTime,@EndTime,@Term,@Room,@EnrollNum,@Year,@M1,@M2,@M3,@M4,@Credits,@Public,@M,@T,@W,@Th,@F,@Sa,@Su,@Fr,@So,@Jr,@Se,@CalYear,@User)";
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
                if (M != "1")
                    M = "0";
                if (T != "1")
                    T = "0";
                if (W != "1")
                    W = "0";
                if (Th != "1")
                    Th = "0";
                if (F != "1")
                    F = "0";
                if (Sa != "1")
                    Sa = "0";
                if (Su != "1")
                    Su = "0";


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
                //command.Parameters.AddWithValue("@Conflict", null);//conflicts
                command.Parameters.AddWithValue("@Public", 0);//conflicts
                                                              //command.ExecuteNonQuery();
                                                              //Response.Redirect("main.aspx");
                                                              //Response.Write("Add Class Success");

                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();

                command = new MySqlCommand("SELECT lastquery from userdata where username = '" + Session["New"] + "'", connection);
                string recentQuery = command.ExecuteScalar().ToString();

                command = new MySqlCommand(recentQuery, connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();


                command = new MySqlCommand("Select * from schedule where public = 0 AND user = '" + Session["New"] + "'", connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView2.DataSource = table;
                GridView2.DataBind();

                

                foreach (GridViewRow row in GridView2.Rows)
                {
                    if (row.Cells[1].Text == CRN)
                    {
                        row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
                        //row.ToolTip = "Room & Major Conflicts";
                    }
                    
                }


                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Add Class Success!')" +
    "</script>"
  );
                connection.Close();
                //checkConflict(false, CRN, DropDownList_instructor.SelectedValue, DropDownList_class.SelectedValue, days, TextBox_StartTime.Text, TextBox_EndTime.Text, DropDownList_term.SelectedValue, DropDownList_Classroom.SelectedValue, M1, M2, M3, M4, credits, M, T, W, Th, F, Sa, Su, fr, so, ju, se,"2017");


            }
            catch (Exception ex)
            {
                Response.Write(
    "<script type=\"text/javascript\">" +
    "alert('Input Error!')" +
    "</script>"
  );
                

            }
            finally
            {
                connection.Close();

            }
            //Label_AddClassDesc.Visible = false;
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
                string CRN = GridView1.SelectedRow.Cells[1].Text;
                string c = "UPDATE schedule SET PUBLIC = 0, final = '', User = '" + Session["New"] + "' WHERE CRN = " + CRN;
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(c, connection);
                    cmd.ExecuteNonQuery();

                    
                    cmd = new MySqlCommand("SELECT lastquery from userdata where username = '" + Session["New"] + "'", connection);
                    string recentQuery = cmd.ExecuteScalar().ToString();
                    connection.Close();
                    checkAll();
                    connection.Open();



                    command = new MySqlCommand(recentQuery, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();


                    string query = "Select * from schedule where public = 0 AND User = '"+ Session["New"] + "'";//if user does not have a saved default query, show all classes

                    //Response.Write(query);

                    command = new MySqlCommand(query, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView2.DataSource = table;
                    GridView2.DataBind();

                    foreach (GridViewRow row in GridView2.Rows)
                    {
                        if (row.Cells[1].Text == CRN)
                        {
                            row.BackColor = ColorTranslator.FromHtml("#CCFFCC");
                            //row.ToolTip = "Room & Major Conflicts";
                        }

                    }

                }
                catch (Exception ex) { Response.Write(ex);  }
                finally
                {
                    connection.Close();
                    GridView1.SelectedIndex = -1;

                }


               
            }

            
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("main.aspx");
        }

     
       

        protected void ExportToExcel(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            string strFileName = "STEMschedule_" + DateTime.Now.ToShortDateString() + ".csv";
            builder.Append("CRN,Faculty,Class,Start Time,End Time,Term,Room,M1,M2,M3,M4,Year,Credits,Enrollment,Calendar Year," + Environment.NewLine);
            string[] M = new string[4];

            

            foreach (GridViewRow row in GridView1.Rows)
            {
                string CRN = row.Cells[1].Text;
                string Faculty = row.Cells[2].Text;
                string numClass = row.Cells[3].Text;
                string startTime = row.Cells[4].Text;
                string endTime = row.Cells[5].Text;
                string term = row.Cells[6].Text;
                string room = row.Cells[7].Text;
                if (room == "")
                    room = " ";

                
                M[0] = row.Cells[8].Text;
                if (row.Cells[8].Text.Equals("&nbsp;")) {  
                    M[0] = " ";
                }
                M[1] = row.Cells[9].Text;
                if (row.Cells[9].Text.Equals("&nbsp;"))
                {
                    M[1] = " ";
                }



                M[2] = row.Cells[10].Text;
                if (row.Cells[10].Text.Equals("&nbsp;")){
                    M[2] = " ";
                }

                M[3] = row.Cells[11].Text;
                if (row.Cells[11].Text.Equals("&nbsp;"))
                {
                    M[3] = " ";
                }


                string year = row.Cells[26].Text;
                string credits = row.Cells[27].Text;
                string enrollNum = row.Cells[28].Text;
                string calYear = row.Cells[32].Text;
                


                builder.Append(CRN + "," + Faculty + "," + numClass+ "," + startTime + "," + endTime + "," + term + "," + room + "," + M[0] + "," + M[1] + "," + M[2] + "," + M[3] + "," + year + "," + credits + "," + enrollNum + "," + calYear +  Environment.NewLine);
            }
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName);
            string built = builder.ToString();
            //built = ScrubHtml(built);
            Response.Write(built);
            Response.End();
            
        }

        public static string ScrubHtml(string value)
        {
            value = value.Replace(System.Environment.NewLine, "new");
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "-").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", "-");

            
            value = value.Replace("new", System.Environment.NewLine);
            return value;
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
                catch (Exception ex) {  }
                finally
                {
                    connection.Close();
                    checkAll();
                    Response.Redirect("main.aspx");
                }
            }

            

        }

        protected void Button_ShowAll_Click(object sender, EventArgs e)
        {
            
            try
            { // user schedule
                connection.Open();
                command = new MySqlCommand("SELECT query from userdata where username = '" + Session["New"] + "'", connection);
                string query = command.ExecuteScalar().ToString();
                if (query == "")
                {
                    query = "Select * from schedule where public = 1";
                }
                command = new MySqlCommand(query, connection);
                table = new DataTable();
                data = new MySqlDataAdapter(command);
                data.Fill(table);
                GridView1.DataSource = table;
                GridView1.DataBind();

                //Response.Write(query);



            }
            catch (Exception ex) {
                
            }
            finally { connection.Close(); }

            
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
                string updateQuery = "update schedule set Conflicts='',RCon='',MCon='' WHERE NOT CRN =''";

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

        public void checkAll()
        {
            resetCheck();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                String CRN = GridView1.Rows[i].Cells[1].Text;
                String Faculty = GridView1.Rows[i].Cells[2].Text;
                String ClassNum = GridView1.Rows[i].Cells[3].Text;
                String startTime = militaryTime(GridView1.Rows[i].Cells[5].Text);
                String endTime = militaryTime(GridView1.Rows[i].Cells[6].Text);
                String term = GridView1.Rows[i].Cells[7].Text;
                String room = GridView1.Rows[i].Cells[8].Text;
                String M1 = GridView1.Rows[i].Cells[9].Text;
                String M2 = GridView1.Rows[i].Cells[10].Text;
                String M3 = GridView1.Rows[i].Cells[11].Text;
                String M4 = GridView1.Rows[i].Cells[12].Text;
                String M = GridView1.Rows[i].Cells[14].Text;
                String T = GridView1.Rows[i].Cells[15].Text;
                String W = GridView1.Rows[i].Cells[16].Text;
                String Th = GridView1.Rows[i].Cells[17].Text;
                String F = GridView1.Rows[i].Cells[18].Text;
                String Sa = GridView1.Rows[i].Cells[19].Text;
                String Su = GridView1.Rows[i].Cells[20].Text;
                String Fr = GridView1.Rows[i].Cells[21].Text;
                String So = GridView1.Rows[i].Cells[22].Text;
                String Jr = GridView1.Rows[i].Cells[23].Text;
                String Se = GridView1.Rows[i].Cells[24].Text;
                String calYear = GridView1.Rows[i].Cells[32].Text;


                checkConflict(false, CRN, Faculty, ClassNum, "", startTime, endTime, term, room, M1, M2, M3, M4, "", M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se, calYear);

            }
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
                String startTime = militaryTime(GridView1.SelectedRow.Cells[5].Text);
                String endTime = militaryTime(GridView1.SelectedRow.Cells[6].Text);
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
                String calYear = GridView1.SelectedRow.Cells[32].Text;

                
                checkConflict(true, CRN, Faculty, ClassNum, "", startTime, endTime, term, room, M1, M2, M3, M4, "", M, T, W, Th, F, Sa, Su, Fr, So, Jr, Se,calYear);
                GridView1.SelectedIndex = -1;
                GridView2.SelectedIndex = -1;
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
                String startTime = militaryTime(GridView2.SelectedRow.Cells[5].Text);
                String endTime = militaryTime(GridView2.SelectedRow.Cells[6].Text);
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
            
            for(int i = 13; i <= 25; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[30].Visible = false;
            e.Row.Cells[31].Visible = false;
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            for (int i = 13; i <= 25; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[30].Visible = false;
            e.Row.Cells[31].Visible = false;
            e.Row.Cells[33].Visible = false;
            e.Row.Cells[34].Visible = false;
        }

        protected void Button_AddUserShow_Click(object sender, EventArgs e)
        {
            //divControl.Attributes("sty") = "height:200px; color:Red"
            this.Form.DefaultButton = this.Button_returnAddUser.UniqueID;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_settings').openModal({ });", true);
        }

        protected void Button_SearchShow_Click(object sender, EventArgs e)
        {

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM Instructor ORDER BY instructor ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_searchInstructor.DataSource = reader;
                            DropDownList_searchInstructor.DataValueField = "instructor";
                            DropDownList_searchInstructor.DataTextField = "instructor";
                            DropDownList_searchInstructor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_searchInstructor.Items.Insert(0, new ListItem("", ""));
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }



            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("select distinct calyear from schedule order by calYear ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_searchCalYear.DataSource = reader;
                            DropDownList_searchCalYear.DataValueField = "calYear";
                            DropDownList_searchCalYear.DataTextField = "calYear";
                            DropDownList_searchCalYear.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_searchCalYear.Items.Insert(0, new ListItem("", ""));
            }
            catch (Exception ex) { }
            finally { connection.Close(); }

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM major ORDER BY major ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_searchMajor.DataSource = reader;
                            DropDownList_searchMajor.DataValueField = "major";
                            DropDownList_searchMajor.DataTextField = "major";
                            DropDownList_searchMajor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_searchMajor.Items.Insert(0, new ListItem("", ""));
            }
            catch (Exception ex) { }
            finally { connection.Close(); }



            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_search').openModal({ });", true);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            
        }

        protected void Button_DeleteUserShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnDeleteUser.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteUser').openModal({ });", true);
        }


        protected void Button_CopyShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnShowCopy.UniqueID;
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("select * from major ORDER BY major ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_copy.DataSource = reader;
                            DropDownList_copy.DataValueField = "major";
                            DropDownList_copy.DataTextField = "major";
                            DropDownList_copy.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_copy.Items.Insert(0, new ListItem("Select Major", ""));
                DropDownList_copy.Items.Insert(1, new ListItem("All", ""));

            }
            catch (Exception ex) {  }
            finally { connection.Close(); }


            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("select DISTINCT calYear from schedule order by calYear Desc", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_copiedYear.DataSource = reader;
                            DropDownList_copiedYear.DataValueField = "calYear";
                            DropDownList_copiedYear.DataTextField = "calYear";
                            DropDownList_copiedYear.DataBind();
                        }
                    }
                }
                
                
            }
            catch (Exception ex) {  }
            finally { connection.Close(); }
            

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM schedule WHERE CalYear = '" + DropDownList_copiedYear.SelectedValue + "' AND Term = '" + DropDownList_CopyTerm.SelectedValue + "' ORDER BY ClassNum ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CheckBoxList_copyAll.DataSource = reader;
                            CheckBoxList_copyAll.DataValueField = "CRN";
                            CheckBoxList_copyAll.DataTextField = "ClassNum";
                            CheckBoxList_copyAll.DataBind();
                            
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No Classes Exist')" +
                        "</script>");
                
            }
            finally { connection.Close(); }





            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_copy').openModal({ });", true);
        }

        protected void Button_ChangePwShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnChgPw.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_chgPass').openModal({ });", true);
        }

        protected void Button_editSessionShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnAdd.UniqueID;
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

                
                String startTime = militaryTime(GridView2.SelectedRow.Cells[5].Text);
                String endTime = militaryTime(GridView2.SelectedRow.Cells[6].Text);
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

                try { DropDownList_class.SelectedValue = ClassNum; }
                catch(Exception ex) { }




                try { DropDownList_term.SelectedValue = term; }
                catch(Exception ex) { }

                try { DropDownList_Classroom.SelectedValue = room; }
                catch(Exception ex) { }
                /*if (DropDownList_Classroom.SelectedValue != room)
                    Response.Write(
                           "<script type=\"text/javascript\">" +
                           "alert('" + room + " no longer exists. Please add Room in Settings if you wish to use')" +
                           "</script>");*/
                Enrollment_Text.Value = enrollment;
                Credits_Text.Value = credits;
                TextBox_StartTime.Text = startTime;
                TextBox_EndTime.Text = endTime;

                try { DropDownList_instructor.SelectedValue = Faculty; }
                catch(Exception ex) { }
                
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
                //Label_AddClassDesc.Visible = true;



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
                catch (Exception ex) {  }
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
                catch (Exception ex) {  }
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
                catch (Exception ex) {  }
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
            this.Form.DefaultButton = this.Button_returnAdd.UniqueID;
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
            catch (Exception ex) {  }
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
            catch (Exception ex) {  }
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
            catch (Exception ex) {  }
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

            bool loggedin = false;
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

                        loggedin = true;


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


            if (loggedin)
            {
                try
                { // user schedule
                    connection.Open();
                    command = new MySqlCommand("SELECT query from userdata where username = '" + Session["New"] + "'", connection);
                    string query = command.ExecuteScalar().ToString();

                    if (query == "")
                        query = "Select * from schedule where public = 1";//if user does not have a saved default query, show all classes



                    string str = "update userdata SET lastquery = @lastquery WHERE username = '" + Session["New"] + "'";
                    command = new MySqlCommand(str, connection);
                    command.Parameters.AddWithValue("@lastquery", query);

                    command.ExecuteNonQuery();


                    command = new MySqlCommand(query, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();


                    command = new MySqlCommand("Select * from schedule where public = 0 AND user = '" + Session["New"] + "'", connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView2.DataSource = table;
                    GridView2.DataBind();

                }
                catch (Exception ex) {  }
                finally { connection.Close(); }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_Login').openModal({dismissible: false });", true);
        }

        protected void Button_AddMajorShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnAddMajor.UniqueID;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addMajor').openModal({ });", true);
        }

        protected void Button_DeleteInstructorShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnDelInstr.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }
            

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteInstructor').openModal({ });", true);
        }

        protected void Button_DeleteRoomShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnDelRoom.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteRoom').openModal({ });", true);
        }

        protected void Button_DeleteClassShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnDeleteClass.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClass').openModal({ });", true);
        }

        protected void Button_AddInstructorShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnAddInstr.UniqueID;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addInstructor').openModal({ });", true);
        }

        protected void Button_AddClassShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnAddClass.UniqueID;
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
            catch (Exception ex) {  }
            finally { connection.Close(); }




            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_addClass').openModal({ });", true);
        }

        protected void Button_AddRoomShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnAddRoom.UniqueID;
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
            catch (Exception ex) {  }
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
                catch (Exception ex) {  }
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
                    catch (Exception ex) {  }
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
                        string insertQuery = "insert into classes (name,M1,M2,M3,M4,Fr,So,Jr,Sr) values (@name,@M1,@M2,@M3,@M4,@Fr,@So,@Jr,@Sr)";
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
                        j = 0;
                        string[] years = new string[4];
                        for (int i = 0; i < CheckBoxList_addClassYear.Items.Count; i++)
                        {
                            if (CheckBoxList_addClassYear.Items[i].Selected)
                            {
                                years[j] = "1";
                            }
                            j++;

                        }

                        command.Parameters.AddWithValue("@name", className);
                        command.Parameters.AddWithValue("@M1", arr[0]);
                        command.Parameters.AddWithValue("@M2", arr[1]);
                        command.Parameters.AddWithValue("@M3", arr[2]);
                        command.Parameters.AddWithValue("@M4", arr[3]);

                        command.Parameters.AddWithValue("@Fr", years[0]);
                        command.Parameters.AddWithValue("@So", years[1]);
                        command.Parameters.AddWithValue("@Jr", years[2]);
                        command.Parameters.AddWithValue("@Sr", years[3]);

                        command.ExecuteNonQuery();
                        Response.Write(
                                "<script type=\"text/javascript\">" +
                                "alert('Succesfully added class')" +
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

        protected void DropDownList_copy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void CheckBoxList_copyAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void Button_selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxList_copyAll.Items)
                item.Selected = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_copy').openModal({ });", true);
        }

        protected void Button_unselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxList_copyAll.Items)
                item.Selected = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_copy').openModal({ });", true);
        }

        

        protected void Button_copyUpdate_Click(object sender, EventArgs e)
        {
            //unselect all
            foreach (ListItem item in CheckBoxList_copyAll.Items)
                item.Selected = false;

            string query = "";
            if(DropDownList_copy.SelectedIndex == 0 || DropDownList_copy.SelectedIndex == 1)
                query = "SELECT* FROM schedule WHERE CalYear = '" + DropDownList_copiedYear.SelectedValue + "' AND Term = '" + DropDownList_CopyTerm.SelectedValue + "' ORDER BY ClassNum ASC";
            else
                query = "SELECT* FROM schedule WHERE CalYear = '" + DropDownList_copiedYear.SelectedValue + "' AND TERM = '" + DropDownList_CopyTerm.SelectedValue + "' AND(M1 = '" + DropDownList_copy.SelectedValue + "' OR M2 = '" + DropDownList_copy.SelectedValue + "' OR M3 = '" + DropDownList_copy.SelectedValue + "' OR M4 = '" + DropDownList_copy.SelectedValue + "') ORDER BY ClassNum ASC";

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CheckBoxList_copyAll.DataSource = reader;
                            CheckBoxList_copyAll.DataValueField = "CRN";
                            CheckBoxList_copyAll.DataTextField = "ClassNum";
                            CheckBoxList_copyAll.DataBind();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No Classes Exist')" +
                        "</script>");
                //
            }
            finally { connection.Close(); }




            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_copy').openModal({ });", true);
        }

        protected void Button_copy_Click(object sender, EventArgs e)
        {
            if (CopyYear_Text.Value.ToString() == "")
            {
                Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('Must enter a new year to copy to')" +
                            "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_copy').openModal({ });", true);
            }
            else
            {
                Random random = new Random();
                int CRN = 0;
                int count = CheckBoxList_copyAll.Items.Count;
                for (int i = 0; i < CheckBoxList_copyAll.Items.Count; i++)
                {
                    if (CheckBoxList_copyAll.Items[i].Selected)
                    {


                        int match = 1;

                        while (match != 0)
                        {
                            CRN = random.Next(0, 1000);
                            try
                            {
                                connection.Open();
                                string check = "select count(*) from schedule where CRN='" + CRN + "'";
                                MySqlCommand com = new MySqlCommand(check, connection);
                                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                                if (temp == 0)
                                    match = 0;
                            }
                            catch (Exception ex) {  }
                            finally { connection.Close(); }
                        }



                        try
                        {
                            connection.Open();
                            string insertQuery = "CREATE table temporary_table AS SELECT * FROM schedule WHERE CRN='" + CheckBoxList_copyAll.Items[i].Value + "';UPDATE temporary_table SET CRN='" + CRN + "', calYear = '" + CopyYear_Text.Value.ToString() + "';INSERT INTO schedule SELECT * FROM temporary_table;";

                            command = new MySqlCommand(insertQuery, connection);
                            //command.Parameters.AddWithValue("@user", newUser);
                            //command.Parameters.AddWithValue("@pass", pass);
                            command.ExecuteNonQuery();

                        }
                        catch (Exception ex) {  }
                        finally
                        {
                            connection.Close();
                            try
                            {
                                connection.Open();
                                string insertQuery = "DROP TABLE temporary_table";
                                command = new MySqlCommand(insertQuery, connection);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex) { }
                            finally { connection.Close(); }
                        }




                    }


                }
                Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('(" + count + ") classes successfully copied')" +
                            "</script>");
            }


            
            
            

        }

        protected void Button_resetDefault_Click(object sender, EventArgs e)
        {

        }

        protected void Button_search_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            for(int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[5].Text = civilianTime(GridView1.Rows[i].Cells[5].Text);
                GridView1.Rows[i].Cells[6].Text = civilianTime(GridView1.Rows[i].Cells[6].Text);
                if (GridView1.Rows[i].Cells[34].Text == "1")
                {
                    GridView1.Rows[i].Cells[34].Text = "<img src='http://reesem2.cs.spu.edu/files/check-mark.png' height='28px' width='28px'/>";
                    GridView1.Rows[i].Cells[33].Text = "";
                }
                if (GridView1.Rows[i].Cells[33].Text == "1")
                {
                    GridView1.Rows[i].Cells[33].Text = "<img src='http://reesem2.cs.spu.edu/files/conflict.png' height='28px' width='28px'/>";
                    
                }

            }
            

        }

        protected void Button_search_Click1(object sender, EventArgs e)
        {
           
           
            string query = "Select * from schedule where public = 1";

            //search by class
            if (ClassSearch_Text.Value.ToString() != "")
                query += "  AND ClassNum LIKE '%" + ClassSearch_Text.Value.ToString() + "%'";
            if (CRNSearch_Text.Value.ToString() != "")
                query += "  AND CRN = '" + CRNSearch_Text.Value.ToString() + "'";
            if (DropDownList_searchInstructor.SelectedIndex != 0)
                query += "  AND Faculty = '" + DropDownList_searchInstructor.SelectedValue + "'";

            string major = DropDownList_searchMajor.SelectedValue;
            if (DropDownList_searchMajor.SelectedIndex != 0)
                query += "  AND (M1 = '" + major + "' OR M2 = '" + major + "' OR M3 = '" + major + "' OR M4 = '" + major + "')";

            if (DropDownList_searchCalYear.SelectedIndex != 0)
                query += " AND CalYear = '" + DropDownList_searchCalYear.SelectedValue + "'";


            if (DropDownList_searchTerm.SelectedIndex != 0)
                query += " AND Term = '" + DropDownList_searchTerm.SelectedValue + "'";


            if (DropDownList_searchClassYear.SelectedIndex == 1)
                query += " AND Fr = 1";
            else if (DropDownList_searchClassYear.SelectedIndex == 2)
                query += " AND So = 1";
            else if (DropDownList_searchClassYear.SelectedIndex == 3)
                query += " AND Ju = 1";
            else if (DropDownList_searchClassYear.SelectedIndex == 4)
                query += " AND Se = 1";

            if (CheckBox_conflicts.Checked)
            {
                query += " AND Conflicts = 1 AND Final = 0 ";
            }

            

            int checkCount = 0;

            if (CheckBox_className.Checked)
            {
                query += " ORDER BY ClassNum ASC";
                checkCount++;

            }
            if (CheckBox_CRN.Checked)
            {
                query += " ORDER BY CRN ASC";
                checkCount++;
            }
            if (CheckBox_instructor.Checked)
            {
                query += " ORDER BY faculty ASC";
                checkCount++;
            }
            if (CheckBox_calYear.Checked)
            {
                query += " ORDER BY calYear DESC";
                checkCount++;
            }
            
            if (checkCount > 1)
            {
                Response.Write(
                         "<script type=\"text/javascript\">" +
                         "alert('Cannot ORDER BY more than one column. Please limit selected to 1')" +
                         "</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_search').openModal({ });", true);

            }
            //Response.Write(query);
            if (checkCount == 0)
                query += " ORDER BY classNum";
            if(checkCount < 2)
            {
                
                try
                { // public schedule
                    connection.Open();
                    command = new MySqlCommand(query, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(command);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();

                }
                catch (Exception ex) {  }
                finally { connection.Close(); }
                //query = "Select * from schedule where public = 3 intersect select * from schedule where public = 1 AND ClassNum LIKE '%123%' ORDER BY CRN ASC";

                if (CheckBox_default.Checked)
                {
                    string command = "update userdata SET query = @query WHERE username = '" +Session["New"]+ "'";
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.Parameters.AddWithValue("@query", query);

                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex) {  }
                    finally
                    {
                        connection.Close();
                    }



                    Response.Write(
                         "<script type=\"text/javascript\">" +
                         "alert('Updated Your Default View')" +
                         "</script>");
                }

                
                
                else
                {
                    Response.Write(
                         "<script type=\"text/javascript\">" +
                         "alert('Showing Search Results')" +
                         "</script>");
                }

                

                //save search query -- works but not sure if they want this
                string str = "update userdata SET lastquery = @lastquery WHERE username = '" + Session["New"] + "'";
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(str, connection);
                    cmd.Parameters.AddWithValue("@lastquery", query);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex) { }
                finally
                {
                    connection.Close();
                }


            }



        }

        protected void Button_Finalize_Click(object sender, EventArgs e)
        {
            //34
            string command = "";



            if (GridView1.SelectedIndex == -1)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No PUBLIC Class Selected')" +
                        "</script>");
            }
            else
            {
                try
                {
                    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                    connection.Open();
                    string checkuser = "select count(*) from schedule where Final = 1 AND CRN = '" + GridView1.SelectedRow.Cells[1].Text + "'";
                    MySqlCommand com = new MySqlCommand(checkuser, connection);
                    int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                    
                    if(temp==0)
                        command = "UPDATE schedule SET Final = '1' WHERE CRN = " + GridView1.SelectedRow.Cells[1].Text;
                    else
                        command = "UPDATE schedule SET Final = '' WHERE CRN = " + GridView1.SelectedRow.Cells[1].Text;
                    MySqlCommand cmd = new MySqlCommand(command, connection);
                    cmd.ExecuteNonQuery();
                }
                catch { }
                finally { connection.Close();
                    
                }
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT lastquery from userdata where username = '" + Session["New"] + "'", connection);
                    string recentQuery = cmd.ExecuteScalar().ToString();




                    cmd = new MySqlCommand(recentQuery, connection);
                    table = new DataTable();
                    data = new MySqlDataAdapter(cmd);
                    data.Fill(table);
                    GridView1.DataSource = table;
                    GridView1.DataBind();
                }
                catch(Exception ex) { }
                finally { connection.Close(); }




                
                
            }
        }

        protected void Button_deleteSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxList_deleteSelect.Items)
                item.Selected = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClasses').openModal({ });", true);
        }

        protected void Button_deleteUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxList_deleteSelect.Items)
                item.Selected = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClasses').openModal({ });", true);
        }

        protected void Button_deleteUpdateMajor_Click(object sender, EventArgs e)
        {
            //unselect all
            foreach (ListItem item in CheckBoxList_deleteSelect.Items)
                item.Selected = false;

            string query = "";
            if (DropDownList_deleteSelectMajor.SelectedIndex == 0 || DropDownList_deleteSelectMajor.SelectedIndex == 1)
                query = "SELECT * FROM schedule WHERE CalYear = '" + DropDownList_deleteSelectYear.SelectedValue + "' AND Term = '" + DropDownList_deleteSelectQuarters.SelectedValue + "' AND calYEAR = '" + DropDownList_deleteSelectYear.SelectedValue + "' ORDER BY ClassNum ASC";
            else
                query = "SELECT * FROM schedule WHERE CalYear = '" + DropDownList_deleteSelectYear.SelectedValue + "' AND TERM = '" + DropDownList_deleteSelectQuarters.SelectedValue + "' AND calYEAR = '" + DropDownList_deleteSelectYear.SelectedValue + "' AND(M1 = '" + DropDownList_deleteSelectMajor.SelectedValue + "' OR M2 = '" + DropDownList_deleteSelectMajor.SelectedValue + "' OR M3 = '" + DropDownList_deleteSelectMajor.SelectedValue + "' OR M4 = '" + DropDownList_deleteSelectMajor.SelectedValue + "') ORDER BY ClassNum ASC";

            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CheckBoxList_deleteSelect.DataSource = reader;
                            CheckBoxList_deleteSelect.DataValueField = "CRN";
                            CheckBoxList_deleteSelect.DataTextField = "ClassNum";
                            CheckBoxList_deleteSelect.DataBind();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No Classes Exist')" +
                        "</script>");
                //
            }
            finally { connection.Close(); }




            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClasses').openModal({ });", true);
        }

        protected void Button_deleteClasses_Click(object sender, EventArgs e)
        {
            int CRN = 0;
            bool success = false;
            int count = 0;
            for (int i = 0; i < CheckBoxList_deleteSelect.Items.Count; i++)
            {
                if (CheckBoxList_deleteSelect.Items[i].Selected)
                {
                    try
                    {
                        //Response.Write(CheckBoxList_deleteSelect.SelectedValue);
                        string sqlCommand = "DELETE FROM SCHEDULE WHERE CRN = '" + CheckBoxList_deleteSelect.SelectedValue + "'";
                        connection.Open();
                        command = new MySqlCommand(sqlCommand, connection);
                        int numRowsUpdated = command.ExecuteNonQuery();
                        count++;
                        success = false;
                    }
                    catch(Exception ex) { }
                    finally { connection.Close(); success = true; }

                }
                
            }
            if (count == 0)
            {
                Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('No classes selected')" +
                            "</script>");
            }
            if (success)
            {
                Response.Write(
                            "<script type=\"text/javascript\">" +
                            "alert('(" + count + ") classes successfully deleted')" +
                            "</script>");
            }
        }


        protected void Button_DeleteClassesShow_Click(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.Button_returnDelClasses.UniqueID;
            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("select * from major ORDER BY major ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteSelectMajor.DataSource = reader;
                            DropDownList_deleteSelectMajor.DataValueField = "major";
                            DropDownList_deleteSelectMajor.DataTextField = "major";
                            DropDownList_deleteSelectMajor.DataBind();
                        }
                    }
                }
                //Add blank item at index 0.
                DropDownList_deleteSelectMajor.Items.Insert(0, new ListItem("Select Major", ""));
                DropDownList_deleteSelectMajor.Items.Insert(1, new ListItem("All", ""));

            }
            catch (Exception ex) { }
            finally { connection.Close(); }


            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("select DISTINCT calYear from schedule order by calYear ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DropDownList_deleteSelectYear.DataSource = reader;
                            DropDownList_deleteSelectYear.DataValueField = "calYear";
                            DropDownList_deleteSelectYear.DataTextField = "calYear";
                            DropDownList_deleteSelectYear.DataBind();
                        }
                    }
                }


            }
            catch (Exception ex) { }
            finally { connection.Close(); }


            try
            {
                connection.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM schedule WHERE CalYear = '" + DropDownList_copiedYear.SelectedValue + "' AND Term = '" + DropDownList_CopyTerm.SelectedValue + "' ORDER BY ClassNum ASC", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CheckBoxList_deleteSelect.DataSource = reader;
                            CheckBoxList_deleteSelect.DataValueField = "CRN";
                            CheckBoxList_deleteSelect.DataTextField = "ClassNum";
                            CheckBoxList_deleteSelect.DataBind();

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Response.Write(
                        "<script type=\"text/javascript\">" +
                        "alert('No Classes Exist')" +
                        "</script>");

            }
            finally { connection.Close(); }





            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modal_deleteClasses').openModal({ });", true);
        }

        protected void Button_returnDefault(object sender, EventArgs e)
        {

        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                GridView2.Rows[i].Cells[5].Text = civilianTime(GridView2.Rows[i].Cells[5].Text);
                GridView2.Rows[i].Cells[6].Text = civilianTime(GridView2.Rows[i].Cells[6].Text);
            }
        }
    }
    
}