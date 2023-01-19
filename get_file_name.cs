using System;
using System.IO;
using System.Text;

class Program {

    static void Main() {
	string folderName = get_folder_name("temp.txt");
	Console.WriteLine("--> {0} <--", folderName);
    }

	
    public static string get_folder_name(string path)
        {
            string rcString;
            rcString = "";

            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[1024];
                

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        rcString=System.Text.Encoding.UTF8.GetString(b).Trim('\0');

                    }
                }
            } 
            return rcString;

        }
}

