using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace InstantGameworksObject
{
    [Serializable()]
    public struct InstantGameworksObjectData
    {
        public Position[] Positions;
        public TextureCoordinates[] TextureCoordinates;
        public Position[] Normals;

        public Face[] Faces;
    }

    [Serializable()]
    public struct Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    [Serializable()]
    public struct TextureCoordinates
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

    [Serializable()]
    public struct Vertex
    {
        public int PositionIndex; //Where in the Positions list can I find this data? (gets rid of repitition)
        public int TextureCoordinatesIndex;
        public int NormalIndex;
    }

    [Serializable()]
    public struct Face
    {
        public Vertex[] Vertices;
    }

    public class InstantGameworksObject
    {
        public static InstantGameworksObjectData ConvertOBJToIGWO(string[] OBJContent)
        {
            InstantGameworksObjectData _newObject = new InstantGameworksObjectData();

            List<Position> _vertexPositions = new List<Position>();
            List<TextureCoordinates> _vertexTextureCoordinates = new List<TextureCoordinates>();
            List<Position> _vertexNormals = new List<Position>();
            List<Face> _faces = new List<Face>();


            // Parse data
            Console.WriteLine("Parsing data");
            foreach (string line in OBJContent)
            {
                string[] splitLine = line.Split(' ');
                if (line.StartsWith("v"))
                {
                    float x = float.Parse(splitLine[1]);
                    float y = float.Parse(splitLine[2]);
                    float z = float.Parse(splitLine[3]);
                    _vertexPositions.Add(new Position(x, y, z));
                }
                else if (line.StartsWith("vn"))
                {
                    float x = float.Parse(splitLine[1]);
                    float y = float.Parse(splitLine[2]);
                    float z = float.Parse(splitLine[3]);
                    _vertexNormals.Add(new Position(x, y, z));
                }
                else if (line.StartsWith("vt"))
                {
                    float u = float.Parse(splitLine[1]);
                    float v = float.Parse(splitLine[2]);
                    float w = 0;
                    if (splitLine.Length > 3) //bigger than 'vt u w'
                    {
                        w = float.Parse(splitLine[3]);
                    }
                    _vertexNormals.Add(new Position(u, v, w));
                }
                else if (line.StartsWith("f"))
                {
                    Face thisFace = new Face();
                    Vertex[] theseVertices = new Vertex[3];
                    for (int i = 1; i < 4; i++) // 1st through 3rd index
                    {
                        string[] thisVertexData = splitLine[i].Split('/'); // '5//1' becomes { '5', '', '1' }

                        Vertex thisVertex = new Vertex();
                        thisVertex.PositionIndex = int.Parse(thisVertexData[0]);

                        int textCoordIndex;
                        if (int.TryParse(thisVertexData[1], out textCoordIndex))
                        {
                            thisVertex.TextureCoordinatesIndex = textCoordIndex;
                        }
                        else
                        {
                            thisVertex.TextureCoordinatesIndex = -1;
                        };

                        int normIndex;
                        if (int.TryParse(thisVertexData[2], out normIndex))
                        {
                            thisVertex.NormalIndex = normIndex;
                        }
                        else
                        {
                            thisVertex.NormalIndex = -1;
                        };

                        theseVertices[i - 1] = thisVertex;
                    }
                    thisFace.Vertices = theseVertices;
                    _faces.Add(thisFace);
                }
            }
            
            _newObject.Positions = _vertexPositions.ToArray();
            _newObject.TextureCoordinates = _vertexTextureCoordinates.ToArray();
            _newObject.Normals = _vertexNormals.ToArray();
            _newObject.Faces = _faces.ToArray();

            
            return _newObject;
        }
    }
}
