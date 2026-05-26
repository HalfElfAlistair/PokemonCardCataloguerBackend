namespace Cataloguer.Dtos;

public class CardsListCreateDto
{
    public Guid ListId { get; set; } = Guid.NewGuid();
    public string ListName { get; set; } = "";
    public List<string> CardIDs { get; set; } = [];
}