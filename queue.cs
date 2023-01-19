using System;

using System.Collections.Generic;
using System.Linq;

static class Program {
static void Main(string[] args)
{
  // 整数を格納するキュー
  var que = new Queue<int>();

  // キューに1から3まで順に投入
  for (int i = 1; i < 4; i++)
    que.Enqueue(i);

  // キューから全てを取り出す
  while (que.Count > 0)
  {
    Console.WriteLine("キューの先頭を見る：" + que.Peek());
    Console.WriteLine("キューに入っている個数：" + que.Count);

    int n = que.Dequeue();
    Console.WriteLine("キューから取り出し："+n);
    Console.WriteLine("キューに入っている個数："+que.Count);
  }	
}
}

  // 出力：



