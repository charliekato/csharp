    using System;  
    using System.IO.Ports;  
      
    public class ComPortCheck  
    {  
        static SerialPort _serialPort;  
      
        public static void Main()  
        {  
      
            // Create a new SerialPort object with default settings.  
            using (_serialPort = new SerialPort() ) {
	      get_port_name();
             }  
	}
      
     
        public static void get_port_name()  
        {  
      
            Console.WriteLine("Available Ports:");  
            foreach (string s in SerialPort.GetPortNames())  
            {  
                Console.WriteLine("   {0}", s);  
            }  
      
        }  
     
  
  
  
    }  
