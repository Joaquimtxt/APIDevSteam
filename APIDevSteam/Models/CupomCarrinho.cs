namespace APIDevSteam.Models
{
    public class CupomCarrinho
    {
        public Guid CupomCarrinhoId { get; set; }
        //Relacionamento
        public Guid? CarrinhoId { get; set; }
        public Carrinho? Carrinho { get; set; }
        public Guid? CupomId { get; set; }
        public CupomCarrinho? Cupom { get; set; }



    }
}
