
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;




namespace SwimSys
{

    public partial class Form1 : Form
    {

        public const string setupFileName = "swmsys.setup.txt";
        public const int NUMSTYLE = 7;
        public const int NUMDISTANCE = 7;

        public const int TIME4DNS = 999999;
        public const int TIME4DQ = 999998;
        public const int DNS = 1;
        public const int DQ = 2;
        

        static Encoding shiftjisEnc = Encoding.GetEncoding("Shift-JIS");
        public bool StopAutoRun;
        // related 大会設定
        public string EventName;
        public string EventDate;
        public string EventVenue;


        event_db evtDB;
        program_db prgDB;
        class_db clsDB;
        team_db tmDB;
        swimmer_db swmDB;




        static public string[] Team = new string[100];
        static public int LastPrgNo;
        static public int FirstPrgNo;
        static public int MaxTeamNum=0;
        // for チームマスター
        static public string[] TeamName4Relay;
 

        public Form1()
        {  
            InitializeComponent();
            init_size(930,780);

          
            set_size_of_list_box(900, 520);
            MDB.folderName = get_folder_name(setupFileName);
            if (MDB.folderName != "")
            {
                txtBxFolder.Text = MDB.folderName;
                call_showEventList();
            }   
        }

        private string get_folder_name(string path)
        {
            string rcString;
            rcString = "";

            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[1024];
                

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        rcString=System.Text.Encoding.UTF8.GetString(b).Trim('\0');

                    }
                }
            } 
            return rcString;

        }
        private void set_folder_name(string path)
        {

            using (FileStream fs = File.Create(path))
            {
                Byte[] folderNameByte = new UTF8Encoding(true).GetBytes(MDB.folderName);
                fs.Write(folderNameByte, 0, folderNameByte.Length);
            }
            
        }
        private void set_size_of_list_box(int width, int height)
        {
            lbxDbContents.Width = width;
            lbxDbContents.Height = height;
        }
        private void init_size(int width, int height) {
            this.Width = width;
            this.Height = height;
        }
       
        
        private void read_mdb(string connectionString)
        {

            evtDB = new event_db(connectionString);
            prgDB = new program_db(connectionString);
            clsDB = new class_db(connectionString);
            tmDB = new team_db(connectionString);
            swmDB = new swimmer_db(connectionString);

        }
        public void update_record(string connectionString, int classNo, int gender, string distance, string shumoku,
            string record, string swimmer,  string shozoku = "", string date = "", string venue = "")
        {
            OleDbConnection conn = new OleDbConnection(connectionString);
            // init_record_db_array(conn);
            conn = new OleDbConnection(connectionString);
            string oldrecord="";
            String sql;
            OleDbCommand comm;
            
            using (conn)
            {
                uint exeError = 0;
                sql = "SELECT 記録区分番号, 性別, 種目, 距離, 記録, 日付, 記録保持者, 所属, 会場 " +
                " from 新記録 where 性別=" + gender + "AND 種目=\"" + shumoku + "\" AND 距離=\"" + distance +
                "\" AND 記録区分番号=" + classNo + ";";
                comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                        oldrecord = (string) dr["記録"];
                }
                if ( misc.time2int(oldrecord)> misc.time2int(record))
                {
                    sql = "update 新記録 set  記録 = \"" + record + "\" where  性別=" + gender + " AND 種目=\"" + shumoku +
                                        "\" AND 距離=\"" + distance + "\" AND 記録区分番号=" + classNo + ";";
                    comm = new OleDbCommand(sql, conn);
                    try { comm.ExecuteNonQuery(); }
                    catch (Exception) {  exeError = exeError | 1; }
                    sql = "update 新記録 set 記録保持者=\"" + swimmer + "\" where  性別=" + gender + " AND 種目=\"" + shumoku +
                        "\" AND 距離=\"" + distance + "\" AND 記録区分番号=" + classNo + ";";
                    comm = new OleDbCommand(sql, conn);
                    try { comm.ExecuteNonQuery(); }
                    catch (Exception ) { exeError = exeError | 2; }
                    sql = "update 新記録 set 日付= \"" + date.Trim() + "\" where  性別=" + gender + " AND 種目=\"" + shumoku +
                        "\" AND 距離=\"" + distance + "\" AND 記録区分番号=" + classNo + ";";
                    comm = new OleDbCommand(sql, conn);
                    try { comm.ExecuteNonQuery(); }
                    catch (Exception ) { exeError = exeError | 4; }
                    sql = "update 新記録 set 所属=\"" + shozoku + "\" where  性別=" + gender + " AND 種目=\"" + shumoku +
                        "\" AND 距離=\"" + distance + "\" AND 記録区分番号=" + classNo + ";";
                    comm = new OleDbCommand(sql, conn);
                    try { comm.ExecuteNonQuery(); }
                    catch (Exception ) { exeError = exeError | 8; }
                    sql = "update 新記録 set 会場 =\"" + venue + "\" where  性別=" + gender + " AND 種目=\"" + shumoku +
                        "\" AND 距離=\"" + distance + "\" AND 記録区分番号=" + classNo + ";";
                    comm = new OleDbCommand(sql, conn);
                    try { comm.ExecuteNonQuery(); }
                    catch (Exception ) { exeError = exeError | 16; }
                    if (exeError == 0)
                    {
                        MessageBox.Show(class_db.get_name(classNo) + program_db.GenderStr[gender] + distance + shumoku + "game record was updated . \n" +
                            "old record : " + oldrecord + "  new record : " + record);
                    }
                }
                
            }
        }
        public void check_result_db_if_togetherable(string connectionString)
        {
            int numLanes;
         
            
            read_mdb(connectionString);
            numLanes = event_db.get_max_lane_number();
            result_db.set_number_of_lanes(numLanes);
            int num_occupied_lane;
            result_db.set_connection_string(connectionString);
            int uid = program_db.Get_first_uid();
            while( uid>0)
            {
                num_occupied_lane = result_db.how_many_lanes_occupied(uid);
                uid = program_db.Get_next_uid(uid);
            }
           
        }
        public void read_result_db(string connectionString)
        {

            int swimmerNo;
            string swimmerName;
            int uid;
            int classNo;
            string style;
            string distance;
            int gender;
            string record;
            string shozoku;
            string date;
            string venue;
            read_mdb(connectionString);
          

            String sql = "SELECT UID, 選手番号, ゴール, 事由入力ステータス, 新記録印刷マーク " +
                                     "FROM 記録マスター  WHERE 新記録印刷マーク=\"大会新\";"; // 
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();

                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        swimmerNo = misc.if_not_null(dr["選手番号"]);
                        swimmerName = swmDB.get_name(swimmerNo);
                        shozoku = swmDB.get_team_name(swimmerNo);
                        date = evtDB.get_eventDate();
                        venue = evtDB.get_eventVenue();
                        uid = misc.if_not_null(dr["UID"]);
                        classNo = program_db.get_class_int_from_uid(uid);
                        gender = program_db.get_gender_int_from_uid(uid);
                        distance = program_db.get_distance_from_uid(uid);
                        style = program_db.get_shumoku_from_uid(uid);
                        record = (string)dr["ゴール"];
                        //compare_record(connectionString, classNo, gender, distance, style, record, swimmerName,shozoku,date,venue);
                        update_record(connectionString, classNo, gender, distance, style, record, swimmerName,shozoku,date,venue);
                    }
                }
            }

        }
        private void btnUpdateGamerecord_Click(object sender, EventArgs e)
        {
            const string magicWord = "Provider=Microsoft.jet.OLEDB.4.0; Data Source=";

            string selectedString;
            string[] sep = { " " };
            selectedString = lbxDbContents.SelectedItem.ToString();
            //------------------------
            string[] eventInfo = selectedString.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            string fullpathDBName = MDB.folderName + "\\" + eventInfo[0];
            string connectionString = magicWord + fullpathDBName;

            read_result_db(connectionString);

        }
        private void call_showEventList()
        {
            DialogResult dr=MessageBox.Show("大会名を読み込みます","OK?",MessageBoxButtons.YesNo);
            if (dr==System.Windows.Forms.DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                MDB.folderName = txtBxFolder.Text;
                showEventList(MDB.folderName);
                this.Cursor = Cursors.Default;
            }
            
        }
        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog dbPickDialog = new FolderBrowserDialog();
            dbPickDialog.Description = "データベースパスの選択";
            dbPickDialog.RootFolder = Environment.SpecialFolder.UserProfile;
            dbPickDialog.SelectedPath = MDB.folderName;
            DialogResult result = dbPickDialog.ShowDialog();
            if (result==DialogResult.OK)
            {
                MDB.folderName = dbPickDialog.SelectedPath;
                txtBxFolder.Text = MDB.folderName;
                set_folder_name(setupFileName);
                call_showEventList();
                
            }
        }
        

        public static void Read_event_db(string dbFileName,ref string eventName, ref string eventDate, ref string eventVenue)
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
              "Data Source="+dbFileName;

            event_db evtDB = new event_db(connectionString);
            
            {
                eventName = evtDB.get_eventName();
                eventDate = evtDB.get_eventDate();
                eventVenue = evtDB.get_eventVenue();
            }
                
        }
        private void btnShowEventList_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("大会名を読み込みます", "OK?", MessageBoxButtons.YesNo);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                MDB.folderName = txtBxFolder.Text;
                showEventList(MDB.folderName);
                this.Cursor = Cursors.Default;
            }
        }
        public void showEventList(string dbPath)
        {
            string[] eventNameA = new string[100];
            string[] eventVenueA = new string[100];
            string[] eventDateA = new string[100];
            string[] fileName = new string[100];
            int counter = 0;
            int numRecords;

            int myLength;
            int[] myByteLen = new int[100];
            string eventName = "";
            string myDate = "";
            string myVenue = "";

            lbxDbContents.Items.Clear();

            try
            {
                var txtFiles = Directory.EnumerateFiles(dbPath, "*.mdb");
                int maxEventNameLength = 0;
                foreach (string currentFile in txtFiles)
                {
                    fileName[counter] = currentFile.Substring(currentFile.Length -10);
                    try
                    {
                        Read_event_db(currentFile, ref eventName, ref myDate, ref myVenue);
                        if (eventName != null)
                        {
                            myLength = eventName.Length;
                            myByteLen[counter] = shiftjisEnc.GetByteCount(eventName);
    			            if (maxEventNameLength < myByteLen[counter]) maxEventNameLength = myByteLen[counter];
				            eventNameA[counter] = eventName;
				            eventDateA[counter] = myDate;
				            eventVenueA[counter] = myVenue;
				            counter++;
			            }
    			    }
		            catch (Exception e)
		            {
			            MessageBox.Show(e.Message);
		            }

                }
                numRecords = counter - 1;
                lbxDbContents.Items.Add("File名      大会名" + new string(' ', maxEventNameLength + 2) +
                    "期間" + new string(' ', 14) + "場所");
                for (counter = 0; counter <= numRecords; counter++)
                {
                    lbxDbContents.Items.Add(fileName[counter] + "  " + eventNameA[counter] +
                        new string(' ', (maxEventNameLength + 2 - myByteLen[counter]))
                         + eventDateA[counter] + "  " + eventVenueA[counter]);
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnWriteRecord_Click(object sender, EventArgs e)
        {

        }
    }
}
 


        
