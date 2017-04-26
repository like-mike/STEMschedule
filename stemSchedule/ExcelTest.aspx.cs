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
            string s = "12:30 AM";
            bool AM = true;
            int hr;
            string min;
            string newtime = "";
            // Split string on spaces.
            // ... This will separate all the words.
            string[] space =s.Split(' ');
            if (space[1] == "PM")
                AM = false;
            for (int i = 0; i < 2; i++)
                newtime += space[i];
            string[] colon = newtime.Split(':');
            hr = Int32.Parse(colon[0]);
            min = colon[1];

            if(space[1]=="PM")
            {
                hr += 12;
            }

            Response.Write(newtime);
            

            
            

        }
        

    }
}