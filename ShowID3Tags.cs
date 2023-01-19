using System;
using Shell32;

class ShowID3Tags {
  static void Main() {

    string dir = @"C:\Users\ykato\Music\iTunes\iTunes_Media\Queen\'Greatest Hits'"; // MP3ファイルのあるディレクトリ
    string file = "'01 Bohemian Rhapsody.mp3'";

    ShellClass shell = new ShellClass();
    Folder f = shell.NameSpace(dir);
    FolderItem item = f.ParseName(file);

    Console.WriteLine(f.GetDetailsOf(item,  9)); // アーティスト
    Console.WriteLine(f.GetDetailsOf(item, 10)); // タイトル
    Console.WriteLine(f.GetDetailsOf(item, 12)); // ジャンル
    Console.WriteLine(f.GetDetailsOf(item, 14)); // コメント
    Console.WriteLine(f.GetDetailsOf(item, 17)); // アルバムのタイトル
    Console.WriteLine(f.GetDetailsOf(item, 18)); // 年
    Console.WriteLine(f.GetDetailsOf(item, 19)); // トラック番号

    // 出力例：
    // Billy Joel
    // James
    // Rock
    // no comment
    // Turnstiles
    // 1976
    // 5
  }
}
