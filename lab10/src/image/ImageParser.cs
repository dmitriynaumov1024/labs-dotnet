namespace Lab10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public interface IImageParser<TOutput>
{
    TOutput[][] Parse();
}

// image parser factory
// currently only one format: PBM(P1)
// im starting to hate this approach
public static class ImageParser
{
    public static Func<string, IImageParser<bool>> BooleanFormatParser (string format)
    {
        // do something
        if (format == "P1") {
            return PbmOneParser.File;
        }

        // default
        return null;
    }
}
