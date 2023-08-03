using System.Linq.Expressions;
using WebApplication2.DTO.Book;
using WebApplication2.Entities;

namespace WebApplication2.Repository.Interfaces
{
    public interface IBookRepository:IRepository<Book>
    {
        Task Delete(int id);
        Task UpdateBook(int id, BookUpdateDto updateDto);

    }
}
