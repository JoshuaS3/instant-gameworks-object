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
        public static void CompressFile(string path)
        {
            FileStream sourceFile = File.OpenRead(path);
            FileStream destinationFile = File.Create(path + ".igwo");

            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);

            using (GZipStream output = new GZipStream(destinationFile,
                CompressionMode.Compress))
            {
                output.Write(buffer, 0, buffer.Length);
            }

            // Close the files.
            sourceFile.Close();
            destinationFile.Close();
        }
        public static string OBJtoIGWO(string[] OBJContent)
        {
            string IGWOContent = "~Instant Gameworks\n";

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
                        colorLine = string.Concat(Normalize(splitLine[3]), " ", Normalize(splitLine[4]), " ", Normalize(splitLine[5]));
                        vertexColors.Add(colorLine);
                    }
                    adjustedLine = string.Concat(Normalize(splitLine[0]), " ", Normalize(splitLine[1]), " ", Normalize(splitLine[2]));
                    vertices.Add(adjustedLine);
                }
                else if (line.StartsWith("vn "))
                {
                    isNormalData = true;
                    adjustedLine = line.Substring(3);
                    splitLine = adjustedLine.Split(new[] { ' ' });
                    adjustedLine = string.Concat(Normalize(splitLine[0]), " ", Normalize(splitLine[1]), " ", Normalize(splitLine[2]));
                    vertexNormals.Add(adjustedLine);
                }
                else if (line.StartsWith("vt "))
                {
                    isTextureData = true;
                    adjustedLine = line.Substring(3);
                    splitLine = adjustedLine.Split(new[] { ' ' });
                    adjustedLine = string.Concat(Normalize(splitLine[0]), " ", Normalize(splitLine[1]), " ", Normalize(splitLine[2]));
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

            foreach (string vertex in sortedVertices)
            {
                vertexData += vertex + ";";
            }
            foreach (string color in vertexColors)
            {
                vertexColorData += color + ";";
            }
            foreach (string normal in sortedNormals)
            {
                vertexNormalData += normal + ";";
            }
            foreach (string texdata in sortedTextureCoords)
            {
                vertexTextData += texdata + ";";
            }
            foreach (string face in sortedFaces)
            {
                faceData += face + ";";
            }




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
