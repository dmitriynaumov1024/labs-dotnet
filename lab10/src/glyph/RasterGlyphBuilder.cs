namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;

public class RasterGlyphBuilder
{
    public IList<bool[]> Rasters { get; set; } = null;
    public IList<char> Chars { get; set; } = null;
    public int GlyphHeight { get; set; } = 0;

    public RasterGlyphMap Build() 
    {
        if (this.GlyphHeight == 0) {
            this.GlyphHeight = this.Rasters.Count / this.Chars.Count;
        }
        if (this.Rasters.Count % this.GlyphHeight != 0) {
            throw new Exception("Raster size not divisible by glyph height");
        }
        var result = new RasterGlyphMap();
        result['\0'] = new RasterGlyph() { Raster = new bool[0][] };
        var RasterChunks = this.Rasters.Chunk(this.GlyphHeight).ToList();
        for (int i=0; i<this.Chars.Count && i<RasterChunks.Count; i++) {
            result[this.Chars[i]] = new RasterGlyph() { Raster = RasterChunks[i] };
        }
        return result;
    }
}
