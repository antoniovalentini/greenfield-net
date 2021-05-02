# Expensi
A simple expenses tracker build upon a claims-based architecture.

![Identity Server Example](https://github.com/antoniovalentini/expensi-claimsbased/blob/master/docs/Expensi_drawio.jpg)

## Main Actors
- Database
- APIs
- Web Application
- Mobile Application
- STS (Identity Server)

## Database
Expenses main storage.

## APIs
Main access-point to the user expenses. The resource to protect.

## Web Application
Razor Pages application for managing your expenses. 

## Mobile Application
Not started yet.

## STS
Identity Provider and Secret Token Service which will interact with all the other application.
It will protect the APIs from unauthorized accesses and make sure you're able to register new users from the Web and Mobile application.

## Solution setup
In order to test the solution you have to start by defining 2 clients secrets in the project configuration named "clientSecret" and "roClientSecret" for the IdentityServer and Web project.
I'm using the .net core secrets manager. This will allow the Web application to properly authenticate against the Id4 application.

You also have to define a "MongoDbConnection" configuration key with the mongodb connection string in order for the API project to work. 
You can get a free mongodb instance at [MongoDB Atlas](https://www.mongodb.com/cloud/atlas).

### log4net
Add a log4net.config file inside the src\Avalentini.Expensi.Api folder to make log4net work. This is the standard log4net configuration file.
[More info here](https://www.loggly.com/docs/net-logs/)
