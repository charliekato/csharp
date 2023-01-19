//

using System;
using System.Windows.Forms;

public class FormsTimerTest {
  public static Timer timer;
  public static EventHandler ev1,ev2;

  public FormsTimerTest() {
  	Console.WriteLine("started...");
	timer = new Timer();
	ev1 = new EventHandler(Program.MyClock);
	ev2 = new EventHandler(Program.anotherClock);
  }

}
class Program {
  private static int counter;
  public static void anotherClock(object sender, EventArgs e) {
    counter++;
    Console.WriteLine("another >>  " + counter);
    FormsTimerTest.timer.Tick -= FormsTimerTest.ev2;
    FormsTimerTest.timer.Tick += FormsTimerTest.ev1;
	
  }


  public static void MyClock(object sender, EventArgs e) {
    counter++;
    Console.WriteLine("MyClock  >>  " + counter);
    FormsTimerTest.timer.Tick -= FormsTimerTest.ev1;
    FormsTimerTest.timer.Tick += FormsTimerTest.ev2;
	
  }

  static void Main() {
    FormsTimerTest ftt = new FormsTimerTest();
    FormsTimerTest.timer.Tick +=FormsTimerTest.ev1;
    FormsTimerTest.timer.Interval = 1000;
    FormsTimerTest.timer.Enabled = true; // timer.Start()と同じ
    MessageBox.Show("...");
    Application.Exit();
  }
}

