using System.Collections.ObjectModel;

namespace APICatalogo.Controllers.Models;

public class Categoria
{
    public Categoria() 
    { 
        Produtos = new Collection<Produto>();
    }

    public int CategoriaID { get; set; }
    public string? Nome { get; set; }
    public string? ImagemURL { get; set; }

    // Definando que Categoria possuí uma coleção de objeto Produto
    public ICollection<Produto>? Produtos { get; set; }
}
