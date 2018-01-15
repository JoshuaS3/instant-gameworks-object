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
~scale factor: 1000

v
125 0 0;121 125 0;1125 216 0;
n
0.96 0.26 -0.06;0.7 0.7 -0.04;0.25 0.96 -0.02;
f
0/0 1/1 2/2;

```