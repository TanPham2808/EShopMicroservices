using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Các table trong database
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        /* ApplyConfigurationsFromAssembly là một phương thức của ModelBuilder. 
         * Nó sẽ tìm và áp dụng tất cả các cấu hình từ assembly hiện tại 
         * (assembly là một khái niệm trong .NET, đại diện cho một tập hợp các mã đã biên dịch, như một file DLL).*/

        /* Dòng này sẽ tìm kiếm và áp dụng tất cả các lớp cấu hình thực thể (implement IEntityTypeConfiguration<TEntity>) trong assembly hiện tại. 
         * Các lớp cấu hình này định nghĩa chi tiết về các thực thể như bảng nào sẽ được tạo ra, các cột, khóa chính, khóa ngoại, chỉ mục, v.v*/
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
