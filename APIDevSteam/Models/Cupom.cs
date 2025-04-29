namespace APIDevSteam.Models
{
    public class Cupom
    {
        public Guid CupomId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Desconto { get; set; }
        public DateTime? DataValidade { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? LimiteUso { get; set; }

        // Método para decrementar o limite de uso
        public void DecrementarLimiteUso()
        {
            if (LimiteUso.HasValue && LimiteUso.Value > 0)
            {
                LimiteUso--; // Decrementa o limite de uso
            }

            // Desativa o cupom se o limite de uso for atingido
            if (LimiteUso.HasValue && LimiteUso.Value <= 0)
            {
                Ativo = false;
            }
        }

    }
}
