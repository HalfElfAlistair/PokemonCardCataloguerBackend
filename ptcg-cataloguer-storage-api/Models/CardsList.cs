namespace Cataloguer.Models;

public class CardsList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ListId { get; set; } = Guid.NewGuid();
    public bool IsFavourites { get; set; }
    public string ListName { get; set; } = "";
    public List<string> CardIDs { get; set; } = new();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}