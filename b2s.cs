using System;  
using System.Text;
 

public class B2S
{
  public static int time2int(string timestr){
    int	a;
    string work;
    work=timestr.Replace(":","");
    work=work.Replace(".","");
    return Convert.ToInt32(work);
  }
  public static void Main()
  {
    string timeStr="1:23.33";
    int	t=time2int(timeStr);
    Console.WriteLine(""+timeStr + "==>" + t);
  }
  /*
  public static void Mainx()
  {
    byte[] bytestr={'A','B','C','D', 0};
    string str=Encoding.GetEncoding("Shift_JIS").GetString(bytestr);
    Console.WriteLine(str);
  }
  */
}
