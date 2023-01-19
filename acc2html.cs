using System;
using System.Text;
using System.Data.OleDb;

public class program_db {
    private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
    private const string prgTblName="PROGRAM";
    public string[] className;
    public string[] distance;
    public string[] gender;
    public string[] style;
    public string[] phase;
    string connectionString ;
    public program_db(string dbFile)
    {
	connectionString = magicWord+dbFile;
	OleDbConnection conn = new OleDbConnection(connectionString);
	init_program_db_array(conn);
	conn = new OleDbConnection(connectionString);
	read_program_db_(conn);
    }

    public void redim_program_db_array(int aSize)
    {

	int ubound = aSize + 1;
	Array.Resize<string>(ref className, ubound);
	Array.Resize<string>(ref phase, ubound);
	Array.Resize<string>(ref distance, ubound);
	Array.Resize<string>(ref gender, ubound);
	Array.Resize<string>(ref style, ubound);

    }
 
    public void init_program_db_array(OleDbConnection conn)
    {
	String sql = "select max(PRG_NO) as numPrg from " + prgTblName +";";
	using (conn)
	{
	    OleDbCommand comm = new OleDbCommand(sql, conn);
	    conn.Open();
	    using (var dr = comm.ExecuteReader())
	    {
		dr.Read();
		redim_program_db_array(Convert.ToInt32(dr["numPrg"]));
	    }
	}

    }
    private void read_program_db_(OleDbConnection conn) {
	string sql = "SELECT * FROM " + prgTblName + ";";
	int	prgNo;
	using (conn) {
	    OleDbCommand comm = new OleDbCommand(sql, conn);
	    conn.Open();
	    using (var dr = comm.ExecuteReader())
	    {
		while (dr.Read())
		{
		    prgNo=Convert.ToInt32(dr["PRG_NO"]);
		    className[prgNo]=Convert.ToString(dr["CLASS"]);
		    distance[prgNo]=Convert.ToString(dr["DISTANCE"]);
		    style[prgNo]=Convert.ToString(dr["STYLE"]);
		    phase[prgNo]=Convert.ToString(dr["PHASE"]);
		    gender[prgNo]=Convert.ToString(dr["GENDER"]);
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

}
/*
arg1 : full path name of access database should be "c:\\Users\\ykato\\SwimTools\\swmresult.accdb"
*/
public class Program {

    private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
    private const string tblName="RESULT";

    static void usage() {
	Console.WriteLine("usage of acc2html2");
	Console.WriteLine(" > acc2html2 accdbname ");
    }

    static void write_paragraph_header(program_db prgDb, int prgNo) {

	Console.Write("<h1>ã£ãZî‘çÜ"+ prgNo +". "+prgDb.className[prgNo]);
	Console.Write(prgDb.gender[prgNo]+" "+prgDb.distance[prgNo]);
	Console.Write(prgDb.style[prgNo]+" "+prgDb.phase[prgNo]);
	Console.WriteLine("</h1>");
    }

    static void write_kumi_header(int kumi) {
    	Console.WriteLine("<h2>"+ kumi + "ëg</h2>");
    }


    static void write_table_header() {
	Console.WriteLine("<table>");
	Console.WriteLine("<tr><th> Lane</th><th>_______Name_______</th><th>Goal</th>");
	int	i;
	for (i=50; i<=1500; i+=50) {
	    Console.WriteLine("<th> "+i+"m </th>");
	}
	Console.WriteLine("</tr>");
    }

    	
    static void write_tailer() {
	// after table tag perl script will print...
    }


    static void write_main(string dbFileName) {
	int	i;
	int	lane;
	int	prgNo,kumi;
	int	prevPrgNo=0, prevKumi=0;
	string  goal, sName;
	string sql;
	program_db prgDb=new program_db(dbFileName);
	string connectionString = magicWord + dbFileName;
	using (OleDbConnection conn = new OleDbConnection(connectionString) ) {
	    sql = "SELECT * FROM " + tblName;
	    using (OleDbCommand comm = new OleDbCommand(sql, conn)) {
		conn.Open();
		using (var dr = comm.ExecuteReader()) {
		    while ( dr.Read() ) {
			lane =misc.if_not_null(dr["LANE_NO"]);
			sName=misc.if_not_null_string(dr["SWIMMER_NAME"]);
			goal =misc.if_not_null_string(dr["GOAL"]);
			prgNo=misc.if_not_null(dr["PRG_NO"]);
			kumi=misc.if_not_null(dr["KUMI"]);
			if (prevPrgNo != prgNo ) {
			    if (prevPrgNo>0) Console.WriteLine("</table>");
			    prevPrgNo=prgNo;
			    prevKumi=0;
			    write_paragraph_header(prgDb,prgNo);
			}
			if (prevKumi != kumi) {
			    if (prevKumi>0) Console.WriteLine("</table>");
			    prevKumi=kumi;
			    write_kumi_header(kumi);
			    write_table_header();
			}
			
			Console.WriteLine("<tr>");
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
	}
    }


    static void Main() {
	int	argc;

	string[] argv = System.Environment.GetCommandLineArgs();
	argc = argv.Length;
	if (argc==1) {
	    usage();
	    return;
	}
	if (argc>2) return;

	
	write_main(argv[1]);
	    

    }
}
	

