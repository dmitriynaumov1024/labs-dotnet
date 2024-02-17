namespace Lab10;
using System;

public interface PbmParser<TOutput>
{
    TOutput[][] Parse();
}
