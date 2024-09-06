namespace EvidencijaAparata.Server.DTOs
{
    public record ReturnDTO<T>(IList<T> items, int count_items);
}
