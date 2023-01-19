
using System;

public class Program {
    public static void Main() {
	string[] cmds = System.Environment.GetCommandLineArgs();
	Console.WriteLine(cmds[1]);
    }
}
