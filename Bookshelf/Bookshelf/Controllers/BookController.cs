using Bookshelf.Models;
using Bookshelf.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Bookshelf.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookRepository _repository;

        
        public BookController()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=1234;Database=bookshelf";

            _repository = new BookRepository(connectionString);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _repository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            var bookId = _repository.Create(book);
            return Ok(bookId);
        }

        [HttpPut]
        public IActionResult Update(Book book)
        {
            _repository.Update(book);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}