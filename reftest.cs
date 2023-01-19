using System;  
using System.Text;
 

public class reftest
{
    string myCharp;
  public static void sub(char *charp){
      strcpy( "abc",charp);

  }
  public static void sub2() {

  }
  public static void Main()
  {
      char aa[100];
      sub( aa);
      Console.WriteLine(">>>>" + aa );
  }
}
