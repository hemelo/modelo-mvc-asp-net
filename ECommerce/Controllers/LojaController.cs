using ECommerce.Models;
using ECommerce.Models.ViewModel;
using ECommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class LojaController : Controller
    {
        private ProdutoRepository produtoRepository;
        private PedidoRepository pedidoRepository;
        private ItemPedidoRepository itemPedidoRepository;
        private CadastroRepository cadastroRepository;

        public LojaController(ProdutoRepository produtoRepository, PedidoRepository pedidoRepository, ItemPedidoRepository itemPedidoRepository, CadastroRepository cadastroRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.Get());
        }

        public IActionResult Adicionar(string codigo)
        {
            Pedido pedido = pedidoRepository.GetPedido() ?? pedidoRepository.CreateNew();

            if (!string.IsNullOrEmpty(codigo))
            {
                Produto? produto = produtoRepository.Find(codigo);

                if (produto is not null)
                {
                    ItemPedido? item = itemPedidoRepository.Find(produto, pedido);

                    if (item is null)
                    {
                        itemPedidoRepository.CreateNew(produto, pedido);
                    }
                }
            }

            return RedirectToAction("Carrossel");
        }

        public IActionResult Carrinho()
        {
            Pedido pedido = pedidoRepository.GetPedidoCompleto() ?? pedidoRepository.CreateNew();
            CarrinhoViewModel carrinho = new CarrinhoViewModel(pedido.Itens);
            return View(carrinho);
        }

        public IActionResult Cadastro()
        {
            Pedido? pedido = pedidoRepository.GetPedidoCompleto();

            if (pedido is null)
            {
                return RedirectToAction("Carrossel");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                Pedido? pedido = pedidoRepository.GetPedidoComCadastro();

                if (pedido is not null)
                {
                    cadastroRepository.Update(pedido.Cadastro.Id, cadastro);
                    return View(pedido);
                }
                
                return RedirectToAction("Carrossel");
            }

            return RedirectToAction("Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public UpdateItemResponse UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            ItemPedido? itemAtualizado = null; 

            if (itemPedido.Quantidade == 0)
            {
                itemPedidoRepository.Remove(itemPedido);
            }
            else
            {
                itemAtualizado = itemPedidoRepository.UpdateQuantidade(itemPedido);
            }

            Pedido pedido = pedidoRepository.GetPedidoCompleto() ?? pedidoRepository.CreateNew();

            CarrinhoViewModel carrinho = new CarrinhoViewModel(pedido.Itens);

            var response = new UpdateItemResponse(itemAtualizado, carrinho);
            return response;
        }
    }
}
