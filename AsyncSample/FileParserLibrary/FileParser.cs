using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileParserLibrary
{
    public class FileParser
    {
        public static async Task ReadFile(string filePath)
        {
            using StreamReader fs = File.OpenText(filePath);
            byte[] b = new byte[100];
            var line = await fs.ReadLineAsync();
            while (line != null)
            {
               
                Console.WriteLine(line);
                line = await fs.ReadLineAsync();
            }
        }
    }
}
