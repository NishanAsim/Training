using System;
using System.Threading.Tasks;
using FileParserLibrary;

namespace AsyncApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await FileParser.ReadFile(@"D:\Code\GitHubPublic\Training\AsyncSample\AsyncApp\Program.cs");
            await FileParser.ReadFile(@"D:\Code\GitHubPublic\Training\AsyncSample\FileParserLibrary\FileParser.cs");

            // var program = await FileParser.ReadFile(@"D:\Code\GitHubPublic\Training\AsyncSample\AsyncApp\Program.cs");
            // var library = await FileParser.ReadFile(@"D:\Code\GitHubPublic\Training\AsyncSample\FileParserLibrary\FileParser.cs");
            // await program;
            // await library;
            Console.WriteLine("Done");

        }
    }
}
