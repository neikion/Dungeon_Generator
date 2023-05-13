using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowyer
{
    public class utils
    {
        public static bool inCircle(Vertex a, Vertex b, Vertex c, Vertex d)
        {
            double ax = a.x - d.x; //ad.x
            double ay = a.y - d.y; //ad.y
            double bx = b.x - d.x; //bd.x
            double by = b.y - d.y; //bd.y
            double cx= c.x - d.x; //cd.x
            double cy= c.y - d.y; //cd.y
            double result = ((ax * ax + ay * ay) * (bx * cy - cx * by) -
                (bx * bx + by * by) * (ax * cy - cx * ay) +
                (cx * cx + cy * cy) * (ax * by - bx * ay)
            );
            if (ccw(a, b, c) > 0)
                return result > 0;
            else return result < 0;
        }
        public static double ccw(Vertex a,Vertex b,Vertex c)
        {
            double ax=a.x;
            double ay=a.y;
            double bx=b.x;
            double by=b.y;
            double cx=c.x;
            double cy=c.y;

            return (bx - ax) * (cy - ay) - (cx - ax) * (by - ay);
        }
        public static bool inCircle2(Vertex a, Vertex b, Vertex c, Vertex d)
        {
            Vertex ad = a - d;
            Vertex bd = b - d;
            Vertex cd = c - d;
            // result > 0 in circle
            // result = 0 on circle
            // result < 0 out circle
            double result = (ad.x * ad.x + ad.y * ad.y) * (bd.x * cd.y - cd.x * bd.y) -
                (bd.x * bd.x + bd.y * bd.y) * (ad.x * cd.y - cd.x * ad.y) +
                (cd.x * cd.x + cd.y * cd.y) * (ad.x * bd.y - bd.x * ad.y);
            if(ccw2(a,b,c)>0)
                return result > 0;
            else return result < 0;
        }
        public static double ccw2(Vertex a, Vertex b, Vertex c)
        {
            Vertex ca = c - a;
            Vertex ba = b - a;
            // result > 0 반시계방향
            // result < 0 시계방향
            // result = 0 평행
            return (ba.x * ca.y) - (ca.x * ba.y);
        }
    }
}
