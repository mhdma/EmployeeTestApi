using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeTestApi.Contract;
using EmployeeTestApi.IInfrastructure;
using EmployeeTestApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeTestProject
{
    [TestClass]
    public class AccountControllerTests
    {
     
        private HttpClient _httpClient;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void SetUp()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
            _serviceProvider=webAppFactory.Services;
        }

        [TestMethod]
        public async Task ShouldExpect401WhenLoginWithInvalidCredentials()
        {
            var credentials = new LoginRequest
            {
                UserName = "admin",
                Password = "invalidPassword"
            };
            var response = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task ShouldReturnCorrectResponseForSuccessLogin()
        {
            var credentials = new LoginRequest
            {
                UserName = "admin",
                Password = "adminPassword"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginResponseContent);
            Assert.AreEqual(credentials.UserName, loginResult.UserName);
            Assert.IsNull(loginResult.OriginalUserName);
            Assert.AreEqual("Admin".ToLower(), loginResult.Role.ToLower());
            Assert.IsFalse(string.IsNullOrWhiteSpace(loginResult.AccessToken));
            Assert.IsFalse(string.IsNullOrWhiteSpace(loginResult.RefreshToken));

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var (principal, jwtSecurityToken) = jwtAuthManager.DecodeJwtToken(loginResult.AccessToken);
            Assert.AreEqual(credentials.UserName, principal.Identity.Name);
            Assert.AreEqual("Admin".ToLower()   , principal.FindFirst(ClaimTypes.Role).Value.ToLower());
            Assert.IsNotNull(jwtSecurityToken);
        }

        [TestMethod]
        public async Task ShouldBeAbleToLogout()
        {
            var credentials = new LoginRequest
            {
                UserName = "admin",
                Password = "adminPassword"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.IsTrue(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var logoutResponse = await _httpClient.PostAsync("api/account/logout", null);
            Assert.AreEqual(HttpStatusCode.OK, logoutResponse.StatusCode);
            Assert.IsFalse(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));
        }

        [TestMethod]
        public async Task ShouldCorrectlyRefreshToken()
        {
            const string userName = "admin";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var jwtResult = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-1));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwtResult.AccessToken);
            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
            var response = await _httpClient.PostAsync("api/account/refresh-token",
                new StringContent(JsonSerializer.Serialize(refreshRequest), Encoding.UTF8, MediaTypeNames.Application.Json));
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResult>(responseContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var refreshToken2 = jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.GetValueOrDefault(result.RefreshToken);
            Assert.AreEqual(refreshToken2.TokenString, result.RefreshToken);
            Assert.AreNotEqual(refreshToken2.TokenString, jwtResult.RefreshToken.TokenString);
            Assert.AreNotEqual(jwtResult.AccessToken, result.AccessToken);
        }


        [TestMethod]
        public async Task ShouldNotAllowToRefreshTokenWhenRefreshTokenIsExpired()
        {
            const string userName = "admin";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var jwtTokenConfig = _serviceProvider.GetRequiredService<JwtConfig>();
            var jwtResult1 = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-jwtTokenConfig.RefreshTokenExpiration - 1));
            var jwtResult2 = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-1));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwtResult2.AccessToken); // valid JWT token
            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = jwtResult1.RefreshToken.TokenString
            };
            var response = await _httpClient.PostAsync("api/account/refresh-token",
                new StringContent(JsonSerializer.Serialize(refreshRequest), Encoding.UTF8, MediaTypeNames.Application.Json)); // expired Refresh token
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual("Invalid token", responseContent);
        }

     
    }
}
