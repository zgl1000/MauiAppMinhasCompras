using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> _listaCompletaNaMemoria = new ObservableCollection<Produto>();
    private bool _navegando = false;

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

            AtualizarRodape();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK"); // ✅ era DisplayAlertAsyncAsync
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
            DisplayAlertAsync("Ops", ex.Message, "OK"); // ✅ era DisplayAlertAsyncAsync
        }
    }

    private async void MenuItem_Remover(object sender, EventArgs e)
    {
        try
        {
            var selecionado = sender as SwipeItem; // ✅ era MenuItem, agora é SwipeItem
            Produto produto = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlertAsync("Tem certeza?", $"Remover {produto.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(produto.Id);
                _listaCompletaNaMemoria.Remove(produto);
                AtualizarRodape(); // ✅ atualiza total ao remover
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

            refresh_view.IsRefreshing = true;

            if (string.IsNullOrWhiteSpace(textoDigitado))
            {
                lst_produtos.ItemsSource = _listaCompletaNaMemoria;
            }
            else
            {
                var listaFiltrada = _listaCompletaNaMemoria
                    .Where(p => p.Descricao.ToLower().StartsWith(textoDigitado))
                    .ToList();

                lst_produtos.ItemsSource = new ObservableCollection<Produto>(listaFiltrada);
            }

            AtualizarRodape();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            refresh_view.IsRefreshing = false;
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            var lista = await App.Db.GetAll();
            _listaCompletaNaMemoria = new ObservableCollection<Produto>(lista);
            lst_produtos.ItemsSource = _listaCompletaNaMemoria;

            AtualizarRodape();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            refresh_view.IsRefreshing = false;
        }
    }

    private async void lst_produtos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_navegando) return; // ✅ evita loop

        try
        {
            var produto = e.CurrentSelection.FirstOrDefault() as Produto;

            if (produto == null) return;

            _navegando = true;
            lst_produtos.SelectedItem = null;

            await Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = produto
            });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
        finally
        {
            _navegando = false; // ✅ sempre libera a flag
        }
    }

    // ✅ Atualiza lbl_total e lbl_contagem no rodapé automaticamente
    private void AtualizarRodape()
    {
        var lista = _listaCompletaNaMemoria;
        lbl_total.Text = lista.Sum(p => p.Total).ToString("C");
        lbl_contagem.Text = $"{lista.Count} {(lista.Count == 1 ? "item" : "itens")}";
    }

    // ✅ Botão Somar agora só exibe o DisplayAlertAsync com o total atual
    private async void ToolbarItem_Somar(object sender, EventArgs e)
    {
        try
        {
            double total = _listaCompletaNaMemoria.Sum(p => p.Total);
            await Navigation.PushAsync(new Views.PixPage(total));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }
}