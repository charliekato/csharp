using System;  
using System.Text;
 

public class program
{

  private static void init(int[] arr) {
      int i;
      for (i=0 ; i<10; i++) {
	  arr[i]=0;
      }

  }
  private static void printarr(int[] arr) {
      int i;
      for (i=0; i<10; i++) {
	  Console.WriteLine(" {0} : {1} ", i, arr[i]);
      }
  }
  public static void Main()
  {
      int[]  aa=new int[10];
      printarr(aa);
      aa[1]=10; aa[2]=20; aa[3]=30;
      printarr(aa);
      init(aa);
      printarr(aa);

  }
}
