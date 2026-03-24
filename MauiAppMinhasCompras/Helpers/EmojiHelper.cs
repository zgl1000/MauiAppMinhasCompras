namespace MauiAppMinhasCompras.Helpers
{
    public static class EmojiHelper
    {
        private static readonly Dictionary<string, string> _emojis = new()
        {
            // Carnes e proteínas
            { "carne",        "🥩" },
            { "frango",       "🍗" },
            { "peixe",        "🐟" },
            { "ovo",          "🥚" },
            { "ovos",          "🥚" },
            { "linguiça",     "🌭" },

            // Laticínios
            { "leite",        "🥛" },
            { "queijo",       "🧀" },
            { "manteiga",     "🧈" },
            { "iogurte",      "🍦" },
            { "requeijão",    "🧀" },

            // Frutas
            { "maçã",         "🍎" },
            { "banana",       "🍌" },
            { "uva",          "🍇" },
            { "laranja",      "🍊" },
            { "morango",      "🍓" },

            // Verduras
            { "cenoura",      "🥕" },
            { "tomate",       "🍅" },
            { "alface",       "🥬" },
            { "batata",       "🥔" },
            { "cebola",       "🧅" },

            // Grãos
            { "arroz",        "🌾" },
            { "feijão",       "🫘" },
            { "macarrão",     "🍝" },

            // Padaria
            { "pão",          "🍞" },
            { "bolo",         "🎂" },
            { "biscoito",     "🍪" },

            // Bebidas
            { "água",         "💧" },
            { "suco",         "🧃" },
            { "café",         "☕" },
            { "cerveja",      "🍺" },

            // Limpeza
            { "sabão",        "🧼" },
            { "detergente",   "🧴" },
            { "papel",        "🧻" },

            // Temperos
            { "sal",          "🧂" },
            { "azeite",       "🫙" },
            { "óleo",         "🫙" },
        };

        public static string ObterEmoji(string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao)) return "🛒";

                var palavras = descricao
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (palavras.Length == 0) return "🛒";

                var chave = _emojis.Keys
                    .FirstOrDefault(k => palavras.Contains(k));

                return chave != null ? _emojis[chave] : "🛒";
            }
            catch
            {
                return "🛒";
            }
        }
    }
}