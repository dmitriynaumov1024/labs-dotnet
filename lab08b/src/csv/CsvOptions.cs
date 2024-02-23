namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;

public class CsvOptions
{
    public bool IgnoreErrors { get; set; } = false;
    public bool IgnoreNulls { get; set; } = false;
    public bool SkipFirstRow { get; set; } = false;
}
