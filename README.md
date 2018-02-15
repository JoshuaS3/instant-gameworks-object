# Instant Gameworks Object
[![Github All Releases](https://img.shields.io/github/downloads/joshuas3/instant-gameworks-object/total.svg?style=flat-square)](https://github.com/JoshuaS3/instant-gameworks/archive/master.zip)
[![GitHub last commit](https://img.shields.io/github/last-commit/joshuas3/instant-gameworks-object.svg?style=flat-square)]()

The binary 3D model format for Instant Gameworks.
The converter indicates the ability to move the object data from an OBJ file into a new IGWO file extremely quickly and efficiently. Most post-conversion objects are about 65% the original file size, yet still consist of the same information and precision. The only information cut out (as it is unneeded) is MTL/material data.

<img src=https://i.imgur.com/T9RkxH2.png width=500px />

## File format:
The format takes on an extremely simplistic form of indicating the beginning of an array, the allocated size of it, and its respective data.

<img src=https://i.imgur.com/ROtCupN.png width=400px />

## TODO:
 * Build the compiler and decompiler as a library, separate from the converter application itself