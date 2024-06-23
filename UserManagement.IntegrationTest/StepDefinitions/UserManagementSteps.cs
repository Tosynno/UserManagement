using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using Castle.Core.Resource;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private const string BaseUrl ="";

        private string ReferenceId;
        private UserDto user;

        public UserManagementSteps(HttpClientService client, IUserService userService)
        {
            _httpClient = client;
            _userService = userService;
        }

        [Given(@"I have a new user with Id ""(.*)"", Username ""(.*)"", Email ""(.*)"", Password ""(.*)"", and ConfirmPassword ""(.*)""")]
        public void GivenIHaveANewUser(string id, string username, string email, string password, string confirmPassword)
        {
            ReferenceId = id;
            var user = new
            {
                Id = id,
                Username = username,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            ScenarioContext.Current["User"] = user;
        }

        [When(@"I send a request to create the user")]
        public async Task WhenISendARequestToCreateTheUser()
        {
            var user = ScenarioContext.Current["User"];
            var Createcustomer = _httpClient.PostAsync($"{BaseUrl}/account/User", user).Result;
            _response = Createcustomer;
            _responseContent = Createcustomer.Content.ReadAsStringAsync().Result;
            Assert.True(string.IsNullOrEmpty(_responseContent));
        }

        [Then(@"the response should be created")]
        public void ThenTheResponseShouldBeCreated()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the user should be retrievable by ReferenceId ""(.*)""")]
        public async Task ThenTheUserShouldBeRetrievableById(string ReferenceId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/account/get-User/{ReferenceId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Given(@"I have an existing user with ReferenceId ""(.*)""")]
        public async void GivenIHaveAnExistingUserWithId(string ReferenceId)
        {
            user = new UserDto();
            var result = await _userService.GetUserByIdAsync(ReferenceId);
            ReferenceId = result.ReferenceId;
            user = result;
        }

        [When(@"I send a request to update the user with ReferenceId ""(.*)"", Username ""(.*)"", Email ""(.*)"", Password ""(.*)"", and ConfirmPassword ""(.*)""")]
        public async Task WhenISendARequestToUpdateTheUser(string referenceId, string username, string email, string password, string confirmPassword)
        {
            var user = new UpdateUserRequest { ReferenceId = referenceId, Username = username, Email = email, Password=password, ConfirmPassword =confirmPassword };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response = await _httpClient.PutAsync($"{BaseUrl}/account/user", content);
            Assert.Equal(HttpStatusCode.OK, _response.StatusCode);
        }

        [Then(@"the response should be no content")]
        public void ThenTheResponseShouldBeNoContent()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [When(@"I send a request to deactivate the user")]
        public async Task WhenISendARequestToDeactivateTheUser()
        {
            _response = await _httpClient.GetAsync($"{BaseUrl}account/{ReferenceId}/deactivate-User");
        }


        [When(@"I call Create user API")]
        public async Task WhenICallCreateUserAPI()
        {
            var user = ScenarioContext.Current["User"];
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response = await _httpClient.PostAsync($"{BaseUrl}/account/User", content);
        }

        [Then(@"I will receive an error code (.*)")]
        public void ThenIWillReceiveAnErrorCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"Response message should contain ""(.*)""")]
        public async Task ThenResponseMessageShouldContain(string message)
        {
            var responseContent = await _response.Content.ReadAsStringAsync();
            responseContent.Should().Contain(message);
        }


        [Then(@"I should receive an error code (.*)")]
        public void ThenIShouldReceiveAnErrorCode(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        
    }
}
