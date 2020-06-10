using SourceConverter;
using SourceConverter.Interfaces;
using System.IO;
using static System.Console;

namespace ChallengeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Please enter the path to the file to use: ");
            var path = ReadLine().Trim();
            ISourceConverter outputConverter;

            if (File.Exists(path))
            {
                WriteLine("File found: Please state your output choice - 1 = JSON, 2 = XML");
                var choice = ReadLine().Trim();

                if (choice != "1" && choice != "2")
                    WriteLine("Invalid Choice");
                else
                {
                    WriteLine("Processing File.....");
                    var sourceConverter = new DelimitedFileConverter(new string[] { "," });
                    var theObjects = sourceConverter.DeSerializeObject(path);
                    if (choice == "1")
                    {
                        outputConverter = new JsonConverter();
                        WriteLine(outputConverter.SerializeObject(theObjects));
                    }
                    else
                    {
                        outputConverter = new XmlConverter();
                        WriteLine(outputConverter.SerializeObject(theObjects));
                    }
                }
            }
            else
                WriteLine("File Not Found");
        }

    }
}
