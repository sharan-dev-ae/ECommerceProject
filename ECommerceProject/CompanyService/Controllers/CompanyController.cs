using CompanyService.Data;
using CompanyService.Interfaces;
using CompanyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly CompanyDbContext _context;
    private readonly ICompanyFactory _factory;

    public CompanyController(CompanyDbContext context, ICompanyFactory factory)
    {
        _context = context;
        _factory = factory;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var companies = await _context.Companies.ToListAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();
        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CompanyCreateRequest request)
    {
        var company = _factory.Create(
            request.Name,
            request.StreetAddress,
            request.City,
            request.State,
            request.PostalAddress,
            request.Zip,
            request.ContactNumber
        );

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Company updatedCompany)
    {
        if (id != updatedCompany.Id) return BadRequest("ID mismatch");

        _context.Entry(updatedCompany).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
