using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

namespace InstantGameworksObject
{
    class Convert
    {
        private static string Normalize(string value)
        {
            return value.Contains(".") ? value.TrimEnd('0').TrimEnd('.') : value;
        }
        private static string Precision(string num, int precision)
        {
            float newNum = float.Parse(num);
            double mult = Math.Pow(10, precision);
            double result = Math.Truncate(mult * newNum) / mult;
            return result.ToString();
        }
        private static string Scale(string num, float scale)
        {
            float newNum = float.Parse(num);
            return Precision((newNum * scale).ToString(), 0);
        }
        public static string OBJtoIGWO(string[] OBJContent)
        {
            string IGWOContent = "~Instant Gameworks\n";

            float scale = 1000f;

            bool isVertexData = false;
            bool isColorData = false;
            bool isTextureData = false;
            bool isNormalData = false;
            bool isFaceData = false;

            List<string> vertices = new List<string>();
            List<string> vertexNormals = new List<string>();
            List<string> vertexColors = new List<string>();
            List<string> vertexTextureCoords = new List<string>();
            List<string> faces = new List<string>();

            string vertexData = "\nv\n";
            string vertexColorData = "\nc\n";
            string vertexNormalData = "\nn\n";
            string vertexTextData = "\nt\n";
            string faceData = "\nf\n";

            string adjustedLine;
            string[] splitLine;
            string colorLine;

            Console.WriteLine("Sorting data");

            foreach (string line in OBJContent) //Sort into groups
            {
                if (line.StartsWith("v "))
                {
                    isVertexData = true;
                    adjustedLine = line.Substring(2);
                    splitLine = adjustedLine.Split(new[] { ' ' });
                    if (splitLine.Length == 6)
                    {
                        isColorData = true;
                        colorLine = string.Concat(Precision(Normalize(splitLine[3]), 2), " ", Precision(Normalize(splitLine[4]),2), " ", Precision(Normalize(splitLine[5]), 2));
                        vertexColors.Add(colorLine);
                    }
                    adjustedLine = string.Concat(Scale(Normalize(splitLine[0]), scale), " ", Scale(Normalize(splitLine[1]), scale), " ", Scale(Normalize(splitLine[2]), scale));
                    vertices.Add(adjustedLine);
                }
                else if (line.StartsWith("vn "))
                {
                    isNormalData = true;
                    adjustedLine = line.Substring(3);
                    splitLine = adjustedLine.Split(new[] { ' ' });
                    adjustedLine = string.Concat(Precision(Normalize(splitLine[0]), 2), " ", Precision(Normalize(splitLine[1]), 2), " ", Precision(Normalize(splitLine[2]),2));
                    vertexNormals.Add(adjustedLine);
                }
                else if (line.StartsWith("vt "))
                {
                    isTextureData = true;
                    adjustedLine = line.Substring(3);
                    splitLine = adjustedLine.Split(new[] { ' ' });
                    adjustedLine = string.Concat(Precision(Normalize(splitLine[0]),2), " ", Precision(Normalize(splitLine[1]),2), " ", Precision(Normalize(splitLine[2]),2));
                    vertexNormals.Add(adjustedLine);
                }
                else if (line.StartsWith("f "))
                {
                    isFaceData = true;
                    adjustedLine = line.Substring(2);
                    faces.Add(adjustedLine);
                }
            }

            string[] sortedVertices = new string[vertices.Count];
            string[] sortedNormals = new string[vertexNormals.Count];
            string[] sortedTextureCoords = new string[vertexTextureCoords.Count];
            string[] sortedFaces = new string[faces.Count];
            int sfi = 0;


            Console.WriteLine("Parsing faces");

            foreach (string face in faces) //  "v1 v2 v3"
            {
                string[] theseVertices = face.Split(new[] { ' ' });
                string thisFaceData = "";
                foreach (string vertex in theseVertices) // "v1/vt1/vn1"
                {
                    string[] splitVertex = vertex.Split(new[] { '/' });
                    int vertexN = Int32.Parse(splitVertex[0]) - 1;
                    sortedVertices[vertexN] = vertices[vertexN];
                    if (vertex.Contains("//"))
                    {
                        int vertexNorm = Int32.Parse(splitVertex[2]) - 1;
                        sortedNormals[vertexNorm] = vertexNormals[vertexNorm];
                        thisFaceData += vertexN + "//" + vertexNorm + " ";
                    }
                    else
                    {
                        if (splitVertex.Length == 2)
                        {
                            int pos = Int32.Parse(splitVertex[1]) - 1;
                            sortedTextureCoords[pos] = vertexTextureCoords[pos];
                            thisFaceData += vertexN + "/" + pos + " ";
                        }
                        else if (splitVertex.Length == 3)
                        {
                            int pos = Int32.Parse(splitVertex[1]) - 1;
                            int pos2 = Int32.Parse(splitVertex[2]) - 1;
                            sortedTextureCoords[pos] = vertexTextureCoords[pos];
                            sortedNormals[pos2] = sortedNormals[pos2];
                            thisFaceData += vertexN + "/" + pos + "/" + pos2 + " ";
                        }
                    }
                }
                sortedFaces[sfi] = thisFaceData.Substring(0, thisFaceData.Length - 1);
                sfi++;
            }

            Console.WriteLine("Reformatting data                      ");

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("Reformatting data (Positions)          ");
            vertexData = string.Join(";", sortedVertices);

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("Reformatting data (Colors)             ");
            vertexColorData = string.Join(";", vertexColors);

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("Reformatting data (Normals)            ");
            vertexNormalData = string.Join(";", sortedNormals);

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("Reformatting data (Texture Coordinates)");
            vertexTextData = string.Join(";", sortedTextureCoords);

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine("Reformatting data (Faces)              ");
            faceData = string.Join(";", sortedFaces);

            IGWOContent += "~scale factor: " + scale;


            Console.WriteLine("Compositing data");

            if (isVertexData)
            {
                IGWOContent += vertexData;
            }
            if (isColorData)
            {
                IGWOContent += vertexColorData;
            }
            if (isNormalData)
            {
                IGWOContent += vertexNormalData;
            }
            if (isTextureData)
            {
                IGWOContent += vertexTextData;
            }
            if (isFaceData)
            {
                IGWOContent += faceData;
            }

            return IGWOContent;
        }
    }
}
