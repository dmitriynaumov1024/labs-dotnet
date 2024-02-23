namespace Lab08;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class MovieRepo : IDisposable
{
    public abstract void Add (Movie item);

    public abstract void Update (Movie item);

    public abstract void Remove (int id);

    public abstract Movie Get (int id);

    public abstract IEnumerable<Movie> Filter (string propName, string value, int page);

    public abstract void Dispose();
}
