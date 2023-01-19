using System;
using System.Text;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;

namespace deleteRecord
{
    public static class misc
    {
        public static string get_mdb_file(string initFile)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = initFile;
            ofd.InitialDirectory = @"C:\Users\ykato\Dropbox\Private\水泳協会\databaseTry\";
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



    public static class delRecord
    {
        

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            deleteRecord();
        }
        public static void deleteRecord()
        {
            string fromMDB = misc.get_mdb_file("");
	    access_db.mdb.delete_goal_time(fromMDB);
           
        }

        
        

    }

}

namespace access_db
{
    public static class mdb
    {
        const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=";

        public static void  delete_goal_time(string mdbFileName)
        {
	    string connectionString;
            connectionString = magicWord + mdbFileName;
            OleDbConnection conn = new OleDbConnection(connectionString);
            using (conn)
            {
		conn.Open();
                String sql = "UPDATE 記録マスター SET ゴール='', ラップ１='', ラップ２='', ラップ３='', "+
		    "中間記録='', 新記録印刷マーク='', 新記録電光マーク='';";
		Console.WriteLine("sql文: " + sql);
                OleDbCommand comm = new OleDbCommand(sql, conn);
		comm.CommandText = sql;
		comm.ExecuteNonQuery();
            }
        }
    }
}
