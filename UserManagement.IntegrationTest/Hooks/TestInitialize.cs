using BoDi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.IntegrationTest.Hooks
{
    [Binding]
    public sealed class TestInitialize
    {
        private readonly IServiceProvider _serviceProvider;

        public TestInitialize(IObjectContainer objectContainer)
        {
            _serviceProvider = objectContainer.Resolve<IServiceProvider>();
        }

        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

            //TODO: implement logic that has to run before executing each scenario
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario
        }
        [BeforeScenario]
        public void BeforeScenario()
        {
            var serviceScope = _serviceProvider.CreateScope();
            ScenarioContext.Current.Set(serviceScope, "ServiceScope");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (ScenarioContext.Current.TryGetValue<IServiceScope>("ServiceScope", out var serviceScope))
            {
                serviceScope.Dispose();
            }
        }
    }
}
