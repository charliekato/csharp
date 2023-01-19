
        public int MaxLaneNo4Yosen;
        public int MaxLaneNo4TimeFinal;
        public int MaxLaneNo4Final;
		
		
            Label label = new Label();
            label.Name = "newlabel";
            label.Location = new Point(250, 250);
            label.Text = "new label";
            label.Font = new Font("MS UI Gothic", 20);
            label.Size = new Size(100, 20);
            label.Show();		
		
// set_score_rule is no longer used...obsolete... but not erased for referrence use
private void set_score_rule_on_program(string dbFile)
{
  String connectionString = "Provider=Microsoft.jet.OLEDB.4.0;" +
  "Data Source=" + dbFile;
  int cnt = 0;

  using (OleDbConnection conn = new OleDbConnection(connectionString))
  {
    DataTable resultDB = new DataTable();
    String sql = "SELECT COUNT([FLDA]) FROM SWPNT WHERE [FLDA]=1;";
    conn.Open();
    OleDbCommand comm = new OleDbCommand(sql, conn);
    if (comm.ExecuteScalar() != null)
    {
      cnt = (int)comm.ExecuteScalar();
      if (cnt==0)
      {
	sql = "INSERT INTO SWPNT (FLDA) VALUES (1);";
	comm = new OleDbCommand(sql, conn);
	comm.ExecuteNonQuery();
      }

    }
    sql = "UPDATE SWPNT SET FLDB='8.0', FLDC='7.0', FLDD='6.0', FLDE='5.0', " +
    "FLDF='4.0', FLDG='3.0', FLDH='2.0', FLDI='1.0', FLDJ='0.0', FLDK='0.0', " +
    "FLDL='0.0', FLDM='0.0, FLDN='0.0', FLDO='0.0', FLDP='0.0', FLDQ='0.0', " +
    "FLDR='0.0', FLDS='0.0', FLDT='0.0', FLDU='0.0' WHERE FLDA=1;";

    comm = new OleDbCommand(sql, conn);             
    comm.ExecuteNonQuery();


  }
  ;
}
