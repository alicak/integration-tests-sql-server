using System.Net.Http;
using NUnit.Framework;

namespace IntegrationTests.Tests
{
    [SetUpFixture]
    public class TestConfig
    {
        public static TestDatabase TestDatabase {get; private set; }

        public static HttpClient HttpClient { get; private set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            var testApplicationFactory = new TestApplicationFactory();
            HttpClient = testApplicationFactory.CreateClient();
            TestDatabase = testApplicationFactory.TestDatabase;
        }
    }
}
