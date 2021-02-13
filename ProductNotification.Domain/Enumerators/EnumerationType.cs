using System.ComponentModel;

namespace ProductNotification.Domain.Enumerations
{
    public enum SituacaoProduto
    {
        [Description("Disponível")]
        Disponivel = 1,
        [Description("Esgotado")]
        Esgotado = 2,
        [Description("Aguardando")]
        Aguardando = 3,
        [Description("Cancelado")]
        Cancelado = 4
    }
}
