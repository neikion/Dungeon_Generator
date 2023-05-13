using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace bowyer{
    public class Vertex : IEquatable<Vertex>
    {
        public double x;
        public double y;

        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public bool Equals(Vertex other)
        {
            return x == other.x && y == other.y;
        }
        public static Vertex operator +(Vertex left,Vertex right)
        {
            return new Vertex(left.x + right.x, left.y + right.y);
        }
        public static Vertex operator -(Vertex left, Vertex right)
        {
            return new Vertex(left.x - right.x, left.y - right.y);
        }
        public static Vertex operator *(Vertex left, Vertex right)
        {
            return new Vertex(left.x * right.x, left.y * right.y);
        }
        public static Vertex operator /(Vertex left, Vertex right)
        {
            return new Vertex(left.x / right.x, left.y / right.y);
        }
    }
}

