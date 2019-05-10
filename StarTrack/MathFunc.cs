using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace StarTrack
{
    class MathFunc
    {
        public static List<int> List_radius = new List<int>();

        public static void InitAll()
        {
            int temp = 0;
            for (int i = 0; i < GV.NUM_TRACKS; i++)
            {
                temp += GV.CANVAS_WIDTH / (2 * GV.NUM_TRACKS);
                List_radius.Add(temp);
            }
        }
        /// <summary>
        /// Transform coordinates, from math to canvas 
        /// from (0,0) to (250, -250)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point TCor(Point p)
        {
            return new Point(GV.CANVAS_WIDTH / 2 + p.X, GV.CANVAS_HEIGHT / 2 - p.Y);
        }

        public static Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            return ellipse;
        }

        public static Point GetStarLocation(int vel_ang, double time, int R)
        {
            Point p = new Point();
            p.X = Math.Cos(vel_ang * time) * R;
            p.Y = Math.Sin(vel_ang * time) * R;
            return p;
        }
    }

}
