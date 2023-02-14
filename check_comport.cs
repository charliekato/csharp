using System;
using System.IO.Ports;
using System.Windows.Forms;

static class Program {
    public static void Main()
    {
	print_available_port();
    }
        
    public static void print_available_port()
    {
	String msgString="";

//	Console console=new Console();
 //       console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            //console.WriteLine("   {0}", s);
	    msgString=msgString + "\n" + s;
        }
	MessageBox.Show("The following Ports are available.\n"+msgString);
    }

}


