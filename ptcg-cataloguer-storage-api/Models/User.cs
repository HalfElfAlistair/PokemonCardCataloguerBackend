namespace Cataloguer.Models;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public List<Card> Cards { get; set; } = new();
    public List<CardsList> Lists { get; set; } = new();
}