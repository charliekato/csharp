//
using System;
using System.Windows.Forms;
using System.IO.Ports;

public class CheckCom {
 
    // This is the method to run when the timer is raised.
    private static void list_available_ports() {
	string	portList;
	portList="";
	foreach ( string s in SerialPort.GetPortNames() ) {
	    portList+=s+"\r\n";
	}
 
       MessageBox.Show("以下のポートが利用可能です。"+ "\r\n" + portList );
    }
 
    public static int Main() {
	list_available_ports();
        return 0;
    }
 }
