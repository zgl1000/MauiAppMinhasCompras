using MauiAppMinhasCompras.Helpers;
using QRCoder;

namespace MauiAppMinhasCompras.Views;

public partial class PixPage : ContentPage
{
    private string _payload;
    private const string ChavePix = "meuemail@hotmail.com";
    private const string NomeRecebedor = "MeuNome";
    private const string Cidade = "MinhaCidade";

    public PixPage(double totalLista)
    {
        InitializeComponent();
        GerarQrCode(totalLista);
    }

    private void GerarQrCode(double valor)
    {
        try
        {
            lbl_valor.Text = valor.ToString("C");
            lbl_chave.Text = ChavePix;

            // Gera o payload PIX
            _payload = PixHelper.GerarPayload(valor, ChavePix, NomeRecebedor, Cidade);

            // Gera a imagem QR Code
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(_payload, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);

            byte[] qrBytes = qrCode.GetGraphic(10);

            img_qrcode.Source = ImageSource.FromStream(() => new MemoryStream(qrBytes));
        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private async void btn_copiar_Clicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(_payload);
        await DisplayAlertAsync("Copiado!", "Código PIX copiado para a área de transferência.", "OK");
    }
}