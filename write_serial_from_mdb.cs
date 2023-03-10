using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

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
            string data = "A1" + lane + "2 " + timeint2str(mytime) + "L01";
            send(data);
        }
        public static void goal(int lane, int mytime)
        {
            string data = "A1" + lane + "2 " + timeint2str(mytime) + "G01";
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
                return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2, '0') + "." + centiseconds.ToString().PadLeft(2, '0');
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
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), "Even");
            _serialPort.DataBits = 7;
            _serialPort.WriteTimeout = -1;
            _serialPort.Open();
            WriteData();
        }
	/*
        public static void WriteData2()
        {
            string fromMDB = misc.get_mdb_file("Swim01");
	    int	prgNo;

            access_db.program_db prgDB = new access_db.program_db(fromMDB);
	    int	distance;
            for (prgNo=1; prgNo<=access_db.program_db.Get_max_program_no(); prgNo++) {
		distance=prgDB.Get_distance(prgNo);
		Console.WriteLine("prgno {0}  distance {1}", prgNo, distance);
	    }
	}
	*/
	    

        public static void WriteData()
        {
            string fromMDB = misc.get_mdb_file("Swim01");
            access_db.program_db prgDB = new access_db.program_db(fromMDB);
            int prgNo;
            int kumi;
            int laneNo;
            int lapcount;
            string startdata = "AR       0.0 S  ";
//	    string[] goalTime = new string[10];;
	    int lapInterval=prgDB.Get_lap_interval();;
	    int maxLapcount;
	    int maxLaneNo=prgDB.Get_max_laneNo();
	    int maxKumi;
            for (prgNo=1; prgNo<=access_db.program_db.Get_max_program_no(); prgNo++)
            {
		maxLapcount=prgDB.Get_distance(prgNo)/50;
		maxKumi=prgDB.Get_max_kumi(prgNo);

                for (kumi=1; kumi<=maxKumi; kumi++)
                {
		    send(startdata);
                    for (lapcount=1; lapcount<maxLapcount; lapcount++)
                    {
                        for (laneNo=1; laneNo<=maxLaneNo; laneNo++)
                        {
			    string mytime=prgDB.get_laptime(prgNo, kumi, laneNo, lapcount);
			    if (mytime=="") continue;
			    Console.WriteLine(" prgNo : {0}  kumi : {1}  laneNo: {2} time: {3}",
				    prgNo, kumi, laneNo, mytime);
			    lap(laneNo,timestr2int(mytime));
			    //??????????????????????????????????????????
			    //lap(laneNo,timestr2int(mytime));
			    //lap(laneNo,timestr2int(mytime));
                        }
			Thread.Sleep(8000);
                    }
		    for (laneNo=1; laneNo<=maxLaneNo; laneNo++)
		    {
			string mytime=prgDB.get_goal_time(prgNo, kumi, laneNo);
			if (mytime=="") continue;
			Console.WriteLine(" Goal! prgNo : {0}  kumi : {1}  laneNo: {2} time: {3}",
				prgNo, kumi, laneNo, mytime);
			goal(laneNo,timestr2int(mytime));
			//??????????????????????????????????????????
			//goal(laneNo,timestr2int(mytime));
			//goal(laneNo,timestr2int(mytime));
		    }
		    
		    Thread.Sleep(10000);
                }
            }
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



    public class program_db
    {

        const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=";
        string mdbFileName;
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

        string connectionString;

        readonly static string[] ShumokuTable = new string[NUMSTYLE]
        { "自由形","背泳ぎ","平泳ぎ","バタフライ", "個人メドレー", "リレー", "メドレーリレー" };
        readonly static string[] distanceTable = new string[NUMDISTANCE] { "25m","50m",
        "100m","200m","400m", "800m", "1500m" };
        public readonly static string[] GenderStr = new string[3] { "男子", "女子", "混合" };




        public static int Get_max_program_no() { return maxProgramNo; }
        public static int Get_first_uid() { return get_uid_from_prgno(1); }

        public static int if_not_null(object dr)
        {
            if (dr == DBNull.Value) return 0;
            return Convert.ToInt32(dr);
        }


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

        public static string get_phase_from_uid(int UID)
        {
            if (UID < 1) { return null; }
            if (UID > maxUID) { return null; }
            return phase[UID];
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

        public program_db(String mdbFilePath)
        {
            mdbFileName = mdbFilePath;

            connectionString = magicWord + mdbFileName;
            OleDbConnection conn = new OleDbConnection(connectionString);
            init_program_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_program_db_(conn);
        }
        public void init_program_db_array(OleDbConnection conn)
        {
//            String sql = "SELECT UID FROM プログラム where UID=(select max(UID) from プログラム);  ";
            String sql = "select max(UID) as maxuid from プログラム;  ";


            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    dr.Read();
		    maxUID = Convert.ToInt32(dr["maxuid"]);
		    redim_program_db_array();
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
	public int Get_distance(int prgNo) {
            int uid = program_db.get_uid_from_prgno(prgNo);

	    string strDistance;
	    strDistance=distancebyUID[uid];
	    return Convert.ToInt32(strDistance.Substring(0, strDistance.Length-1));;
	}

	public int Get_max_kumi(int prgNo) {
	    int		maxKumi;
	    connectionString = magicWord + mdbFileName;

            int uid = program_db.get_uid_from_prgno(prgNo);
            OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
                String sql = "SELECT  MAX(組) AS maxkumi FROM 記録マスター WHERE UID=" + uid + ";";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (OleDbDataReader dr = comm.ExecuteReader())
                {
		    dr.Read();
		    maxKumi= Convert.ToInt32(dr["maxkumi"]);
		}
	    }
	    return maxKumi;
	}

        public string get_goal_time(int prgNo, int kumi, int laneNo)
        {
            int swimmerID;

            connectionString = magicWord + mdbFileName;

            int uid = program_db.get_uid_from_prgno(prgNo);
            OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
                String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス,ゴール " +
                             "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid +
                             " AND 水路=" + laneNo + ";";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();

                using (OleDbDataReader dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        laneNo = Convert.ToInt32(dr["水路"]);
                        if (laneNo > 10) continue;
                        swimmerID = if_not_null(dr["選手番号"]);
                        if (swimmerID > 0)
                        {
                            if (Convert.ToInt32(dr["事由入力ステータス"]) == 0)
                            {
                                if (dr["ゴール"] == DBNull.Value) return "";
                                else
                                {
                                    return Convert.ToString(dr["ゴール"]).Trim();
                                }
                            }
                        }
                        else return "";
                    }
                    return "";
                }
            }
        }
        public static string record_concatenate(object stra, object strb, object strc)
        {
            string returnStr = "";
            if (stra != DBNull.Value) returnStr = Convert.ToString(stra);
            if (strb != DBNull.Value) returnStr += Convert.ToString(strb);
            if (strc != DBNull.Value) returnStr += Convert.ToString(strc);
            return returnStr;
        }
        public static string get_ith_lap(string lapstr, int ith)
        {

            const int ELEMENTLENGTH = 18;
            if (lapstr.Length >= ELEMENTLENGTH * ith)
            {
                return lapstr.Substring(ELEMENTLENGTH * (ith - 1), ELEMENTLENGTH).Trim();
            }

            return "";
        }
	public int Get_max_laneNo() {
	    int maxLaneNo;
	    int maxLaneNo4tf;
	    int maxLaneNo4f;
	    OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
                String sql = "SELECT 使用水路予選, 使用水路タイム決勝, 使用水路決勝 FROM 大会設定 ;";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (OleDbDataReader dr = comm.ExecuteReader())
                {
                    dr.Read();
		    maxLaneNo=if_not_null(dr["使用水路予選"]);
		    maxLaneNo4tf=if_not_null(dr["使用水路タイム決勝"]);
		    maxLaneNo4f=if_not_null(dr["使用水路決勝"]);
		    if (maxLaneNo< maxLaneNo4tf) maxLaneNo=maxLaneNo4tf;
		    if (maxLaneNo< maxLaneNo4f) maxLaneNo=maxLaneNo4f;
		}
	    }
	    return maxLaneNo;
	}

	public int Get_lap_interval() {
	    int poolID;
	    int touchBoardID;
	    OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
                String sql = "SELECT プール, タッチ板 FROM 大会設定 ;";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (OleDbDataReader dr = comm.ExecuteReader())
                {
                    dr.Read();
		    poolID=if_not_null(dr["プール"]);
		    touchBoardID = if_not_null(dr["タッチ板"]);
		    if ((poolID>2)&&(touchBoardID==4)) return 50;
		    if ((poolID<3)&&(touchBoardID==2)) return 100;
		}
	    }
	    return 50;
	}

        public string get_laptime(int prgNo, int kumi, int laneNo, int lapNo)
        {
            int swimmerID;
            string lapTimeString;


            int uid = program_db.get_uid_from_prgno(prgNo);
            OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
                String sql = "SELECT UID, 選手番号, 組, 水路, ラップ１, ラップ２, ラップ３,事由入力ステータス " +
                                    "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid +
                                    " AND 水路=" + laneNo + ";";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (OleDbDataReader dr = comm.ExecuteReader())
                {
		    while (dr.Read())
		    {
			swimmerID = if_not_null(dr["選手番号"]);
			if (swimmerID > 0)
			{
			    lapTimeString = record_concatenate(dr["ラップ１"], dr["ラップ２"], dr["ラップ３"]);
			    return get_ith_lap(lapTimeString, lapNo);
			}
			else return "";
		    }
		    return "";
                }
            }
        }

    }
}
