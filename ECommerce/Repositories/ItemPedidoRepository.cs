using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class ItemPedidoRepository : BaseRepository<ItemPedido>
    {
        public ItemPedidoRepository(ApplicationContext context) : base(context)
        {
        }

        public ItemPedido? Find(Produto produto, Pedido pedido)
        {
            return set.Where(item => item.Produto.Codigo == produto.Codigo && item.Pedido.Id == pedido.Id).SingleOrDefault();
        }

        public ItemPedido? Find(ItemPedido itemPedido)
        {
            return set.Where(item => item.Id == itemPedido.Id).SingleOrDefault();
        }

        public ItemPedido CreateNew(Produto produto, Pedido pedido)
        {
            var itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
            set.Add(itemPedido);
            context.SaveChanges();
            return itemPedido;
        }

        public void Remove(ItemPedido item)
        {
            set.Remove(item);
            context.SaveChanges();
        }

        public ItemPedido? UpdateQuantidade(ItemPedido item)
        {
            ItemPedido? itemDb = Find(item);

            if (itemDb is not null)
            {
                itemDb.Quantidade = item.Quantidade;
                context.SaveChanges();
            }

            return itemDb;
        }
    }
}
