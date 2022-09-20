using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Services;

public class PizzaContext : DbContext
{       

    public PizzaContext()
    {
    }

    public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
    {
    }

    public DbSet<PizzaEntity> Pizzas { get;  set; } = null!;
}

[Table("pizzas")]
public class PizzaEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("is_gluten_free")]
    public bool IsGlutenFree { get; set; }
}
