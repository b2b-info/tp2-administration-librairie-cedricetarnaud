using System.Text;

namespace BookStore;

public class Book
{
    public uint Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public Book(uint id, string title, string author, double price, int quantity)
    {
        Id = id;
        Title = title;
        Author = author;
        Price = price;
        Quantity = quantity;
        
    }

    public string ShowDetailsCollum()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Id : {Id}" +
                   "\n");
        sb.Append($"Title : {Title} " +
                   "\n");
        sb.Append($"Author : {Author}" + 
                  "\n");
        sb.Append($"Price : {Price}" +
                   "\n");
        sb.Append($"Quantity : {Quantity}" + 
            "\n");
        return sb.ToString();
    }
    public string ShowDetailsRow()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"[{Id}] ");
        sb.Append($"{Title}, ");
        sb.Append($" {Author}");
        sb.Append($" - {Price} ");
        sb.Append($"({Quantity} in stock)");
        return sb.ToString();
    }
}