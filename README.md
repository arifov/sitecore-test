# sitecore-test

“Single sign on” service for several sites that consists of several components:

Sitecore.MembershipAPI - Authentication web service that verifies user credentials against an SQL database.
Sitecore.Membership - Authentication module on every site that connects to the web service and also implements caching of credentials.
Sitecore.Web - a site-client that shows different content depending on a user. Uses JWT token for authentication.

Installation:

1. Create an empty database with name sitecoretest.
2. Set Properties\Target Platform for Database\Sitecore.Database project to your SQL Server version.
3. Publish Database\Sitecore.Database project into created database (right click in Solution Explorer then Publish).
4. Specify connection string in appsettings.json
5. Run solution

Notes:

I haven't managed to add any unit test. Please have a look on MyUnitTestsFromAnotherProject folder. There are few tests from my live application based on xUnit and Moq