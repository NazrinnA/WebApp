
using AutoMapper;
using WebApplication2.DTO.Author;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;

public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorUpdateDto, Author>();
            CreateMap<BookCreateDto, Book>();
        }
    }

