using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace bowyer
{

    public class bowyer_watson
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Triangle> triangles = new List<Triangle>();
        public Triangle super;
        bool debug_log = true;
        public void main(Action<List<Triangle>> action)
        {
            
            
            Triangle supertriangle = getSuperTriangle(vertices);
            super = supertriangle;
            triangles.Add(supertriangle);

            for (int i = 0, j = 0; i < vertices.Count&&j<3; i++, j++)
            {
                if (j > 98)
                {
                    Debug.LogError("while error");
                }
                AddVertex(ref triangles, vertices[i],action);
                if (debug_log)
                {
                    for (int i2 = 0; i2 < triangles.Count; i2++)
                    {
                        Debug.Log($"check triangle {triangles[i2]}");
                    }
                }
            }
            /*
            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                Triangle triangle = triangles[i];
                if (checkTouchSuperObject(supertriangle, triangle))
                {
                    triangles.RemoveAt(i);
                }
            }
            if (debug_log)
            {
                for (int i2 = 0; i2 < triangles.Count; i2++)
                {
                    Debug.Log($"result {triangles[i2]}");
                }
            }*/
        }

        void testEqualEdge()
        {
            Edge t1, t2;
            t1 = new Edge(new Vertex(54, -2), new Vertex(-2, -2));
            t2 = new Edge(new Vertex(-2, 54), new Vertex(54, -2));
            Debug.Log(t1.Equals(t2));
            return;
        }
        public bool checkTouchSuperObject(Triangle super, Triangle child)
        {
            if(super.v1==child.v1 || super.v1 == child.v2 || super.v1 == child.v3)
            {
                return true;
            }
            if (super.v2 == child.v1 || super.v2 == child.v2 || super.v2 == child.v3)
            {
                return true;
            }
            if (super.v3 == child.v1 || super.v3 == child.v2 || super.v3 == child.v3)
            {
                return true;
            }

            return false;
        }
        public Triangle getSuperTriangle(List<Vertex> mypos)
        {
            double minx = double.MaxValue, miny = double.MaxValue, maxx = double.MinValue, maxy = double.MinValue;
            for (int i = 0; i < mypos.Count; i++)
            {
                if (minx > mypos[i].x)
                {
                    minx = mypos[i].x;
                }
                if (miny > mypos[i].y)
                {
                    miny = mypos[i].y;
                }
                if (maxx < mypos[i].x)
                {
                    maxx = mypos[i].x;
                }
                if (maxy < mypos[i].y)
                {
                    maxy = mypos[i].y;
                }
            }
            minx += -10;
            maxx += 10;
            miny += -10;
            maxy += 10;
            double width = (maxx - minx) * 2;
            double height = (maxy - miny) * 2;
            Triangle result = new Triangle();
            result.v1 = new Vertex(minx, miny);
            result.v2 = new Vertex(minx, miny + height);
            result.v3 = new Vertex(width + minx, miny);
            return result;
        }
        void AddVertex(ref List<Triangle> triangle, Vertex vertex,Action<List<Triangle>> action)
        {
            List<Triangle> tmp = new List<Triangle>();
            if (debug_log)
            {
                Debug.Log($"triangle count : {triangle.Count}");
            }


            List<Edge> edges = new List<Edge>();
            foreach (Triangle triangle1 in triangle)
            {
                if (debug_log)
                {
                    Debug.Log($"{triangle1} \n select {vertex} \n incircle? {triangle1.inCircle2(vertex)}");
                }
                if (triangle1.inCircle2(vertex))
                {
                    edges.Add(new Edge(triangle1.v1, triangle1.v2));
                    edges.Add(new Edge(triangle1.v2, triangle1.v3));
                    edges.Add(new Edge(triangle1.v3, triangle1.v1));
                }
                else
                {
                    tmp.Add(triangle1);
                }
            }
            
            edges = new List<Edge>(uniqueEdges(edges));
            if (debug_log)
            {
                for (int i = 0; i < edges.Count; i++)
                {
                    Debug.Log("edges " + edges[i]);
                }
            }
            triangle.Clear();
            triangle = new List<Triangle>(tmp);
            action(triangle);
            foreach (Edge edge in edges)
            {
                triangle.Add(new Triangle(edge.v1, edge.v2, vertex));
                action(triangle);
                if (debug_log)
                {
                    Debug.Log($"add triangle \n now edge {edge} \n setTriangle {new Triangle(edge.v1, edge.v2, vertex)}");
                }
                

            }
        }
        List<Edge> uniqueEdges(List<Edge> edges)
        {
            List<Edge> results=new List<Edge>();
            
            int index=edges.Count-1;
            bool overlap = false;
            while (index>-1)
            {
                results.Add(edges[index]);
                edges.RemoveAt(index);
                index = edges.Count - 1;
                for (int i=index; i>-1; i--)
                {
                    if (results[results.Count - 1].Equals(edges[i]))
                    {
                        edges.RemoveAt(i);
                        overlap = true;
                    }
                }
                if (overlap)
                {
                    results.RemoveAt(results.Count - 1);
                    overlap = false;
                }
                index = edges.Count - 1;
            }
            /*
            bool unique=true;
            for(int i=0;i<edges.Count;i++)
            {
                unique = true;

                for(int j = 0; j < edges.Count; j++)
                {
                    if (i != j && edges[i].Equals(edges[j]))
                    {
                        if (debug_log)
                        {
                            Debug.Log($"overlap {edges[i]} {edges[j]}");
                        }
                        unique = false;
                        break;
                    }
                }
                if (unique)
                {
                    results.Add(edges[i]);
                }

            }*/
            return results;
        }
    }
    

}
