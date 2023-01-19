
using System;
using System.IO;
using System.Text;

class Program
{
  static void Main(string[] args)
  {
    Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
    // （1）テキストファイルを開いて（なければ作って）StreamWriterオブジェクトを得る
    using (StreamWriter writer = new StreamWriter("Test.txt", false, sjisEnc))
    {
      // （2）ファイルにテキストを書き込む
      writer.WriteLine(DateTime.Now.ToString("書き込み時刻：HH:mm:ss"));

    } // （3）usingブロックを抜けるときにファイルが閉じられる
  }
}