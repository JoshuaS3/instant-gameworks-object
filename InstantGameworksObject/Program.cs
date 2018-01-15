using System;
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
                    
                    foreach (string file in fileNames)
                    {
                        string output = Convert.OBJtoIGWO(File.ReadAllLines(file));
                        FileStream a = new FileStream(outputPath + @"\" + Path.GetFileNameWithoutExtension(file) + ".igwo", FileMode.Create);
                        BinaryWriter b = new BinaryWriter(a);
                        b.Write(output);
                        Console.WriteLine(outputPath + @"\" + Path.GetFileNameWithoutExtension(file) + ".igwo");
                    }
                    Console.WriteLine("Success!");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadKey();

                    
                }
            }
        }
    }
}
