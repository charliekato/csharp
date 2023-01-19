using System;
using System.IO.Ports;

static class Program {
    public static void Main()
    {
	print_available_port();
    }
        
    public static void print_available_port()
    {

	Console console=new Console();
        console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            console.WriteLine("   {0}", s);
        }
    }

}


