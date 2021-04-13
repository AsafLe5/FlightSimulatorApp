using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FlightSimulatorApp.AnomalyDetector;

namespace FlightSimulatorApp.Model
{
    class MyFlightSimulatorModel : IFlightSimulatorModel
    {
        #region CTOR and INPC

        ITelnetClient telnetClient;
        volatile Boolean stop;

        public MyFlightSimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region Connect and Start

        public void connect()
        {
            telnetClient.connect();
        }

        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }

        //bool isInitiallize = false;
        public void start()
        {

            loadAllCorrelatedFeatures();

            this.PlaybackSpeed = 10;

            new Thread(delegate ()
            {
                bool playbackSpeedZero = false;
                while (!stop)
                {
                    float playbackSpeedRational = 10;
                    if (this.playbackSpeed == 0.0)
                    {
                        playbackSpeedZero = true;
                    }
                    else
                    {
                        playbackSpeedZero = false;
                        playbackSpeedRational = 1000 / this.playbackSpeed;
                    }

                    // Keeping the animation running
                    if (this.currentLineIndex < this.csvLinesNumber && !playbackSpeedZero && !isPaused)
                    {
                        if (this.isOnline)
                        {
                            telnetClient.write(this.csvLines[this.currentLineIndex]);
                        }

                        // In Function
                        AileronCurrentValue = (float)Convert.ToDouble(this.csvDict[0][this.currentLineIndex]);
                        ElevatorCurrentValue = (float)Convert.ToDouble(this.csvDict[1][this.currentLineIndex]);
                        ThrottleCurrentValue = (float)Convert.ToDouble(this.csvDict[6][this.currentLineIndex]);
                        RudderCurrentValue = (float)Convert.ToDouble(this.csvDict[2][this.currentLineIndex]);
                        //

                        // In Function
                        CurrentAltimeter = (float)Convert.ToDouble(this.csvDict[24][this.currentLineIndex]);
                        CurrentAirSpeed = (float)Convert.ToDouble(this.csvDict[20][this.currentLineIndex]);
                        CurrentHeading = (float)Convert.ToDouble(this.csvDict[18][this.currentLineIndex]);
                        CurrentPitch = (float)Convert.ToDouble(this.csvDict[17][this.currentLineIndex]);
                        CurrentRoll = (float)Convert.ToDouble(this.csvDict[16][this.currentLineIndex]);
                        CurrentYaw = (float)Convert.ToDouble(this.csvDict[19][this.currentLineIndex]);
                        //

                        this.CurrentLineIndex += 1;

                        float nTime = this.currentLineIndex / 10;
                        //Time = TimeSpan.FromSeconds(nTime).ToString();
                        Time = "asd";
                    }

                    DataPointsList = RenderDataPointsList(this.CurrentAttribute);
                    // What if there is no most correlative by pearson?
                    DataPointsListToCorrelative = RenderDataPointsList(this.CurrentCorrelativeAttribute);

                    LastRecordsPointsList = getLastRecrods(this.DataPointsList, findNumberOfPoints());

                    Thread.Sleep((int)playbackSpeedRational);

                }
            }).Start();
        }

        #endregion

        #region Start Menu

        private bool isOnline;
        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                isOnline = value;
                NotifyPropertyChanged("IsOnline");
            }
        }

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged("Time");
            }
        }

        #endregion
        
        #region XML

        private Dictionary<int, string> xmlDict = new Dictionary<int, string>();
        private List<string> attributesList = new List<string>();
        public void initXML()
        {
            xmlDict[0] = "aileron";
            xmlDict[1] = "elevator";
            xmlDict[2] = "rudder";
            xmlDict[3] = "flaps";
            xmlDict[4] = "slats";
            xmlDict[5] = "speedbrake";
            xmlDict[6] = "throttle0";
            xmlDict[7] = "throttle1";
            xmlDict[8] = "engine-pump0";
            xmlDict[9] = "engine-pump1";
            xmlDict[10] = "electric-pump0";
            xmlDict[11] = "electric-pump1";
            xmlDict[12] = "external-power";
            xmlDict[13] = "APU-generator";
            xmlDict[14] = "latitude-deg";
            xmlDict[15] = "longitude-deg";
            xmlDict[16] = "altitude-ft";
            xmlDict[17] = "roll-deg";
            xmlDict[18] = "pitch-deg";
            xmlDict[19] = "heading-deg";
            xmlDict[20] = "side-slip-deg";
            xmlDict[21] = "airspeed-kt";
            xmlDict[22] = "glideslope";
            xmlDict[23] = "vertical-speed-fps";
            xmlDict[24] = "airspeed-indicator_indicated-speed-kt";
            xmlDict[25] = "altimeter_indicated-altitude-ft";
            xmlDict[26] = "altimeter_pressure-alt-ft";
            xmlDict[27] = "attitude-indicator_indicated-pitch-deg";
            xmlDict[28] = "attitude-indicator_indicated-roll-deg";
            xmlDict[29] = "attitude-indicator_internal-pitch-deg";
            xmlDict[30] = "attitude-indicator_internal-roll-deg";
            xmlDict[31] = "encoder_indicated-altitude-ft";
            xmlDict[32] = "encoder_pressure-alt-ft";
            xmlDict[33] = "gps_indicated-altitude-ft";
            xmlDict[34] = "gps_indicated-ground-speed-kt";
            xmlDict[35] = "gps_indicated-vertical-speed";
            xmlDict[36] = "indicated-heading-deg";
            xmlDict[37] = "magnetic-compass_indicated-heading-deg";
            xmlDict[38] = "slip-skid-ball_indicated-slip-skid";
            xmlDict[39] = "turn-indicator_indicated-turn-rate";
            xmlDict[40] = "vertical-speed-indicator_indicated-speed-fpm";
            xmlDict[41] = "engine_rpm";

            initAttributesList();
        }

        private void initAttributesList()
        {
            // all strings from this.xmlDict to list
            List<string> xmlPropList = new List<string>();
            for (int i = 0; i < xmlDict.Count; i++)
            {
                this.attributesList.Add(xmlDict[i]);
            }
        }

        public List<string> AttributesList
        {
            get { return this.attributesList; }
            set
            {
                this.attributesList = value;
                NotifyPropertyChanged("AttributesList");
            }
        }

        #endregion

        // TODO initXML out of csvParser

        #region CSV

        private string trainCSVPath;
        public void updateTrainCSVPath(string csvPath)
        {
            this.trainCSVPath = csvPath;
            praseCSV(this.trainCSVPath);
        }

        private string testCSVPath;
        public void updateTestCSVPath(string csvPath)
        {
            this.testCSVPath = csvPath;
            praseCSV(this.testCSVPath);
        }

        private List<string> csvLines;
        private Dictionary<int, List<string>> csvDict = new Dictionary<int, List<string>>();
        public void praseCSV(string csvPath)
        {
            initXML();

            // Init csvDict:
            for (int i = 0; i < xmlDict.Count; i++)
            {
                csvDict.Add(i, new List<string>());
            }

            this.csvLines = File.ReadLines(csvPath).ToList();

            int csvLinesNumberCounter = 0;
            int j = 0;
            String line = String.Empty;
            System.IO.StreamReader file = new System.IO.StreamReader(csvPath);
            while ((line = file.ReadLine()) != null)
            {
                String[] parts_of_line = line.Split(',');
                for (j = 0; j < parts_of_line.Length; j++)
                {
                    csvDict[j].Add(parts_of_line[j].Trim());
                }
                csvLinesNumberCounter++;
            }
            this.CSVLinesNumber = csvLinesNumberCounter;

            setMinAndMax(this.AileronMaximunValue, this.AileronMinimumValue, 0);
            setMinAndMax(this.ElevatorMaximunValue, this.ElevatorMinimumValue, 1);
            setMinAndMax(this.ThrottleMaximunValue, this.ThrottleMaximunValue, 6);
            setMinAndMax(this.RudderMinimumValue, this.RudderMinimumValue, 2);

            file.Close();
        }

        private void setMinAndMax(float max, float min, int columnNumber)
        {
            float tempMaximunValue = float.MinValue;
            float tempMinimumValue = float.MaxValue;
            for (int i = 0; i < this.csvLinesNumber; i++)
            {
                float currentValue = (float)Convert.ToDouble(this.csvDict[columnNumber][i]);
                if (currentValue > tempMaximunValue)
                {
                    tempMaximunValue = currentValue;
                }
                if (currentValue < tempMinimumValue)
                {
                    tempMinimumValue = currentValue;
                }
            }
            max = tempMaximunValue;
            min = tempMinimumValue;
            //Console.WriteLine(max.ToString());
        }

        #endregion

        #region Media

        private bool isPaused = false;

        public void onPlay()
        {
            this.isPaused = false;
        }

        public void onPause()
        {
            this.isPaused = true;
        }

        private int playbackSpeed;
        public int PlaybackSpeed
        {
            get { return playbackSpeed; }
            set
            {
                playbackSpeed = value;
                NotifyPropertyChanged("PlaybackSpeed");
            }
        }

        private int currentLineIndex;
        public int CurrentLineIndex
        {
            get { return currentLineIndex; }
            set
            {
                currentLineIndex = value;
                NotifyPropertyChanged("CurrentLineIndex");
            }
        }

        private int csvLinesNumber;
        public int CSVLinesNumber
        {
            get { return csvLinesNumber; }
            set
            {
                csvLinesNumber = value;
                NotifyPropertyChanged("CSVLinesNumber");
            }
        }

        #endregion

        #region Joystick

        private CSVProperty aileron = new CSVProperty();
        public float AileronCurrentValue
        {
            get { return aileron.propertyCurrentValue; }
            set
            {
                aileron.propertyCurrentValue = value;
                NotifyPropertyChanged("AileronCurrentValue");
            }
        }

        public float AileronMaximunValue
        {
            get { return aileron.propertyMaximunValue; }
            set
            {
                aileron.propertyMaximunValue = value;
                NotifyPropertyChanged("AileronMaximunValue");
            }
        }

        public float AileronMinimumValue
        {
            get { return aileron.propertyMinimumValue; }
            set
            {
                aileron.propertyMinimumValue = value;
                NotifyPropertyChanged("AileronMinimumValue");
            }
        }

        private CSVProperty elevator = new CSVProperty();
        public float ElevatorCurrentValue
        {
            get { return elevator.propertyCurrentValue + 20; }
            set
            {
                elevator.propertyCurrentValue = value;
                NotifyPropertyChanged("ElevatorCurrentValue");
            }
        }

        public float ElevatorMaximunValue
        {
            get { return elevator.propertyMaximunValue * -1; }
            set
            {
                elevator.propertyMaximunValue = value;
                NotifyPropertyChanged("ElevatorMaximunValue");
            }
        }

        public float ElevatorMinimumValue
        {
            get { return elevator.propertyMinimumValue * -1; }
            set
            {
                elevator.propertyMinimumValue = value;
                NotifyPropertyChanged("ElevatorMinimumValue");
            }
        }

        private CSVProperty throttle = new CSVProperty();
        public float ThrottleCurrentValue
        {
            get { return throttle.propertyCurrentValue; }
            set
            {
                throttle.propertyCurrentValue = value;
                NotifyPropertyChanged("ThrottleCurrentValue");
            }
        }

        public float ThrottleMaximunValue
        {
            get { return throttle.propertyMaximunValue; }
            set
            {
                throttle.propertyMaximunValue = value;
                NotifyPropertyChanged("ThrottleMaximunValue");
            }
        }

        public float ThrottleMinimumValue
        {
            get { return throttle.propertyMinimumValue; }
            set
            {
                throttle.propertyMinimumValue = value;
                NotifyPropertyChanged("ThrottleMinimumValue");
            }
        }

        private CSVProperty rudder = new CSVProperty();
        public float RudderCurrentValue
        {
            get { return rudder.propertyCurrentValue; }
            set
            {
                rudder.propertyCurrentValue = value;
                NotifyPropertyChanged("RudderCurrentValue");
            }
        }

        public float RudderMaximunValue
        {
            get { return rudder.propertyMaximunValue; }
            set
            {
                rudder.propertyMaximunValue = value;
                NotifyPropertyChanged("RudderMaximunValue");
            }
        }

        public float RudderMinimumValue
        {
            get { return rudder.propertyMinimumValue; }
            set
            {
                rudder.propertyMinimumValue = value;
                NotifyPropertyChanged("RudderMinimumValue");
            }
        }

        #endregion

        #region DataDisplay

        private CSVProperty altimeter = new CSVProperty();
        public float CurrentAltimeter
        {
            get { return altimeter.propertyCurrentValue; }
            set
            {
                altimeter.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentAltimeter");
            }
        }

        private CSVProperty airSpeed = new CSVProperty();
        public float CurrentAirSpeed
        {
            get { return airSpeed.propertyCurrentValue; }
            set
            {
                airSpeed.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentAirSpeed");
            }
        }

        private CSVProperty heading = new CSVProperty();
        public float CurrentHeading
        {
            get { return heading.propertyCurrentValue; }
            set
            {
                heading.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentHeading");
            }
        }

        private CSVProperty pitch = new CSVProperty();
        public float CurrentPitch
        {
            get { return pitch.propertyCurrentValue; }
            set
            {
                pitch.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentPitch");
            }
        }

        private CSVProperty roll = new CSVProperty();
        public float CurrentRoll
        {
            get { return roll.propertyCurrentValue; }
            set
            {
                roll.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentRoll");
            }
        }

        private CSVProperty yaw = new CSVProperty();
        public float CurrentYaw
        {
            get { return yaw.propertyCurrentValue; }
            set
            {
                yaw.propertyCurrentValue = value;
                NotifyPropertyChanged("CurrentYaw");
            }
        }

        #endregion

        #region Graph

        #region Functions

        private List<DataPoint> RenderDataPointsList(string attribute)
        {
            List<DataPoint> currentList = new List<DataPoint>();
            int currentAttributeIndex = 0;

            currentAttributeIndex = findIndexByAttribute(attribute);
            for (int i = 0; i < this.currentLineIndex; i++)
            {
                currentList.Add(new DataPoint(i, Convert.ToDouble(csvDict[currentAttributeIndex][i])));
            }

            return currentList;
        }

        // Finding the most correlative attribute by pearson to this.currentAttribute
        private void findCorrelativeAttribute(string attribute)
        {
            /*            SimpleAnomalyDetector smp = new SimpleAnomalyDetector((float)0.5);
                        CurrentCorrelativeAttribute = smp.findMostCorrelated(attribute, this.csvPath, this.xmlDict);*/

            foreach (correlatedFeatures corf in this.cf)
            {
                if (corf.feature1.Equals(attribute))
                    CurrentCorrelativeAttribute = corf.feature2;
                if (corf.feature2.Equals(attribute))
                    CurrentCorrelativeAttribute = corf.feature1;
            }

        }

        private void InitiallizeRegression()
        {
            RegressionDataPointsList = getRegressionPoints(this.CurrentCorrelativeAttribute);

            calcLineOfCurrentAttribute();

            LineIntercept = getIntercept();
            LineSlope = getSlope();
        }

        private AnomalyDetector.Line PointsLine;
        private void calcLineOfCurrentAttribute()
        {
            anomaly_detection_util adu = new anomaly_detection_util();
            AnomalyDetector.Point[] pointsList = new AnomalyDetector.Point[csvLinesNumber];
            //= dataPointsList.ToList<Point>();

            int i;
            for (i = 0; i < csvLinesNumber; i++)
            {
                AnomalyDetector.Point point = new AnomalyDetector.Point((float)regressionList[i].X, (float)regressionList[i].Y);
                pointsList[i] = point;
            }
            PointsLine = adu.linear_reg(pointsList, i);

        }

        private float getSlope()
        {
            return PointsLine.a;
        }

        private float getIntercept()
        {
            return PointsLine.a;
        }

        private int findIndexByAttribute(string attribute)
        {
            for (int i = 0; i < xmlDict.Count; i++)
            {
                if (xmlDict[i].ToString().Equals(attribute))
                {
                    return i;
                }
            }
            return 0;
        }

        private List<DataPoint> getLastRecrods(List<DataPoint> list, int numberOfRecords)
        {
            if (list.Count < numberOfRecords)
            {
                return list;
            }
            return list.GetRange(list.Count - numberOfRecords, numberOfRecords);
        }

        private int findNumberOfPoints()
        {
            // this.playbackSpeed = number of lines we write in a second.
            return 30 * this.playbackSpeed;
        }

        #endregion

        #region Properties

        private List<DataPoint> dataPointsList = new List<DataPoint>();
        public List<DataPoint> DataPointsList
        {
            get { return this.dataPointsList; }
            set
            {
                dataPointsList = value;
                NotifyPropertyChanged("DataPointsList");
            }
        }

        private string currentAttribute;
        public string CurrentAttribute
        {
            get { return this.currentAttribute; }
            set
            {
                this.currentAttribute = value;
                NotifyPropertyChanged("CurrentAttribute");
                findCorrelativeAttribute(this.currentAttribute);
                InitiallizeRegression();

            }
        }

        private string currentCorrelativeAttribute;
        public string CurrentCorrelativeAttribute
        {
            get { return currentCorrelativeAttribute; }
            set
            {
                currentCorrelativeAttribute = value;
                NotifyPropertyChanged("CurrentCorrelativeAttribute");
            }
        }

        private List<DataPoint> dataPointsListToCorrelative = new List<DataPoint>();
        public List<DataPoint> DataPointsListToCorrelative
        {
            get { return this.dataPointsListToCorrelative; }
            set
            {
                this.dataPointsListToCorrelative = value;
                NotifyPropertyChanged("DataPointsListToCorrelative");
            }
        }

        private List<DataPoint> regressionDataPointsList = new List<DataPoint>();
        public List<DataPoint> RegressionDataPointsList
        {
            get { return this.regressionDataPointsList; }
            set
            {
                this.regressionDataPointsList = value;
                NotifyPropertyChanged("RegressionDataPointsList");
            }
        }

        private List<DataPoint> lastRecordsPointsList = new List<DataPoint>();
        public List<DataPoint> LastRecordsPointsList
        {
            get { return this.lastRecordsPointsList; }
            set
            {
                this.lastRecordsPointsList = value;
                NotifyPropertyChanged("LastRecordsPointsList");
            }
        }

        private float lineIntercept;
        public float LineIntercept
        {
            get { return this.lineIntercept; }
            set
            {
                lineIntercept = value;
                NotifyPropertyChanged("LineIntercept");
            }
        }

        private float lineSlope;
        public float LineSlope
        {
            get { return this.lineSlope; }
            set
            {
                lineSlope = value;
                NotifyPropertyChanged("LineSlope");
            }
        }

        #endregion

        #endregion

        #region Correlation Handler

        // Adds a header to the a new temproray csv file.
        private string addHeader(string csvPath, Dictionary<int, string> xmlDict)
        {
            string tempFilename = "temp.csv";

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

            return tempFilename;
        }

        private SimpleAnomalyDetector ad;
        private Timeseries ts;
        List<correlatedFeatures> cf;
        private void loadAllCorrelatedFeatures()
        {
            // What csv?
            string newPath = addHeader(this.trainCSVPath, this.xmlDict);

            this.ts = new Timeseries(newPath);

            this.ad = new SimpleAnomalyDetector((float)0.5);

            this.ad.learnNormal(this.ts);
            this.cf = ad.getNormalModel();
        }

        #endregion

        #region Regrassion

        private List<DataPoint> regressionList = new List<DataPoint>();
        private List<DataPoint> getRegressionPoints(string attribute)
        {
            this.regressionList = new List<DataPoint>();

            int currentAttributeIndex = 0;

            currentAttributeIndex = findIndexByAttribute(attribute);

            for (int i = 0; i < this.csvLinesNumber; i++)
            {
                regressionList.Add(new DataPoint(i, Convert.ToDouble(csvDict[currentAttributeIndex][i])));
            }

            return regressionList;
        }

        #endregion
    }
}
