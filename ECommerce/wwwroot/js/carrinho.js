class Carrinho {
    incrementItem(btn) {
        var data = this.getItemData(btn);
        data.Quantidade++;
        this.postItemQuantidade(data);
    }

    decrementItem(btn) {
        var data = this.getItemData(btn);
        data.Quantidade--;
        this.postItemQuantidade(data);
    }

    inputItemQuantidade(input) {
        var data = this.getItemData(input);
        this.postItemQuantidade(data);
    }

    getItemData(elemento) {
        var itemId = $(elemento).parents('[item-id]').attr('item-id');
        var novaQuantidade = $(elemento).parents('[item-id]').find('input').val();

        return {
            Id: itemId,
            Quantidade: novaQuantidade
        }
    }

    postItemQuantidade(data) {
        let token = $('[name=__RequestVerificationToken]').val();

        let headers = {};
        headers['RequestVerificationToken'] = token;


        $.ajax({
            url: '/Loja/UpdateQuantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        }).done(function (response) {
            
            let itemPedido = response.itemPedido;

            if (itemPedido.quantidade == null) {
                return linhaDoItem.remove();
            }

            let linhaDoItem = $('[item-id=' + itemPedido.id + ']')
            linhaDoItem.find('input').val(itemPedido.quantidade);
            linhaDoItem.find('[subtotal]').html((itemPedido.subtotal).duasCasas());
            let carrinhoViewModel = response.carrinhoViewModel;
            $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');
            $('[total]').html((carrinhoViewModel.total).duasCasas());
            debugger;
        })

    }
}

Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}

var carrinho = new Carrinho();