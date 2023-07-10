using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
            /*
            //show detail
            Debug.Log($"{this} {other}");
            Debug.Log(((Math.Abs(x) - Math.Abs(other.x)) < double.Epsilon));
            Debug.Log(((Math.Abs(y) - Math.Abs(other.y)) < double.Epsilon));
            */
            return x == other.x && y == other.y; 
            //return (Math.Abs(Math.Abs(x) - Math.Abs(other.x)) < double.Epsilon) && (Math.Abs(Math.Abs(y) - Math.Abs(other.y)) < double.Epsilon);
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
        public override string ToString()
        {
            return $"vertext x : {x}   y: {y} ";
        }
        public override bool Equals(object obj)
        {
            if(!(obj is Vertex))
            {
                return false;
            }
            return Equals((Vertex)obj);
        }
        public override int GetHashCode()
        {
            // x == other.x && y == other.y
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
    }
}

