# Weather Forecast App MAUI

![GitHub all releases](https://img.shields.io/github/downloads/GiorgioCitterio/WeatherForecastAppMAUI/total)
![GitHub](https://img.shields.io/github/license/GiorgioCitterio/WeatherForecastAppMAUI)
![GitHub deployments](https://img.shields.io/github/deployments/GiorgioCitterio/WeatherForecastAppMAUI/github-pages)
![GitHub repo size](https://img.shields.io/github/repo-size/GiorgioCitterio/WeatherForecastAppMAUI)
![GitHub Repo stars](https://img.shields.io/github/stars/GiorgioCitterio/WeatherForecastAppMAUI)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)

<a href="https://github.com/GiorgioCitterio/WeatherForecastAppMAUI/blob/master/README.it.md">README.it 🇮🇹</a>

---

## Table of Contents
- <a  href="#appoverview">App Overview</a>
- Activity 2: <a  href="#serpy">Serial Communication with Python and Arduino</a>
- Activity 3: <a  href="#isoosi">Communication with ISO/OSI Model Packet</a>
- Activity 4: <a  href="#guisensatt">Sensor/Actuator Control via GUI (Python)</a>
- Activity 5: <a  href="#sensjson">Storing Sensor Data in a JSON File</a>
- Activity 7: <a  href="#websens">Making Sensor Data Available on the Web</a>
- Activity 8: <a  href="#webatt">Motor Control via Web Browser</a>
- Activity 9: <a  href="#initrasp">Installation and Importing Programs on Raspberry Pi</a>
- Activity 10: <a  href="#rf24">Sending Packets via RF24 Radio Module</a>
- Activity 11: <a  href="#sensattrasp">Controlling Motor and Sensor from Raspberry Pi</a>
- Activity 12: <a  href="#mqtt">Using MQTT for IoT Device Communication</a>
- Activity 13: <a  href="#mqttesp">Using MQTT with ESP8266</a>
- Activity 14: <a  href="#alexa">Controlling Sensor and Actuator with Alexa</a>
- Activity 15: <a  href="#ble">Using BLE for Environment Control</a>

## Overview <a name="appoverview"></a>

The Weather Forecast App is a mobile application developed using .NET MAUI framework. It provides users with weather forecast information for different cities. The app offers various features such as setting a default location, displaying the current location using GPS, searching for weather forecasts of any city, supporting light/dark theme based on system settings, and allowing users to view a bar chart of daily temperatures. Additionally, users can add cities to favorites and easily access their forecasts. The app is currently available for the Android platform.
## System Requirements
Android device running Android OS (version 9 or higher)

## Installation

To install the Weather Forecast App on your Android device, follow [this guide.](https://github.com/GiorgioCitterio/WeatherForecastAppMAUI/wiki)

## Features

The Weather Forecast App offers the following features:
### 1. Set Default Location

- Users can set a default location for the app to display the weather forecast upon launch.
- This feature allows users to quickly access the weather information for their preferred location without manually searching for it.

### 2. Display Current Location
- The app utilizes GPS to determine the user's current location.
- Upon app launch, the current location's weather forecast is displayed.
- This feature provides users with real-time weather information for their current location.

### 3. Search for Weather Forecasts
- Users can search for weather forecasts of any city.
- By entering the name of the city in the search bar, users can retrieve the weather information for that specific location.
- This feature allows users to access weather forecasts for various cities around the world.

### 4. Light/Dark Theme
- The app supports both light and dark themes based on the system settings.
- Users can enjoy a visually appealing and comfortable user interface based on their preferred theme.

### 5. View Bar Chart of Daily Temperatures
- The app provides a bar chart that visualizes the daily temperatures for a selected location.
- Users can easily understand the temperature trends and variations for each day.
- This feature allows users to plan their activities based on the weather conditions.

### 6. Add Cities to Favorites
- Users can add cities to their favorites list for quick access to their weather forecasts.
- The favorites feature enables users to conveniently view the weather information for their frequently visited or preferred locations.

## .NET MAUI Version

The Weather Forecast App was developed using [.NET MAUI 7](https://learn.microsoft.com/en-us/dotnet/maui/whats-new/dotnet-7?view=net-maui-7.0). This version of .NET MAUI offers the latest features and enhancements for building cross-platform mobile applications.
### NuGet Packages

The Weather Forecast App utilizes the following NuGet packages:
- **CommunityToolkit.Mvvm**: This Microsoft library simplifies the implementation of the Model-View-ViewModel (MVVM) architecture in the app.
- **sqlite-net-pc**: This SQLite-net library is used for handling SQLite database operations in the app.
- **SQLitePCLRaw.bundle_green**: This Eric Sink library provides the necessary components for SQLite database functionality.
- **Syncfusion.Maui.Charts**: This Syncfusion library is used to create the bar chart visualization of daily temperatures in the app.

---
### Gifs

<img src="gifs/app_start.gif" width=250px></img>

<img src="gifs/search_city.gif" width=250px></img>

<img src="gifs/favourites.gif" width=250px></img>

<img src="gifs/settings.gif" width=250px></img>
