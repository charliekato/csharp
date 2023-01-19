// array resize test
// Array.Resize をしてもArrayの中身はイニシャライズされません。
// その実験。
using System;
using System.Windows.Forms;

public class ArrayTest {
  static int[] myarray;
  public static void init1(int mysize) {
    Array.Resize<int>(ref myarray, mysize);
  }
  public static void fillin(int value) {
    int i;
    for (i=0; i<myarray.Length;i++) {
      myarray[i]=value;
    }
  }
  public static void dump() {
    for (int i=0; i<myarray.Length;i++) {
      Console.WriteLine(">"+i+"< " +myarray[i] );
    }
  }

}
public class Program {
  
  static void Main() {
    ArrayTest.init1(3);
    ArrayTest.fillin(5);
    ArrayTest.dump();
    Console.WriteLine("--- after init1---");
    ArrayTest.init1(5);
    ArrayTest.dump();


 	
  }
}
