using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;

namespace InstantGameworksObject
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("OBJ file to Instant Gameworks Object (IGWO)");
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select file to convert OBJ files";
            fileDialog.Filter = "OBJ files|*.obj";
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = @"C:\";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string inputFile = fileDialog.FileName;
                Console.WriteLine("Input: " + inputFile);

                SaveFileDialog outputDialog = new SaveFileDialog();
                outputDialog.Title = "Select output location";
                outputDialog.Filter = "IGWO files|*.igwo";
                outputDialog.InitialDirectory = Path.GetDirectoryName(inputFile);
                outputDialog.OverwritePrompt = true;
                if (outputDialog.ShowDialog() == DialogResult.OK)
                {

                    string outputFile = outputDialog.FileName;
                    Console.WriteLine("Output: " + outputFile);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Beginning conversion process");

                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    
                    var output = Convert.ConvertOBJToIGWO(File.ReadAllLines(inputFile));
                    
                    Console.WriteLine();
                    Console.WriteLine(output.Positions.Length + "\tvertices");
                    Console.WriteLine(output.TextureCoordinates.Length + "\ttexture coordinates");
                    Console.WriteLine(output.Normals.Length + "\tnormals");
                    Console.WriteLine(output.Faces.Length + "\tfaces");
                    Console.WriteLine();
                    Console.WriteLine();

                    DataHandler.WriteFile(outputFile, output);

                    long inputSize = new FileInfo(inputFile).Length;
                    long outputSize = new FileInfo(outputFile).Length;
                    Console.WriteLine("Input file size:  " + inputSize/1024f + " KB\t(" + inputSize/1048576f + " MB)");
                    Console.WriteLine("Output file size: " + outputSize/1024f + " KB\t(" + outputSize / 1048576f + " MB)");
                    Console.WriteLine();
                    Console.WriteLine("File conversion efficiency: " + ((float)inputSize/(float)outputSize)*100f + "% (output is " + ((float)outputSize/(float)inputSize)*100f + "% the size of the original)");
                    Console.WriteLine();
                    Console.WriteLine();

                    timer.Stop();
                    Console.WriteLine("Finished (" + timer.ElapsedTicks / 1000000f + " seconds)");

                    Console.WriteLine("Success!");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadKey();

                    
                }
            }
        }
    }
}
