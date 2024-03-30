using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Bebidas', 'bebidas.jpeg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Lanches', 'lanches.jpeg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Sobremesa', 'sobremesa.jpeg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Categorias");
        }
    }
}
