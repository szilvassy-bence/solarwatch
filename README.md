# SolarWatch Project
SolarWatch is a full-stack web application project that provides sunrise and sunset times for a given city on a given date in UTC Time, handles user accounts and authorization. Users can also save their favorite cities to their account.

# Table of Contents
1. [About](#about)
2. [How to use](#how-to-use)
  - [Requirements](#requirements)
  - [Use with docker](#use-with-docker)

# About
SolarWatch is a full-stack web application project that provides sunrise and sunset times for a given city/place on a given date in UTC Time. The backend is an ASP.NET Web API that is connected to a Microsoft SQL database. The frontend uses React with the help of CSS styling.

To work with the SQL Server, backend uses Entity Framework Core, which is an ORM (Object-Relational Mapping) framework for .NET. With the help of it, interaction with the database can be done using object-oriented programming concepts, instead of writing SQL statements.

The application's features are only accessible through registration. Authentication is done via JWT tokens and while using ASP.NET Core's Identity.

For getting sunrise/sunset information, the application works with 3 external APIs:
- Coordinates are received from https://openweathermap.org/api .
- Sunrise/sunset information for a given coordinate is received from https://sunrise-sunset.org/ .
- Autoimplementing city name search https://developers.amadeus.com/self-service/category/destination-experiences/api-doc/city-search/api-reference .

# How to use
The project is dockerized, you should build and run the docker-compose.yml file to start use the application.

This section helps to set up the project locally and gives instructions about how to run it.

## Requirements
The project uses OpenWeather's API. The API proides 1000 API calls for free, but for every call we need an API key. Get a free API key at https://openweathermap.org/ by registering.

This section helps to set up the project locally and gives instructions about how to run it.

Further notes
The project uses OpenWeather's API. The API proides 1000 API calls for free, but for every call we need an API key. Get a free API key at https://openweathermap.org/ by registering.

## Use with docker
1. Clone the repo
```
git clone https://github.com/szilvassy-bence/solarwatch.git
```
2. Configure .env file according to .env.example file
   - Note that an api key is needed from OpenWeather's API: https://openweathermap.org/
3. Ensure that the following line (line 34.) in program.cs is uncommented: //context.Database.Migrate(); when running the first time.
4. Run the following command in your terminal:
```
docker-compose up
```
5. Visit the web application: http://localhost:5173/
