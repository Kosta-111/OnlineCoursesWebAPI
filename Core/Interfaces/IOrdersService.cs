using Data.Entities;

namespace Core.Interfaces;

public interface IOrdersService
{
    List<Order> GetOrders(string? userId);
    void Add(string? userId);
}
