//

using System;
using System.Windows.Forms;

public class FormsTimerTest {

  static void Main() {
    FormsTimerTest ftt = new FormsTimerTest();
    ftt.Run();
  }

  public void Run() {
    Timer timer = new Timer();
    timer.Tick += new EventHandler(MyClock);
    timer.Interval = 1000;
    timer.Enabled = true; // timer.Start()と同じ

    Application.Run(); // メッセージループを開始
  }

  public void MyClock(object sender, EventArgs e) {
    Console.WriteLine(DateTime.Now);
    // 出力例：
    // 2005/11/08 19:59:10
    // 2005/11/08 19:59:11
    // 2005/11/08 19:59:12
    // ……
  }
}
