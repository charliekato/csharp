//
using System;
using System.Windows.Forms;
public class Class1 {
  private static bool already_occupied(int[] array, int laneNo) { return (array[laneNo] > 0); }


    public static int Main() {
      int[] swimmerID=new int[5];
      swimmerID[1]=5;
      if (already_occupied(swimmerID,0) ) MessageBox.Show("error!!");
      if (already_occupied(swimmerID,1) ) MessageBox.Show("OK!!");
    return 0;
    }
 }
