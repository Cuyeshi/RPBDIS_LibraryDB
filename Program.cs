using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using RPBDIS_lab2.Models;
namespace EFCore_LINQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (LibraryDbContext context = new LibraryDbContext())
            {
                bool isRun = true;


                while (isRun)
                {
                    Console.WriteLine("Выберите один из пунктов");
                    Console.WriteLine("1. Выборка всех данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один» – 1 шт.\n" +
                                      "2. Выборка данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один», \n" +
                                      "отфильтрованные по определенному условию, налагающему ограничения на одно или несколько полей – 1 шт.\n" +
                                      "3. Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового \n" +
                                      "результата (min, max, avg, сount или др.) по выбранному полю из таблицы, стоящей в схеме базы данных \n" +
                                      "нас стороне отношения «многие» – 1 шт.\n" +
                                      "4. Выборка данных из двух полей двух таблиц, связанных между собой отношением «один-ко-многим» – 1 шт.\n" +
                                      "5. Выборка данных из двух таблиц, связанных между собой отношением «один-ко-многим» и отфильтрованным " +
                                      "по некоторому условию, налагающему ограничения на значения одного или нескольких полей – 1 шт.\n" +
                                      "6. Вставка данных в таблицы, стоящей на стороне отношения «Один» – 1 шт.\n" +
                                      "7. Вставка данных в таблицы, стоящей на стороне отношения «Многие» – 1 шт.\n" +
                                      "8. Удаление данных из таблицы, стоящей на стороне отношения «Один» – 1 шт.\n" +
                                      "9. Удаление данных из таблицы, стоящей на стороне отношения «Многие» – 1 шт.\n" +
                                      "10. Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных – 1 шт." +
                                      "\n\n" +
                                      "0. ВЫХОД" +
                                      "\n\n\n");

                    switch(Convert.ToInt32(Console.ReadLine()))
                {
                    // Выборка всех данных из таблицы на стороне отношения "один".
                    case 1:
                        {
                            var genres = context.Genres.ToList();
                            foreach (var genre in genres)
                            {
                                Console.WriteLine($"\n{genre.GenreId}: {genre.Name} - {genre.Description}");
                            }

                        }
                        break;

                    // Выборка с условием.
                    case 2:
                        {
                            var filteredGenres = context.Genres.Where(g => g.Name.Contains("фантастика")).ToList();
                            foreach (var genre in filteredGenres)
                            {
                                Console.WriteLine($"\n{genre.GenreId}: {genre.Name} - {genre.Description}");
                            }
                        }
                        break;

                    // Группировка данных и получение итогового значения.
                    case 3:
                        {
                            var booksByGenre = context.Books
                                .GroupBy(b => b.GenreId)
                                .Select(g => new { GenreId = g.Key, BookCount = g.Count() })
                                .ToList();

                            foreach (var group in booksByGenre)
                            {
                                Console.WriteLine($"\nGenreId: {group.GenreId}, Count: {group.BookCount}");
                            }
                        }
                        break;

                    // Выборка данных из двух связанных таблиц.
                    case 4:
                        {
                            var booksWithGenres = context.Books
                                .Select(b => new { b.Title, b.Genre.Name })
                                .ToList();

                            foreach (var item in booksWithGenres)
                            {
                                Console.WriteLine($"\n{item.Title} - {item.Name}");
                            }
                        }
                        break;

                    // Фильтрованная выборка из связанных таблиц.
                    case 5:
                        {
                            var booksWithSpecificGenre = context.Books
                                .Where(b => b.Genre.Name == "Фантастика")
                                .Select(b => new { b.Title, b.Genre.Name })
                                .ToList();

                            foreach (var item in booksWithSpecificGenre)
                            {
                                Console.WriteLine($"\n{item.Title} - {item.Name}");
                            }
                        }
                        break;

                    // Вставка данных в таблицу на стороне отношения "Один".
                    case 6:
                        {
                            var newGenre = new Genre { Name = "Приключения", Description = "Книги о приключениях" };
                            context.Genres.Add(newGenre);
                            context.SaveChanges();
                        }
                        break;

                    // Вставка данных в таблицу на стороне отношения "Многие".
                    case 7:
                        {
                            var newBook = new Book
                            {
                                Title = "Новая книга",
                                Author = "Неизвестный автор",
                                GenreId = 1,
                                PublisherId = 2,
                                Price = 500,
                                PublishYear = 2024
                            };
                            context.Books.Add(newBook);
                            context.SaveChanges();
                        }
                        break;

                    // Удаление данных из таблицы на стороне отношения "Один".
                    case 8:
                        {
                            var genreToDelete = context.Genres.FirstOrDefault(g => g.Name == "Приключения");
                            if (genreToDelete != null)
                            {
                                context.Genres.Remove(genreToDelete);
                                context.SaveChanges();
                            }
                        }
                        break;

                    // Удаление данных из таблицы на стороне отношения "Многие".
                    case 9:
                        {
                            var bookToDelete = context.Books.FirstOrDefault(b => b.Title == "Новая книга");
                            if (bookToDelete != null)
                            {
                                context.Books.Remove(bookToDelete);
                                context.SaveChanges();
                            }
                        }
                        break;

                    // Обновление данных в таблице.
                    case 10:
                        {
                            var bookToUpdate = context.Books.FirstOrDefault(b => b.Title == "Новая книга");
                            if (bookToUpdate != null)
                            {
                                bookToUpdate.Price = 600;
                                context.SaveChanges();
                            }
                        }
                        break;

                    case 0:
                        {
                            isRun = false;
                            Console.WriteLine("-------------------Программа завершена-------------------");
                        }
                        break;

                    default:
                        Console.WriteLine("\n\n!!!ВВЕДИТЕ КОРРЕКТНЫЙ НОМЕР ФУНКЦИИ!!!");
                        break;
                }
                    
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
