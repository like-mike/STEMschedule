using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;//
using System.Configuration;//
using System.Globalization;



namespace stemSchedule
{
    
    public partial class schedule : System.Web.UI.Page
    {
        SqlCommand com;
        SqlCommand comm;
        SqlConnection conn;
        static int cof;
        protected void Page_Load(object sender, EventArgs e)
        {

            Panel_delete.Visible = false;
            Panel_confirmAdd.Visible = false;
            string time = "08:00";
            string time2 = "09:20";
            string time3 = "09:21";
            DateTime dateTime1 =  DateTime.ParseExact(time, "HH:mm",
                                    CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(time2, "HH:mm",
                                    CultureInfo.InvariantCulture);
            DateTime dateTime3 = DateTime.ParseExact(time3, "HH:mm",
                                    CultureInfo.InvariantCulture);
            if (dateTime3.Ticks >= dateTime1.Ticks && dateTime3.Ticks <= dateTime2.Ticks)
            {
                //this.Label1.Text = "in range";
            }
            if (Session["New"] != null)
            {
                //label_welcome.Text += Session["New"].ToString();
                       
            }
            else
                Response.Redirect("Login.aspx");
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void Button_Logout_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("Login.aspx");
        }

        protected void button_addAppear_Click(object sender, EventArgs e)
        {
            Panel_addClass.Visible = true;
            button_addAppear.Enabled = false;
            
        }

        protected void addQuery()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string insertQuery = "insert into schedule (CRN,Days,StartTime,EndTime,Classroom,Department,CourseNumber,Instructor,startTicks,endTicks) values (@crn,@days,@startTime,@endTime,@classroom,@department,@courseNum,@instructor,@startTicks,@endTicks)";
            com = new SqlCommand(insertQuery, conn);
            com.Parameters.AddWithValue("@crn", TextBoxCRN.Text);
            com.Parameters.AddWithValue("@startTime", TextBox_startTime.Text);

            com.Parameters.AddWithValue("@days", DropDownList_Days.SelectedItem.Text);


            com.Parameters.AddWithValue("@endTime", TextBox_endTime.Text);


            string sTime = "01/01/2000 " + TextBox_startTime.Text;
            string eTime = "01/01/2000 " + TextBox_endTime.Text;
            DateTime ssTime = DateTime.ParseExact(sTime, "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture);
            DateTime eeTime = DateTime.ParseExact(eTime, "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture);

            com.Parameters.AddWithValue("@startTicks", ssTime.Ticks.ToString());
            com.Parameters.AddWithValue("@endTicks", eeTime.Ticks.ToString());
            com.Parameters.AddWithValue("@classroom", DropDownList_classroom.SelectedValue);

            com.Parameters.AddWithValue("@department", DropDownList_dept.SelectedValue);
            com.Parameters.AddWithValue("@courseNum", DropDownList_courses.SelectedValue);
            com.Parameters.AddWithValue("@instructor", Session["New"].ToString());
            
                com.ExecuteNonQuery();
                Response.Redirect("schedule.aspx");
                Response.Write("Add Class Success");
                conn.Close();
            
        }

        protected void Button_addClass_Click(object sender, EventArgs e)
        {
            try
            {
                
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
                conn.Open();
                string insertQuery = "insert into schedule (CRN,Days,StartTime,EndTime,Classroom,Department,CourseNumber,Instructor,startTicks,endTicks) values (@crn,@days,@startTime,@endTime,@classroom,@department,@courseNum,@instructor,@startTicks,@endTicks)";
                com = new SqlCommand(insertQuery, conn);
                string startTime = TextBox_startTime.Text;
                string endTime = TextBox_endTime.Text;
                string days = DropDownList_Days.SelectedItem.Text;
                string department = DropDownList_dept.SelectedValue;
                string courseNum = DropDownList_courses.SelectedValue;
                string instructor = Session["New"].ToString();

                com.Parameters.AddWithValue("@crn", TextBoxCRN.Text);
                com.Parameters.AddWithValue("@startTime", TextBox_startTime.Text);

                com.Parameters.AddWithValue("@days", DropDownList_Days.SelectedItem.Text);


                com.Parameters.AddWithValue("@endTime", TextBox_endTime.Text);


                string sTime = "01/01/2000 " + TextBox_startTime.Text;
                string eTime = "01/01/2000 " + TextBox_endTime.Text;
                DateTime ssTime = DateTime.ParseExact(sTime, "dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture);
                DateTime eeTime = DateTime.ParseExact(eTime, "dd/MM/yyyy HH:mm",
                                    CultureInfo.InvariantCulture);
   
                com.Parameters.AddWithValue("@startTicks", ssTime.Ticks.ToString());
                string startTicks = ssTime.Ticks.ToString();
                com.Parameters.AddWithValue("@endTicks", eeTime.Ticks.ToString());
                string endTicks = eeTime.Ticks.ToString();
                com.Parameters.AddWithValue("@classroom", DropDownList_classroom.SelectedValue);
                string classroom = DropDownList_classroom.SelectedValue;
                string tickFinder = "select count(*) from schedule where '" + ssTime.Ticks + "' between startTicks and endTicks";
                string conflict = "select (CRN) from schedule where '" + ssTime.Ticks + "' between startTicks and endTicks";
                comm = new SqlCommand(tickFinder, conn);
                int temp = Convert.ToInt32(comm.ExecuteScalar().ToString());

                SqlCommand checkConflict = new SqlCommand(conflict, conn);
                
                //string cof = checkConflict.ExecuteScalar().ToString();
                //this.Label_errorConflict.Text = comm.ToString();

                com.Parameters.AddWithValue("@department", DropDownList_dept.SelectedValue);
                com.Parameters.AddWithValue("@courseNum", DropDownList_courses.SelectedValue);
                com.Parameters.AddWithValue("@instructor", Session["New"].ToString());
                if (temp == 1)
                {
                    cof = Convert.ToInt32(checkConflict.ExecuteScalar().ToString());
                    Label_conflict.Text = "Conflict: CRN '" + cof.ToString() + "'";
                    //this.Label_errorConflict.Text = "Cannot add class. Conflict with another";
                    //Label_errorConflict.Visible = true;
                    
                    Panel_confirmAdd.Visible = true;
                    
             
                }
                else
                {
                
                    com.ExecuteNonQuery();
                    Response.Redirect("schedule.aspx");
                    Response.Write("Add Class Success");
                    
                }

                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.ToString());
            }
        }


        protected void Button_addHide_Click(object sender, EventArgs e)
        {
            Panel_addClass.Visible = false;
            button_addAppear.Enabled = true;
        }

        protected void Button_AddHide_Click1(object sender, EventArgs e)
        {
            Panel_addClass.Visible = false;
            button_addAppear.Enabled = true;
        }

        protected void DropDownList_SortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_SortBy.SelectedIndex == 0)
            {
                schedule_SqlDataSource.SelectCommand = "select * from schedule";
            }
            else if (DropDownList_SortBy.SelectedIndex==1)
            {
                schedule_SqlDataSource.SelectCommand = "select * from schedule WHERE Instructor='" + Session["New"].ToString() + "'";
            }
            
        }

        protected void Button_Settings_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin.aspx");
        }

        protected void SqlDataSource_Rooms_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void Button_confirmAdd_Click(object sender, EventArgs e)
        {
            addQuery();
            Response.Redirect("schedule.aspx");
            Response.Write("Add Class Success");
            
        }

        protected void Button_dontAdd_Click(object sender, EventArgs e)
        {
            Panel_confirmAdd.Visible = false;
        }

        protected void Button_showConflict_Click(object sender, EventArgs e)
        {
            schedule_SqlDataSource.SelectCommand = "select * from schedule WHERE CRN = '" + cof.ToString() + "'";
            Panel_confirmAdd.Visible = true;
        }

        protected void Button_Delete_Click(object sender, EventArgs e)
        {
           
        }

        protected void Button_HideDelete_Click(object sender, EventArgs e)
        {
           
        }

        protected void DropDownList_rooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            String room = DropDownList_rooms.SelectedValue.ToString();
            schedule_SqlDataSource.SelectCommand = "select * from schedule WHERE classroom='" + room + "'";
        }

        protected void Button_showDelete_Click(object sender, EventArgs e)
        {
            Panel_delete.Visible = true;
        }

        protected void Button_deleteHide_Click(object sender, EventArgs e)
        {
            Panel_delete.Visible = false;
        }

        protected void Button_delete_Click1(object sender, EventArgs e)
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RegistrationConnectionString"].ConnectionString);
            conn.Open();
            string deleteQuery = "delete from schedule where CRN = '" + TextBox_delete.Text + "'";
            com = new SqlCommand(deleteQuery, conn);
            com.ExecuteNonQuery();
            Response.Redirect("schedule.aspx");
            conn.Close();


        }

        protected void DropDownList_instructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            String instructor = DropDownList_instructor.SelectedValue.ToString();
            schedule_SqlDataSource.SelectCommand = "select * from schedule WHERE Instructor='" + instructor + "'";
        }
    }
}