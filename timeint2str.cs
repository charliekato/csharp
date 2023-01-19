
using System;

namespace atest {
    static class prg {
	static void Main() {
	    Console.WriteLine(" "+timeint2str(10302));
	}



	public static string timeint2str(int mytime) {
	    int minutes = mytime / 10000;
	    int temps = mytime % 10000;
	    int seconds = temps / 100;
	    int centiseconds = temps % 100;
	    if (minutes>0)
	    {
		return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2, '0') + "."+ centiseconds.ToString().PadLeft(2, '0');
	    }
	    return  "   "+seconds.ToString().PadLeft(2) + "."+ centiseconds.ToString().PadLeft(2, '0');
	}
    }
}
 
