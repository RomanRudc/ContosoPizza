using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("pizza")]
public class PizzaController : ControllerBase
{
    private readonly PizzaContext _context;
    private readonly ILogger<PizzaController> _logger;

    public PizzaController(PizzaContext context, ILogger<PizzaController> logger) 
    {
        _context = context;
        _logger = logger;
    }
    
    // TODO: instead of ActionResult<T>, use IActionResult, use controller methods
    [HttpGet]
    public async Task<IActionResult> GetPizzas()
        => Ok(await _context.Pizzas.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPizza(int id)
    {
        var pizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == id);
        // var pizza = await _context.Pizzas.FindAsync(id);

        if (pizza == null)
        {
            return NotFound();
        }

        return Ok(pizza);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPizza(int id, PizzaUpdate pizzaUpdate)
    {
        var pizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == id);

        if (pizza == null)
        {
            return NotFound();
        }

        pizza.Name = pizzaUpdate.Name;
        pizza.IsGlutenFree = pizzaUpdate.IsGlutenFree;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> PostPizza(PizzaCreate pizzaCreate)
    {
        var pizza = new PizzaEntity
        {
            Id = pizzaCreate.Id,
            Name = pizzaCreate.Name,
            IsGlutenFree = pizzaCreate.IsGlutenFree
        };

        await _context.Pizzas.AddAsync(pizza);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Pizza update failed");
            return BadRequest(new 
            { 
                Message = "Something went wrong."
            });
        }

        return CreatedAtAction(nameof(GetPizza), new { id = pizza.Id }, pizza);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePizza(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);
        if (pizza == null)
        {
            return NotFound();
        }

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public sealed class PizzaUpdate
{
    public string Name { get; set; } = string.Empty;

    public bool IsGlutenFree { get; set; }
}

public sealed class PizzaCreate
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsGlutenFree { get; set; }
}
