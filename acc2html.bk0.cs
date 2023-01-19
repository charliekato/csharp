using System;
using System.Text;
using System.Data.OleDb;
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

}
/*
arg1 : full path name of access database should be "c:\\Users\\ykato\\SwimTools\\swmresult.accdb"
arg2 : program No. If ommitted, all program No will be passed to sql
arg3 : kumi No. If ommitted, all kumi will be returned.
*/
public class Program {

    private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
    private const string tblName="RESULT";

    static void usage() {
	Console.WriteLine("usage of acc2html");
	Console.WriteLine(" > acc2html accdbname prgNo kumi");
    }
    static void write_header() {
	int i;
	//Console.WriteLine("<!DOCTYPE html>");
	//Console.WriteLine("<html lang=\"ja\"><head>");
	//Console.WriteLine("<meta charset=\"shift_jis\" />");
	//Console.WriteLine("</head><body>");
	// above statement will be issued from perl script...
	Console.WriteLine("<table>");
	Console.WriteLine("<tr>");
	Console.WriteLine("<th>PRG#</th><th>Kumi</th><th>Lane</th><th>____name____</th><th>goal</th>");
	for (i=50; i<=1500; i+=50) {
	    Console.Write("<th>" + i + "m</th>");
	}
	Console.WriteLine("");
    }
    static void write_tailer() {
	Console.WriteLine("</table>");
	// after table tag perl script will print...
    }



    static void Main() {
	int	argc;
	int	i;
	int	lane;
	int	prgNo,kumi;
	string  goal, sName;
	string sql;

	string[] argv = System.Environment.GetCommandLineArgs();
	argc = argv.Length;
	if (argc==1) {
	    usage();
	    return;
	}

	write_header();

	string connectionString = magicWord + argv[1];
	using (OleDbConnection conn = new OleDbConnection(connectionString) ) {
	    if (argc==2) {
		sql = "SELECT DISTINCT * FROM " + tblName;
	    } else 
	    if (argc==3) {
		sql = "SELECT DISTINCT * FROM " + tblName;
		sql += " WHERE PRG_NO= " + argv[2];
	    } else
	    {
		sql = "SELECT DISTINCT * FROM " + tblName;
		sql += " WHERE PRG_NO= " + argv[2] + " AND KUMI= " + argv[3];
	    }
	    OleDbCommand comm = new OleDbCommand(sql, conn);
	    conn.Open();
	    using (var dr = comm.ExecuteReader()) {
		while ( dr.Read() ) {
		    lane =misc.if_not_null(dr["LANE_NO"]);
		    sName=misc.if_not_null_string(dr["SWIMMER_NAME"]);
		    goal =misc.if_not_null_string(dr["GOAL"]);
		    prgNo=misc.if_not_null(dr["PRG_NO"]);
		    kumi=misc.if_not_null(dr["KUMI"]);
		    Console.WriteLine("<tr>");
		    Console.WriteLine("<td>" + prgNo + "</td>" );
		    Console.WriteLine("<td>" + kumi + "</td>" );
		    Console.WriteLine("<td>" + lane + "</td>" );
		    Console.WriteLine("<td>" + sName + "</td>" );
		    Console.WriteLine("<td>" + goal + "</td>" );
		    for (i=50; i<=1500; i+=50) {
			string dist=""+i+"m";
			Console.WriteLine("<td>" + misc.if_not_null_string(dr[dist]) + "</td>");
		    }
		    Console.WriteLine("</tr>");
		}
	    }
	}
	write_tailer();
	    

    }
}
	

