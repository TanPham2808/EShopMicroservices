using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using OrderModel = Ordering.Domain.Models.Order;

namespace Ordering.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Product> Products { get; }
    DbSet<OrderModel> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
