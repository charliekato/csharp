using System;
using System.Text;
using System.IO.Ports;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SerialDump
{
    static SerialPort _serialPort;
    static byte[] sendData;
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
    public static void read_data()
    {
        byte[] buffer = new byte[256];
        int howManyRead;
        int counter=0;
        try
        {
            while (true)
            {
                howManyRead = _serialPort.Read(buffer, 0, 256);
                for (int i = 0; i < howManyRead; i++)
                {
                    Console.Write(buffer[i].ToString("X2") + " ");
                    counter++;
                    if ((counter % 18) == 0)
                    {
                        Console.WriteLine("");
                    }
                }
            }
        }
        catch
        {

        }
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
            return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2) + "."+ centiseconds.ToString().PadLeft(2, '0');
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
    public static void Main()
    {
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.  
        _serialPort.PortName =  SetPortName(_serialPort.PortName); // Usually COM3
        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
        //   _serialPort.Parity = SetPortParity((Parity)Enum.Parse(typeof(Parity), "Odd"));
        _serialPort.Parity = SetPortParity(_serialPort.Parity);
        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

        // Set the read/write timeouts  
        _serialPort.ReadTimeout = -1;

        _serialPort.Open();
        read_data();

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

    public static void serialdump()
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
			Console.WriteLine("");

                    }
                }
            }
        } catch
        {

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

