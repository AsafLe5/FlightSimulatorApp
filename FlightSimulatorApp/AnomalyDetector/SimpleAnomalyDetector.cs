using System;
using System.Collections.Generic;
using System.IO;
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
    public List<correlatedFeatures> cf = new List<correlatedFeatures>();
    protected float threshold;
    public SimpleAnomalyDetector(float threshold)
    {
        this.threshold = threshold;
    }

    public void learnNormal(Timeseries ts)
    {
        anomaly_detection_util adu = new anomaly_detection_util();
        Dictionary<string, List<float>> tsMap = ts.table;
        List<string> tsFeaturesVector = ts.features;
        int featuresVectorSize = tsFeaturesVector.Count();
        int valueVectorSize = tsMap[tsFeaturesVector[0]].Count();
        for (int i = 0; i < featuresVectorSize; ++i)
        {
            string fiName = tsFeaturesVector[i];
            float[] fiData = tsMap[fiName].ToArray();
            float maxPearson = 0;
            correlatedFeatures currentCF = new correlatedFeatures();
            currentCF.feature1 = fiName;
            for (int j = i + 1; j < featuresVectorSize; ++j)
            {
                string fjName = tsFeaturesVector[j];
                float[] fjData = tsMap[fjName].ToArray();
                float currentPearson = Math.Abs(adu.pearson(fiData, fjData, valueVectorSize));
                if (currentPearson > maxPearson)
                {
                    maxPearson = currentPearson;
                    currentCF.feature2 = fjName;
                }
            }
            if (maxPearson > 0.9)
            {
                currentCF.corrlation = maxPearson;
                Point[] points = new Point[valueVectorSize];
                for (int j = 0; j < valueVectorSize; ++j)
                {
                    points[j] = new Point(tsMap[currentCF.feature1][j], tsMap[currentCF.feature2][j]);
                }
                currentCF.lin_reg = adu.linear_reg(points, valueVectorSize);
                currentCF.threshold = 0;
                for (int j = 0; j < valueVectorSize; ++j)
                {
                    currentCF.threshold = Math.Max(currentCF.threshold, Math.Abs(adu.dev(points[j], currentCF.lin_reg)));
                }
                currentCF.threshold *= (float)1.1;
                cf.Add(currentCF);
            }
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
                if (isAnomalous(var, p))
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

    public bool isAnomalous(correlatedFeatures cf, Point point)
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
}