using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace bowyer
{
    //만약 삼각분할이 제대로 되지 않을 경우 슈퍼 트라이앵글을 생성하는 점의 위치를 더 크게 조정해 줄 것.
    public class bowyer_watson
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Triangle> triangles = new List<Triangle>();
        public Triangle super;
        bool debug_log = false;
        public void main(Action<List<Triangle>> action)
        {
            
            
            Triangle supertriangle = getSuperTriangle(vertices);
            super = supertriangle;
            triangles.Add(supertriangle);
            action(triangles);
            for (int i = 0; i < vertices.Count; i++)
            {
                AddVertex(ref triangles, vertices[i],action);
                if (debug_log)
                {
                    for (int i2 = 0; i2 < triangles.Count; i2++)
                    {
                        Debug.Log($"check triangle {triangles[i2]}");
                    }
                }
            }
            
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
            }
        }
        public List<Edge> getEdge()
        {
            Dictionary<Edge, Edge> checkdic = new Dictionary<Edge, Edge>();
            for (int i = 0; i < triangles.Count; i++)
            {
                Edge checkedge = new Edge(triangles[i].v1, triangles[i].v2);
                if (!checkdic.ContainsKey(checkedge))
                {
                    checkdic.Add(checkedge, checkedge);
                }
                checkedge = new Edge(triangles[i].v1, triangles[i].v3);
                if (!checkdic.ContainsKey(checkedge))
                {
                    checkdic.Add(checkedge, checkedge);
                }
                checkedge = new Edge(triangles[i].v2, triangles[i].v3);
                if (!checkdic.ContainsKey(checkedge))
                {
                    checkdic.Add(checkedge, checkedge);
                }
            }
            return checkdic.Values.ToList();
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

        //https://math.stackexchange.com/questions/4001660/bowyer-watson-algorithm-for-delaunay-triangulation-fails-when-three-vertices-ap
        //super triangle은 모든 점의 외접원보다 커야함.
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
            
            minx += -10000;
            maxx += 10000;
            miny += -10000;
            maxy += 10000;
            
            double width = (maxx - minx)*2;
            double height = (maxy - miny)*2;
            
            
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
        /// <summary>
        /// 한번도 겹치지 않는 선들만 가져온다. 겹쳤던 선들은 결과에서 제외한다.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
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
            return results;
        }
    }
    

}
