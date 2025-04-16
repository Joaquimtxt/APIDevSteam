namespace APIDevSteam.Models
{
    public class JogoMidia
    {
        public Guid JogoMidiaId { get; set; }
        public Guid JogoId { get; set; }
        public Jogo? Jogo { get; set; }
        public string Tipo { get; set; }//Tipo da midia (imagem, video)
        public string Url { get; set; }//URL da Mídia
    }
}
