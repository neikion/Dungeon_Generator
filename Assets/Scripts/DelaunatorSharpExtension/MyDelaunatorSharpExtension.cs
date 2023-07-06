using DelaunatorSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyDelaunatorSharpExtension
{
    public static ref List<Edge> getEdge(Delaunator delaunator, out List<Edge> result)
    {
        result = new List<Edge>();
        foreach(Edge edge in delaunator.GetEdges())
        {
            result.Add(edge);
        }
        return ref result;
    }
    static void showlibedge(Delaunator delaunator)
    {
        for (int e = 0; e < delaunator.Triangles.Length; e++)
        {
            if (e > delaunator.Halfedges[e])
            {
                IPoint p = delaunator.Points[delaunator.Triangles[e]];
                IPoint q = delaunator.Points[delaunator.Triangles[Delaunator.NextHalfedge(e)]];
                Debug.Log($"edge {p} and {q}");
            }
        }
    }

}
