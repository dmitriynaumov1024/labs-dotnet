namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenTK.Mathematics;

// represents a cube at location (X, Y, Z) with size (1, 1, 1) 
public class Voxel
{
    public static readonly float[] White = new float[] { 0.99f, 0.99f, 0.99f };
    public static readonly int VertSize = 9;

    public int X;
    public int Y;
    public int Z;

    public float[] RGB;

    public Voxel () 
    {
        // do nothing
        this.RGB = White;
    }

    public Voxel (int x, int y, int z) 
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.RGB = White;
    }

    public Voxel (int x, int y, int z, float[] rgb) 
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.RGB = rgb;
    }

    public Voxel (Vector3 pos, float[] rgb) 
    {
        this.X = (int)pos.X;
        this.Y = (int)pos.Y;
        this.Z = (int)pos.Z;
        this.RGB = rgb;
    }

    public float[] ToVerts() {
        var source = Cube.VertsNormals;
        var result = new float[324]; // 2 3-angles for each of 6 faces of cube
        for (int i=0; i<36; i++) {
            result[i*9+0] = source[i*6+0] + this.X;
            result[i*9+1] = source[i*6+1] + this.Y;
            result[i*9+2] = source[i*6+2] + this.Z;
            result[i*9+3] = source[i*6+3];
            result[i*9+4] = source[i*6+4];
            result[i*9+5] = source[i*6+5];
            result[i*9+6] = this.RGB[0];
            result[i*9+7] = this.RGB[1];
            result[i*9+8] = this.RGB[2];
        }
        return result;
    } 
}
