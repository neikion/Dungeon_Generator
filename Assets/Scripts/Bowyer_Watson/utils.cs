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
    }
}
