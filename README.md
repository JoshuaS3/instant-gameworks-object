# Instant Gameworks Object
[![Github All Releases](https://img.shields.io/github/downloads/joshuas3/instant-gameworks-object/total.svg?style=flat-square)](https://github.com/JoshuaS3/instant-gameworks/archive/master.zip)
[![GitHub last commit](https://img.shields.io/github/last-commit/joshuas3/instant-gameworks-object.svg?style=flat-square)]()

The binary 3D model format for Instant Gameworks (stemmed from Wavefront's OBJ).


## File format:
Indicate new data sets:

1.  v — vertices
2.  c — vertex color, order corresponds to vertex set
3.  t — vertex texture coordinates, order corresponds to vertex set
4.  n — vertex normals, order corresponds to vertex set
5.  f — faces

Final format:

```
~Instant Gameworks

v
1.250000 0.000000 0.000000;1.216506 0.125000 0.000000;1.125000 0.216506 0.000000;
n
0.9640 0.2583 -0.0632;0.7063 0.7063 -0.0463;0.2588 0.9658 -0.0170;
f
0/0 1/1 2/2;

```