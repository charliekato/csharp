
using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;

namespace serialWrite
{
    public static class misc
    {

        public static string get_mdb_file(string initFile)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = initFile;
            ofd.InitialDirectory = @"C:\Users\ykato\Dropbox\Private\水泳協会\SwimDB\";
            ofd.Filter = "アクセスMDBファイル(*.mdb)|*.mdb";
            ofd.FilterIndex = 2;
            ofd.Title = "MDBファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                Console.WriteLine("> {0} <", ofd.FileName);
                return (ofd.FileName);
            }
            return (initFile);
        }
    }



    public class serialSend
    {
        static SerialPort _serialPort;
        static byte[] sendData;
        public static void init_data()
        {

            sendData = new byte[256];
            byte counter = 0;
            for (counter = 0; counter == 0; counter++)
            {
                sendData[counter] = counter;
            }
        }
        public static void Write()
        {
            byte i;
            for (i = 0; i < 100; i++)
            {
                _serialPort.Write(sendData, 0, 256);
            }
        }
        public static void send(string data)
        {
            byte stx = 2;
            byte etx = 3;
            byte[] ctrdat = { stx, etx }; // stx, etx

            _serialPort.Write(ctrdat, 0, 1);
            _serialPort.Write(data);
            _serialPort.Write(ctrdat, 1, 1);
        }
        public static void lap(int lane, int mytime)
        {
            string data = "A1" + lane + "  " + timeint2str(mytime) + "L01";
            send(data);
        }
        public static void goal(int lane, int mytime)
        {
            string data = "A1" + lane + "  " + timeint2str(mytime) + "G01";
            send(data);
        }
        public static string timeint2str(int mytime)
        {
            int minutes = mytime / 10000;
            int temps = mytime % 10000;
            int seconds = temps / 100;
            int centiseconds = temps % 100;
            if (minutes > 0)
            {
                return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2) + "." + centiseconds.ToString().PadLeft(2, '0');
            }
            return "   " + seconds.ToString().PadLeft(2) + "." + centiseconds.ToString().PadLeft(2, '0');
        }
        public static int timestr2int(string mytime)
        {
            int position;
            position = mytime.IndexOf(':');
            if (position >= 0)
            {
                mytime = mytime.Remove(position, 1);
            }

            position = mytime.IndexOf('.');

            mytime = mytime.Remove(position, 1);

            return Convert.ToInt32(mytime);
        }
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            _serialPort = new SerialPort();

            _serialPort.PortName = SetPortName(_serialPort.PortName); // Usually COM3
            _serialPort.BaudRate = 9800;
            _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), "Even");
            _serialPort.DataBits = 7;
            _serialPort.WriteTimeout = -1;
            _serialPort.Open();
            WriteData();
        }
        public static void WriteData()
        {
            string fromMDB = misc.get_mdb_file("Swim01");
            program_db prgDB = new program_db(fromMDB);
        }

        public static void WriteOrg()
        {
            string startdata = "AR       0.0 S  ";
            int skip;
            for (int i = 0; i < 10; i++)
            {
                skip = i % 10;
                send(startdata);
                for (int l = 1; l < 10; l++)
                {
                    if (l != skip)
                    {
                        lap(l, 2934 - l + i);
                    }
                }
                //Thread.Sleep(2000);
                for (int l = 1; l < 10; l++)
                {
                    if (l != skip)
                        goal(l, 12345 + l + i);
                }
                //Thread.Sleep(2000);
            }

        }
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("COM port({0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "")
            {
                portName = defaultPortName;
            }
            return portName;
        }

    }

}

namespace access_db
{
    public class class_db
    {
        static string[] className;
        public class_db(string connectionString)
        {

            OleDbConnection conn = new OleDbConnection(connectionString);
            init_class_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_class_db_(conn);
        }
        public static string get_name(int classID)
        {
            return className[classID];
        }
        public void init_class_db_array(OleDbConnection conn)
        {
            String sql = "SELECT 番号 FROM クラス where 番号=(select max(番号) from クラス);  ";
            int maxClassNumber;

            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        maxClassNumber = Convert.ToInt32(dr["番号"]);
                        Array.Resize<string>(ref className, maxClassNumber + 1);
                    }
                }
            }
        }
        public void read_class_db_(OleDbConnection conn)
        {

            String sql = "SELECT 番号,クラス名称 FROM クラス;";
            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (OleDbDataReader dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        className[Convert.ToInt32(dr["番号"])] = Convert.ToString(dr["クラス名称"]);

                    }
                }

            }
        }
    }

    public class program_db
    {
        static int[] classNumberbyUID;
        static string[] phase;
        static string[] distancebyUID;
        static int[] GenderbyUID;
        static int[] ShumokubyUID;
        static int[] raceNobyUID;
        static int[] UIDFromRaceNo;
        static int[] numSwimmers;
        static int maxUID;
        static int maxProgramNo;
        const int NUMSTYLE = 7;
        const int NUMDISTANCE = 7;

        const string magicWord = "Provider=Microsoft.jet.OLEDB.4.0; Data Source=";
        string connectionString;

        readonly static string[] ShumokuTable = new string[NUMSTYLE]
        { "自由形","背泳ぎ","平泳ぎ","バタフライ", "個人メドレー", "リレー", "メドレーリレー" };
        readonly static string[] distanceTable = new string[NUMDISTANCE] { "25m","50m",
    "100m","200m","400m", "800m", "1500m" };
        public readonly static string[] GenderStr = new string[3] { "男子", "女子", "混合" };




        public static int Get_max_program_no() { return maxProgramNo; }
        public static int Get_first_uid() { return get_uid_from_prgno(1); }
        public static int Incr_num_swimmers(int uid)
        {
            return ++numSwimmers[uid];
        }
        public static int Get_num_swimmers(int uid) { return numSwimmers[uid]; }
        public static int Get_next_uid(int uid)
        {
            int prgNo = get_race_number_from_uid(uid);
            int nextUID;
            do
            {
                prgNo++;
                if (prgNo > maxProgramNo) return 0;
                nextUID = UIDFromRaceNo[prgNo];
            } while (nextUID == 0);

            return nextUID;
        }
        public static int get_prev_uid(int uid)
        {
            int prgNo = get_race_number_from_uid(uid);
            int prevUID;
            do
            {
                prgNo--;
                if (prgNo == 0) return 0;
                prevUID = UIDFromRaceNo[prgNo];
            } while (prevUID == 0);
            return prevUID;
        }
        public static bool inc_race_number(ref int prgNo)
        {
            do
            {
                prgNo++;
                if (prgNo > maxProgramNo)
                {
                    //   dec_race_number(ref prgNo);
                    return false;
                }
            } while (UIDFromRaceNo[prgNo] == 0);
            return true;
        }
        public static void dec_race_number(ref int prgNo)
        {
            do
            {
                prgNo--;
                if (prgNo == 0) return;
            } while (UIDFromRaceNo[prgNo] == 0);
        }
        public static int get_uid_from_prgno(int prgNO)
        {
            return UIDFromRaceNo[prgNO];
        }
        public static string get_gender_from_uid(int UID)
        {
            return GenderStr[GenderbyUID[UID] - 1];
        }
        public static int get_gender_int_from_uid(int UID)
        {
            return GenderbyUID[UID];
        }
        public static string get_class_from_uid(int UID)
        {
            if (UID < 1) { return null; }
            if (UID > maxUID) { return null; }
            return class_db.get_name(classNumberbyUID[UID]);
        }
        public static int get_class_int_from_uid(int UID)
        {
            if (UID < 1) return 0;
            if (UID > maxUID) return 0;
            return classNumberbyUID[UID];
        }


        public static string get_phase_from_uid(int UID)
        {
            if (UID < 1) { return null; }
            if (UID > maxUID) { return null; }
            return phase[UID];
        }
        public static string get_distance_from_uid(int UID)
        {
            if (UID < 1) { return null; }
            if (UID > maxUID) { return null; }
            return distancebyUID[UID];
        }

        public static string get_shumoku_from_uid(int UID)
        {
            if (UID < 1) { return null; }
            if (UID > maxUID) { return null; }
            return ShumokuTable[ShumokubyUID[UID]];
        }
        public static int get_race_number_from_uid(int UID)
        {
            if (UID < 1) { return -1; }
            if (UID > maxUID) { return -1; }
            return raceNobyUID[UID];
        }
        public static bool is_relay(int UID)
        {
            if (ShumokubyUID[UID] > 4) return true;
            return false;
        }
        public static bool is_same_distance_style(int uid1, int uid2)
        {
            //    if (get_distance_from_uid(uid1) != get_distance_from_uid(uid2)) return false;
            if (get_shumoku_from_uid(uid1) != get_shumoku_from_uid(uid2)) return false;
            if (get_phase_from_uid(uid1) != get_phase_from_uid(uid2)) return false;
            return true;
        }
        public program_db(String mdbFileName)
        {

            string connectionString = magicWord + mdbFileName;
            OleDbConnection conn = new OleDbConnection(connectionString);
            init_program_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_program_db_(conn);
        }
        public void init_program_db_array(OleDbConnection conn)
        {
            String sql = "SELECT UID FROM プログラム where UID=(select max(UID) from プログラム);  ";


            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        maxUID = Convert.ToInt32(dr["UID"]);
                        redim_program_db_array();
                    }
                }
                sql = "SELECT 競技番号 FROM プログラム where 競技番号=(select max(競技番号) from プログラム);";
                comm = new OleDbCommand(sql, conn);
                using (var dr = comm.ExecuteReader())
                {
                    dr.Read();
                    maxProgramNo = Convert.ToInt32(dr["競技番号"]);
                    Array.Resize<int>(ref UIDFromRaceNo, maxProgramNo + 1);
                }
            }

        }
        public void redim_program_db_array()
        {

            int ubound = maxUID + 1;
            Array.Resize<int>(ref classNumberbyUID, ubound);
            Array.Resize<string>(ref phase, ubound);
            Array.Resize<string>(ref distancebyUID, ubound);
            Array.Resize<int>(ref GenderbyUID, ubound);
            Array.Resize<int>(ref ShumokubyUID, ubound);
            Array.Resize<int>(ref raceNobyUID, ubound);
            Array.Resize<int>(ref numSwimmers, ubound);

        }
        public int locate_style_number(string styleName)
        {
            int cnt;
            for (cnt = 0; cnt < NUMSTYLE; cnt++)
            {
                if (ShumokuTable[cnt] == styleName) return cnt;
            }
            return -1;
        }
        public int locate_distance_number(string distance)
        {
            int cnt;
            for (cnt = 0; cnt < NUMDISTANCE; cnt++)
            {
                if (distanceTable[cnt] == distance) return cnt;
            }
            return -1;
        }
        public void read_program_db_(OleDbConnection conn)
        {

            String sql = "SELECT UID, 競技番号, 種目, 距離, 性別, 予決, クラス番号  FROM プログラム ;";
            int raceNo;
            int uid;
            using (conn)
            {

                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        raceNo = Convert.ToInt32(dr["競技番号"]);
                        uid = Convert.ToInt32(dr["UID"]);
                        raceNobyUID[uid] = raceNo;
                        UIDFromRaceNo[raceNo] = uid;
                        ShumokubyUID[uid] = locate_style_number(Convert.ToString(dr["種目"]));
                        distancebyUID[uid] = Convert.ToString(dr["距離"]);
                        phase[uid] = Convert.ToString(dr["予決"]);
                        GenderbyUID[uid] = Convert.ToInt32(dr["性別"]);
                        classNumberbyUID[uid] = Convert.ToInt32(dr["クラス番号"]);
                    }
                }
            }
        }
        public void get_goal_time(int prgNo)
        {
            int kumi;
            int swimmerID;

            string connectionString = magicWord + dbfilename;
            bool rc = true;
            int laneNo;

            while (prgNo <= LastPrgNo)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス,ゴール " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();

                    using (OleDbDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            if (laneNo > 10) continue;
                            swimmerID = serialWrite.misc.if_not_null(dr["選手番号"]);
                            if (swimmerID > 0)
                            {
                                if (Convert.ToInt32(dr["事由入力ステータス"]) == 0)
                                {
                                    if (dr["ゴール"] == DBNull.Value) rc = false;
                                    else
                                    {
                                        string goalTime = Convert.ToString(dr["ゴール"]);
                                        if (goalTime == "") rc = false;
                                        else
                                        {
                                            bool newRecord = (misc.time_str2int(goalTime) < program_db.bestRecord[uid]);
                                            if (program_db.is_relay(uid))
                                            {
                                                Controls["lblTime4Relay" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord4Relay" + laneNo);

                                            }
                                            else
                                            {
                                                Controls["lblTime" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord" + laneNo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                program_db.inc_race_number(ref prgNo);
            }
            return rc;
        }


        public void get_laptime(int prgNo)
        {
            int kumi;
            int swimmerID;
            int howManyLaps;
            string lapTimeString;


            int laneNo;
            string thisLaptime;
            int thisLap;

            for (kumi = 1; ; kumi++)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                howManyLaps = how_many_laps(uid) - 1;
                if (howManyLaps <= 0) return;
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, ラップ１, ラップ２, ラップ３,事由入力ステータス " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();
                    using (var dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            if (laneNo > 10) continue;
                            swimmerID = misc.if_not_null(dr["選手番号"]);
                            if (swimmerID > 0)
                            {
                                if (Convert.ToInt32(dr["事由入力ステータス"]) == 0)
                                {
                                    lapTimeString = misc.record_concatenate(dr["ラップ１"], dr["ラップ２"], dr["ラップ３"]);
                                    thisLap = misc.get_lapcounter(laneNo);
                                    if (thisLap <= howManyLaps)
                                    {
                                        thisLaptime = misc.get_ith_lap(lapTimeString, thisLap);
                                        if (thisLaptime != "")
                                        {
                                            misc.inc_lapcounter(laneNo);
                                            if (program_db.is_relay(uid))
                                            {
                                                Controls["lblTime4Relay" + laneNo].Text = misc.format_time(thisLaptime) + " (@" + (thisLap * get_lap_interval()) + ")";
                                            }
                                            else
                                            {
                                                Controls["lblTime" + laneNo].Text = misc.format_time(thisLaptime) + " (@" + (thisLap * get_lap_interval()) + ")";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                program_db.inc_race_number(ref prgNo);
            }

        }
    }
}



