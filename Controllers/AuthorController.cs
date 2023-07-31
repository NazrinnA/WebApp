using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net;
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
            author.Name = updateDto.Name;
            author.Surname = updateDto.Surname;
            _db.Update(author);
            await _db.SaveChangesAsync();
            return Ok();
        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var author = await _db.Authors.FindAsync(id);
        //    _db.Remove(author);
        //    await _db.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
