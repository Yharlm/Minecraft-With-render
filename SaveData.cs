using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Minecraft
{
    class main{
        static void CreateFile(string folderPath, string fileName, string content)
        {
            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            // Combine the folder path and file name to get the full file path
            string filePath = Path.Combine(folderPath, fileName);

            // Write the content to the file
            File.WriteAllText(filePath, content);
        }
    }

}
