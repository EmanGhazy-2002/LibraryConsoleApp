using LibraryConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LibraryConsoleApp.Services
{
    internal class AuthorService
    {
        private readonly LibraryDbContext _context;

        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }

        // Add
        public Author AddAuthor(string name)
        {
            var newAuthor = new Author { Name = name };
            _context.Authors.Add(newAuthor);
            int affect = _context.SaveChanges();
            Console.WriteLine($"Added author: {newAuthor.Name}, Changes: {affect}");
            return newAuthor;
        }

        // Update
        public bool UpdateAuthorName(int authorId, string newName)
        {
            var authorToUpdate = _context.Authors.Find(authorId);
            authorToUpdate.Name = newName;
            int affect = _context.SaveChanges();
            Console.WriteLine($"Updated author ID {authorId} name to {newName}, Changes: {affect}");
            return true;
        }

        // Read
        public void ShowAuthors()
        {
            foreach (var author in _context.Authors)
            {
                Console.WriteLine($"Author Id: {author.Id}, Author Name: {author.Name}");
            }
        }
    }
}