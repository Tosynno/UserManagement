using Application.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.IntegrationTest.StepDefinitions
{
    [Binding]
    public class UserManagementSteps
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private string _userId;

        public UserManagementSteps()
        {
            // Assume we have a TestServer or HttpClientFactory setup to get HttpClient instance
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api") };
        }

        [Given(@"I have a new user with Id ""(.*)"", Username ""(.*)"", Email ""(.*)"", Password ""(.*)"", and ConfirmPassword ""(.*)""")]
        public void GivenIHaveANewUser(string id, string username, string email, string password, string confirmPassword)
        {
            _userId = id;
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
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("/users", content);
        }

        [Then(@"the response should be created")]
        public void ThenTheResponseShouldBeCreated()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Then(@"the user should be retrievable by Id ""(.*)""")]
        public async Task ThenTheUserShouldBeRetrievableById(string id)
        {
            var response = await _client.GetAsync($"/users/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Given(@"I have an existing user with Id ""(.*)""")]
        public void GivenIHaveAnExistingUserWithId(string id)
        {
            // Assuming the user already exists in the database for testing
            _userId = id;
        }

        [When(@"I send a request to update the user with Username ""(.*)"" and Email ""(.*)""")]
        public async Task WhenISendARequestToUpdateTheUser(string username, string email)
        {
            var user = new { Username = username, Email = email };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response = await _client.PutAsync($"/users/{_userId}", content);
        }

        [Then(@"the response should be no content")]
        public void ThenTheResponseShouldBeNoContent()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [When(@"I send a request to deactivate the user")]
        public async Task WhenISendARequestToDeactivateTheUser()
        {
            _response = await _client.DeleteAsync($"/users/{_userId}");
        }

        [Given(@"I have a user in database")]
        public void GivenIHaveAUserInDatabase(Table table)
        {
            // Mocking or setup database state
        }

        [When(@"I call Create user API")]
        public async Task WhenICallCreateUserAPI()
        {
            var user = ScenarioContext.Current["User"];
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("/users", content);
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
