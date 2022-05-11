using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>
    {
        private readonly IHttpContextAccessor contextAccessor;
        public PedidoRepository(ApplicationContext context, IHttpContextAccessor httpContext) : base(context)
        {
            this.contextAccessor = httpContext; 
        }

        public Pedido CreateNew()
        { 
            var pedido = new Pedido();
            set.Add(pedido);
            context.SaveChanges();
            SetPedidoId(pedido.Id);
            return pedido;
        }

        public Pedido? GetPedido()
        {
            return set.Where(p => p.Id == GetPedidoId()).SingleOrDefault();
        }

        public Pedido? GetPedidoCompleto()
        {
            return set
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Where(p => p.Id == GetPedidoId())
                .SingleOrDefault();
        }

        public Pedido? GetPedidoComCadastro()
        {
            return set
                .Include(p => p.Cadastro)
                .SingleOrDefault();
        }

        private int? GetPedidoId()
        {
            return this.contextAccessor?.HttpContext?.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            this.contextAccessor?.HttpContext?.Session.SetInt32("pedidoId", pedidoId);
        }
    }
}
