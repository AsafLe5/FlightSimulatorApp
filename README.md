# FlightSimulatorApp
**Preview from FlightSimulatorApp**

<img width="583" alt="2" src="https://user-images.githubusercontent.com/62257681/114745436-51802400-9d57-11eb-8327-d279c6c5ce7e.png">
<img width="960" alt="1" src="https://user-images.githubusercontent.com/62257681/114745553-6f4d8900-9d57-11eb-9622-42db4cf84746.png">


Our App (DesktopFGApp) is made especially for pilots and flight researchers that want to explore flights.
The app is getting a CSV file with flight data, and XML (with headers for CSV file and other information that will be used late in FlightGear app)- 
according to CSV and XML data FG will project flight on its window. Our app (will display on a separate window from the FG app) provides the following features -

* **Time Control** - Mainwindow contain time control buttons and a slider that will allow the user to control flight time.
* **Steers Display** - From the main window user can open the steers window which shows you steer picture for the current time in the flight. Among steers features you can find - joystick, aileron, elevator, yaw, pitch, and etc.
* **Data Display** - From the main window user can open the data display window which allows him to choose one of the attributes of the flight. According to the chosen attribute, the user will see a graph that displays the value of this attribute during the flight. Users will also see the most correlative attributes display on a separate graph. The third graph will display a correlation graph for the attributes above. All of the graphs display real-time data (meaning live data is streaming into the graphs as the flight is playing).
* **Anomaly Algorithm** - From the main window user can dynamically load the anomaly algorithm before or during the flight. The algorithm will be used by our app only if the DLL file implements the "IAnomalyDetector" interface. After detecting anomalies in current flight data - the user can see where anomalies happen in the main window slider (anomaly time will be display as ticks under the slider). Users can also find which attributes are in the anomaly report in the data display - the attributes button will be in red. In the correlation graph in the data display window - anomalies will be shown as blue points in the graph.
# Directory hierarchy
**Model:**

* **IFlightSimulatorModel** - an interface of the model.
* **MyFlightSimulatorModel** - open a TCP connection and sending data using this connection. In our case, the server is the FG app. Every data that send properties that change notifies the observers (View Model).
* **CSVPropery** - holds a float numbers from the CSV file and send them to the View Model's implementations.

**View Model:**

* **MediaViewModel** - controls the play speed, and sending notifies to the ViewController (MainWindow) of changes.
* **JoystickViewModel** - calculates the position of the flight variables and notify the JoystickView.
* **GraphViewModel** - user choose an attribute from a list. ViewModel finds taking the chosen feature and finds the most correlated attributes in data. Display different graph for every attributes and also their regression line. Data is updated in real-time.
* **DataDisplayViewModel** - get data attributes from the CSV file.
* **startMenuViewModel** - controls the start menu. have a 'Show Instructions' button that explain how to play the program. A 'upload train csv file' button that will upload a CSV file from the user and has a 'Start Online Mode' button that start the program.

**View:**

* **MainWindow** - display the time management. From this window, the user can open all of the other views.
* **MediaPanelView** - display the play speed and other controls like Stop Pause and etc.
* **JoystickView** - display the position of the joystick and steers variables.
* **GraphView** - display the different graphs as described above.
* **DataDisplayView** - display the values from the CSV file.
* **StartMenuView** - display the main menu of the program.


**Plugins:**

* **OxyPlot** - OxyPlot is a cross-platform plotting library for .NET using for graphs and etc.
* **Class Designer** - Used for building the a graph designer for the UML file.

# Installation
Before running our app you need to download and install the FlightGear application - you can find [download link here](https://www.flightgear.org/download/). For more information about FlightGear, you can find [here](https://www.flightgear.org/)

<img width="740" alt="3" src="https://user-images.githubusercontent.com/62257681/114746066-ff8bce00-9d57-11eb-8202-c206808ef756.png">

You will also need .Net Framework version 4.6.1 and up. You can find download link [here](https://dotnet.microsoft.com/download/dotnet-framework).

# Run application
run the exe file FlightSimulatorApp from FlightSimulatorApp folder. Make sure to run it from the exact location it is (\FlightSimulatorApp\FlightSimulatorApp) so it can find all the necessary files it's needed.

# Documentation
Here you can find a Link to UML contains partial information of the central classes and about the DLL. UML represents the various connections between the classes and the most important information found in each class. UML can be found [here](https://docs.microsoft.com/en-us/previous-versions/ff657806(v=vs.140)?redirectedfrom=MSDN). If you are a developer you can find full documentation of functions, variables, and more in the code.

# Video
Here you can find a link to our demo video - [link](https://youtu.be/Qs-tEBZg7to).
