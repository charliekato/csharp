using System;
using System.Text;
using System.IO.Ports;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;

public static class ExcelConnection
{
    private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
//  private const string magicTail = ";Extended Properties='Excel 12.0; HDR=Yes'";
    private const string magicTail="";
    private const string tblName="RESULT";
//  private const string tblName="[Sheet1$]";

   
    static int lid=0;
    public static void delete(string mdbFile)
    {

	String connectionString = magicWord + mdbFile + magicTail;
	using (OleDbConnection conn = new OleDbConnection(connectionString))
	{
	    String sql = "DELETE * from " + tblName;
	    OleDbCommand comm = new OleDbCommand(sql, conn);
	    conn.Open();
//	    comm.CommandText = query;
	    comm.ExecuteNonQuery();

	}


    }
     public static void append(string mdbFile, int prgNo, int kumi, int laneNo, string name="")
    {

	String connectionString = magicWord + mdbFile + magicTail;
	using (OleDbConnection conn = new OleDbConnection(connectionString))
	{
	    conn.Open();
	    string query = string.Empty;
	    query += " INSERT INTO "+tblName+" (PRG_NO, KUMI, LANE_NO, SWIMMER_NAME)";
	    query += " VALUES (" + prgNo + ", ";
	    query += ""+  kumi + ", ";
	    query += ""+  laneNo + ", ";
	    query += "'"+ name + "') ;";
//	    Console.WriteLine("query > " + query );
	    OleDbCommand comm = new OleDbCommand(query, conn);
	    comm.CommandText = query;
	    comm.ExecuteNonQuery();

	}


    }
    public static void fake_append(string mdbFile, int prgNo,int kumi, int laneNo, string name="")
    {
        String connectionString = magicWord + mdbFile + magicTail;
        using (OleDbConnection conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            string query = string.Empty;
	    lid++;

            query += " UPDATE "+ tblName;
            query += " SET ";
	    query += " PRG_NO = " + prgNo;
	    query += ", KUMI = " + kumi;
	    query += ", LANE_NO = " + laneNo;
	    query += ", NAME = '" + name + "' ";
            query += " WHERE ";
            query += "     ID =" + lid +" ;" ; 
//	    Console.WriteLine(">>" + query);

            OleDbCommand comm = new OleDbCommand(query, conn);
            comm.CommandText = query;
            comm.ExecuteNonQuery();

        }
    }
    public static void insert(string mdbFile, int prgNo,int kumi, int laneNo, string time)
    {
        String connectionString = magicWord + mdbFile + magicTail;
        using (OleDbConnection conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            string query = string.Empty;

            query += " UPDATE "+ tblName;
            query += " SET ";
	    query += "      GOAL = '" + time + "' ";
            query += " WHERE ";
            query += "     PRG_NO =" + prgNo + " AND KUMI = " + kumi ; 
	    query += " AND LANE_NO = " + laneNo + " ;";
//	    Console.WriteLine(">>" + query);

            OleDbCommand comm = new OleDbCommand(query, conn);
            comm.CommandText = query;
            comm.ExecuteNonQuery();

        }
    }
}

public class adoTry {
    public static void Main() {
	string mdbFileName=@"C:\users\ykato\swmresult.accdb";
//	string mdbFileName=@"C:\users\ykato\tt.xlsx";
	ExcelConnection.delete(mdbFileName);
	for ( int prgNo=1 ; prgNo<100; prgNo++) {
	    for (int laneNo=1; laneNo<=6; laneNo++) {
		ExcelConnection.append(mdbFileName,prgNo,1,laneNo);
	    }
	}
	/*
	ExcelConnection.insert(mdbFileName,2,1,1,"1:12.35");
	ExcelConnection.insert(mdbFileName,2,1,2,"1:12.12");
	ExcelConnection.insert(mdbFileName,2,1,3,"1:14.33");
	ExcelConnection.insert(mdbFileName,2,1,4,"1:10.33");
	*/
    }
}
