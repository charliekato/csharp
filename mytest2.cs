using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
/*
static class Constants {
  public const int NUMSTYLE=7;
  
}
*/

namespace hello {
public   class timeData
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
class Program {
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

  static void Main()
    {
		int timeint=111111;
		int laneNo=0;
		int goal=0;
		bool rc;
		string[] gl = new string[] {"Lap","Goal"};
		timeData tmd=new timeData();
      	tmd.push(123456,1,0);
		tmd.push(111111,2,0);
		tmd.push(5999,3,0);
		tmd.push(11233,4,0);
		tmd.push(321234,1,1);
		tmd.push(121212,2,1);
		
		rc=tmd.pop(ref timeint, ref laneNo, ref goal);
		while (rc) {
			Console.WriteLine(" "+ timeint2str(timeint)+ "lane : " + laneNo +
		      gl[goal] );
			rc=tmd.pop(ref timeint, ref laneNo, ref goal);  			
		}

	
		

    }
  }    
}    

