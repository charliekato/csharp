using System;
using System.Data.OleDb;

class program_db {
	static int[] classNumberbyUID;
	static string[] phase;
	static int[] distancebyUID;
	static int[] genderbyUID;
	static int[] shumokubyUID;
	static int[] raceNobyUID;
	static int[] UIDFromRaceNo;
	public static int	maxUID;
	static string[] shumokuTable = new string[7]
        { "自由形","背泳ぎ","平泳ぎ","バタフライ", "個人メドレー", "リレー", "メドレーリレー" };
        static public string[] distanceTable = new string[7] { "25m","50m",
        "100m","200m","400m", "800m", "1500m" };
    string[] GenderStr = new string[3] { "男子", "女子", "混合" };
	public static int get_class_from_uid(int UID) {
		return classNumberbyUID[UID];
	}
	public static string get_phase_from_uid(int UID) {
		return phase[UID];
	}
	public static string get_distance_from_uid(int UID) {
		return distanceTable[distancebyUID[UID]];
	}
	public static string get_shumoku_from_uid(int UID) {
		return shumokuTable[shumokubyUID[UID]];
	}
	public static int get_race_number_from_uid(int UID) {
		return raceNobyUID[UID];
	}
	public  program_db(string mdbFilePath)
        {
	    string magicWord ="Provider=Microsoft.jet.OLEDB.4.0; Data Source=" ;
            string connectionString = magicWord+ mdbFilePath;
			OleDbConnection conn = new OleDbConnection(connectionString);
            init_program_db_array(conn);
            conn = new OleDbConnection(connectionString);
            read_program_db_(conn);
        }
        public static void init_program_db_array(OleDbConnection conn)
        {
            String sql = "SELECT UID FROM プログラム where UID=(select max(UID) from プログラム);  ";
            int maxProgramNo;

            using (conn)
            {
                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        maxUID = Convert.ToInt32(dr["UID"]);
                        redim_program_db_array(maxUID);  
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
        public static void redim_program_db_array(int maxProgramNo)
        {
            int ubound = maxProgramNo + 1;
            Array.Resize<int>(ref classNumberbyUID, ubound);
            Array.Resize<string>(ref phase, ubound);
            Array.Resize<int>(ref distancebyUID, ubound);
            Array.Resize<int>(ref genderbyUID, ubound);
            Array.Resize<int>(ref shumokubyUID, ubound);
            Array.Resize<int>(ref raceNobyUID, ubound);

        }
        public static int locate_style_number(string styleName)
        {
            int cnt;
            for (cnt=0; cnt<7; cnt++)
            {
                if (shumokuTable[cnt] == styleName) return cnt;
            }
            return -1;
        }
        public static int locate_distance_number(string distance)
        {
            int cnt;
            for (cnt=0; cnt<7; cnt++)
            {
                if (distanceTable[cnt] == distance) return cnt;
            }
            return -1;
        }
        public static void read_program_db_(OleDbConnection conn)
        {
         
            String sql = "SELECT UID, 競技番号, 種目, 距離, 性別, 予決, クラス番号  FROM プログラム ;";
            int raceNo;
            int uid;
            using (conn )
            {

                OleDbCommand comm = new OleDbCommand(sql, conn);
                conn.Open();
                using (
                    var dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        raceNo = Convert.ToInt32(dr["競技番号"]);
                        uid = Convert.ToInt32(dr["UID"]);
                        raceNobyUID[uid]=raceNo;
                        UIDFromRaceNo[raceNo] = uid;
                        shumokubyUID[uid] = locate_style_number(Convert.ToString(dr["種目"]));
                        distancebyUID[uid] = locate_distance_number(Convert.ToString(dr["距離"]));
                        phase[uid] = Convert.ToString(dr["予決"]);
                        classNumberbyUID[uid] = Convert.ToInt32(dr["クラス番号"]);
                    }
                }
            }
        }	
}

		
class Program
{
    static void Main()
	{
	      program_db thisProgram = new program_db("c:\\users\\ykato\\Dropbox\\Private\\水泳協会\\Database3\\Swim38.m");
		int	i;
	      for (i=0; i<=program_db.maxUID; i++) {
		Console.WriteLine("UID : {0}  種目 : {1}",i,program_db.get_shumoku_from_uid(i));
	      }
	}
}
