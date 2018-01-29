using System;
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
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select files to convert OBJ files";
            fileDialog.Filter = "OBJ files|*.obj";
            fileDialog.Multiselect = true;
            fileDialog.InitialDirectory = @"C:\";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = fileDialog.FileNames;

                FolderBrowserDialog outputDialog = new FolderBrowserDialog();
                outputDialog.Description = "Select folder to output files to";
                outputDialog.RootFolder = Environment.SpecialFolder.Desktop;
                if (outputDialog.ShowDialog() == DialogResult.OK)
                {

                    string outputPath = outputDialog.SelectedPath;

                    IFormatter formatter = new BinaryFormatter();

                    long Beginning = DateTime.Now.Ticks / 10000000;
                    foreach (string file in fileNames)
                    {
                        Console.WriteLine("Begin " + Path.GetFileName(file));

                        var output = InstantGameworksObject.ConvertOBJToIGWO(File.ReadAllLines(file));

                        Console.WriteLine("Writing file");

                        Stream fileOutputStream = new FileStream(outputPath + @"\" + Path.GetFileNameWithoutExtension(file) + ".igwo", FileMode.Create, FileAccess.Write, FileShare.None);
                        formatter.Serialize(fileOutputStream, output);


                        Console.WriteLine(outputPath + @"\" + Path.GetFileNameWithoutExtension(file) + ".igwo");
                    }

                    Console.WriteLine("Finished (" + (DateTime.Now.Ticks / 10000000 - Beginning) + " seconds)");

                    Console.WriteLine("Success!");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadKey();

                    
                }
            }
        }
    }
}
