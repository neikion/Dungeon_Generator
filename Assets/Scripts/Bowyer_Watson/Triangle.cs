using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace bowyer
{
    public class Triangle
    {
        public Vertex v1;
        public Vertex v2;
        public Vertex v3;
        public Triangle() { }
        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
        
        public bool inCircle2(Vertex d)
        {
            Vertex ad = v1 - d;
            Vertex bd = v2 - d;
            Vertex cd = v3 - d;
            // result > 0 in circle
            // result = 0 on circle
            // result < 0 out circle
            double result = (ad.x * ad.x + ad.y * ad.y) * (bd.x * cd.y - cd.x * bd.y) -
                (bd.x * bd.x + bd.y * bd.y) * (ad.x * cd.y - cd.x * ad.y) +
                (cd.x * cd.x + cd.y * cd.y) * (ad.x * bd.y - bd.x * ad.y);
            if (ccw2() > 0)
                return result > 0;
            else return result < 0;
        }
        public double ccw2()
        {
            Vertex ca = v3 - v1;
            Vertex ba = v2 - v1;
            // result > 0 반시계방향
            // result < 0 시계방향
            // result = 0 평행
            return (ba.x * ca.y) - (ca.x * ba.y);
        }
        public override string ToString()
        {

            return $"\n v1 : {v1}\n v2: {v2}\n v3: {v3}";
        }
    }
    
}
