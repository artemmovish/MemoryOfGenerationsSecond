using Infastructure.Context;
using Infastructure.Services;
using Entity.Models;
using Entity.Enums;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        // Настройка контекста базы данных
        var context = new AppDbContext();

        // Создание сервисов
        var authorService = new AuthorService(context);
        var bookService = new BookService(context);

        Console.WriteLine("Начало добавления тестовых данных...");

        try
        {
            // Общая ссылка на фото для всех авторов
            string commonPhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/7/76/Hugo2.jpg";

            // Добавление авторов
            var author1 = new Author
            {
                Name = "Лев Толстой",
                Biography = "Русский писатель, мыслитель, философ и публицист.",
                BirthDate = new DateTime(1828, 9, 9),
                DeathDate = new DateTime(1910, 11, 20),
                InterestingFacts = "Был номинирован на Нобелевскую премию по литературе.",
                PhotoPath = commonPhotoUrl
            };

            var author2 = new Author
            {
                Name = "Фёдор Достоевский",
                Biography = "Русский писатель, мыслитель, философ и публицист.",
                BirthDate = new DateTime(1821, 11, 11),
                DeathDate = new DateTime(1881, 2, 9),
                InterestingFacts = "Пережил инсценировку казни и каторгу.",
                PhotoPath = commonPhotoUrl
            };

            var author3 = new Author
            {
                Name = "Антон Чехов",
                Biography = "Русский писатель, прозаик, драматург.",
                BirthDate = new DateTime(1860, 1, 29),
                DeathDate = new DateTime(1904, 7, 15),
                InterestingFacts = "По профессии был врачом.",
                PhotoPath = commonPhotoUrl
            };

            await authorService.AddAuthorAsync(author1);
            await authorService.AddAuthorAsync(author2);
            await authorService.AddAuthorAsync(author3);

            Console.WriteLine("Авторы добавлены успешно.");

            // Общая ссылка на обложки книг
            string commonCoverUrl = "https://upload.wikimedia.org/wikipedia/commons/7/76/Hugo2.jpg";
            // Общий путь к файлу книги
            string commonBookFilePath = "E:\\Загрузки\\Telegram Download\\Маленький принц.epub";

            // Добавление книг для авторов
            var book1 = new Book
            {
                Title = "Война и мир",
                AuthorId = author1.Id,
                Description = "Роман-эпопея, описывающий русское общество в эпоху войн против Наполеона.",
                Genre = Genre.Роман,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            var book2 = new Book
            {
                Title = "Анна Каренина",
                AuthorId = author1.Id,
                Description = "Роман о трагической любви замужней женщины.",
                Genre = Genre.Роман,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            var book3 = new Book
            {
                Title = "Преступление и наказание",
                AuthorId = author2.Id,
                Description = "Роман о бывшем студенте, совершившем убийство.",
                Genre = Genre.Роман,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            var book4 = new Book
            {
                Title = "Братья Карамазовы",
                AuthorId = author2.Id,
                Description = "Последний роман Достоевского, философская драма.",
                Genre = Genre.Роман,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            var book5 = new Book
            {
                Title = "Вишнёвый сад",
                AuthorId = author3.Id,
                Description = "Пьеса о вынужденной продаже родового имения.",
                Genre = Genre.Драма,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            var book6 = new Book
            {
                Title = "Чайка",
                AuthorId = author3.Id,
                Description = "Пьеса о сложных взаимоотношениях между людьми искусства.",
                Genre = Genre.Драма,
                BookFilePath = commonBookFilePath,
                CoverImagePath = commonCoverUrl
            };

            await bookService.AddBookAsync(book1);
            await bookService.AddBookAsync(book2);
            await bookService.AddBookAsync(book3);
            await bookService.AddBookAsync(book4);
            await bookService.AddBookAsync(book5);
            await bookService.AddBookAsync(book6);

            Console.WriteLine("Книги добавлены успешно.");
            Console.WriteLine("Тестовые данные успешно добавлены в базу данных.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}