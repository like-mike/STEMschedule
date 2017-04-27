using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace stemSchedule
{
    public partial class ExcelTest : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }




        protected void ExportToExcel(object sender, EventArgs e)
        {
            string s = "1:30 PM";
            string[] space = s.Split(' ');
            string newtime;





            if (space[1] == "PM")
            {
                string[] colon = space[0].Split(':');
                int hr = Convert.ToInt32(colon[0]);
                hr += 12;
                int min = Convert.ToInt32(colon[1]);
                newtime = hr.ToString() + ":" + min.ToString();
            }
            else
                newtime = space[0].ToString();
            Response.Write(newtime);
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write(TextBox2.Text);
        }
    }
        

    }
