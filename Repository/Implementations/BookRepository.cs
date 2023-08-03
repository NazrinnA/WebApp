using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApplication2.DataAccess;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Repository.Implementations
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly IAuthorRepository _authorrepo;
        private readonly IAuthorBookRepository _authorbookrepo;
        public BookRepository(AppDb db, IAuthorRepository authorrepo, IAuthorBookRepository authorbookrepo) : base(db)
        {
            _authorrepo = authorrepo;
            _authorbookrepo = authorbookrepo;
        }

        public async Task Delete(int id)
        {
            var exbook = await Get(b=>b.Id==id);
            exbook.IsActive = false;
            await Update(exbook);
            await Save();
        }

        public async Task UpdateBook(int id, BookUpdateDto updateDto)
        {
            var book =  await Get( b => b.Id == id, "AuthorBooks");
            if (book is null) throw new Exception("Not found");
            if (updateDto.AuthorsId.Count == 0) throw new Exception("Not found");
            book.Price = updateDto.Price;
            book.IsActive = updateDto.IsActive;
            book.Name= updateDto.Name;
            book.AuthorBooks.Clear();
            foreach (var item in updateDto.AuthorsId)
            {
                var aut = await _authorrepo.Get(a => a.Id == item);
                if (aut is null) throw new Exception("notfound");
                AuthorBook autb = new AuthorBook
                {
                    Author = aut,
                    Book = book
                };
              await _authorbookrepo.Create(autb);
            }
            await Update(book);
        }
    }
}
