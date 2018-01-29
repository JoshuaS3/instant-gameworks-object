using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace InstantGameworksObject
{
    class InstantGameworksObject
    {
        struct InstantGameworksObjectData
        {
            public float ScaleFactor;

            public Position[] Positions;
            public Color[] Colors;
            public TextureCoordinates[] TextureCoordinates;
            public Position[] Normals;

            public Face[] Faces;
        }
        struct Position
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
            Position(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
                W = 1;
            }
            Position(float x, float y, float z, float w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }
        }
        struct Color
        {

            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }
            Color(float r, float g, float b)
            {
                R = r;
                G = g;
                B = b;
                A = 1f;
            }
            Color(float r, float g, float b, float a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
            }
        }
        struct TextureCoordinates
        {

            public float U { get; set; }
            public float V { get; set; }
            public float W { get; set; }
            TextureCoordinates(float u, float v)
            {
                U = u;
                V = v;
                W = 0;
            }
            TextureCoordinates(float u, float v, float w)
            {
                U = u;
                V = v;
                W = w;
            }
        }
        struct Vertex
        {
            public int PositionIndex; //Where in the Positions list can I find this data? (gets rid of repitition)
            public int ColorIndex;
            public int TextureCoordinatesIndex;
            public int NormalIndex;
        }
        struct Face
        {
            Vertex[] Vertices;
        }

        

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
            
            
            Console.WriteLine("Reformatting data (Positions)          ");
            vertexData += string.Join(";", sortedVertices) + ";";
            

            Console.WriteLine("Reformatting data (Colors)             ");
            vertexColorData += string.Join(";", vertexColors) + ";";
            

            Console.WriteLine("Reformatting data (Normals)            ");
            vertexNormalData += string.Join(";", sortedNormals) + ";";
            

            Console.WriteLine("Reformatting data (Texture Coordinates)");
            vertexTextData += string.Join(";", sortedTextureCoords) + ";";
            

            Console.WriteLine("Reformatting data (Faces)              ");
            faceData += string.Join(";", sortedFaces) + ";";




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
