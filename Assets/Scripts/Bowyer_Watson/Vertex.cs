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
        //���� �ʵ��� �ؽ� �ڵ带 �� ��Ʈ �̻� �������� �̵��Ͽ� ���� �ؽ� �ڵ��� ����ġ�� ����
        public override int GetHashCode()
        {
            // x == other.x && y == other.y
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }
        /* https://learn.microsoft.com/ko-kr/dotnet/api/system.object.gethashcode?view=net-7.0
        �ڷ����� �Ѱ踦 �ѰԵǸ� ��Ʈ�� �����Ǵ� �Ʒ� �Լ�ó�� �Ѱ踦 �Ѵ� ��Ʈ�� �ڿ��� ���� �ٽ� ä�������.
        public int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
        */
    }
}

