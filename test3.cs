using System;


namespace mytest {
  
  class Program {
    public static int timestr2int(string mytime)
    {
		int position;
		position = mytime.IndexOf(':');
		if (position>=0) {
		  mytime=mytime.Remove(position, 1);	
		}
          
		position=mytime.IndexOf('.');

        mytime=mytime.Remove(position, 1);

        return Convert.ToInt32(mytime);
    }
    public static string timeint2str(int mytime) {
      int minutes = mytime / 10000;
      int temps = mytime % 10000;
      int seconds = temps / 100;
      int centiseconds = temps % 100;
      if (minutes>0) {
	return minutes.ToString().PadLeft(2) + ":" + seconds.ToString().PadLeft(2) + "."+ centiseconds.ToString().PadLeft(2, '0');
      }
	return "   "+seconds.ToString().PadLeft(2) +"."+  centiseconds.ToString().PadLeft(2, '0');
    }
    static void Main() {
      Console.WriteLine( ">" + timeint2str(12345));
      Console.WriteLine( ">" + timeint2str(3212));
      Console.WriteLine( ">" + timeint2str(123210));
      Console.WriteLine( ">" + timeint2str(3200));
      Console.WriteLine( ">" + timeint2str(1205));
	  Console.WriteLine("<"+ timestr2int("32.55"));
	  Console.WriteLine("<"+ timestr2int("1:12.34"));
    }
  }
}


