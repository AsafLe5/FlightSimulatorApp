using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.AnomalyDetector
{
    // Checked
    public class Line
    {
        public float a, b;
        public Line()
        {
            a = 0;
            b = 0;
        }
        public Line(float aa, float bb)
        {
            this.a = aa;
            this.b = bb;
        }
        public float f(float x)
        {
            return a * x + b;
        }
    }

    // Checked
    public class Point
    {
        public float x, y;
        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Checked
    public class anomaly_detection_util
    {

        public float avg(float[] x, int size)
        {
            if (size == 0)
            {
                return 0;
            }
            float sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

        public float var(float[] x, int size)
        {
            if (size == 0)
            {
                return 0;
            }
            float av = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }

        public float cov(float[] x, float[] y, int size)
        {
            if (size == 0)
            {
                return 0;
            }
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x, size) * avg(y, size);
        }

        public float pearson(float[] x, float[] y, int size)
        {
            float a = (float)(cov(x, y, size) / (Math.Sqrt(var(x, size)) * Math.Sqrt(var(y, size))));
            return a;
        }

        public Line linear_reg(Point[] points, int size)
        {
            float[] x = new float[size];
            float[] y = new float[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = points[i].x;
                y[i] = points[i].y;
            }
            float a = cov(x, y, size) / var(x, size);
            float b = avg(y, size) - a * avg(x, size);
            Line line = new Line(a, b);
            return line;
        }

        public float dev(Point p, Point[] points, int size)
        {
            Line l = linear_reg(points, size);
            return dev(p, l);
        }

        public float dev(Point p, Line l)
        {
            float x = p.y - l.f(p.x);
            if (x < 0)
                x *= -1;
            return x;
        }
    }
}
