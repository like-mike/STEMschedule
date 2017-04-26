using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Data.OleDb;

namespace stemSchedule
{
    public partial class excel : System.Web.UI.Page
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
            /*connection.Open();
            command = new MySqlCommand(PUBLIC_SCHEDULE, connection);
            table = new DataTable();
            data = new MySqlDataAdapter(command);
            data.Fill(table);
            GridView1.DataSource = table;
            GridView1.DataBind();
            connection.Close();
            connection.Dispose();*/
            

        }

        private void populateDatabaseData()
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {

            string fileName = "";
            //check to make sure a file is selected
            if (FileUpload1.HasFile)
            {
                //create the path to save the file to
                fileName = Path.Combine(Server.MapPath("~/Files"), FileUpload1.FileName);
                //save the file to our local path
                FileUpload1.SaveAs(fileName);
            }

            String name = "Sheet1";
            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            "C:\\Users\\Michael Reese\\Downloads\\" + "Book1.xlsx" +
                            ";Extended Properties='Excel 8.0;HDR=YES;';";

            OleDbConnection con = new OleDbConnection(constr);
            OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
            con.Open();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            DataTable data = new DataTable();
            sda.Fill(data);
            GridView1.DataSource = data;
            GridView1.DataBind();


        }
    }
}