using System;
using System.Text.Json.Serialization;
public class Book
{
    public Book(string title, string author, string genre, DateTime publicationDate, string isbn, string Annotation)
    {
        this.Title = title;
        this.Author = author;
        this.Genre = genre;
        this.PublicationDate = publicationDate;
        this.ISBN = isbn;
        this.Annotation = Annotation;
    }
    public Book(){}
    [JsonInclude]
    public string Title { get; set; }
    [JsonInclude]
    public string Author { get; set; }
    [JsonInclude]
    public string Genre { get; set; }
    [JsonInclude]
    public DateTime PublicationDate { get; set; }
    [JsonInclude]
    public string ISBN { get; set; }
    [JsonInclude]
    public string Annotation { get; set; }

    public override string ToString()
    {
        return this.Title + " | " + this.Author + " | " + this.Genre + " | " + this.PublicationDate + " | " + this.ISBN + " | " + this.Annotation;
    }
}
