using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomScatter
{
    public static class Extensions
    {
        public static PointF GetCenter(this RectangleF rect)
        {
            return new PointF((rect.Right + rect.Left) / 2f, (rect.Bottom + rect.Top) / 2f);
        }

        public static RectangleF Union(this RectangleF rect1, RectangleF rect2)
        {
            float minX = Math.Min(rect1.Left, rect2.Left);
            float minY = Math.Min(rect1.Top, rect2.Top);
            float maxX = Math.Max(rect1.Right, rect2.Right);
            float maxY = Math.Max(rect1.Bottom, rect2.Bottom);
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
