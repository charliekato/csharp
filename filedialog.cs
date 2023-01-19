using System;
using System.Windows.Forms;

public static class Program {
    [STAThread]
    static string get_file(string initFile) {
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);


	OpenFileDialog ofd = new OpenFileDialog();
	ofd.FileName = initFile;
	ofd.InitialDirectory = @"C:\Users\ykato";
	ofd.Filter = "エクセルファイル(*.xlsx;*.xls;*.xlsm)|すべてのファイル(*.*)|*.*";
	ofd.FilterIndex = 2;
	ofd.Title = "エクセルファイルを選択してください";
	ofd.RestoreDirectory = true;
	ofd.CheckFileExists = true;
	ofd.CheckPathExists = true;

	if (ofd.ShowDialog() == DialogResult.OK)
	{
	    //OKボタンがクリックされたとき、選択されたファイル名を表示する
	    return (ofd.FileName);
	}
    }
}
