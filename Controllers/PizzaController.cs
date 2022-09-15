using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ContosoPizza.Services.PizzaContext;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("pizza")]
public class PizzaController : ControllerBase
{
    private readonly PizzaContext _context;

    public PizzaController(PizzaContext context) => _context = context;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaEntity>>> GetPizzas()
    {
        return await _context.Pizzas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PizzaEntity>> GetPizza(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);

        if (pizza == null)
        {
            return NotFound();
        }

        return pizza;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPizza(int id, PizzaEntity pizza)
    {
        if (id != pizza.Id)
        {
            return BadRequest();
        }

        _context.Entry(pizza).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PizzaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Pizza>> PostPizza(PizzaEntity pizza)
    {
        _context.Pizzas.Add(pizza);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (PizzaExists(pizza.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetPizza", new { id = pizza.Id }, pizza);
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

    private bool PizzaExists(int id)
    {
        return _context.Pizzas.Any(e => e.Id == id);
    }
}
