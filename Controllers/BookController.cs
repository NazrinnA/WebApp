﻿using AutoMapper;
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

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto book)
        {
            Book newBook = new Book();
            newBook = _mapper.Map<Book>(book);
            foreach (var item in book.AuthorsId)
            {
                var aut = _db.Authors.Where(a => a.Id == item).First();
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
        [HttpPut]
        [Route("/id")]
        public async Task<IActionResult> Update(int id, BookUpdateDto updateDto)
        {
            var book = _db.Books.Include("AuthorBooks").Where(b => b.Id == id).FirstOrDefault();
            if (book is null) return NotFound();
            if (updateDto.AuthorsId.Count == 0) return BadRequest();
            book.Price = updateDto.Price;
            book.IsActive = updateDto.IsActive;
            book.AuthorBooks.Clear();
            foreach (var item in updateDto.AuthorsId)
            {
                var aut = _db.Authors.Find(item);
                if (aut is null) return NotFound();
                AuthorBook autb = new AuthorBook
                {
                    Author = aut,
                    Book = book
                };
                _db.AuthorBook.Add(autb);
            }
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
