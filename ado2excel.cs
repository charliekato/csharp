using System;
using System.Windows.Forms;

using System.Data.OleDb;


namespace excelConnection
{

    public static class ExcelConnection
    {
        private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
        private const string magicTail = ";Extended Properties='Excel 12.0; HDR=Yes'";
        private int[] counter;
        public init_counter()
        {
            counter = new int[10];
            int n;
            for (n=0; n<10; n++)
            {
                counter[n] = 1;
            }
        }
        public static void append(string excelFile,int laneNo,string time)
        {

            String connectionString = magicWord + excelFile + magicTail;
            
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                String sql = " INSERT INTO [Sheet1$] " +
                            "   (lane"+laneNo+") " +
                            "    VALUES " +
                            "   ('"+time+"')";
                OleDbCommand comm = new OleDbCommand(sql, conn);     
                comm.ExecuteNonQuery();
            }
        }
        public static void equery(string excelFile) {
            String connectionString = magicWord + excelFile + magicTail;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * from [Sheet1$];";
                OleDbCommand comm = new OleDbCommand(query, conn);
                conn.Open();
                using (var dr = comm.ExecuteReader()) {
                    while (dr.Read()) {
			Console.WriteLine("{0}: Lane1 = {1}, Lane2 = {2}", dr["ID"],dr["lane1"], dr["lane2"]);
			if (dr["ID"]==DBNull.Value) break;
                    }
                }
            }
        }

        public static string get_time(string excelFile,int laneNo) {
            String connectionString = magicWord + excelFile + magicTail;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string laneName = "lane" + laneNo;
                string query = "SELECT " +laneName + " from [Sheet1$] where ID=" + counter[laneNo] + ";";
                OleDbCommand comm = new OleDbCommand(query, conn);
                dr.Read();
                if (dr[laneName] == DBNull.Value) return "";
                counter[laneNo]++;
                return (string)dr[laneName];
            }
	    }
	}


        public static void insert(string excelFile, int ID, int laneNo, string time)
        {
            String connectionString = magicWord + excelFile + magicTail;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = string.Empty;

                query += " UPDATE [Sheet1$] ";
                query += " SET ";
                query += "      lane"+laneNo+" = '"+time+"' ";
                query += " WHERE ";
                query += "     ID ="+ID;
                OleDbCommand comm = new OleDbCommand(query, conn);
                comm.CommandText = query;
                comm.ExecuteNonQuery();

            }
        }
	static string get_file(string initFile) {
	    Application.EnableVisualStyles();
	    Application.SetCompatibleTextRenderingDefault(false);


	    OpenFileDialog ofd = new OpenFileDialog();
	    ofd.FileName = initFile;
	    ofd.InitialDirectory = @"D:\swwork";
	    ofd.Filter = "エクセルファイル(*.xlsx)|*.xlsx";
	    ofd.FilterIndex = 2;
	    ofd.Title = "エクセルファイルを選択してください";
	    ofd.RestoreDirectory = true;
	    ofd.CheckFileExists = true;
	    ofd.CheckPathExists = true;

	    if (ofd.ShowDialog() == DialogResult.OK)
	    {
		//OKボタンがクリックされたとき、選択されたファイル名を表示する
		return (ofd.FileName);
	    }
	    else return "";
	}
   
	static class Program
	{
	    [STAThread]
	    static void Main()
	    {

		ExcelConnection.equery(get_file(@"adoTest.xlsx"));
	    }
	}
    }
}
