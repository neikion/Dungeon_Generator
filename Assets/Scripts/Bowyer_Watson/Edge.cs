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
    }
}