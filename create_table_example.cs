using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
namespace Sample {
    public static class adoSample
    {
        private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
	private const string mdbFile=@"C:\Users\ykato\OneDrive\MyPrograms\cs\swim01.accdb";
       
        public static void create_table(string mdbFile)
        {
            String connectionString = magicWord + mdbFile ;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"CREATE TABLE TimeDATA (
			UID int NOT NULL, 競技番号 int NOT NULL,
			組 int NOT NULL,
			lap int NOT NULL,
			lane1 nvarchar(8) NULL,
			lane2 nvarchar(8) NULL,
			lane3 nvarchar(8) NULL,
			lane4 nvarchar(8) NULL,
			lane5 nvarchar(8) NULL,
		        lane6 nvarchar(8) NULL,
			lane7 nvarchar(8) NULL,
			lane8 nvarchar(8) NULL,
			lane9 nvarchar(8) NULL,
			lane10 nvarchar(8) NULL	);";

		OleDbCommand comm = new OleDbCommand(query, conn);
                comm.CommandText = query;
                comm.ExecuteNonQuery();

            }
        }
	public static void Main() {
	    create_table(mdbFile);

	} 
    }
    
}
