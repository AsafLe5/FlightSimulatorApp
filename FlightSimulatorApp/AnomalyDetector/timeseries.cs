using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.AnomalyDetector;

namespace FlightSimulatorApp.AnomalyDetector
{
    public class Timeseries
    {
        private FileStream csv;
        public List<string> features = new List<string>();
        public Dictionary<string, List<float>> table = new Dictionary<string, List<float>>();
        public Timeseries(string CSVfileName)
        {

            List<List<string>> csvTwoD = new List<List<string>>();

            bool notRead = true;
            String line = String.Empty;
            System.IO.StreamReader file = new System.IO.StreamReader(CSVfileName);
            while ((line = file.ReadLine()) != null)
            {
                String[] parts_of_line = line.Split(',');
                if (notRead)
                {
                    for (int i = 0; i < parts_of_line.Length; i++)
                    {
                        csvTwoD.Add(new List<string>());
                    }
                    notRead = false;
                }
                List<string> cur = new List<string>();
                for (int j = 0; j < parts_of_line.Length; j++)
                {
                    csvTwoD[j].Add(parts_of_line[j].Trim());
                }
            }


            // Transpose the csvTwoD to get the correct order of lines as in the CVS file.
            List<List<string>> transVec = new List<List<string>>();
            for (int i = 0; i < csvTwoD.Count(); i++)
            {
                List<string> curVec = new List<string>();
                for (int j = 0; j < csvTwoD[i].Count(); j++)
                {
                    curVec.Add(csvTwoD[i][j]);
                }
                transVec.Add(curVec);
            }
            Dictionary<string, List<float>> itMap = new Dictionary<string, List<float>>();
            for (int i = 0; i < transVec.Count(); i++)
            { // Loop through the map.
                List<float> col = new List<float>();
                for (int j = 0; j < transVec[i].Count(); ++j)
                { // checks whether we are in the first line.
                    if (j == 0) // Case line is 0 then it means its the title of the column.
                    {
                        this.features.Add(transVec[i][j]);
                        continue;
                    }
                    string valueAsString = transVec[i][j];
                    float valueAsFloat = float.Parse(valueAsString);
                    col.Add(valueAsFloat);
                }
                this.table.Add(transVec[i][0], col); // Adding the titles and column to the map.
            }
        }
    }
}
