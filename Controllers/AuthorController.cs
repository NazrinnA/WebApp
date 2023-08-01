using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApplication2.DataAccess;
using WebApplication2.DTO.Author;
using WebApplication2.Entities;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AppDb _db;
        private readonly IMapper _mapper;
        public AuthorController(AppDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            List<Author> authors = await _db.Authors.ToListAsync();
            if (authors.Count == 0) return NotFound();
            return StatusCode((int)HttpStatusCode.OK, authors);
        }
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var author = _db.Authors.Where(b => b.Id == id).FirstOrDefault();
            if (author == null) return NotFound();
            return StatusCode((int)HttpStatusCode.OK, author);
        }
        [HttpGet]
        [Route("authorBooks/id")]
        public IActionResult GetAuthor(int id)
        {
            var authorBooks = _db.AuthorBook.Where(a => a.AuthorId == id).ToList();
            if (authorBooks.Count == 0)
            {
                return NotFound();
            }

            var bookIds = authorBooks.Select(ab => ab.BookId).ToList();
            var books = _db.Books.Where(b => bookIds.Contains(b.Id)).ToList();

            return StatusCode((int)HttpStatusCode.OK, books);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorCreateDto author)
        {
            Author nauthor = _mapper.Map<Author>(author);
            _db.Add(nauthor);
            await _db.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto updateDto)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author is null) return NotFound();
            author.Name = updateDto.Name;
            author.Surname = updateDto.Surname;
            _db.Update(author);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
