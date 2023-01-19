using System;
using System.Windows.Forms;
using System.Data.OleDb;

namespace SwimSys
{
    public static class MDB
    {
        public static string folderName;
        public static string fileName;
    }


    public static class entry_db {
        static string thisConnectionString;
	public static void set_connection_string(string connectionString) {
	    thisConnectionString = connectionString;
        }
        public static void read_entry_db(string connectionString)
        {
            int swimmerNo;
            int uid;
            using (OleDbConnection conn = new OleDbConnection(thisConnectionString))
            {
                String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス " +
                                     "FROM 記録マスター";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();

                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        swimmerNo = misc.if_not_null(dr["選手番号"]);
                        uid = misc.if_not_null(dr["UID"]);
                        program_db.Incr_num_swimmers(swimmerNo);
                    }
                }
            }
        }


    public static class result_db
    {
        static string thisConnectionString;
        static int numLanes;
        public static void set_number_of_lanes(int numLanes) { result_db.numLanes = numLanes; }
      
        public static void set_connection_string(string connectionString)
        {
            thisConnectionString = connectionString;
        }
        public static void read_result_db(string connectionString)
        {
            int swimmerNo;
            int uid;
            using (OleDbConnection conn = new OleDbConnection(thisConnectionString))
            {
                String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス " +
                                     "FROM 記録マスター";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();

                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        swimmerNo = misc.if_not_null(dr["選手番号"]);
                        uid = misc.if_not_null(dr["UID"]);
                        program_db.Incr_num_swimmers(swimmerNo);
                    }
                }
            }
        }
        public static bool race_exist(int uid, int kumi)
        {

            int swimmerNo;
            using (OleDbConnection conn = new OleDbConnection(thisConnectionString))
            {
                String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス " +
                                     "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + " ORDER BY 水路;";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();

                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        swimmerNo = misc.if_not_null(dr["選手番号"]);
                        if (swimmerNo > 0) return true;
                    }
                }
            }
            return false;
        }
        // how_many_lanes_occupied

        public static int how_many_lanes_occupied(int uid)
        {
            int swimmerCode;
            int num_occupied_lane = 0;
            String sql = "SELECT UID, 選手番号, 水路 FROM 記録マスター WHERE UID=" + uid + ";";
            using (OleDbConnection conn = new OleDbConnection(thisConnectionString))
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                

                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        swimmerCode = misc.if_not_null(dr["選手番号"]);
                        if (swimmerCode > 0) num_occupied_lane++;
                        if (num_occupied_lane == numLanes) return numLanes;
                    }
                }

            }
            return num_occupied_lane;
        }





    } //---end of class result_db

    public class event_db
    {
        static int maxLaneNo4Yosen;
        static int maxLaneNo4TimeFinal;
        static int maxLaneNo4Final;
        static int maxLaneNo;
        public static string eventName;
        public static string eventVenue;
        public static string eventDate;
        public static int get_max_lane_number()
        {
            if (maxLaneNo > 0) return maxLaneNo;
            maxLaneNo = maxLaneNo4Final;
            if (maxLaneNo4TimeFinal > maxLaneNo) maxLaneNo = maxLaneNo4TimeFinal;

            if (maxLaneNo4Yosen > maxLaneNo) maxLaneNo = maxLaneNo4Yosen;

            return maxLaneNo;
        }
        public string get_eventName() { return eventName; }
 
        public string get_eventVenue() { return eventVenue; }

        public string get_eventDate() { return eventDate; }
        public event_db(string connectionString)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);
            maxLaneNo = 0;
            read_event_db_(conn);
        }
        void read_event_db_(OleDbConnection conn)
        {

            using (conn)
            {

                String sql = "SELECT 大会名１, 開催地, 始期間, 終期間, 使用水路予選, 使用水路タイム決勝, 使用水路決勝 " +
                 "FROM 大会設定 WHERE 大会名１ IS NOT NULL;";
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                try
                {
                    using (var dr = comm.ExecuteReader())
                    {
                        dr.Read();
                    
                        eventName = misc.if_not_null_string(dr["大会名１"]);
                        if ((misc.if_not_null_string(dr["始期間"])).Equals(misc.if_not_null_string(dr["終期間"])))
                        {
                            eventDate = "      " + misc.if_not_null_string(dr["始期間"]) + "      ";
                        }
                        else
                        {
                            eventDate = misc.if_not_null_string(dr["始期間"]) + "～" + misc.if_not_null_string(dr["終期間"]);
                        }
                        eventVenue = misc.if_not_null_string(dr["開催地"]);
                        maxLaneNo4Final = misc.if_not_null(dr["使用水路決勝"]);
                        maxLaneNo4TimeFinal = misc.if_not_null(dr["使用水路タイム決勝"]);
                        maxLaneNo4Yosen = misc.if_not_null(dr["使用水路予選"]);
                    }
                } catch ( Exception )
                {
                   // MessageBox.Show(e.Message);
                }
                
            }
        }
    }

    public class record_db
    {
        int[] classNumber;
        int[] genderNumber;
        string[] shumoku;
        string[] distance;
        string[] record;

        string[] swimmerName;
        string[] belongsto;
        string[] venue;


        public void redim_record_db_array(int recordCount)
        {
            Array.Resize<int>(ref classNumber, recordCount);
            Array.Resize<int>(ref genderNumber, recordCount);
            Array.Resize<string>(ref shumoku,recordCount);
            Array.Resize<string>(ref distance, recordCount);
            Array.Resize<string>(ref record, recordCount);
            Array.Resize<string>(ref swimmerName, recordCount);
            Array.Resize<string>(ref belongsto, recordCount);
            Array.Resize<string>(ref venue, recordCount);
        }

        private void init_record_db_array(OleDbConnection conn)
        {
            String sql = "SELECT * from 新記録;";
            int recordcount=0;
            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        recordcount++;
                    }
                }
                redim_record_db_array(recordcount);
            }
                
                
        }
    }

    public class team_db
    {
        string[] teamName4Relay;
        public string get_name(int teamID)
        {
            return teamName4Relay[teamID];
        }
        public team_db(string connectionString)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);
            init_team_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_team_db_(conn);
        }
        public void init_team_db_array(OleDbConnection conn)
        {
            String sql = "SELECT チーム番号 FROM チームマスター where チーム番号=(select max(チーム番号) from チームマスター);  ";
            int numTeam;

            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        numTeam = Convert.ToInt32(dr["チーム番号"]);
                        redim_team_db_array(numTeam);
                    }
                }
            }
        }
        public void redim_team_db_array(int numTeam)
        {
            int ubound = numTeam + 1;
            Array.Resize<string>(ref teamName4Relay, ubound);
        }
        public void read_team_db_(OleDbConnection conn)
        {

            String sql = "SELECT チーム番号, チーム名  FROM チームマスター ;";
            int teamID;

            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (
                    var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        teamID = Convert.ToInt32(dr["チーム番号"]);
                        teamName4Relay[teamID] = (Convert.ToString(dr["チーム名"])).Trim();
                    }
                }
            }
        }
    }


    public class swimmer_db
    {

        public string[] SwimmerName;
        public string[] SwimmerNameKANA;
        public int[] BelongsTo;
        string[] team = new string[1000];

        int maxTeamNum = 0;
        public string get_name(int swimmerID)
        {
            return SwimmerName[swimmerID];
        }
        public string get_furigana(int swimmerID)
        {
            return SwimmerNameKANA[swimmerID];
        }
        public string get_team_name(int swimmerID)
        {
            return team[BelongsTo[swimmerID]];
        }
        public swimmer_db(string connectionString)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);
            init_swimmer_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_swimmer_db_(conn);
        }
        public void init_swimmer_db_array(OleDbConnection conn)
        {
            String sql = "SELECT 選手番号 FROM 選手マスター where 選手番号=(select max(選手番号) from 選手マスター);  ";
            int numSwimmer;

            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        numSwimmer = Convert.ToInt32(dr["選手番号"]);
                        redim_swimmer_db_array(numSwimmer);
                    }
                }

            }
        }
        public void redim_swimmer_db_array(int numSwimmer)
        {
            int ubound = numSwimmer + 1;
            Array.Resize<string>(ref SwimmerName, ubound);
            Array.Resize<string>(ref SwimmerNameKANA, ubound);
            Array.Resize<int>(ref BelongsTo, ubound);
        }
        public void read_swimmer_db_(OleDbConnection conn)
        {

            String sql = "SELECT 選手番号, 氏名, カナ, 所属１, 所属名称１  FROM 選手マスター ;";
            int swimmerID;
            int clubNo;
            using (conn)
            {

                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (
                    var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["選手番号"] == null) swimmerID = 0;
                        else
                            swimmerID = Convert.ToInt32(dr["選手番号"]);
                        SwimmerName[swimmerID] = (Convert.ToString(dr["氏名"])).Trim();
                        SwimmerNameKANA[swimmerID] = (Convert.ToString(dr["カナ"])).Trim();
                        clubNo = locate_team_id(Convert.ToString(dr["所属名称１"]));
                        BelongsTo[swimmerID] = clubNo;
                    }
                }
            }
        }
        public int locate_team_id(string teamName)
        {
            int cnt;
            for (cnt = 0; cnt < maxTeamNum; cnt++)
            {
                if (team[cnt] == teamName) return cnt;
            }
            team[maxTeamNum++] = teamName;
            return maxTeamNum - 1;
        }
    }





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
                using (
                    var dr = comm.ExecuteReader())
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
        public program_db(String connectionString)
        {

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
 
    }
    public static class misc
    {
        static int[] lapCounter = new int[10];
        public static int time2int(string timestr)
        {

            string work;
            work = timestr.Replace(":", "");
            work = work.Replace(".", "");
            return Convert.ToInt32(work);
        }
        public static string format_time(string orgtime)
        {
            const int totalLength = 9;
            int mylength = orgtime.Length;
            string header = new string(' ', totalLength - mylength);
            return header + orgtime;
        }

        public static int get_lapcounter(int lane)
        {
            return lapCounter[lane - 1];
        }
        public static void inc_lapcounter(int lane)
        {
            lapCounter[lane - 1]++;
        }

        public static void init_lapcounter()
        {

            for (int i = 0; i < 10; i++) lapCounter[i] = 1;

        }
        public static int if_not_null(object dr)
        {
            if (dr == DBNull.Value) return 0;
            return Convert.ToInt32(dr);
        }
        public static string if_not_null_string(object dr)
        {
            if (dr==DBNull.Value) return "";
            return (string)dr;
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
 
 
    }
}

