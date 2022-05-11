using Microsoft.EntityFrameworkCore;
using ECommerce.Models;
using Newtonsoft.Json;
using ECommerce.Repositories;

namespace ECommerce
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>().HasKey(entity => entity.Id);
            modelBuilder.Entity<Produto>().HasIndex(entity => entity.Codigo).IsUnique();

            modelBuilder.Entity<ItemPedido>().HasKey(entity => entity.Id);
            modelBuilder.Entity<ItemPedido>().HasOne(entity => entity.Pedido);
            modelBuilder.Entity<ItemPedido>().HasOne(entity => entity.Produto);

            modelBuilder.Entity<Pedido>().HasKey(entity => entity.Id);
            modelBuilder.Entity<Pedido>().HasMany(entity => entity.Itens).WithOne(entity => entity.Pedido);
            modelBuilder.Entity<Pedido>().HasOne(entity => entity.Cadastro).WithOne(entity => entity.Pedido).HasForeignKey<Cadastro>(entity => entity.Id).IsRequired();

            modelBuilder.Entity<Cadastro>().HasKey(entity => entity.Id);
            modelBuilder.Entity<Cadastro>().HasOne(entity => entity.Pedido);
        }
    }

    public static class MigrationManager
    {
        public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    appContext.Database.Migrate();
                    var produtoRepository = scope.ServiceProvider.GetRequiredService<ProdutoRepository>();
                    var json = await File.ReadAllTextAsync("livros.json");
                    var data = JsonConvert.DeserializeObject<List<Livro>>(json);
                    await produtoRepository.Add(data);
                }
            }
            return host;
        }
    }
}
