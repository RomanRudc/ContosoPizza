using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using ContosoPizza.Models;

namespace ContosoPizza.Services;

public class PizzaContext : DbContext
{       
    // public string dbPath = Path.GetFullPath("InventoryManagement.db");

    public PizzaContext()
    {
    }

    public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
    {
    }

    public DbSet<PizzaEntity> Pizzas { get;  set; } = null!;

    [Table("pizzas")]
    public class PizzaEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("is_gluten_free")]
        public bool IsGlutenFree { get; set; }
    }      
}
