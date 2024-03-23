namespace Lab03;
using System;
using System.Collections.Generic;
using System.Linq;

public class Shape<TPoint> 
where TPoint : Point
{
    public List<TPoint> Verts { get; set; }
    public int VertCount => Verts.Count;

    public Shape ()
    {
        this.Verts = new List<TPoint>();
    }

    public Shape (IEnumerable<TPoint> verts)
    {
        this.Verts = verts.ToList();
    }

    public TPoint GetNearest (Point p) 
    {
        return this.Verts.MinBy(v => v.GetSquaredDistance(p));
    }
}
