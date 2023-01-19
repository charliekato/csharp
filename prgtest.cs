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
    public int	arraySize;
    string connectionString ;
    public program_db(string dbFile)
    {
	connectionString = magicWord+dbFile;
	OleDbConnection conn = new OleDbConnection(connectionString);
	init_program_db_array(conn);
	conn = new OleDbConnection(connectionString);
	read_program_db_(conn);
    }

    public int get_array_length() {
	return arraySize;
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
		arraySize = Convert.ToInt32(dr["numPrg"]);
		redim_program_db_array(arraySize);
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
public class Program{
    public static void Main() {
	int 	argc;
	string[] argv = System.Environment.GetCommandLineArgs();
	program_db prgDb=new program_db(argv[1]);
	int	i;
	for (i=0; i< prgDb.arraySize; i++) {
	    Console.WriteLine(" " + i +". "+ prgDb.className[i] + prgDb.gender[i] + 
		    prgDb.distance[i] + prgDb.style[i]+prgDb.phase[i] );
	}
    }
}


