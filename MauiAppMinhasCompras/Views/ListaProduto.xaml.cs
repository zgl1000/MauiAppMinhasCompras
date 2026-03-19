using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> _listaCompletaNaMemoria = new ObservableCollection<Produto>();

    public ListaProduto()
	{
		InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            var lista = await App.Db.GetAll();
            _listaCompletaNaMemoria = new ObservableCollection<Produto>(lista);
            lst_produtos.ItemsSource = _listaCompletaNaMemoria;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
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
            var selecionado = sender as MenuItem;
            Produto produto = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlertAsync("Tem certeza?", $"Remover {produto.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(produto.Id);
                _listaCompletaNaMemoria.Remove(produto);
            }
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

                _listaCompletaNaMemoria = new ObservableCollection<Produto>(listaFiltrada);
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
            double soma = ((ObservableCollection<Produto>)lst_produtos.ItemsSource).Sum(p => p.Total);

            string mensagem = $"O total é {soma:C}";

            await DisplayAlertAsync("Total dos produtos", mensagem, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto produto = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = produto
            });
        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}