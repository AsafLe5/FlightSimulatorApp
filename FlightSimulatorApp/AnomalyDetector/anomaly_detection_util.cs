using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.AnomalyDetector
{
    public class Line
    {

        public float a, b;
        public Line()  {
            a = 0;
            b = 0;
        }
        public Line(float aa, float bb) {
            this.a = aa;
            this.b = bb;
        }
        public float f(float x)
        {
            return a * x + b;
        }
    }

    public class Point
    {

        public float x, y;
        public Point(float x, float y) {
            this.x = x;
            this.y = y;
         }
    }

    public class anomaly_detection_util
    {

        public float avg(float[] x, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

        // returns the variance of X and Y
        public float var(float[] x, int size)
        {
            float av = avg(x, size);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }

        // returns the covariance of X and Y
        public float cov(float[] x, float[] y, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x, size) * avg(y, size);
        }


        // returns the Pearson correlation coefficient of X and Y
        public float pearson(float[] x, float[] y, int size)
        {
            float a = (float)(cov(x, y, size) / (Math.Sqrt(var(x, size)) * Math.Sqrt(var(y, size))));
            return a;
        }

        // performs a linear regression and returns the line equation
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

        // returns the deviation between point p and the line equation of the points
        public float dev(Point p, Point[] points, int size)
        {
            Line l = linear_reg(points, size);
            return dev(p, l);
        }

        // returns the deviation between point p and the line
        public float dev(Point p, Line l)
        {
            float x = p.y - l.f(p.x);
            if (x < 0)
                x *= -1;
            return x;
        }



    }
}
