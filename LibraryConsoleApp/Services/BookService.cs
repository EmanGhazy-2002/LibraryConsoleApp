using LibraryConsoleApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryConsoleApp.Services
{
    internal class BookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public void AddBookWithAuthor(string bookTitle, int publishedYear, string authorName)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Name == authorName);
            var newBook = new Book { Title = bookTitle, PublishedYear = publishedYear, Author = author };
            _context.Books.Add(newBook);
            int affectedRows = _context.SaveChanges();
            Console.WriteLine($"Book '{newBook.Title}' added successfully (Id: {newBook.Id}).");
        }

        public void AddMultipleBooks(List<Book> books)
        {
            _context.Books.AddRange(books);
            int affectedRows = _context.SaveChanges();
            Console.WriteLine($"Added {books.Count} books successfully.");
            foreach (var book in books)
            {
                string authorName = book.Author != null ? book.Author.Name : $"(AuthorId: {book.AuthorId})";
                Console.WriteLine($"Book: '{book.Title}' (Id: {book.Id}), Author: {authorName}");
            }
        }

        public bool UpdateBookTitle(int bookId, string newTitle)
        {
            var bookToUpdate = _context.Books.Find(bookId);
            bookToUpdate.Title = newTitle;
            int affect = _context.SaveChanges();
            Console.WriteLine($"Updated book ID {bookId} title to {newTitle}, Changes: {affect}");
            return true;
        }

        public void DeleteBooksByAuthor(int authorId)
        {
            var booksToDelete = _context.Books.Where(b => b.AuthorId == authorId).ToList();
            _context.Books.RemoveRange(booksToDelete);
            int affectedRows = _context.SaveChanges();
            Console.WriteLine($"Deleted {booksToDelete.Count} books.");
        }

        public List<Book> GetBooksWithAuthorsEagerLoading()
        {
            return _context.Books.Include(book => book.Author).ToList();
        }

        public List<Book> GetBooksOnlyLazyLoadingDemo()
        {
            return _context.Books.ToList();
        }

        public List<Book> GetBooksReadOnlyNoTracking()
        {
            return _context.Books.Include(b => b.Author).AsNoTracking().ToList();
        }

        public void UpdateBookAuthorUsingAttach(int bookId, string newTitle)
        {
            var bookToUpdate = _context.Books.Local.FirstOrDefault(b => b.Id == bookId) ?? new Book { Id = bookId, Title = newTitle };
            _context.Books.Attach(bookToUpdate);
            _context.Entry(bookToUpdate).Property(b => b.Title).IsModified = true;
            int changes = _context.SaveChanges();
            Console.WriteLine($"Updated book ID {bookId} title to {newTitle} using Attach, Changes: {changes}");
        }

        public List<(int Id, string Title, int PublishedYear, string AuthorName)> GetBooksByAuthorId(int authorId)
        {
            var parameter = new SqlParameter("@AuthorId", authorId);
            return _context.Database
                .SqlQueryRaw<(int Id, string Title, int PublishedYear, string AuthorName)>(
                    "EXEC GetBooksByAuthorId @AuthorId", parameter)
                .ToList();
        }
    }
}