# TeamCity Theatre 

[![Build Status](https://travis-ci.org/amoerie/teamcity-theatre.svg?branch=master)](https://travis-ci.org/amoerie/teamcity-theatre)

A .NET MVC web application to monitor your TeamCity builds. 
Stick a TV on the wall, open a browser there and enjoy your TeamCity projects in all their red and green glory.

## Screenies

### The home page: choose your team
![Choose your team](http://i.imgur.com/64YxBRb.png)

### Team view
![The dashboard screen](http://i.imgur.com/izZiWVd.png)

### Configuration: manage your views and their tiles
![The config screen](http://i.imgur.com/4Rg4yi6.png)

## Features

- First-class support for branches! (This is a feature many others are lacking)
- Create multiple dashboards, one for each team!
- Customizable amount of branches shown per tile
- Customizable amount of columns shown per view, make optimal use of the size of your wall TV!
- Customizable labels on tiles

## Requirements

- A TeamCity server (d'uh). TeamCityTheatre is confirmed to be compatible with 2017.1.4 (build 47070). Other versions may or may not work, JetBrains likes to break its own APIs from time to time.
- A Windows Server with IIS to host the web application, or your Windows dev machine if you just want to try it out.
- Some knowledge on how to add a .NET web application in IIS, or the willingness to learn.
- A nice cup of coffee to drink while you install this. 

## Installation instructions

### Windows IIS

1. Ensure you have [.NET Core Windows Server Hosting Bundle](https://docs.microsoft.com/en-us/aspnet/core/publishing/iis) installed 
2. Download and unzip the [the latest release](https://github.com/amoerie/teamcity-theatre/releases)
3. Add the following to the `appsettings.json` file:

```
  "Connection": {
    "Url": "http://your-teamcity-server/",
    "Username": "your-teamcity-username",
    "Password": "your-teamcity-password"
  }
```

4. (Optional) Change the location of the configuration.json file or leave the default
5. Install this folder as a web application in IIS:
  - Application pool should use .NET CLR version 'No Managed Code'
  - Application pool should use Managed Pipeline mode 'Integrated'
  
## Usage instructions

Open the web application from a browser
  - Make sure that you type the URL in lowercase
  - Open the settings page from the main menu. 
    - If you see any errors, your server or credentials might be incorrect. Check in the log files why the network request failed.
  - Add a new view, give it a name.
  - Expand your TeamCity projects in the left bottom pane and select one to see its build configurations.
  - Add build configurations to your view. These will become the tiles of your view.
  - Open the dashboard from the main menu and select your view
  - Wait for the data to load. 
  - Enjoy.

## Compilation instructions

1. Ensure you have [.NET Core SDK](https://www.microsoft.com/net/download/core) installed
2. Ensure you have [Yarn](https://yarnpkg.com) installed
3. Execute "publish.cmd" or "publish.sh" (.cmd for Windows)
4. If all goes well, that should create a folder 'publish-output' which is all you need to host the application. See Installation instructions from here.

