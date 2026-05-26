namespace Cataloguer.Dtos;

public class CardsListDto
{
    public Guid ListId { get; set; } = Guid.NewGuid();
    public string ListName { get; set; } = "";
    public List<string> CardIDs { get; set; } = [];
    public bool IsFavourites { get; set; }
}