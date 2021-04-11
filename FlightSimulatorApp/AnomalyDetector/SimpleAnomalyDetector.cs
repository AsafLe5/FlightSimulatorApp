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

    //public void learnNormal(Timeseries ts)
    //{
    //    correlatedFeatures temp = new correlatedFeatures();
    //    float maxCorr = 0;
    //    float[] sFlotIn;
    //    int size = 0;
    //    bool isCorrelation = false;
    //    Point[] ps = new Point[200];//should be 200
    //    for (dynamic outItr = ts.table.First(); outItr != ts.table.Last(); ++outItr)
    //    { // Loop through the map.
    //        float[] floOut = new float[outItr.second.Count()];
    //        for (int i = 0; i < outItr.second.Count(); floOut[i] = outItr.second[i], i++) ; // Turns the strings to float.
    //        for (dynamic inItr = ts.table.First(); inItr != ts.table.Last(); ++inItr)
    //        { // Loop through the map.
    //            size = outItr.second.Count();
    //            float[] floIn = new float[outItr.second.Count()];
    //            for (int i = 0; i < outItr.second.Count(); floIn[i] = inItr.second[i], i++) ; // Turns strings to float.
    //            if (outItr == inItr)
    //                continue;
    //            int flagInCf = 0; // Flag that carry the fact if the current correlation is already in cf vector.
    //            for (int cfI = 0; cfI < this.cf.Count(); cfI++)
    //            {
    //                if (outItr.first == this.cf[cfI].feature2)
    //                    flagInCf = 1;
    //            }
    //            if (flagInCf == 1)
    //                continue;
    //            // Case the current correlation is the maximum so far.
    //            if (adu.pearson(floOut, floIn, inItr.second.Count()) > maxCorr)
    //            {
    //                maxCorr = adu.pearson(floOut, floIn, inItr.second.Count());
    //                temp.feature1 = outItr.first;
    //                temp.feature2 = inItr.first;
    //                temp.corrlation = maxCorr;
    //                //Point *ps[outItr->second.size()];
    //                float maxDev = 0;
    //                for (int k = 0; k < outItr.second.Count(); k++)
    //                {
    //                    ps[k] = new Point(outItr.second[k], inItr.second[k]); // Turns each local correlation to point.
    //                }
    //                temp.lin_reg = adu.linear_reg(ps, outItr.second.size());
    //                for (int k = 0; k < outItr.second.Count(); k++)
    //                {
    //                    if (maxDev < Math.Abs(adu.dev(ps[k], temp.lin_reg))) // case the current deviation is max deviation.
    //                        maxDev = Math.Abs(adu.dev(ps[k], temp.lin_reg));
    //                }
    //                temp.threshold = (float)(maxDev * 1.1); // ten present higher for little deviation.
    //                                                        //isCorrelation = true;
    //                sFlotIn = floIn;
    //            }
    //        }
    //        if (maxCorr > threshold)
    //            this.cf.Add(temp);
    //        else if (maxCorr > 0.5 && maxCorr < threshold)
    //            cirCorr(temp, ps, size);
    //        //isCorrelation = false;
    //        maxCorr = 0;
    //    }
    //}

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

/*    public string findMostCorrelated(string original, string csvPath, Dictionary<int, string> xmlDict)
    {
        //string newPath = AddHeader(csvPath, xmlDict);

        //Timeseries ts = new Timeseries(newPath);

        SimpleAnomalyDetector ad = new SimpleAnomalyDetector((float)0.5);

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
    }*/

    /*private string AddHeader(string csvPath, Dictionary<int, string> xmlDict)
    {
        string tempFilename = "temp.csv";
        bool toCopy = false;

        string header = "";

        for (int i = 0; i < xmlDict.Count; i++)
        {
            if (i == 0)
            {
                header += xmlDict[i];
            }
            else
            {
                header = header + "," + xmlDict[i];
            }
        }

        //check if header exists
        using (var sr = new StreamReader(csvPath))
        {
            using (var temp = new StreamWriter(tempFilename, false))
            {
                var line = sr.ReadLine(); // first line
                if (line != null && line != header) // check header exists
                {
                    toCopy = true; // need copy temp file to your original csv

                    // write your header into the temp file
                    temp.WriteLine(header);

                    while (line != null)
                    {
                        temp.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
            }
        }

        *//*      if (toCopy)
                  File.Copy(tempFilename, csvPath, true);
              File.Delete(tempFilename);*//*
        
        return tempFilename;
    }*/
}

