using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.AnomalyDetector;

public class correlatedFeatures
{
    public bool isCircle = false;
    public string feature1, feature2;  // names of the correlated features
    public float corrlation;
    public Line lin_reg;
    public float threshold = 0.9F;
    public float centerX;
    public float centerY;
    public float rad;
};
public class SimpleAnomalyDetector : TimeSeriesAnomalyDetector
{
    anomaly_detection_util adu = new anomaly_detection_util();
    public List<correlatedFeatures> cf;
    protected float threshold;
    public SimpleAnomalyDetector(float threshold)
    {
        this.threshold = threshold;
    }

    public void learnNormal(Timeseries ts)
    {
        correlatedFeatures temp = new correlatedFeatures();
        float maxCorr = 0;
        float[] sFlotIn;
        int size = 0;
        bool isCorrelation = false;
        Point[] ps = new Point[200];//should be 200
        for (dynamic outItr = ts.table.First(); outItr != ts.table.Last(); ++outItr)
        { // Loop through the map.
            float[] floOut = new float[outItr.second.Count()];
            for (int i = 0; i < outItr.second.Count(); floOut[i] = outItr.second[i], i++) ; // Turns the strings to float.
            for (dynamic inItr = ts.table.First(); inItr != ts.table.Last(); ++inItr)
            { // Loop through the map.
                size = outItr.second.Count();
                float[] floIn = new float[outItr.second.Count()];
                for (int i = 0; i < outItr.second.Count(); floIn[i] = inItr.second[i], i++) ; // Turns strings to float.
                if (outItr == inItr)
                    continue;
                int flagInCf = 0; // Flag that carry the fact if the current correlation is already in cf vector.
                for (int cfI = 0; cfI < this.cf.Count(); cfI++)
                {
                    if (outItr.first == this.cf[cfI].feature2)
                        flagInCf = 1;
                }
                if (flagInCf == 1)
                    continue;
                // Case the current correlation is the maximum so far.
                if (adu.pearson(floOut, floIn, inItr.second.Count()) > maxCorr)
                {
                    maxCorr = adu.pearson(floOut, floIn, inItr.second.Count());
                    temp.feature1 = outItr.first;
                    temp.feature2 = inItr.first;
                    temp.corrlation = maxCorr;
                    //Point *ps[outItr->second.size()];
                    float maxDev = 0;
                    for (int k = 0; k < outItr.second.Count(); k++)
                    {
                        ps[k] = new Point(outItr.second[k], inItr.second[k]); // Turns each local correlation to point.
                    }
                    temp.lin_reg = adu.linear_reg(ps, outItr.second.size());
                    for (int k = 0; k < outItr.second.Count(); k++)
                    {
                        if (maxDev < Math.Abs(adu.dev(ps[k], temp.lin_reg))) // case the current deviation is max deviation.
                            maxDev = Math.Abs(adu.dev(ps[k], temp.lin_reg));
                    }
                    temp.threshold = (float)(maxDev * 1.1); // ten present higher for little deviation.
                                                            //isCorrelation = true;
                    sFlotIn = floIn;
                }
            }
            if (maxCorr > threshold)
                this.cf.Add(temp);
            else if (maxCorr > 0.5 && maxCorr < threshold)
                cirCorr(temp, ps, size);
            //isCorrelation = false;
            maxCorr = 0;
        }
    }

    public List<AnomalyReport> detect(Timeseries ts)
    {
        List<AnomalyReport> vector = new List<AnomalyReport>();
        int i = 0;
        foreach (correlatedFeatures var in cf)
        {
            for (int j = 0; j < ts.table.First().Value.Count(); ++j)
            { // Loop through the map.
                int iter = -1;
                // Loop through the cf vector.
                iter++;
                float floatA = ts.table[var.feature1][j];
                float floatB = ts.table[var.feature2][j];
                dynamic p = new Point(floatA, floatB); // A point represent the two current features.

                //float correlation = dev(*p, i->lin_reg);
                // Case the correlation is bigger then the threshold determined by the current correlation deviation.
                if (isAnomal(var, p))
                {
                    string description = var.feature1 + "-" + var.feature2;
                    long timeStep = j + 1; // For time step to start with 1 instead of 0.
                    AnomalyReport ar = new AnomalyReport(description, timeStep); // Build a anomaly report with those features.
                    vector.Add(ar);
                }
            }
            i++;
        }
        return (vector);
    }

    public float cirCorr(correlatedFeatures cf, Point[] points, int size)
    {
        return 0;
    }



    public bool isAnomal(correlatedFeatures cf, Point point)
    {
        float correlation = adu.dev(point, cf.lin_reg);
        if (correlation > cf.threshold)
            return true;
        return false;
    }

    public List<correlatedFeatures> getNormalModel()
    {
        return cf;
    }

    public string findMostCorrelated(string original, string csvPath)
    {
        SimpleAnomalyDetector ad = new SimpleAnomalyDetector((float)0.5);
        Timeseries ts = new Timeseries(csvPath);
        ad.learnNormal(ts);
        List<correlatedFeatures> cf = ad.getNormalModel();
        foreach (correlatedFeatures cor in cf)
        {
            if (cor.feature1.Equals(original) == true)
            {
                return cor.feature2;
            }
            if (cor.feature2.Equals(original) == true)
            {
                return cor.feature1;
            }
        }
        return "";
    }
}

