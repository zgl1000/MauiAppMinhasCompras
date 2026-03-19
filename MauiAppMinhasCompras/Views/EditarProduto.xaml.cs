using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var produtoAntigo = BindingContext as Produto;

            Produto produto = new Produto
            {
                Id = produtoAntigo.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            if (string.IsNullOrWhiteSpace(produto.Descricao) || produto.Quantidade <= 0 || produto.Preco <= 0)
            {
                await DisplayAlertAsync("Erro!", "Preencha todos os campos!", "OK");
                return;
            }

            var quantidadeInserido = await App.Db.Update(produto);
            await DisplayAlertAsync("Sucesso!", $"{quantidadeInserido} registro atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}