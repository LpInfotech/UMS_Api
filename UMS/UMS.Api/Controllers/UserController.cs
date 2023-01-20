using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;
using UMS.Api.Models;

namespace UMS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UserController : ControllerBase
    {
        #region Prop
        private readonly IConfiguration _configuration;
        #endregion

        #region CTOR
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods


        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public List<UserModel> GetUserList()
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecert = _configuration.GetValue<string>("AzureAd:SecretId");
            var clientSecertCredentials = new ClientSecretCredential(tenantId, clientId, clientSecert);
            GraphServiceClient graphServiceClient = new GraphServiceClient(clientSecertCredentials);

            List<UserModel> userList = new List<UserModel>();
            var users = graphServiceClient.Users.Request().Select(x => new { x.DisplayName, x.Mail, x.StreetAddress }).GetAsync().Result.ToList();
            if (users != null && users.Count > 0)
            {
                UserModel user = new UserModel();
                foreach (var item in users)
                {
                    user.DisplayName = item.DisplayName;
                    user.EmailAddress = item.Mail;
                }
                userList.AddRange((IEnumerable<UserModel>)user);
            }
            return userList;
        }

        [HttpPost]
        public async Task<User> AddUser()
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecert = _configuration.GetValue<string>("AzureAd:SecretId");
            var clientSecertCredentials = new ClientSecretCredential(tenantId, clientId, clientSecert);
            GraphServiceClient graphClient = new GraphServiceClient(clientSecertCredentials);

            var user = new User
            {
                AccountEnabled = true,
                DisplayName = "Sunil",
                MailNickname = "Su",
                UserPrincipalName = "AdeleV@contoso.onmicrosoft.com",
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = "xWwvJ]6NMw+bWH-d"
                }
            };
           var response =  await graphClient.Users
                .Request()
                .AddAsync(user);
            return response;
        }
    }
    #endregion
}

