using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;
using UserManagement.IntegrationTest.Services;

namespace UserManagement.IntegrationTest.StepDefinitions
{
    [Binding]
    public class UserManagementSteps
    {
        private readonly HttpClientService _httpClient;
        private readonly IUserService _userService;
        private HttpResponseMessage _response;
        private string _responseContent;
        private const string BaseUrl = "https://localhost:44388";
        private string _name;
        private string _email;
        private string _password;
        private string _Confirmpassword;

        private string ReferenceId;
        private UserRequest user;

        public UserManagementSteps(HttpClientService client, IUserService userService)
        {
            _httpClient = client;
            _userService = userService;
        }
        [Given(@"I have a new user with Username ""([^""]*)"", Email ""([^""]*)"", Password ""([^""]*)"", and ConfirmPassword ""([^""]*)""")]
        public void GivenIHaveANewUserWithUsernameEmailPasswordAndConfirmPassword(string testuser, string p1, string p2, string p3)
        {
            try
            {
                user = new UserRequest
                {
                    Username = testuser,
                    Email = p1,
                    Password = p2,
                    ConfirmPassword = p3
                };
                var result = _userService.CreateUser(user).Result;
                Console.WriteLine(result);
            }
            catch (Exception xe)
            {

                Console.WriteLine(xe.Message);
            }
            
            //Assert.That(string.IsNullOrEmpty(result));
        }

        [When(@"I send a request to create the user")]
        public void WhenISendARequestToCreateTheUser()
        {
            var Createcustomer = _httpClient.PostAsync($"{BaseUrl}/account/User", user).Result;
            _response = Createcustomer;
            _responseContent = Createcustomer.Content.ReadAsStringAsync().Result;
            Assert.That(string.IsNullOrEmpty(_responseContent));
        }

        [Then(@"the response should be (.*)")]
        public void ThenTheResponseShouldBe(int p0)
        {
            Assert.Equals(p0, (int)_response.StatusCode);
        }

        [Then(@"the user should be retrievable by ReferenceId ""([^""]*)""")]
        public void ThenTheUserShouldBeRetrievableByReferenceId(string p0)
        {
            ReferenceId = p0;
            var response =  _httpClient.GetAsync($"{BaseUrl}/account/get-User/{p0}").Result;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Given(@"I have an existing user with ReferenceId ""([^""]*)"", Username ""([^""]*)"", Email ""([^""]*)"", Password ""([^""]*)"", and ConfirmPassword ""([^""]*)""")]
        public void GivenIHaveAnExistingUserWithReferenceIdUsernameEmailPasswordAndConfirmPassword(string p0, string testuser, string p2, string p3, string p4)
        {
            _response = _httpClient.GetAsync($"{BaseUrl}/account/get-User").Result;
            var res = _response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<UserDto>>(res);
            foreach (var item in result)
            {
                Console.WriteLine(item);
                Assert.Equals(p0, item.ReferenceId);
                Assert.Equals(testuser, item.Username);
                Assert.Equals(p2, item.Email);
                Assert.Equals(p3, item.Password);
            }
            Assert.Equals(HttpStatusCode.OK, _response.StatusCode);
        }

        [When(@"I send a request to update the user with ReferenceId ""([^""]*)"", Username ""([^""]*)"" and Email ""([^""]*)"", Password ""([^""]*)"", and ConfirmPassword ""([^""]*)""")]
        public void WhenISendARequestToUpdateTheUserWithReferenceIdUsernameAndEmailPasswordAndConfirmPassword(string p0, string updateduser, string p2, string p3, string p4)
        {
            var user = new UpdateUserRequest { ReferenceId = p0, Username = updateduser, Email = p2, Password = p3, ConfirmPassword = p4 };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response =  _httpClient.PutAsync($"{BaseUrl}/account/user", content).Result;
            Assert.Equals(HttpStatusCode.OK, _response.StatusCode);
        }

        [Given(@"I have an existing user with ReferenceId ""([^""]*)""")]
        public void GivenIHaveAnExistingUserWithReferenceId(string p0)
        {
            var response = _httpClient.GetAsync($"{BaseUrl}/account/get-User/{p0}").Result;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"I send a request to deactivate the user")]
        public void WhenISendARequestToDeactivateTheUser()
        {
            _response = _httpClient.GetAsync($"{BaseUrl}account/{ReferenceId}/deactivate-User").Result;
            _responseContent = _response.Content.ReadAsStringAsync().Result;
            Assert.That(string.IsNullOrEmpty(_responseContent));
        }

        [Given(@"I have a user in database")]
        public void GivenIHaveAUserInDatabase(Table table)
        {
            var row = table.Rows[0];

            var response = _httpClient.GetAsync($"{BaseUrl}/account/get-User").Result;
            _responseContent = response.Content.ReadAsStringAsync().Result;

            var resuser = JsonConvert.DeserializeObject<List<UserDto>>(_responseContent);
            foreach (var item in resuser)
            {
                Assert.Equals(row["UserName"], item.Username);
                Assert.Equals(row["Email"], item.Email);
                Assert.Equals(row["Password"], item.Password);
                Assert.Equals(row["ReferenceId"], item.ReferenceId);
            }

            Assert.That(string.IsNullOrEmpty(_responseContent));
        }

        [Given(@"I have a user data")]
        public void GivenIHaveAUserData(Table table)
        {
            var row = table.Rows[0];
            user = new UserRequest();
            user.Username = row["UserName"];
            user.Email = row["Email"];
            user.Password = row["Password"];
            user.ConfirmPassword = row["ConfirmPassword"];

            var res = _userService.GetAllUsersAsync().Result;
            var chk = res.FirstOrDefault(c => c.Username.ToLower() == row["UserName"].ToLower());
            if (chk != null)
            {
                Assert.Equals(row["UserName"], chk.Username);
                Console.WriteLine("User already exist");
            }
            else
            {
                var result = _userService.CreateUser(user).Result;
                Console.WriteLine(result);
            }
            
        }

        [When(@"I call Create user API")]
        public void WhenICallCreateUserAPI()
        {
            var Createcustomer = _httpClient.PostAsync($"{BaseUrl}/account/User", user).Result;
            _response = Createcustomer;
            _responseContent = Createcustomer.Content.ReadAsStringAsync().Result;
            Assert.That(string.IsNullOrEmpty(_responseContent));
        }

        [Then(@"I will receive an error code (.*)")]
        public void ThenIWillReceiveAnErrorCode(int p0)
        {
            Assert.Equals(p0, (int)_response.StatusCode);
        }

        [Then(@"Response message should contain ""([^""]*)""")]
        public void ThenResponseMessageShouldContain(string p0)
        {
            Assert.Equals(p0, _response.StatusCode.ToString());
        }

        [Then(@"I should receive an error code (.*)")]
        public void ThenIShouldReceiveAnErrorCode(int p0)
        {
            Assert.Equals(p0, (int)_response.StatusCode);
        }
    }
}
