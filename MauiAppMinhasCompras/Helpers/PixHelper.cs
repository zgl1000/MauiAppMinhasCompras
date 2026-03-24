using PixNet.Src;
using pix_payload_generator.net.Models.CobrancaModels;
using System.Globalization;
using pix_payload_generator.net.Models.PayloadModels;

namespace MauiAppMinhasCompras.Helpers
{
    public static class PixHelper
    {
        public static string GerarPayload(double valor, string chave, string nome, string cidade)
        {
            var cobranca = new Cobranca(_chave: chave)
            {
                SolicitacaoPagador = "Minhas Compras",
                Valor = new Valor
                {
                    Original = valor.ToString("F2", CultureInfo.InvariantCulture)
                }
            };

            // ✅ ToPayload + GenerateStringToQrCode é o fluxo correto do pacote
            var payload = cobranca.ToPayload("COMPRAS01", new pix_payload_generator.net.Models.PayloadModels.Merchant(nome, cidade));
            return payload.GenerateStringToQrCode();
        }
    }
}