﻿/*  Copyright (c) Joshua Stockin 2018
 *
 *  This file is part of Instant Gameworks.
 *
 *  Instant Gameworks is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Instant Gameworks is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Instant Gameworks.  If not, see <http://www.gnu.org/licenses/>.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace InstantGameworksObject
{
    public class Convert
    {
        public static InstantGameworksObject ConvertOBJToIGWO(string[] OBJContent)
        {
            InstantGameworksObject _newObject = new InstantGameworksObject();

            List<Position> _vertexPositions = new List<Position>();
            List<TextureCoordinates> _vertexTextureCoordinates = new List<TextureCoordinates>();
            List<Position> _vertexNormals = new List<Position>();
            List<Face> _faces = new List<Face>();


            // Parse data
            foreach (string line in OBJContent)
            {
                string[] splitLine = line.Split(' ');
                if (line.StartsWith("v "))
                {
                    float x = float.Parse(splitLine[1]);
                    float y = float.Parse(splitLine[2]);
                    float z = float.Parse(splitLine[3]);
                    _vertexPositions.Add(new Position(x, y, z));
                }
                if (line.StartsWith("vn "))
                {
                    float x = float.Parse(splitLine[1]);
                    float y = float.Parse(splitLine[2]);
                    float z = float.Parse(splitLine[3]);
                    _vertexNormals.Add(new Position(x, y, z));
                }
                if (line.StartsWith("vt "))
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
                if (line.StartsWith("f "))
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
