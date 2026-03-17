using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private List<Produto> _listaCompletaNaMemoria = new List<Produto>();

    public ListaProduto()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _listaCompletaNaMemoria = await App.Db.GetAll();
        lst_produtos.ItemsSource = _listaCompletaNaMemoria;
    }

    private void ToolbarItem_Adicionar(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlertAsync("Ops", ex.Message, "OK");
		}
    }

    private async void MenuItem_Remover(object sender, EventArgs e)
    {
		try
		{
			var idProduto = (int)((MenuItem)sender).CommandParameter;

            await App.Db.Delete(idProduto);
            await CarregarProdutos();

            await DisplayAlertAsync("Sucesso", "Item removido.", "OK");
        }
		catch (Exception ex)
		{
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string textoDigitado = e.NewTextValue.ToLower();

            if (string.IsNullOrWhiteSpace(textoDigitado))
            {
                lst_produtos.ItemsSource = _listaCompletaNaMemoria;
            }
            else
            {
                var listaFiltrada = _listaCompletaNaMemoria
                            .Where(produto => produto.Descricao.ToLower().Contains(textoDigitado))
                            .ToList();

                lst_produtos.ItemsSource = listaFiltrada;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        
    }

    private async Task CarregarProdutos()
    {
        lst_produtos.ItemsSource = await App.Db.GetAll();
    }

    private async void ToolbarItem_Somar(object sender, EventArgs e)
    {
        try
        {
            double soma = ((List<Produto>)lst_produtos.ItemsSource).Sum(p => p.Total);

            string mensagem = $"O total é {soma:C}";

            await DisplayAlertAsync("Total dos produtos", mensagem, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        
    }
}