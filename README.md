# sitecore-test

“Single sign on” service for several sites that consists of several components:

## Sitecore.MembershipAPI
Authentication web service that verifies user credentials against an SQL database.

REST web service which can be called by any platform: .NET, php, mobile app etc.
If client app is .NET based then it can use Sitecore.Membership library.

Note: From security point of view within current implementation the API must be deployed to the same data center as client apps cuz it doesn't support request/response authorization.

## Sitecore.Membership
Authentication module or shared library that connects to the web service and also implements caching of credentials.

This library can be shared between company .NET projects as a custom nuget package and consumed by both .NET Core and .NET Framework aplications, cuz it was created as .NET Standard class librarry. 
If client app is not .NET based then it can call Sitecore.MembershipAPI REST API direct.

## Sitecore.Web
a site-client that shows different content depending on a user. Uses JWT token for authentication.

## Installation

1. Create an empty database with name sitecoretest.
2. Set Properties\Target Platform for Database\Sitecore.Database project to your SQL Server version.
3. Publish Database\Sitecore.Database project into created database (right click in Solution Explorer then Publish).
4. Specify connection string in appsettings.json
5. Run solution

## Notes

1. Implemented using .NET Core 2.2 for applications and .NET Standard 2.0 for class libs
2. I haven't managed to add any unit test. Please have a look on MyUnitTestsFromAnotherProject folder. There are few tests from my live application based on xUnit and Moq