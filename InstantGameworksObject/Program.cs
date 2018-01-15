using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace InstantGameworksObject
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile;
            try //Convert specific file
            {
                string outputFile;
                inputFile = args[0];
                outputFile = args[1];
                string output = Convert.OBJtoIGWO(File.ReadAllLines(inputFile));
                FileStream a = new FileStream(outputFile, FileMode.Create);
                StreamWriter b = new StreamWriter(a);
                b.Write(output);
                Console.WriteLine(Path.GetFullPath(outputFile));
                Console.WriteLine("Success!");
            }
            catch //Convert all files in a directory
            {
                try
                {
                    string directory = args[0];
                    char Answer;
                    bool _Answer = false;
                    while (!_Answer)
                    {
                        Console.Write("Are you sure you want to convert all files in the directory (y/n)?: ");
                        Answer = char.ToLower(Console.ReadKey().KeyChar);
                        Console.WriteLine();
                        switch (Answer)
                        {
                            case 'y':
                                _Answer = true;
                                break;
                            case 'n':
                                Console.WriteLine("Exiting...");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid input. Try again.");
                                break;
                        }
                    }

                    var ext = new List<string> { ".obj" };
                    var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s)));
                    foreach (string file in files)
                    {
                        string output = Convert.OBJtoIGWO(File.ReadAllLines(file));
                        FileStream a = new FileStream(Path.GetFileNameWithoutExtension(file) + ".igwo", FileMode.Create);
                        BinaryWriter b = new BinaryWriter(a);
                        b.Write(output);
                        Console.WriteLine(Path.GetFullPath(Path.GetFileNameWithoutExtension(file) + ".igwo"));
                    }
                    Console.WriteLine("Success!");
                }
                catch
                {
                    Console.WriteLine("Invalid arguments");
                }
            }
        }
    }
}
