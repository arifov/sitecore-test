## Sitecore.MembershipAPI

Authentication web service that verifies user credentials against an SQL database.

REST web service which can be called by any platform: .NET, php, mobile app etc.
If client app is .NET based then it can use Sitecore.Membership library.

Note: From security point of view within current implementation the API must be deployed to the same data center as client apps cuz it doesn't support request/response authorization.