using System;
using System.IO;
using System.Collections;
using System.Data.OleDb;

public class  SwimSys
{
    public static void Main() {
	const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
	string currentDir=System.Environment.CurrentDirectory;
	string[] files=Directory.GetFiles(currentDir,"*.mdb");
	foreach (string filename in files) {
	    event_db evtDb = new event_db(magicWord+filename);
	    Console.WriteLine(Path.GetFileName(filename) + " : " + evtDb.get_eventDate()+ "  " +evtDb.get_eventName()) ;
	}

    }

}
public class event_db
{
    static int maxLaneNo4Yosen;
    static int maxLaneNo4TimeFinal;
    static int maxLaneNo4Final;
    static int maxLaneNo;
    public static string eventName;
    public static string eventVenue;
    public static string eventDate;
    public static int if_not_null(object dr)
    {
	if (dr == DBNull.Value) return 0;
	return Convert.ToInt32(dr);
    }
    public static string if_not_null_string(object dr)
    {
	if (dr == DBNull.Value) return "";
	return (string)dr;
    }


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
		
		    eventName = if_not_null_string(dr["大会名１"]);
		    if ((if_not_null_string(dr["始期間"])).Equals(if_not_null_string(dr["終期間"])))
		    {
			eventDate = "      " + if_not_null_string(dr["始期間"]) + "      ";
		    }
		    else
		    {
			eventDate = if_not_null_string(dr["始期間"]) + "～" + if_not_null_string(dr["終期間"]);
		    }
		    eventVenue = if_not_null_string(dr["開催地"]);
		    maxLaneNo4Final = if_not_null(dr["使用水路決勝"]);
		    maxLaneNo4TimeFinal = if_not_null(dr["使用水路タイム決勝"]);
		    maxLaneNo4Yosen = if_not_null(dr["使用水路予選"]);
		}
	    } catch ( Exception )
	    {
	       // MessageBox.Show(e.Message);
	    }
	    
	}
    }
}

