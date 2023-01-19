using System;
public class User
{
  private static int[] id=new int[5];


  public User()
  {
    System.Console.WriteLine("constructor...");
  }

  ~User()
  {
    System.Console.WriteLine("decon....");
  }
  public static void fill(int i,int v){
    id[i]=v;
  }
  public static void dump() {
    int i;
    for (i=0; i<5; i++) Console.WriteLine(" >> " + id[i] + " <<");
  }
}


namespace mytest {
  class Program {
    static void do_something() {
//      User myuser=new User();
      User.fill(0,1);
      User.fill(1,2);
    }
    static void do_another() {
      User second=new User();
      User.fill(2,3);
      User.fill(3,4);
      User.fill(4,5);
    }
    static void Main() {
      do_something();
      do_another();
          User.dump();

    }
  }
}



