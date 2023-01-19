using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace ShowLaneOrder
{
 
    public partial class Form2 : Form
    {
        const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=";
       
        event_db evtDB;
        program_db prgDB ;
        class_db clsDB ;
        team_db tmDB ;
        swimmer_db swmDB ;
        record_db rcdDB;
        private bool monitorEnable;
        private int maxLaneNo;
        private string dbfilename;
        private Label mylabel;
        private int FirstPrgNo;
        private int LastPrgNo;
        // for label (mainly lane label) position...



        public bool stopAutoRun = true;

        public void enable_race_monitor() { monitorEnable = true; }
        public void disable_race_monitor() { monitorEnable = false; }
        public int get_lap_interval()
        {
            return Convert.ToInt32(tbxLapInterval.Text);
        }
        public int how_many_laps(int uid)
        {
            int distance = Convert.ToInt32(program_db.get_distance_from_uid(uid).Substring(0, 4));
            int laps = distance / Convert.ToInt32(tbxLapInterval.Text);
            return laps;
        }
        public void set_program_number(int prgNo)
        {
            this.tbxPrgNo.Text = align(prgNo);   
        }
        public int get_program_number()
        {
            return Convert.ToInt32(this.tbxPrgNo.Text);
        }
        public void set_kumi_number(int kumi)
        {
            this.tbxKumi.Text = align2( kumi);
        }
        public int get_kumi_number()
        {
            return Convert.ToInt32(this.tbxKumi.Text);
        }



        public Form2(string filename)
        {
            InitializeComponent();
            this.dbfilename = filename;
            string connectionString = magicWord + filename;
            result_db.set_connectionString(connectionString);
            
            evtDB = new event_db(connectionString);
            prgDB = new program_db(connectionString);
            clsDB = new class_db(connectionString);
            tmDB = new team_db(connectionString);
            swmDB = new swimmer_db(connectionString);
            rcdDB = new record_db(connectionString);
            maxLaneNo = event_db.get_max_lane_number();
         
            init_form2();  
          
        }

      
        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (!stopAutoRun) 
            stop_auto_run();
            this.Close();
          
        }

        private void layout_header_label()
        {
            Font myFont = new Font("MS UI Gothic", 18);
            Controls["lblPrgNo"].Left = 10;
            Controls["lblPrgNo"].Top = 3;
//            Controls["lblPrgNo"].Width = 25;
            Controls["lblPrgNo"].Height = 25;
            Controls["lblPrgNo"].Font = myFont;

            Controls["tbxPrgNo"].Width = 75;
            Controls["tbxPrgNo"].Height = 27;
            Controls["tbxPrgNo"].Left= 5;
            Controls["tbxPrgNo"].Top = 30;
            Controls["tbxPrgNo"].Font = myFont;
            Controls["lblHyphen"].Left = 85;
            Controls["lblHyphen"].Top = 30;
            Controls["lblHyphen"].Height = 25;
            Controls["lblHyphen"].Font = myFont;
            Controls["lblKumi"].Height = 25;
            Controls["lblKumi"].Left = 115;
            Controls["lblKumi"].Top = 3;
            Controls["lblKumi"].Font = myFont;
            Controls["tbxKumi"].Width = 50;
            Controls["tbxKumi"].Height = 27;
            Controls["tbxKumi"].Left = 115;
            Controls["tbxKumi"].Top = 30;
            Controls["tbxKumi"].Font = myFont;

        }
        private void layout_button()
        {
            int top = 15;
            int left = 170;
            int width = 50;
            int height = 50;
            int space = 4;
            Controls["btnShow"].Left = left;
            Controls["btnShow"].Top = top;
            Controls["btnShow"].Height = height;
            Controls["btnShow"].Width = width ;
            Controls["btnShowPrev"].Left = left+width+space;
            Controls["btnShowPrev"].Top = top;
            Controls["btnShowPrev"].Height = height;
            Controls["btnShowPrev"].Width = width;
            Controls["btnShowNext"].Left = 2*(width+space)+left;
            Controls["btnShowNext"].Top = top;
            Controls["btnShowNext"].Height = height;
            Controls["btnShowNext"].Width = width;
            Controls["lblRaceName0"].Top = 3;
            Controls["lblRaceName0"].Left = 3*(width+space)+left;
        }
        private void layout_label()
        {

            int topMargin = this.Height/9;  // 88
        
            int buttomMargin = this.Height/15; //53
            int leftMargin = this.Width/80;     //15
            int rightMargin= this.Width/100;     //12 
            int laneNoWidth = this.Width/20;   //60
            int nameWidth = (this.Width - leftMargin - rightMargin - laneNoWidth )/ 4; //220
            int relayNameWidth = nameWidth - 5;
            int shozokuWidth = nameWidth - 5;
            int relaySwimmerWidth = nameWidth * 5 / 10;
            int laneHeight = (this.Height - topMargin - buttomMargin) / maxLaneNo;
            int kanaHeight = 18; // laneHeight / 5;
            int raceNameHeight = 32; // laneHeight * 3 / 10;
            //      laneHeight = 100;
            const int fontSize = 30; // was  laneHeight *1 / 5; was 22
            const int fontSizeKana = 12;
            const int fontSize4Rly = 18;
            const int fontSizeShozoku = 22;
            const int fontsize4relayTeam = 24;
            const string fontName = "MS UI Gothic";
            Font nameFont = new Font(fontName, fontSize); // was 23
            Font relayTeamFont = new Font(fontName, fontsize4relayTeam);
            Font shozokuFont = new Font(fontName, fontSizeShozoku);
            Font kanaFont = new Font(fontName, fontSizeKana); // was 11
            Font raceNameFont = new Font(fontName, 18);
            Font smallNameFont = new Font(fontName, fontSize4Rly);
            int i;

            for ( i=1; i<=maxLaneNo; i++)
            {
                int xpos = leftMargin;
                int yposr = topMargin + (laneHeight * (i - 1));
                int yposk = yposr+raceNameHeight ;
                int ypos = yposk + kanaHeight;
                create_lblName("lblLane",i, xpos, ypos,  nameFont,""+i+".");
                xpos = xpos + laneNoWidth;
                create_lblName("lblRaceName",i, xpos, yposr, raceNameFont, "");
                create_lblName("lblName", i, xpos, ypos,  nameFont, "");
                create_lblName("lblRealyTeamName", i, xpos, ypos, relayTeamFont);
                create_lblName("lblKana", i, xpos+15, yposk, kanaFont,"");
                xpos += nameWidth;
                create_lblName("lblShozoku", i, xpos, ypos+3,  shozokuFont,"");
                int xpos4relay = xpos-5;
                xpos += nameWidth;
                create_lblName("lblTime", i, xpos, ypos, nameFont, "");
                create_lblName("lblNewRecord", i, xpos + nameWidth, ypos, nameFont, "");
                for (int j=1; j<5; j++)
                {
                    create_lblName("lblRelaySwimmer" + j, i, xpos4relay, ypos+5, smallNameFont,"");
                    create_lblName("lblRelaySwimmerKana"+j,i,xpos4relay,yposk+5,kanaFont,"");
                    xpos4relay += relaySwimmerWidth;
                }
                
                create_lblName("lblTime4Relay", i, xpos4relay, ypos, nameFont,"");
                create_lblName("lblNewRecord4Relay", i, xpos4relay, ypos, nameFont, "");
             
            }
            
        }

        private void create_lblName(string head,int i,int x, int y, Font myFont,string txt="")
        {
            
            this.mylabel = new Label();
            this.mylabel.AutoSize = true;
            this.mylabel.Location = new System.Drawing.Point(x, y);
            this.mylabel.Name = head + i;
            //this.mylabel.Size = new System.Drawing.Size(w, h);
            // this.mylabel.TabIndex = 19;
            this.mylabel.Text = txt;
            this.mylabel.Font = myFont;
            this.Controls.Add(this.mylabel);
        }

        public  void show_swimmer_name(int laneNo,int swimmerID)
        {
            if (swimmerID==0)
            {
                this.Controls["lblName" + laneNo].Text = "";
                this.Controls["lblKana" + laneNo].Text = "";
                this.Controls["lblShozoku" + laneNo].Text = "";
            } else {
                this.Controls["lblName" + laneNo].Text = swmDB.get_name(swimmerID);
                this.Controls["lblKana" + laneNo].Text = swmDB.get_furigana(swimmerID);
                this.Controls["lblShozoku" + laneNo].Text = swmDB.get_team_name(swimmerID);
            }
            
        }
        public void show_relay_team(int laneNo, int teamID,int s1,int s2,int s3, int s4)
        {
            this.Controls["lblName" + laneNo].Text = tmDB.get_name(teamID);
            this.Controls["lblKana" + laneNo].Text = "";
            this.Controls["lblRelaySwimmer1" + laneNo].Text = swmDB.get_name(s1);
            this.Controls["lblRelaySwimmer2" + laneNo].Text = swmDB.get_name(s2);
            this.Controls["lblRelaySwimmer3" + laneNo].Text = swmDB.get_name(s3);
            this.Controls["lblRelaySwimmer4" + laneNo].Text = swmDB.get_name(s4);
            this.Controls["lblRelaySwimmerKana1" + laneNo].Text = swmDB.get_furigana(s1);
            this.Controls["lblRelaySwimmerKana2" + laneNo].Text = swmDB.get_furigana(s2);
            this.Controls["lblRelaySwimmerKana3" + laneNo].Text = swmDB.get_furigana(s3);
            this.Controls["lblRelaySwimmerKana4" + laneNo].Text = swmDB.get_furigana(s4);
        }
        public  void show_reason(int laneNo, int reason_code)
        {
            if (reason_code == 0) return;
            if (reason_code == 1)
            {
                Controls["lblTime" + laneNo].Text = " 棄権";
                Controls["lblTime" + laneNo].BackColor = Color.FromArgb(240, 240, 240);
            }
        }
        public void show_reason4Relay(int laneNo, int reason_code)
        {
            if (reason_code == 0) return;
            if (reason_code == 1) Controls["lblTime4Relay" + laneNo].Text = " 棄権";
        }
        private void show_header(int uid, int prgNo, int laneNo)
        {
            
            Controls["lblRaceName"+laneNo].Text ="No." + prgNo + "   " + program_db.get_class_from_uid(uid) + program_db.get_gender_from_uid(uid) + " " +
               program_db.get_distance_from_uid(uid) + program_db.get_shumoku_from_uid(uid) + " " + program_db.get_phase_from_uid(uid);
           
        }


        private void set_quit_button()
        {
            btnQuit.Location = new Point(this.Width - 65, 10);
            btnQuit.Size = new Size(55, 25);
            btnQuit.Show();
            btnQuit.Font = new Font("MS UI Gothic", 12);
        }
        private void set_start_button()
        {
            btnStart.Location = new Point(this.Width - 125, 10);
            btnStart.Size = new Size(55, 25);
            btnStart.Show();
            btnStart.Font = new Font("MS UI Gothic", 12);
        }
        private void set_tbxLapInterval()
        {
            tbxLapInterval.Location = new Point(this.Width - 60, 38);
            tbxLapInterval.Size = new Size(40, 25);
            tbxLapInterval.Show();
            tbxLapInterval.Font = new Font("MS UI Gothic", 12);
            tbxLapInterval.Text = "50";
        }
        private void set_lblLapInterval()
        {
            lblLapInterval.Location = new Point(this.Width - 142, 40);
            
            lblLapInterval.Show();
            lblLapInterval.Font = new Font("MS UI Gothic", 12);
        }
        private void set_tooltip()
        {
            toolTip1.SetToolTip(tbxLapInterval, "何メートルおきにラップタイムが来るか。\n" +
                    "有効な値 : 50 もしくは 100\n" +
                    " 25mプールでは 50\n" +
                    " 50mプールでは片側なら100\n" +
                    "              両側なら 50\n");
        }
        private void init_form2()
        {
            const int outerMarginX = 0;
            const int outerMarginY = 0;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width -outerMarginX;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height -outerMarginY;
           
            this.Text = evtDB.get_eventName() + " " + evtDB.get_eventDate() + " " + evtDB.get_eventVenue();
            //            MessageBox.Show(" width: " + Width + "   Height: " + Height);
            // in my surface   width=1196 height=791
            layout_header_label();
            layout_label();
            layout_button();
            set_tooltip();
            set_quit_button();
            set_start_button();
            set_lblLapInterval();
            set_tbxLapInterval();
            set_program_number(1);
            set_kumi_number(1);
            FirstPrgNo = 1;
            show_lane_order();
        }
        private void show_lane_order()
        {
            int prgNo;
            int kumi;
            int uid;
            int maxPrgNo = program_db.get_max_program_no();

            prgNo = get_program_number();
            kumi = get_kumi_number();
            if (prgNo>maxPrgNo)
            {
                MessageBox.Show("該当のレースはありません。最終レースを表示します。");
                prgNo = maxPrgNo;
                kumi = 1;
                uid = program_db.get_uid_from_prgno(prgNo);
                while (result_db.race_exist(uid, kumi)) kumi++;
                kumi--;
                set_program_number( prgNo);
                set_kumi_number( kumi);
            }
            uid = program_db.get_uid_from_prgno(prgNo);
            if (!result_db.race_exist(uid, kumi)) MessageBox.Show("該当のレースはありません。");
            else
            {
                if (kumi == 1)
                {
                    uid = program_db.get_uid_from_prgno(prgNo);
                    while (mdb_interface.can_go_with_prev(uid,kumi) )
                    {
                        program_db.dec_race_number(ref prgNo);
                        uid = program_db.get_uid_from_prgno(prgNo);
                    }
                }
            }
            set_program_number(prgNo);
            set_kumi_number(kumi);
            show();
            //show_record_using_serial_data();
            show_record();
        }
        private static bool already_occupied(int[] array, int laneNo) { return (array[laneNo] > 0); }

        private void clear_lane_info()
        {
            for (int lane=1; lane<=event_db.get_max_lane_number();lane++)
            {
                
                Controls["lblRaceName" + lane].Text = "";
                Controls["lblName" + lane].Text = "";
                Controls["lblShozoku" + lane].Text = "";
                Controls["lblTime" + lane].Text = "";
                Controls["lblTime4Relay" + lane].Text = "";
                Controls["lblKana" + lane].Text = "";
                for (int j=1; j<5; j++)
                {
                    Controls["lblRelaySwimmer" + j + lane].Text = "";
                    Controls["lblRelaySwimmerKana" + j + lane].Text = "";
                }
                Controls["lblNewRecord4Relay" + lane].Text = "";
                Controls["lblNewRecord" + lane].Text = "";
            }
        }

        public void show()
        {
            int prgNo = get_program_number();
            int kumi = get_kumi_number();
            int[] swimmerID = new int[10];
            int uid = program_db.get_uid_from_prgno(prgNo);
            int prevUID=uid;
            string connectionString = magicWord + dbfilename;


            LastPrgNo = prgNo;
            FirstPrgNo = prgNo;
            show_header(uid,prgNo,0);
           
            int laneNo;
            int lastOccupiedLane = 0;
            bool togetherflag = false;

            int first_lane = 0;
            clear_lane_info();
            do
            {
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス, " +
                                        "第１泳者, 第２泳者, 第３泳者, 第４泳者 " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();
                    prgNo = program_db.get_race_number_from_uid(uid);
                    using (var dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            //if (dr["選手番号"] == DBNull.Value) continue;
                            if (laneNo > 9) continue;
                            swimmerID[laneNo] = misc.if_not_null(dr["選手番号"]);
                            if (swimmerID[laneNo] > 0)
                            {
                                if (first_lane == 0) first_lane = laneNo;
                               
                                if (prevUID!=uid) {  //change header
                                    togetherflag = true;
                                    show_header(uid, prgNo, laneNo);
                                    prevUID = uid;
                                }
                                
                                if (program_db.is_relay(uid))
                                {
                                    show_relay_team(laneNo, swimmerID[laneNo], misc.if_not_null(dr["第１泳者"]),
                                      misc.if_not_null(dr["第２泳者"]),misc.if_not_null(dr["第３泳者"]),misc.if_not_null(dr["第４泳者"]));
                                    show_reason4Relay(laneNo, Convert.ToInt32(dr["事由入力ステータス"]));
                                } else
                                {
                                    show_swimmer_name(laneNo, swimmerID[laneNo]);
                                    show_reason(laneNo, Convert.ToInt32(dr["事由入力ステータス"]));
                                }
                                lastOccupiedLane = laneNo;
                            }  
                        }
                    }
                }
                if (result_db.race_exist(uid, kumi+1)) break;
                prevUID = uid;
                uid = program_db.get_next_uid(uid);
                
            } while (can_go_with_next(uid, prevUID, kumi,lastOccupiedLane));
            LastPrgNo = prgNo;
            if (togetherflag)
            {
                Controls["lblRaceName"+first_lane].Text= Controls["lblRaceName0"].Text;
                Controls["lblRaceName0"].Text = "合同レース";
            }
        }
        private static bool can_go_with_next(int uid,int prevUID,int kumi, int prevLastLane)
        {
            if (kumi > 1) return false;

            if (!program_db.is_same_distance_style(uid, prevUID)) return false;
  //          if (result_db.race_exist(uid, 2)) return false;
            if (result_db.get_first_occupied_lane(uid, 1) <= prevLastLane) return false;
            return true;
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            if (monitorEnable) btnStart.Visible = true;
            else btnStart.Visible = false;
            
        }

        private void btnShow_click(object sender, EventArgs e)
        {
            show_lane_order();
        }

        private void btnShowPrev_Click(object sender, EventArgs e)
        {
            int prgNo = get_program_number();
            int kumi = get_kumi_number();
            result_db.get_prev_race(ref prgNo ,ref kumi);
            
            set_program_number(prgNo);
            set_kumi_number(kumi);
            show_lane_order();
        }
        public void show_next_race()
        {
            int prgNo = LastPrgNo;
            int kumi = get_kumi_number();

            while (true)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                kumi++;
                if (result_db.race_exist(uid, kumi))
                {
                    set_program_number(program_db.get_race_number_from_uid(uid));
                    set_kumi_number(kumi);
                    show_lane_order();
                    return;
                }
                if (program_db.inc_race_number(ref prgNo) == false)
                {
                    stop_auto_run();
                    MessageBox.Show("最終レースです。");
                    return;
                }

                kumi = 0;
            }
        }
        private void btnShowNext_Click(object sender, EventArgs e)
        {
            show_next_race();
      
            
        }
        private string align(int n)
        {
            if (n < 10) return "  " + n;
            if (n < 100) return " " + n;
            return "" + n;
        }
        private string align2(int n)
        {
            if (n < 10) return " " + n;
         
            return "" + n;
        }
        public bool show_record_using_serial_data()
        {
            int prgNo = get_program_number();
            int kumi = get_kumi_number();
            int swimmerID;

            string connectionString = magicWord + dbfilename;
            bool rc=true;
            int laneNo;

            while (prgNo<=LastPrgNo)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス,ゴール " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();
                   
                    using (OleDbDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            if (laneNo > 10) continue;
                            swimmerID = misc.if_not_null(dr["選手番号"]);
                            if (swimmerID> 0)
                            {
                                if (Convert.ToInt32(dr["事由入力ステータス"])==0)
                                {
                                    if (dr["ゴール"] == DBNull.Value) rc = false;
                                    else
                                    {
                                        string goalTime = Convert.ToString(dr["ゴール"]);
                                        if (goalTime == "") rc = false;
                                        else
                                        {
                                            bool newRecord = (misc.time_str2int(goalTime) < program_db.bestRecord[uid]);
                                            if (program_db.is_relay(uid))
                                            {
                                                Controls["lblTime4Relay" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord4Relay" + laneNo);
                                                
                                            } else
                                            {
                                                Controls["lblTime" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord" + laneNo);
                                            }
                                        }
                                    }
                                    
                                    
                                }
                            }
                        }
                    }
                }
                program_db.inc_race_number(ref prgNo);
            }
            return rc;
        }


        public bool show_record()
        {
            int prgNo = get_program_number();
            int kumi = get_kumi_number();
            int swimmerID;

            string connectionString = magicWord + dbfilename;
            bool rc=true;
            int laneNo;

            while (prgNo<=LastPrgNo)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, 事由入力ステータス,ゴール " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();
                   
                    using (OleDbDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            if (laneNo > 10) continue;
                            swimmerID = misc.if_not_null(dr["選手番号"]);
                            if (swimmerID> 0)
                            {
                                if (Convert.ToInt32(dr["事由入力ステータス"])==0)
                                {
                                    if (dr["ゴール"] == DBNull.Value) rc = false;
                                    else
                                    {
                                        string goalTime = Convert.ToString(dr["ゴール"]);
                                        if (goalTime == "") rc = false;
                                        else
                                        {
                                            bool newRecord = (misc.time_str2int(goalTime) < program_db.bestRecord[uid]);
                                            if (program_db.is_relay(uid))
                                            {
                                                Controls["lblTime4Relay" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord4Relay" + laneNo);
                                                
                                            } else
                                            {
                                                Controls["lblTime" + laneNo].Text = goalTime;
                                                if (newRecord) show_new_record("lblNewRecord" + laneNo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                program_db.inc_race_number(ref prgNo);
            }
            return rc;
        }
        public void show_new_record(string lblname)
        {
            Controls[lblname].Text = "大会新";
            Controls[lblname].BackColor = Color.Pink;
        }


        private void auto_run()
        {
            if (stopAutoRun) return;
            misc.init_lapcounter();
            myTimer myTimer = new myTimer();
            myTimer.timer.Tick += myTimer.ev1;
            myTimer.timer.Interval = 1000;
            myTimer.timer.Enabled = true;
            
        }
        private void stop_auto_run()
        {
            Controls["btnStart"].Text = "開始";
            myTimer.timer.Stop();
            stopAutoRun = true;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            int lapInterval = get_lap_interval();
            if (lapInterval!=50 & lapInterval!=100)
            {
                MessageBox.Show("lap interval should be 50 or 100!!");
                return;
            }
            if (stopAutoRun)
            {
                Controls["btnStart"].Text = "停止";
                stopAutoRun = false;
                auto_run();
            } else
            {
                stop_auto_run(); 
            }
        }
    }
    
    public static class mdb_interface
    {

        public static bool can_go_with_next(int uid, int kumi, int maxLaneNumber)
        {
            int prgNo = program_db.get_race_number_from_uid(uid);
            int nextuid;
            if (maxLaneNumber == event_db.get_max_lane_number()) return false;
            if (kumi > 1) return false;
            prgNo++;
            if (prgNo > program_db.get_max_program_no()) return false;
            nextuid = program_db.get_uid_from_prgno(prgNo);
            if (!program_db.is_same_distance_style(uid, nextuid)) return false;
            if (maxLaneNumber < result_db.get_first_occupied_lane(nextuid, 1)) return true;
            return false;

        }
        public static bool can_go_with_prev(int uid, int kumi)
        {
            int prgNo;
            int prevuid;
            int minLaneNumber = result_db.get_first_occupied_lane(uid, kumi);

            if (minLaneNumber == 1) return false;
            prgNo = program_db.get_race_number_from_uid(uid);
            if (prgNo == 1) return false;
            program_db.dec_race_number(ref prgNo);
            if (prgNo == 0) return false;
            prevuid = program_db.get_uid_from_prgno(prgNo);
            if (result_db.race_exist(prevuid, 2)) return false;
            if (!program_db.is_same_distance_style(uid, prevuid)) return false;
            if (minLaneNumber > result_db.get_last_occupied_lane(prevuid, 1)) return true;
            return false;
        }
        
    }
    public class myTimer
    {
        public static Timer timer;
        public static EventHandler ev1, ev2;
        public static EventHandler recordFlash;
        public myTimer()
        {
            
            timer = new Timer();
            ev1 = new EventHandler(run_show_record);
            ev2 = new EventHandler(run_show_next_race);
        }
        public static void run_show_record(object sender, EventArgs e)
        {
            if (Form1.form2.stopAutoRun)
            {
                myTimer.timer.Tick -= ev1;
                myTimer.timer.Stop();
            }
            //Form1.form2.show_laptime();
            if (Form1.form2.show_record())
            {
                myTimer.timer.Tick -= ev1;
                myTimer.timer.Tick += ev2;
                myTimer.timer.Interval = 5000;
            }
        }
        static void run_show_next_race(object sender, EventArgs e)
        {
            if (Form1.form2.stopAutoRun)
            {
                myTimer.timer.Tick -= ev2;
                myTimer.timer.Stop();
            }
            Form1.form2.show_next_race();
            myTimer.timer.Tick -= ev2;
            myTimer.timer.Interval = 500;
            myTimer.timer.Tick += ev1;
        }
    }
    public static class ExcelConnection
    {
        private const string magicWord = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
        private const string magicTail = ";Extended Properties='Excel 12.0; HDR=Yes'";
       
        public static void insert(string excelFile, int ID, int laneNo, string time)
        {
            String connectionString = magicWord + excelFile + magicTail;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = string.Empty;

                query += " UPDATE [Sheet1$] ";
                query += " SET ";
                query += "      lane" + laneNo + " = '" + time + "' ";
                query += " WHERE ";
                query += "     ID =" + ID;
                OleDbCommand comm = new OleDbCommand(query, conn);
                comm.CommandText = query;
                comm.ExecuteNonQuery();

            }
        }
    }

}
//------------------------trash--------------------------------
/*
        public void show_laptime()
        {
            int prgNo = get_program_number();
            int kumi = get_kumi_number();
            int swimmerID;
            int howManyLaps;
            string lapTimeString;

            string connectionString = magicWord + dbfilename;

            int laneNo;
            string thisLaptime;
            int thisLap;

            while (prgNo <= LastPrgNo)
            {
                int uid = program_db.get_uid_from_prgno(prgNo);
                howManyLaps = how_many_laps(uid)-1;
                if (howManyLaps <= 0) return;
                OleDbConnection conn = new OleDbConnection(connectionString);
                using (conn)
                {
                    String sql = "SELECT UID, 選手番号, 組, 水路, ラップ１, ラップ２, ラップ３,事由入力ステータス " +
                                        "FROM 記録マスター WHERE 組=" + kumi + " AND UID=" + uid + ";";
                    OleDbCommand comm = new OleDbCommand(sql, conn);
                    conn.Open();
                    using (var dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            laneNo = Convert.ToInt32(dr["水路"]);
                            if (laneNo > 10) continue;
                            swimmerID = misc.if_not_null(dr["選手番号"]);
                            if (swimmerID > 0)
                            {
                                if (Convert.ToInt32(dr["事由入力ステータス"]) == 0)
                                {
                                    lapTimeString = misc.record_concatenate(dr["ラップ１"], dr["ラップ２"], dr["ラップ３"]);
                                    thisLap = misc.get_lapcounter(laneNo);
                                    if (thisLap<=howManyLaps)
                                    {
                                        thisLaptime = misc.get_ith_lap(lapTimeString, thisLap   );
                                        if (thisLaptime!="")
                                        {
                                            misc.inc_lapcounter(laneNo);
                                            if (program_db.is_relay(uid))
                                            {
                                                Controls["lblTime4Relay" + laneNo].Text = misc.format_time(thisLaptime) + " (@" + (thisLap * get_lap_interval()) + ")";
                                            }
                                            else
                                            {
                                                Controls["lblTime" + laneNo].Text = misc.format_time( thisLaptime) + " (@" + (thisLap * get_lap_interval()) + ")";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                program_db.inc_race_number(ref prgNo);
            }

        }
*/
