using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace bowyer
{
    public class Edge : IEquatable<Edge>
    {
        public Vertex v1;
        public Vertex v2;

        public bool Equals(Edge other)
        {
            /*
            //show detail
            Debug.Log($"{this} {other}");
            Debug.Log($"{v1.Equals(other.v1)} and {v2.Equals(other.v2)}");
            Debug.Log($"reverse {v1.Equals(other.v2)} and {v2.Equals(other.v1)}");
            */
            return (v1.Equals(other.v1) && v2.Equals(other.v2)) || (v1.Equals(other.v2) && v2.Equals(other.v1));
        }
        public override int GetHashCode()
        {
            return v1.GetHashCode() ^ v2.GetHashCode();
        }
        public Edge()
        {

        }
        public Edge(Vertex v1, Vertex v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
        public override string ToString()
        {
            return $"\n v1 : {v1} \n v2 : {v2} \n";
        }
        
        private void CompareValue(double value1,double value2,out double min, out double max)
        {
            if (value1 < value2)
            {
                min = value1;
                max = value2;
                return;
            }
            min = value2;
            max = value1;
        }
        public double getDistance2()
        {
            CompareValue(v1.x,v2.x,out double minx,out double maxx);
            CompareValue(v1.y, v2.y, out double miny, out double maxy);
            double x = maxx - minx;
            double y = maxy - miny;
            return Math.Sqrt(x * x + y * y);
        }
        public double getDistance()
        {
            double x = Math.Max(v1.x, v2.x) == v1.x ? v1.x - v2.x : v2.x - v1.x;
            double y = Math.Max(v1.y, v2.y) == v1.y ? v1.y - v2.y : v2.y - v1.y;
            return Math.Sqrt(x * x + y * y);
        }

        public static int CompareDistanceMax(Edge Standard, Edge target)
        {
            double xdistance=Standard.getDistance(), ydistance=target.getDistance();
            if (xdistance > ydistance)
            {
                return -1;
            }
            else if (xdistance < ydistance)
            {
                return 1;
            }
            return 0;
        }
        public static int CompareDistanceMin(Edge Standard, Edge target)
        {
            double xdistance = Standard.getDistance(), ydistance = target.getDistance();
            if (xdistance > ydistance)
            {
                return 1;
            }
            else if (xdistance < ydistance)
            {
                return -1;
            }
            return 0;
        }
    }
}