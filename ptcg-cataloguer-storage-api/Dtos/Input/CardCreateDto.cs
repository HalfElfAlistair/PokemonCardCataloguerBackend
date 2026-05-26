namespace Cataloguer.Dtos;

public class CardCreateDto
{
    public string CardId { get; set; } = "";
    public string CardName { get; set; } = "";
    public int Count { get; set; } = 0;
    public string Illustrator { get; set; } = "";
}