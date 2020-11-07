using FluentAssertions;
using IntegrationTests.Api.Entities;
using NUnit.Framework;
using System.Net.Http;

namespace IntegrationTests.Tests
{
	[TestFixture]
    public class BooksTests
    {
        private readonly TestDatabase testDb = TestConfig.TestDatabase;

        [Test]
        public void GetBook_IsSuccessful()
        {
            testDb.RunDbScript("TestSeed.sql");

        	var response = TestConfig.HttpClient.GetAsync("api/books/1");
        	response.AssertOk<Book>();
        }

        [Test]
        public void GetBook_NonExisting_ReturnsNotFound()
        {
            var response = TestConfig.HttpClient.GetAsync("api/books/12345");
            response.AssertNotFound();
        }

        [Test]
        public void CreateBook_IsSuccessful()
        {
            var book = new Book
            {
                Name = "Island",
                Author = "Aldous Huxley",
                YearPublished = 1962
            };
            var response = TestConfig.HttpClient.PostAsJsonAsync("api/books", book);
            var createdBook = response.AssertCreated<Book>();

            createdBook.Id.Should().BePositive();
            createdBook.Name.Should().Be(book.Name);
            createdBook.Author.Should().Be(book.Author);
            createdBook.YearPublished.Should().Be(book.YearPublished);

            testDb.CleanData();
        }
    }
}
