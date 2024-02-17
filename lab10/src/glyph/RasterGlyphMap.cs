namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;

public class RasterGlyphMap 
{
    private Dictionary<char, RasterGlyph> data = new Dictionary<char, RasterGlyph>();

    public RasterGlyph this[char c] 
    {
        get {
            return data.ContainsKey(c) ? data[c] : data['\0'];
        }
        set {
            data[c] = value;
        }
    }

    public static RasterGlyphMap operator + (RasterGlyphMap map1, RasterGlyphMap map2) 
    {
        var result = new RasterGlyphMap();
        foreach (var kvpair in map1.data) {
            result.data[kvpair.Key] = kvpair.Value;
        }
        foreach (var kvpair in map2.data) {
            result.data[kvpair.Key] = kvpair.Value;
        }
        return result;
    }
}
