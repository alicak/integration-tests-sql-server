using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

namespace IntegrationTests.Tests
{
    public static class HttpResponseExtensions
    {
        // expects 200 and content
        public static T AssertOk<T>(this Task<HttpResponseMessage> messageTask) => AssertContent<T>(messageTask, HttpStatusCode.OK);

        // expects 201 and content
        public static T AssertCreated<T>(this Task<HttpResponseMessage> messageTask) => AssertContent<T>(messageTask, HttpStatusCode.Created);

        // expects 204 and no content
        public static void AssertNoContent(this Task<HttpResponseMessage> messageTask) => AssertEmpty(messageTask, HttpStatusCode.NoContent);

        // expects 404 and no content
        public static void AssertNotFound(this Task<HttpResponseMessage> messageTask) => AssertEmpty(messageTask, HttpStatusCode.NotFound);

        private static T AssertContent<T>(this Task<HttpResponseMessage> messageTask, HttpStatusCode expectedStatusCode)
        {
            var message = messageTask.Result;

            message.StatusCode.Should().Be(expectedStatusCode);

            return message.Content.ReadAsAsync<T>().Result;
        }

        private static void AssertEmpty(this Task<HttpResponseMessage> messageTask, HttpStatusCode expectedStatusCode)
        {
            var message = messageTask.Result;

            message.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}
