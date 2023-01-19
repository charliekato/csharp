using System;
using System.Windows.Forms;
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
public  class timeData
{
    Queue<int> dataFifo = new Queue<int>();
    private static int timeDataEncode(int timeint, int laneNo, int goalFlag)
    {
        return laneNo * 1000000 + (goalFlag * 10000000) + timeint;
    }
    private static void timeDataDecode(int timedata, ref int timeint, ref int laneNo, ref int goalFlag)
    {
        goalFlag = timedata / 10000000;
        laneNo = (timedata % 10000000) / 1000000;
        timeint = timedata % 1000000;
    }
    public void push()
    {
        dataFifo.Enqueue(0);
    }
    public void push(int timeint, int laneNo, int goalFlag)
    {
        dataFifo.Enqueue(timeDataEncode(timeint, laneNo, goalFlag));
    }
    public bool pop(ref int timeint, ref int laneNo, ref int goalFlag)
    {
        if (dataFifo.Count>0)
        {
            timeDataDecode(dataFifo.Dequeue(), ref timeint, ref laneNo, ref goalFlag);
            return true;
        }
        return false;
        
    }
}

public class LoopTest
{
    static timeData tmd = new timeData();
    static SerialPort _serialPort;
    static byte[] sendData;
    static string excelFile= @"C:\Users\ykato\Dropbox\Private\水泳協会\adoTest.xlsx";
    //static string excelFile = @"Y:\adoTest.xls";
    public static void init_data()
    {

        sendData = new byte[256];
        byte counter = 0;
        for ( counter=0; counter==0; counter++)
        {
            sendData[counter] = counter;
        }
    }
    public static void Write()
    {
        byte i;
        for (i=0; i<100; i++)
        {
            _serialPort.Write(sendData, 0, 256);
        }
    }
    public static void send(string data)
    {
        byte stx = 2;
        byte etx = 3;
        byte[] ctrdat = { stx, etx }; // stx, etx

        _serialPort.Write(ctrdat, 0, 1);
        _serialPort.Write(data);
        _serialPort.Write(ctrdat, 1, 1);
    }
    public static void lap(int lane,int mytime)
    {
        string data = "A1" + lane + "  " + timeint2str(mytime) + "L01";
        send(data);
    }
    public static void goal(int lane, int mytime)
    {
        string data = "A1" + lane + "  " + timeint2str(mytime) + "G01";
        send(data);
    }
    public static string timeint2str(int mytime) {
        int minutes = mytime / 10000;
        int temps = mytime % 10000;
        int seconds = temps / 100;
        int centiseconds = temps % 100;
        if (minutes>0)
        {
            return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2,'0') + "."+ centiseconds.ToString().PadLeft(2, '0');
        }
        return  "   "+seconds.ToString().PadLeft(2) + "."+ centiseconds.ToString().PadLeft(2, '0');
    }
    public static int timestr2int(string mytime)
    {
        int position;
        position = mytime.IndexOf(':');
        if (position >= 0)
        {
            mytime = mytime.Remove(position, 1);
        }

        position = mytime.IndexOf('.');

        mytime = mytime.Remove(position, 1);
        return Convert.ToInt32(mytime);
    }
    static string get_file(string initFile) {

	OpenFileDialog ofd = new OpenFileDialog();
	ofd.FileName = initFile;
	ofd.InitialDirectory = @"C:\Users\ykato";
	ofd.Filter = "エクセルファイル(*.xlsx;*.xls;*.xlsm)|*.xlsx;*.xls;*.xlsm";
	ofd.FilterIndex = 2;
	ofd.Title = "エクセルファイルを選択してください";
	ofd.RestoreDirectory = true;
	ofd.CheckFileExists = true;
	ofd.CheckPathExists = true;

	if (ofd.ShowDialog() == DialogResult.OK)
	{
	    //OKボタンがクリックされたとき、選択されたファイル名を表示する
	    Console.WriteLine("> {0} <",ofd.FileName);
	    return (ofd.FileName);
	}
	return(initFile);
    }

    [STAThread]
    public static void Main()
    {
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);

        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(readandFifoPush);
        _serialPort = new SerialPort();
        _serialPort.PortName =  SetPortName(_serialPort.PortName); // Usually COM3
        _serialPort.BaudRate = 9600;
        _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), "Even");
        _serialPort.DataBits = 7;
        _serialPort.ReadTimeout = -1;
	//how about stop bits?

	excelFile=get_file(excelFile);
        _serialPort.Open();
        readThread.Start();
        fifoRead();

//        Thread.Sleep(4000);
        readThread.Join();
        _serialPort.Close();
    }
    public static void WriteOrg()
    {
        string startdata = "AR       0.0 S  ";
		int skip;
        for (int i=0; i<10; i++)
        {
			skip=i%10;
            send(startdata);
            for (int l = 1; l < 10; l++)
            {
				if (l!=skip) {
					lap(l, 2934 - l+i);	
				}             
            }
			//Thread.Sleep(2000);
            for (int l = 1; l < 10; l++)
            {
				if (l!=skip)
                goal(l, 12345 + l+i);
            }
            //Thread.Sleep(2000);
        }

    }
    public static int convert_ascii_2_time(byte[] vs,int start=0)
    {
        int minutes;
        int seconds;
        int tenmilseconds;
        minutes = Convert.ToInt32( Encoding.ASCII.GetString(vs,start,2));
        seconds = Convert.ToInt32(Encoding.ASCII.GetString(vs, start+3, 2));
        tenmilseconds = Convert.ToInt32(Encoding.ASCII.GetString(vs, start+6, 2));
        return tenmilseconds + (100 * seconds) + (10000 * minutes);
    }
    public static bool is_start(byte[] timedt)
    {
        return ((timedt[13] == 'S')&(timedt[0]=='A')&(timedt[1]=='R'));
    }
    public static bool is_start2(byte[] timedt)
    {
        return (timedt[13] == 'S');
    }
    public static bool is_lap(byte[] timedt)
    {

        return (timedt[13] == 'L'); 
    }
    public static bool is_goal(byte[] timedt)
    {

        return (timedt[13] == 'G');
    }
    public static int get_lane_number(string timedata)
    {
        Encoding enc = Encoding.GetEncoding("Shift_JIS");
        byte[] bstr = enc.GetBytes(timedata);
        return (int)(bstr[2] - 48);
    }
    public static int get_time(byte[] timedt)
    {
        return convert_ascii_2_time(timedt, 5);
    }
    public static void adjust(int[] counter)
    {
        int i;
        int adjustValue=0;
        for (i=0; i<10; i++)
        {
            if (adjustValue < counter[i]) adjustValue = counter[i];
        }
        for (i = 0; i < 10; i++) counter[i] = adjustValue;
    }
    public static void fifoRead()
    {
        int timeint = 111111;
        int laneNo = 0;
        int goal = 0;
        int[] lapCounter = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        
        while (true)
        {

            bool rc;
            rc = tmd.pop(ref timeint, ref laneNo, ref goal);
            if (rc)
            {
                if (timeint == 0) adjust(lapCounter);
                else
                {
                    lapCounter[laneNo]++;
                    string mytime = timeint2str(timeint);
                    Console.WriteLine("Laptime,counter: " + lapCounter[laneNo] + " Lane " + laneNo + "  time :" + mytime);
                    ExcelConnection.insert(excelFile, lapCounter[laneNo], laneNo, mytime);

                }
            }
        }
    }
    public static void read_data()
    {
        byte[] buffer = new byte[256];
        int howManyRead;
        try
        {
            while(true)
            {
                howManyRead = _serialPort.Read(buffer, 0, 256);
                for (int i=0; i<howManyRead; i++)
                {
                    Console.Write(buffer[i].ToString("X2") + " ");
                    if ((i%16)==0)
                    {

                    }
                }
            }
        } catch
        {

        }
    }
    public static void readandFifoPush()
    {
        byte[] buffer = new byte[100];
        byte[] charbyte = new byte[20];
        const byte stx = 2;
        const byte etx = 3;
        int howmanyread;
        int counter = -1;
        int laneNo=0;

        string mytime = "";
        try
        {
            while (true)
            {
                howmanyread = _serialPort.Read(buffer, 0, 54); // 54=18*3
                //Console.WriteLine("{0} bytes read", howmanyread);
                for (int j = 0; j < howmanyread; j++)
                {
                    if (buffer[j] == stx)
                    {
                        counter = 0;
                    }
                    else if (buffer[j] == etx)
                    {
                        counter = -1;  
                        if (is_start(charbyte))
                        {
                            tmd.push();
                        }
                        
                        if (is_lap(charbyte))
                        {
                            laneNo = charbyte[2] - 48;
                            
                            mytime = Encoding.ASCII.GetString(charbyte, 5, 8);
                            tmd.push(timestr2int(mytime), laneNo, 0);
                           
                        }
                        if (is_goal(charbyte))
                        {
                            laneNo = charbyte[2] - 48;
                           
                            mytime = Encoding.ASCII.GetString(charbyte, 5, 8);
                            tmd.push(timestr2int(mytime), laneNo, 1);
                        }
                       
                    } else if (counter>=0)
                    {
                        charbyte[counter++] = buffer[j];
                        //Console.Write("{0} ,{1}", buffer[j], Encoding.ASCII.GetString(buffer, j,1));
                        if (counter > 16)
                        {
                            Console.WriteLine("error counter reaches 17.");
                            counter = -1;
                        }
                    }

                }
            }

           
        }
        catch (TimeoutException e) {
            Console.WriteLine(e.Message);   
        }
    }

    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("COM port({0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "")
        {
            portName = defaultPortName;
        }
        return portName;
    }

    public static int SetPortBaudRate(int defaultPortBaudRate)
    {
        string baudRate;

        Console.Write("Baud Rate({0}): ", defaultPortBaudRate);
        baudRate = Console.ReadLine();

        if (baudRate == "")
        {
            baudRate = defaultPortBaudRate.ToString();
        }

        return int.Parse(baudRate);
    }

    public static Parity SetPortParity(Parity defaultPortParity)
    {
        string parity;

        Console.WriteLine("Available Parity options:");
        foreach (string s in Enum.GetNames(typeof(Parity)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Parity({0}):", defaultPortParity.ToString());
        parity = Console.ReadLine();

        if (parity == "")
        {
            parity = defaultPortParity.ToString();
        }

        return (Parity)Enum.Parse(typeof(Parity), parity);
    }

    public static int SetPortDataBits(int defaultPortDataBits)
    {
        string dataBits;

        Console.Write("Data Bits({0}): ", defaultPortDataBits);
        dataBits = Console.ReadLine();

        if (dataBits == "")
        {
            dataBits = defaultPortDataBits.ToString();
        }

        return int.Parse(dataBits);
    }

    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
    {
        string stopBits;

        Console.WriteLine("Available Stop Bits options:");
        foreach (string s in Enum.GetNames(typeof(StopBits)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Stop Bits({0}):", defaultPortStopBits.ToString());
        stopBits = Console.ReadLine();

        if (stopBits == "")
        {
            stopBits = defaultPortStopBits.ToString();
        }

        return (StopBits)Enum.Parse(typeof(StopBits), stopBits);
    }

    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
    {
        string handshake;

        Console.WriteLine("Available Handshake options:");
        foreach (string s in Enum.GetNames(typeof(Handshake)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Handshake({0}):", defaultPortHandshake.ToString());
        handshake = Console.ReadLine();

        if (handshake == "")
        {
            handshake = defaultPortHandshake.ToString();
        }

        return (Handshake)Enum.Parse(typeof(Handshake), handshake);
    }
}

