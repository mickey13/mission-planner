# MissionPlanner

![Dot Net](https://github.com/ardupilot/missionplanner/actions/workflows/main.yml/badge.svg) ![Android](https://github.com/ardupilot/missionplanner/actions/workflows/android.yml/badge.svg) ![OSX/IOS](https://github.com/ardupilot/missionplanner/actions/workflows/mac.yml/badge.svg)

Website : http://ardupilot.org/planner/

Forum : http://discuss.ardupilot.org/c/ground-control-software/mission-planner

Download latest stable version : http://firmware.ardupilot.org/Tools/MissionPlanner/MissionPlanner-latest.msi

Changelog : https://github.com/ArduPilot/MissionPlanner/blob/master/ChangeLog.txt

License : https://github.com/ArduPilot/MissionPlanner/blob/master/COPYING.txt


## How to compile

### On Windows (Recommended)

#### 1. Install software

##### Main requirements

Currently, Mission Planner needs:

Visual Studio 2022

##### IDE

### Visual Studio Community
To compile Mission Planner, we recommend using Visual Studio. You can download Visual Studio Community from the [Visual Studio Download page](https://visualstudio.microsoft.com/downloads/ "Visual Studio Download page").

Visual Studio is a comprehensive suite with built-in Git support, but it can be overwhelming due to its complexity. To streamline the installation process, you can customize your installation by selecting the relevant "Workloads" and "Individual components" based on your software development needs.

To simplify this selection process, we have provided a configuration file that specifies the components required for MissionPlanner development. Here's how you can use it:

1. Go to "More" in the Visual Studio installer.
2. Select "Import configuration."
3. Use the following file: [vs2022.vsconfig](https://raw.githubusercontent.com/ArduPilot/MissionPlanner/master/vs2022.vsconfig "vs2022.vsconfig").

By following these steps, you'll have the necessary components installed and ready for Mission Planner development.

#### AFT Specific Information
The AFT software has only been tested on Windows using Visual Studio 2022. Support for other systems does not currently exist.

##### To-do before compiling
1. Open `AFTController.cs` and set the following objects that are at the top of the file:
   - `bingMapsKey``bingMapsKey`
     - This is a Bing Maps API key
   - `locationStart`
     - This is a (Latitude, Longitude) pair
  a.  The following objects in `AFTController.cs` are optional to change:
     - `zoomStart`
       - This is the zoom level that the map will start at
     - `inputtedPortName`
       - This is the port name that will be used in MAV connection
       - Options for this are `preset`, `TCP`, `UDP`, `WS`, `UDPCl`, and `AUTO`
     - `inputtedBaud`
       - This is the baudrate that will be used in MAV connection

##### How to use this software
1. Upon running this application, the user will have the option to choose between using the AFT software or booting into the complete, original MissionPlanner software.       The Ground and Air buttons will launch the same software, but will each assume a different MAV firmware; `ArduRover` and `ArduCopter2`, respectively.
2. Mouse click controls for Bing Maps control
   - Double click on the map to create a pushpin and add the clicked position to the mission boundary. This first point that is added to the mission boundary will be set        as the drone's home position and its pushpin will have a different color.
   - Left click and drag to move the map around
   - Right click on a pushpin and drag to move a pushpin and edit the mission boundary. Release the right mouse button to stop editing the pushpin location and missoin          boundary
3. To initiate the MAV connection sequence, click the button in the top right.
4. To add a polyline to the map that represents the shortest safe path between the MAV and the home location, click the compass button in the bottom left.
5. To switch to a video downlink from the MAV, click the button in the bottom right.
6. Saving/loading missions
   - Missions are saved to .json files using a custom format. See the `MissionSettings` class in `AFTController.cs` to see the structure of this format.

##### Known/common issues & quirks
1. Forms that contain circular selection buttons
   - When one of these forms are created, two clicks are required to select any one selection button. The current belief is that the first click gives focus to the button,      and the second click is what is registered and triggers the button click event.
2. Child form control boxes
   - Child form control boxes appear in the software. This can be fixed by docking a menustrip to the parent form and settings its visibility to false (see                      `AFTMDIContainer.cs`). But, the addition of this menustrip causes the top menu bar in `MainV2` to not render when it (`MainV2`) is instantiated.
3. Map polygon boundary and pushpins
   - The default anchor point for Bing Maps control pushpins are not at the point of the pushpin, where one would expect. This causes the pushpin and polygon boundary to        intersect in the upper left part of the pushpin and not at the point. It is believed that this cannot be fixed with out creating a custom pushpin.
4. The image of the button in the top right is supposed to change based on the MAV connection status, but this is not functional.
5. "The base class 'System.Void' cannot be designed"
   - This has been a recurring error that has blocked the ability to view/edit form designs. This is believed to be an internal VS issue. Sometimes, but not always, the following steps work to fix this:
     1. Close all files
     2. Clean solution
     3. Close and reopen Visual Studio
     4. Open solution
     5. Rebuild Solution
     6. Build solution

###### VSCode
Currently VSCode with C# plugin is able to parse the code but cannot build.

#### 2. Get the code

If you get Visual Studio Community, you should be able to use Git from the IDE. 
Clone `https://github.com/ArduPilot/MissionPlanner.git` to get the full code.

In case you didn't install an IDE, you will need to manually install Git. Please follow instruction in https://ardupilot.org/dev/docs/where-to-get-the-code.html#downloading-the-code-using-git

Open a git bash terminal in the MissionPlanner directory and type, "git submodule update --init" to download all submodules

#### 3. Build

To build the code:
- Open MissionPlanner.sln with Visual Studio
- From the Build menu, select "Build MissionPlanner"

### On other systems
Building Mission Planner on other systems isn't support currently.

## Launching Mission Planner on other system

Mission Planner is available for Android via the Play Store. https://play.google.com/store/apps/details?id=com.michaeloborne.MissionPlanner
Mission Planner can be used with Mono on Linux systems. Be aware that not all functions are available on Linux.
Native MacOS and iOS support is experimental and not recommended for inexperienced users. https://github.com/ArduPilot/MissionPlanner/releases/tag/osxlatest 
For MacOS users it is recommended to use Mission Planner for Windows via Boot Camp or Parallels (or equivalent).

### On Linux

#### Requirements

Those instructions were tested on Ubuntu 20.04.
Please install Mono, either :
- `sudo apt install mono-complete mono-runtime libmono-system-windows-forms4.0-cil libmono-system-core4.0-cil libmono-winforms4.0-cil libmono-corlib4.0-cil libmono-system-management4.0-cil libmono-system-xml-linq4.0-cil`

#### Launching

- Get the lastest zipped version of Mission Planner here : https://firmware.ardupilot.org/Tools/MissionPlanner/MissionPlanner-latest.zip
- Unzip in the directory you want
- Go into the directory
- run with `mono MissionPlanner.exe`

You can debug Mission Planner on Mono with `MONO_LOG_LEVEL=debug mono MissionPlanner.exe`

### External Services Used

| Source | Use | How to disable | Custodian |
|---|---|---|---|
| https://firmware.oborne.me  | used as a global cdn for checking for MP update check - checked once per day at startup | edit missionplanner.exe.config | Michael Oborne |
| https://firmware.ardupilot.org  | used for updates to stable, firmware metadata, firmware, user alerts, gstreamer, SRTM, SITL | updates to stable (edit missionplanner.exe.config) - all others Not possible | Ardupilot Team |
| https://github.com/ | used for updates to beta | edit missionplanner.exe.config | Michael Oborne |
| https://raw.githubusercontent.com | old param metadata, sitl config files | Not possible | Ardupilot Team |
| https://api.github.com/ | ardupilot preload param files | Not possible | Ardupilot Team |
| https://raw.oborne.me/  | used as glocal cdn for parameter metadata generator, no longer primary source | only used at user request to regenerate, edit missionplanner.exe.config | Michael Oborne |
| https://maps.google.com  | used for elevation api - removed due to abuse | N/A | N/A |
| https://discuss.cubepilot.org/ | use for SB2 reporting - only on affected boards when user enters details | only used at user request | CubePilot |
| https://altitudeangel.com  | utm data - user enabled | only used at user request | Altitude Angel |
| https://autotest.ardupilot.org  | dataflash log meta data, parameter metadata | Not Possible | Ardupilot Team |
| Many | your choice of map provider google/bing/openstreetmap/etc | User selectable | User/Many |
| https://www.cloudflare.com | geo location provider - for NFZ selection | Not Possible | Michael Oborne |
| https://esua.cad.gov.hk | HK no fly zones - user enabled | User selectable | HK Gov |
| https://ssl.google-analytics.com | Google Analytics Anonymous Stats - Screen Loads, Exceptions/Crashs, Events (Connect), Startup Timing, FW upload (FW Type and Board Type) | disable in Config > Planner > OptOut Anon Stats | Michael Oborne |
| https://api.dronelogbook.com | logging - disabled | N/A | N/A |
| https://ardupilot.org | help urls on many pages | User Initiated | ArduPilot Team |
| https://www.youtube.com | help videos on many pages | User Initiated | ArduPilot Team |
| https://files.rfdesign.com.au | RFD firmwares | User Initiated | RFDesign |
| https://teck.airmarket.io | airmarket - disabled | N/A | N/A |

### Offline Use - No Internet

| Location | Use | Transferable between pcs |
|---|---|---|
| C:\ProgramData\Mission Planner\gmapcache | Map cache | yes |
| C:\ProgramData\Mission Planner\srtm | Elevation data cache | yes |
| C:\ProgramData\Mission Planner\\*.pdef.xml | Parameter cache | yes |
| C:\ProgramData\Mission Planner\LogMessages*.xml | DF Log metadata cache | yes |

on linux this is in /home/<user>/.local/share/Mission Planner/

### Offline Data Supported
#### Elevation
* SRTM Cache
* GeoTiff's in WGS84/EGM96
* DTED

#### Images
* Map Cache
* WMS
* WMTS
* GDAL

### Paths used - Default

| Location | Use |
|---|---|
| C:\ProgramData\Mission Planner | All cross user content |
| C:\Users\USERNAME\Documents\Mission Planner | All per user content |

on linux this is in /home/<user>/.local/share/Mission Planner/

### CA Cert
A CA cert is installed to the root store and used to sign the windows serial port drivers, and is installed as part of the MSI install.

[![FlagCounter](https://s01.flagcounter.com/count2/A4bA/bg_FFFFFF/txt_000000/border_CCCCCC/columns_8/maxflags_40/viewers_0/labels_1/pageviews_0/flags_0/percent_0/)](https://info.flagcounter.com/A4bA)
