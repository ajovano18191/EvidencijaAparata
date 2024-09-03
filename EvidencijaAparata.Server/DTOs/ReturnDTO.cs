namespace EvidencijaAparata.Server.DTOs
{
    public record ReturnDTO<T>(IQueryable<T> items, int count_items);
}
