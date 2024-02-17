namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;

public class RasterGlyphBuilder
{
    public bool[][] Rasters { get; set; } = null;
    public char[] Chars { get; set; } = null;
    public int GlyphHeight { get; set; } = 0;

    public RasterGlyphMap Build() 
    {
        if (this.GlyphHeight == 0) {
            this.GlyphHeight = this.Rasters.Length / this.Chars.Length;
        }
        if (this.Rasters.Length % this.GlyphHeight != 0) {
            throw new Exception("Raster size not divisible by glyph height");
        }
        var result = new RasterGlyphMap();
        result['\0'] = new RasterGlyph() { Raster = new bool[0][] };
        var RasterChunks = this.Rasters.Chunk(this.GlyphHeight).ToArray();
        for (int i=0; i<this.Chars.Length && i<RasterChunks.Length; i++) {
            result[this.Chars[i]] = new RasterGlyph() { Raster = RasterChunks[i] };
        }
        return result;
    }
}
