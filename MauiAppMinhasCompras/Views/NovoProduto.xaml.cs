using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Produto produto = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text)
			};

			var quantidadeInserido = await App.Db.Insert(produto);
			await DisplayAlertAsync("Sucesso!", $"{quantidadeInserido} registro inserido", "OK");
		}
		catch (Exception ex)
		{
            await DisplayAlertAsync("Ops", ex.Message, "OK");
		}
    }
}