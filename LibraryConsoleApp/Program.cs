using LibraryConsoleApp;
using LibraryConsoleApp.Models;
using LibraryConsoleApp.Services;
using Microsoft.EntityFrameworkCore;




namespace LibraryConsoleApp.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new LibraryDbContext();
            var bookService = new BookService(context);
            var memberService = new MemberService(context);
            var authorService = new AuthorService(context);

            // 1. Add members
            var member1 = memberService.AddMember("Emma Gray");
            memberService.AddMember("Ava Brown");


            // 2. Add authors and books
            authorService.AddAuthor("David");
            authorService.AddAuthor("Ola");
            context.SaveChanges();

            bookService.AddBookWithAuthor("C# Basics", 1988, "David");
            bookService.AddBookWithAuthor("OOP Guide", 1994, "Ola");
            bookService.AddBookWithAuthor("C++ Programming", 2010, "Ola");
            bookService.AddBookWithAuthor("JavaScript", 2000, "Ola");
            bookService.AddBookWithAuthor("CSS", 2011, "Ola");


            // 3. Insert list of books
            var olaBooksId = context.Authors.First(a => a.Name == "Ola").Id;
            var bookList = new List<Book>
            {
                new Book { Title = "C Language", PublishedYear = 1970, AuthorId = olaBooksId },
                new Book { Title = "Java", PublishedYear = 1975, AuthorId = olaBooksId }
            };
            bookService.AddMultipleBooks(bookList);


            // 4. Update objects and 5. Manipulate EntityState
            try
            {
                memberService.UpdateMemberName(member1.Id, "Ahmed Smith");
                var jsBook = context.Books.First(b => b.Title == "JavaScript");
                bookService.UpdateBookTitle(jsBook.Id, "JavaScript Advanced");

                var cssBook = context.Books.First(b => b.Title == "CSS");
                bookService.UpdateBookAuthorUsingAttach(cssBook.Id, "CSS3");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // 6. Delete books by author
            var authorDavidId = context.Authors.First(a => a.Name == "David").Id;
            bookService.DeleteBooksByAuthor(authorDavidId);
        }
    }
}