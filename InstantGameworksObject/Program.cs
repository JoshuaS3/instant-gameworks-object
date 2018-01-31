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
            fileDialog.Title = "Select file to convert OBJ files";
            fileDialog.Filter = "OBJ files|*.obj";
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = @"C:\";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;

                SaveFileDialog outputDialog = new SaveFileDialog();
                outputDialog.Title = "Select output location";
                outputDialog.Filter = "IGWO files|*.igwo";
                outputDialog.InitialDirectory = @"C:\";
                outputDialog.OverwritePrompt = true;
                if (outputDialog.ShowDialog() == DialogResult.OK)
                {

                    string outputFile = outputDialog.FileName;
                    
                    long Beginning = DateTime.Now.Ticks / 10000000;
                    

                    var output = InstantGameworksObject.ConvertOBJToIGWO(File.ReadAllLines(fileName));
                    DataHandler.WriteFile(outputFile, output);
                    Console.WriteLine(outputFile);

                    Console.WriteLine("Finished (" + (DateTime.Now.Ticks / 10000000 - Beginning) + " seconds)");

                    Console.WriteLine("Success!");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadKey();

                    
                }
            }
        }
    }
}
