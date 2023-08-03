using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApplication2.DataAccess;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _repo;
        private readonly IAuthorRepository _aut;
        private readonly IAuthorBookRepository _autbook;
        public BookController(IMapper mapper, IBookRepository repo, IAuthorRepository aut, IAuthorBookRepository autbook)
        {
            _mapper = mapper;
            _repo = repo;
            _aut = aut;
            _autbook = autbook;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            List<Book> books = await _repo.GetAll();
            return StatusCode((int)HttpStatusCode.OK, books);
        }
        [HttpGet]
        [Route("/id")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _repo.Get(b=>b.Id==id);
                return Ok(book);
            }
            catch (Exception)
            {
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto book)
        {
            Book newBook = new Book();
            newBook = _mapper.Map<Book>(book);
            foreach (var item in book.AuthorsId)
            {
                var aut = await _aut.Get(a => a.Id == item);
                if (aut is not null)
                {
                    AuthorBook authorBook = new AuthorBook
                    {
                        Author = aut,
                        Book = newBook
                    };
                    await _autbook.Create(authorBook);
                }
            };
            await _repo.Create(newBook);
            return Ok(newBook);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok();
        }
        [HttpPut]
        [Route("/id")]
        public async Task<IActionResult> Update(int id, BookUpdateDto updateDto)
        {
            await _repo.UpdateBook(id, updateDto);
            return Ok();
        }
        [HttpGet]
        [Route("AuthorBooks/{id}")]
        public async Task<IActionResult> GetBookByAuthorId(int id)
        {
            var authorBooks = await _autbook.GetAll(a => a.AuthorId == id);
            if (authorBooks.Count == 0)
            {
                return NotFound();
            }
            var bookIds = authorBooks.Select(ab => ab.BookId).ToList();
            var books = await _repo.GetAll(b => bookIds.Contains(b.Id));
            return StatusCode((int)HttpStatusCode.OK, books);
        }
    }
}
