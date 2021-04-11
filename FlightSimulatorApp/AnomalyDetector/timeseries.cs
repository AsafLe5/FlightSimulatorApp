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
        public Dictionary<string, List<float>> table;
        public Timeseries(string CSVfileName)
        {
            string val;
            // using (var reader = new StreamReader(CSVfileName));
            string path = CSVfileName;
            // Two dimensional vector of string, represent the whole CSV file.

            List<List<string>> csvTwoD = new List<List<string>>();
            // Open the stream and read it back.
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] line = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                int i = 0;
                while (fs.Read(line, 0, line.Length) > 0)
                {
                    List<float> vals;
                    temp.GetString(line);
                    string str = System.Text.Encoding.UTF8.GetString(line);
                    List<string> row = new List<string>();
                    string[] rowStr = str.Split(',');
                    for (int j = 0; j < rowStr.Count(); j++)
                    {
                        row.Add(rowStr[j]);
                    }
                    csvTwoD.Add(row);
                    i++;
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
                        continue;
                    col.Add(float.Parse(transVec[i][j]));
                }
                this.table.Add(transVec[i][0], col); // Adding the titles and column to the map.
            }
        }
    }
}
