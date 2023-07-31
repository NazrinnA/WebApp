using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using WebApplication2.DataAccess;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly AppDb _db;
        private readonly IMapper _mapper;
        public BookController(AppDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            List<Book> books = await _db.Books.Where(b => b.IsActive).ToListAsync();
            if (books.Count == 0) return NotFound();
            return StatusCode((int)HttpStatusCode.OK, books);
        }
        [HttpGet]
        [Route("/id")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = _db.Books.Where(b => b.Id == id && b.IsActive).FirstOrDefault();
                if (book == null) return NotFound();
                return Ok(book);
            }
            catch (Exception ex)
            {


            }
            return BadRequest();
        }
        [HttpGet]
        [Route("authorBooks/id")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var authorbook = _db.AuthorBook.Where(a => a.AuthorId == id);
            if(authorbook == null) return NotFound();
            var books = new List<Book>();
            foreach (var item in authorbook)
            {
                var book =await  _db.Books.Where(b => b.Id == item.BookId).FirstOrDefaultAsync();
                if (book is not null) books.Add(book);
            }
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto book)
        {
            Book newBook = new Book();
            newBook = _mapper.Map<Book>(book);
            foreach (var item in book.AuthorsId)
            {
                var aut = await _db.Authors.Where(a => a.Id == item).FirstOrDefaultAsync();
                if (aut is not null)
                {
                    AuthorBook authorBook = new AuthorBook
                    {
                        Author = aut,
                        Book = newBook
                    };
                    newBook.AuthorBooks.Add(authorBook);
                    aut.AuthorBooks.Add(authorBook); 
                    _db.AuthorBook.Add(authorBook);
                }
            };
            await _db.AddAsync(newBook);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.Where(b => b.Id == id && b.IsActive).FirstOrDefaultAsync();
            if (book is null) return NotFound();
            book.IsActive = false;
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}
