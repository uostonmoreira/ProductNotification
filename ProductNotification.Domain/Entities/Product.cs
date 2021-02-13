using ProductNotification.Domain.Enumerations;
using ProductNotification.Domain.Interfaces.Mensageria;

namespace ProductNotification.Domain.Entities
{
    public sealed class Product : Base
    {
        private const string _filaNotificacaoProduto = "customerproductnotification";

        public int Codigo { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public SituacaoProduto Situacao { get; set; }

        private void EnviaNotificacao(Product product)
        {
            
        }

        public override bool Valido()
        {
            if (this.Codigo == 0 || string.IsNullOrEmpty(this.Name) || this.Price == 0)
                return false;
            else
                return true;
        }
    }
}