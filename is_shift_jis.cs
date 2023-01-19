using System;
using System.IO;
using System.Collections;
using System.Data.OleDb;

public class detect {

    public static void Main() {
	System.IO.FileStream fs = new System.IO.FileStream(
	    @"C:\Users\ykato\OneDrive\MyPrograms\cs\aa.utf8.txt",
	    System.IO.FileMode.Open,
	    System.IO.FileAccess.Read);
	byte[] bs = new byte[fs.Length];
	fs.Read(bs, 0, bs.Length);
	fs.Close();
	if (isShiftJis(bs) ) Console.WriteLine("shift jis");
	else Console.WriteLine("may be UTF-8");

    }

    private static bool isShiftJis(byte[] bs)
    {
	int count = 0;

	for (int i = 0; i < bs.Length - 1; i++)
	{
	    if (bs[i] == 0x82 && (0x9f <= bs[i + 1] || bs[i + 1] <= 0xf1))
	    {
		count += 1;
	    }
	    else if(bs[i] == 0x83 && (0x40 <= bs[i + 1] || bs[i + 1] <= 0x96))
	    {
		count += 1;
	    }
	}
	if ((double)count / (double)bs.Length > 0.07) return true;
	else return false;
    }
}
