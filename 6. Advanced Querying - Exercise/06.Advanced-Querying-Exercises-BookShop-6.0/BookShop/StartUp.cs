using BookShop.Data;

namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //string? input = Console.ReadLine();

            RemoveBooks(db);

            //Console.WriteLine(result);
        }




        //Problem 2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction;
            if(Enum.TryParse(command, true, out ageRestriction))
            {
                var books = context.Books.
                            Where(b => b.AgeRestriction == ageRestriction)
                            .OrderBy(b => b.Title)
                            .Select(b => b.Title)                       
                            .ToArray();

                return string.Join(Environment.NewLine, books);
            }

            return null!;
        }

        //Problem  3
        public static string GetGoldenBooks(BookShopContext context)
        {
            var result = context.Books.
                Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title).ToArray();


            //Console.WriteLine(context.Books.
            //    Where(b => b.EditionType.Equals(EditionType.Gold) && b.Copies > 5000)
            //    .OrderBy(b => b.BookId)
            //    .Select(b => b.Title).ToQueryString());

            Console.WriteLine( );

            return string.Join (Environment.NewLine, result);
        }

        //Problem 4 
        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context.Books.
                         Where(b => b.Price > 40)
                         .Select(b => new
                         {
                             b.Title,
                             b.Price
                         })
                         .OrderByDescending(b => b.Price)
                         .ToArray();

            return string.Join(Environment.NewLine, result.Select(b => $"{b.Title} - ${b.Price:F2}"));
        }

        //Problem 5 
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var result = context.Books
                         .Where(b => b.ReleaseDate.HasValue
                                  && b.ReleaseDate.Value.Year != year)
                         .Select(b => b.Title)
                         .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        //Problem 6 
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var result = context.Books
                         .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                         .OrderBy(b => b.Title)
                         .Select(b => b.Title)
                         .ToArray();
       
            return string.Join(Environment.NewLine, result);
        }

        //Problem 7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            
            var result = context.Books
                         .Where(b => b.ReleaseDate.HasValue &&
                                     b.ReleaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                         .OrderByDescending(b => b.ReleaseDate.Value)
                         .Select(b => new
                         {
                             b.Title,
                             b.EditionType,
                             b.Price
                         })
                         .ToArray();

            return string.Join(Environment.NewLine, result.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
        }

        //Problem 8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                          .Where(a => a.FirstName.EndsWith(input))
                          .OrderBy(a => a.FirstName)
                          .ThenBy(a => a.LastName)
                          .Select(a => $"{a.FirstName} {a.LastName}")
                          .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        //Problem 9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                             .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                             .OrderBy(b => b.Title)
                             .Select(b => b.Title)
                             .ToArray();

            return string.Join(Environment.NewLine , bookTitles);
                             
        }

        //Problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var results = context.Authors
                         .Where(a => a.LastName.ToLower().StartsWith(input.ToLower()))
                         .Select(a => new
                         {
                             AuthorName = a.FirstName + " " + a.LastName,
                             Books = a.Books
                                      .OrderBy(b => b.BookId)
                                      .Select(b => b.Title).ToArray()
                         })
                         .ToArray();

            foreach (var result in results)
            {
                foreach(var book in result.Books)
                {
                    sb.AppendLine($"{book} ({result.AuthorName})");
                }
            }

            return sb.ToString();
        }

        //Problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var result = context.Books
                         .Count(b => b.Title.Length > lengthCheck);

            return result;                   
                         
        }

        //Problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var result = context.Authors
                         .Select(a => new
                         {
                             Author = a.FirstName + " " + a.LastName,
                             BookCopies = a.Books.Select(x => x.Copies).Sum()
                         })
                         .ToArray()
                         .OrderByDescending(c => c.BookCopies);

            return string.Join(Environment.NewLine, result.Select(r => $"{r.Author} - {r.BookCopies}"));
        }

        //Problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result = context.Categories
                         .Select(c => new
                         {
                             Category = c.Name,
                             Profit = c.CategoryBooks.Select(cb => cb.Book.Price * cb.Book.Copies).Sum()
                         })
                         .ToArray()
                         .OrderByDescending(r => r.Profit)
                         .ThenBy(r => r.Category);

            return string.Join(Environment.NewLine, result.Select(r => $"{r.Category} ${r.Profit:F2}"));
        }

        //Problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Categories
                         .Select(c => new
                         {
                             Category = c.Name,
                             Books = c.CategoryBooks
                                      .OrderByDescending(b => b.Book.ReleaseDate)
                                      .Select(cb => new
                                      {
                                          BookTitle = cb.Book.Title,
                                          ReleaseDate = cb.Book.ReleaseDate!.Value.Year
                                      })       
                                      .ToArray()
                                     
                         })
                         .OrderBy(c => c.Category)
                         .ToArray();

            foreach(var item in result)
            {
                sb.AppendLine($"--{item.Category}");

                for (int i = 0; i < 3; i++)
                {
                    sb.AppendLine($"{item.Books[i].BookTitle} ({item.Books[i].ReleaseDate})");
                }
            }

            return sb.ToString();
        }

        //Problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                        .Where(b => b.ReleaseDate.HasValue && 
                                    b.ReleaseDate.Value.Year < 2010)
                        .ToArray();

            Array.ForEach(books, b => b.Price = b.Price + 5);
            
            context.BulkUpdate(books);

            
        }

        //Problem 16
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books
                                .Where(b => b.Copies < 4200)
                                .ToArray();




            int deletedBooks = booksToDelete.Count();

            context.BulkDelete(booksToDelete);

           

            
            return deletedBooks;
        }
    }






}




