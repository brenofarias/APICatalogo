namespace APICatalogo.Controllers.Models;

public class Produto
{
    public int ProdutoID { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemURL { get; set; }
    public float Estoque { get; set;}
    public DateTime DataCadastro { get; set; }
    
    // Definindo chave estrangeira
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }   

}
