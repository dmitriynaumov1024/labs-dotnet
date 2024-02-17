namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RasterGlyph
{
    public bool[][] Raster { get; set; }

    public float[] ColorRGB { get; set; } = Voxel.White;

    public RasterGlyph Color (float r, float g, float b) 
    {
        return new RasterGlyph () { 
            Raster = this.Raster,
            ColorRGB = new float[] { r, g, b }
        };
    }

    public Voxel[] ToVoxels (int leftX, int topY, int Z) 
    {
        var count = this.Raster.Sum (
            row => row.Count(item => item)
        );

        var result = new Voxel[count];
        int resultIdx = 0;
        for (int rowY=0; rowY<this.Raster.Length; rowY++) {
            var row = this.Raster[rowY];
            for (int colX=0; colX<row.Length; colX++) {
                if (row[colX]) {
                    result[resultIdx++] = new Voxel(leftX + colX, topY - rowY, Z, this.ColorRGB);
                }
            }
        }
        return result;
    }
}
