using System;
using System.Collections.Generic;

public class Book
{
    public int id { get; }
    public string title { get; }
    public string author { get; }

    public Book(int getid, string gettitle, string getauthor)
    {
        id = getid;
        title = gettitle;
        author = getauthor;
    }
}

public class Reader
{
    public int id {get;}
    public string name {get;}

    public Reader(int Id, string Name) {
        id = Id;
        name = Name;
    }
}

public abstract class Notifier
{
    public abstract void Notify(string message); 
}

public class SMSNotifier: Notifier
{
    private string _phonenumber;

    public SMSNotifier(string phonenumber)
    {
        _phonenumber = phonenumber;
    }

    public override void Notify(string message)
    {
        Console.WriteLine($"SMSNotifier: SMS to {_phonenumber}: {message}");
    }
}

public class EmailNotifier: Notifier
{
    private string _email;

    public EmailNotifier(string email)
    {
        _email = email;
    }

    public override void Notify(string message)
    {
        Console.WriteLine($"EmailNotifier: Email to {_email}: {message}");
    }
}

public class Library
{
    private List<Book> _books;
    private List<Reader> _readers;
    private Dictionary<Book, (Reader, DateTime)> _loans;
    private List<Notifier> _notifiers;
    public Library()
    {
        _books = new List<Book>();
        _readers = new List<Reader>();
        _loans = new Dictionary<Book, (Reader, DateTime)>();
        _notifiers = new List<Notifier>();
    }

    public void AddNotifier(Notifier notifier)
    {
        _notifiers.Add(notifier);
    }

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public void RegisterReader(Reader reader)
    {
        _readers.Add(reader);
    }

    public void LendBook(int bookid, int readerid)
    {
        Book book = _books.Find(b => b.id == bookid);
        Reader reader = _readers.Find(b => b.id == readerid);
        if (book != null && reader != null && !_loans.ContainsKey(book))
        {
            _loans[book] = (reader, DateTime.Now.AddDays(-300));
            Console.WriteLine($"Книга '{book.title}' выдана читателю {reader.name}.");
        }
        else
        {
            Console.WriteLine($"Не удалось выдать книгу.");
        }
    }

    public void ReturnBook(int bookid)
    {
        Book book = _books.Find(b => b.id == bookid);

        if (book != null && _loans.ContainsKey(book))
        {
            _loans.Remove(book);
            Console.WriteLine($"Книга '{book.title}' возвращена.");
        }
        else
        {
            Console.WriteLine($"Книга не найдена или не была выдана.");
        }
    }

    public void CheckOverdueBooks()
    {
        DateTime now = DateTime.Now;
        foreach (var loan in _loans)
        {
            Book book = loan.Key;
            Reader reader = loan.Value.Item1;
            DateTime loanDate = loan.Value.Item2;
            int difference = (now - loanDate).Days;
            Console.WriteLine($"Разница: {difference}");
            if (difference > 30)
            {
                foreach (var notifier in _notifiers)
                {
                    notifier.Notify($"Книга '{book.title}' просрочена у читателя {reader.name}.");
                }
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        // Создаем библиотеку и добавляем уведомители
        Library library = new Library();
        library.AddNotifier(new SMSNotifier("123-456-7890"));
        library.AddNotifier(new EmailNotifier("librarian@example.com"));

        // Добавляем книги
        Book book1 = new Book(1, "1984", "George Orwell");
        library.AddBook(book1);

        // Регистрируем читателя
        Reader reader1 = new Reader(1, "Alice");
        library.RegisterReader(reader1);

        // Выдаем книгу читателю
        library.LendBook(1, 1);

        // Проверяем просроченные книги (при необходимости можно вручную изменить дату выдачи для теста)
        library.CheckOverdueBooks();

        // Возвращаем книгу
        library.ReturnBook(1);
    }
}