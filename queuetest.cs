using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace queuetest
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*  Application.EnableVisualStyles();
              Application.SetCompatibleTextRenderingDefault(false);
              Application.Run(new Form1()); */
            // 整数を格納するキュー
            var que = new Queue<int>();

            // キューに1から3まで順に投入
            for (int i = 1; i < 10; i++)
                que.Enqueue(i);

            // キューから全てを取り出す
            while (que.Count > 0)
            {
                Console.WriteLine("キューの先頭を見る：{0}",que.Peek());
                Console.WriteLine("キューに入っている個数：{0}",que.Count);

                int n = que.Dequeue();
                Console.WriteLine("キューから取り出し：{0}",n);
                Console.WriteLine("キューに入っている個数：{0}",que.Count);
            }
        }
    }
}