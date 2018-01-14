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
            string outputFile;
            try //Convert specific files
            {
                inputFile = args[0];
                outputFile = args[1];
                Console.WriteLine("Input:" + inputFile + " Output:" + outputFile);
            }
            catch //Convert all files in a directory
            {
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
                Console.WriteLine("Execute");
                string s = Console.ReadLine();
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                foreach (byte b in bytes)
                {
                    Console.WriteLine(Convert.ToString(b, 2));
                }
            }
        }
    }
}
