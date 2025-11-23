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
}