using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApplication2.DataAccess;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDb _db;
        private readonly IMapper _mapper;
        public BooksController(AppDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            List<Book> books = await _db.Books.Where(b => b.IsActive).ToListAsync();
            if(books.Count == 0) return NotFound();
            return StatusCode((int)HttpStatusCode.OK, books);
        }
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _db.Books.Where(b => b.Id == id && b.IsActive).FirstOrDefault();
            if (book == null) return NotFound();
            return StatusCode((int)HttpStatusCode.OK, book);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto book)
        {
            Book newBook=new Book();
             newBook = _mapper.Map<Book>(book);
            foreach (var item in book.AuthorsId)
            {
                var aut = await _db.Authors.Where(a => a.Id == item).FirstOrDefaultAsync();
                if (aut is not null)
                {
                    newBook.Authors.Add(aut);
                    aut.Books.Add(newBook);
                }
                _db.Update(aut);
            };
            await _db.AddAsync(newBook);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}
