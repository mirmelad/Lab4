using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Serialization;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections;
using Lab4;
using System.Web.UI;
public class Library
{
    public static List<Book> books;
    private static SQLiteConnection m_dbConn;
    private static SQLiteCommand m_sqlCmd;

    public string ToString(int i)
    {
        if (i >= 0 && i < books.Count)
        {
            return books[i].ToString();
        }
        return "";
    }

    public bool Update(int i,String value)
    {
        String[] fields = value.Split(';');
        if (fields.Length != 6)
        {
            return false;
        }
        books[i].Title = fields[0];
        books[i].Author = fields[1];
        books[i].Genre = fields[2];
        books[i].PublicationDate = Convert.ToDateTime(fields[3]);
        books[i].ISBN = fields[4];
        books[i].Annotation = fields[5];
        return true;
    }

    public bool SaveToXML()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
            StreamWriter writer = new StreamWriter("library.xml");
            serializer.Serialize(writer, books);
            writer.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool LoadFromXML()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Book>));
            TextReader reader = new StreamReader("library.xml");
            books = (List<Book>)serializer.Deserialize(reader);
            reader.Close();
            return (books.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    public bool SaveToJSON()
    {
        try
        {
            string json = JsonSerializer.Serialize(books);
            File.WriteAllText("library.json", json);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool LoadFromJSON()
    {
        try
        {
            string data = File.ReadAllText("library.json");
            books = JsonSerializer.Deserialize<List<Book>>(data);
            return (books.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    private static bool Create_SQLite()
    {
        if (!File.Exists("library.sqlite"))
        {
            SQLiteConnection.CreateFile("library.sqlite");
        }

        try
        {
            m_dbConn = new SQLiteConnection("Data Source=library.sqlite;Version=3;");
            m_dbConn.Open();
            m_sqlCmd = m_dbConn.CreateCommand();
            m_sqlCmd.Connection = m_dbConn;

            m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS library (id integer primary key autoincrement, title varchar(200), author varchar(200), genre varchar(200), pubdate varchar(50), isbn varchar(50), annotation varchar(500) )";
            m_sqlCmd.ExecuteNonQuery();
            m_sqlCmd.CommandText = "DELETE FROM library";
            m_sqlCmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool Connect_SQLite()
    {
        if (!File.Exists("library.sqlite"))
        {
            return false;
        }
        try
        {
            m_dbConn = new SQLiteConnection("Data Source=library.sqlite;Version=3;");
            m_dbConn.Open();
            m_sqlCmd = m_dbConn.CreateCommand();
            m_sqlCmd.Connection = m_dbConn;
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static bool Read_SQLite()
    {
        DataTable table = new DataTable();
        String sqlQuery;

        if (m_dbConn.State != ConnectionState.Open)
        {
            return false;
        }
        books.Clear();
        try
        {
            sqlQuery = "SELECT * FROM library";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    books.Add(new Book(
                        Convert.ToString(row["title"]), 
                        Convert.ToString(row["author"]), 
                        Convert.ToString(row["genre"]), 
                        Convert.ToDateTime(Convert.ToString(row["pubdate"])), 
                        Convert.ToString(row["isbn"]), 
                        Convert.ToString(row["annotation"])));
                }
            }
            else
            {
                return false;
            }
            return (books.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    private static bool Write_SQLite()
    {
        if (m_dbConn.State != ConnectionState.Open)
        {
            return false;
        }
        try
        {
            foreach (Book book in books)
            {
                string str = Convert.ToString(book.PublicationDate);
                Console.WriteLine(str);
                m_sqlCmd.CommandText = "INSERT INTO library ('title','author','genre','pubdate','annotation','isbn') values ('" + book.Title + "','" + book.Author + "','"+book.Genre  + "','" +  Convert.ToString(book.PublicationDate) + "','" + book.Annotation + "','" + book.ISBN + "')";
                m_sqlCmd.ExecuteNonQuery();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool SaveToSQLite()
    {
        if (Create_SQLite())
        {
            return Write_SQLite();
        }
        else
        {
            return false;
        }
    }
    public bool LoadFromSQLite()
    {
        if (Connect_SQLite())
        {
            return Read_SQLite();
        }
        else
        {
            return false;
        }
    }

    public Library()
    {
        books = new List<Book>();
    }

    public bool Add(Book book)
    {
        if (book.Title.Length == 0 || book.Author.Length == 0 || book.Genre.Length == 0)
        {
            return false;
        }
        books.Add(book);
        return true;
    }

    public IEnumerable<Book> Books
    {
        get { return books; }
    }
    public bool Remove(int index)
    {
        if (index < books.Count)
        {
            books.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Clear()
    {
        books.Clear();
    }
    public int Count()
    {
        return books.Count();
    }

    public IEnumerable<BookSearchResult> Search(Predicate<Book> predicate)
    {
        var searchResults = new List<BookSearchResult>();
        var resbook = books.FindAll(predicate);
        foreach (Book book in resbook)
        {
           searchResults.Add(new BookSearchResult(book.Title, book.Author, book.Genre, book.PublicationDate, book.ISBN, book.Annotation, 0));
        }
        return searchResults.AsEnumerable();
    }

    public IEnumerable<BookSearchResult> SearchKeywords(params string[] keywords)
    {
        var searchResults = new List<BookSearchResult>();
        int MatchCount;
        foreach (Book book in Books)
        {
            MatchCount = CountKeywordMatches(book, keywords);
            if (MatchCount > 0)
            {
                searchResults.Add(new BookSearchResult(book.Title, book.Author, book.Genre, book.PublicationDate, book.ISBN, book.Annotation, MatchCount));
            }
        }
        return searchResults.AsEnumerable().OrderByDescending(item => item.MatchCount);//.Where(item => item.MatchCount>0)
    }

    public IEnumerable<BookSearchResult> ReturnAll()
    {
        var searchResults = new List<BookSearchResult>();
        foreach (Book book in Books)
        {
           searchResults.Add(new BookSearchResult(book.Title, book.Author, book.Genre, book.PublicationDate, book.ISBN, book.Annotation, 0));
        }
        return searchResults.AsEnumerable();
    }

    private int CountKeywordMatches(Book book, string[] keywords)
    {
        int count = 0;
        foreach (string keyword in keywords)
        {
            if (book.Title.Contains(keyword))
            {
                count++;
            }
            if (book.Author.Contains(keyword))
            {
                count++;
            }
            if (book.Genre.Contains(keyword))
            {
                count++;
            }
            if (book.Annotation.Contains(keyword))
            {
                count++;
            }
        }
        return count;
    }

    public class BookSearchResult
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; }
        public string Annotation { get; set; }
        public int MatchCount { get; set; }
        public BookSearchResult(string title, string author, string genre, DateTime publicationDate, string isbn, string Annotation, int MatchCount)
        {
            this.Title = title;
            this.Author = author;
            this.Genre = genre;
            this.PublicationDate = publicationDate;
            this.ISBN = isbn;
            this.Annotation = Annotation;
            this.MatchCount = MatchCount;
        }
        public BookSearchResult() { }
        public override string ToString()
        {
            return Title + " | " + Author + " | " + Genre + " | " + PublicationDate + " | " + ISBN;
        }

    }

}
