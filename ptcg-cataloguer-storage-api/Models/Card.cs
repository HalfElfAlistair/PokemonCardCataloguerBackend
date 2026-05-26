namespace Cataloguer.Models;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CardId { get; set; } = "";
    public string CardName { get; set; } = "";
    public int Count { get; set; } = 0;
    public string Illustrator { get; set; } = "";
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}