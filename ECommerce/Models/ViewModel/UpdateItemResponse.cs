namespace ECommerce.Models.ViewModel
{
    public class UpdateItemResponse
    {
        public ItemPedido? itemPedido { get; }

        public CarrinhoViewModel carrinhoViewModel { get; }

        public UpdateItemResponse(ItemPedido? itemPedido, CarrinhoViewModel carrinhoViewModel)
        {
            this.itemPedido = itemPedido;
            this.carrinhoViewModel = carrinhoViewModel;
        }
    }
}
