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
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repo;
        public AuthorController(IMapper mapper, IAuthorRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            List<Author> authors = await _repo.GetAll();
            return StatusCode((int)HttpStatusCode.OK, authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _repo.Get(b => b.Id == id);
            return StatusCode((int)HttpStatusCode.OK, author);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorCreateDto author)
        {
            Author nauthor = _mapper.Map<Author>(author);
            await _repo.Create(nauthor);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto updateDto)
        {
            var author = await _repo.Get(b => b.Id == id);
            author.Name = updateDto.Name;
            author.Surname = updateDto.Surname;
            await _repo.Update(author);
            return Ok();
        }
    }
}
