using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace testWorkForSirius.Methods
{
    class Methods
    {
        public static Shape DrawLinkArrow(Point p1, Point p2)
        {
            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = p2;

            Point lpoint = new Point(p2.X + 6, p2.Y + 15);
            Point rpoint = new Point(p2.X - 6, p2.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p2;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);
            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p2.X;
            transform.CenterY = p2.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);
            Path path = new Path();
            path.Data = lineGroup;
            path.StrokeThickness = 2;
            path.Stroke = path.Fill = Brushes.Green;

            return path;
        }
    }
}
