using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bowyer
{
    public class Edge : IEquatable<Edge>
    {
        public Vertex v1;
        public Vertex v2;
        public bool Equals(Edge other)
        {
            return v1.Equals(v2) || v2.Equals(v1);
        }
    }
}