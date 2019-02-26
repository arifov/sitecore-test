using RestSharp;
using Sitecore.Membership.Cache;

namespace Sitecore.Membership
{
    /// <summary>
    /// A wrapper around Sitecore.MembershipAPI.
    /// Supports user profile caching
    /// </summary>
    public class UserManager : IUserManager
    {
        private RestClient _resApiClient;
        private readonly ICacheStorage _cache = new MemoryCacheStorage();

        public void Init(string serviceUrl)
        {
            if (_resApiClient == null)
            {
                _resApiClient = new RestClient(serviceUrl);
            }
        }

        public UserProfile Validate(string email, string password)
        {
            return _cache.GetOrCreate(email, () =>
            {
                var request = new RestRequest("account/validateUser", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { email = email, password = password });

                var response = _resApiClient.Execute<UserProfile>(request);

                return response.Data;
            });
        }

        public ResponseData RegisterUser(string email, string password)
        {
            var request = new RestRequest("account/registerUser", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { email = email, password = password });

            var response = _resApiClient.Execute<ResponseData>(request);

            return response.Data;
        }

        public void Logout(string email)
        {
            _cache.Remove(email);
        }
    }
}
