using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAll()
    {
        return this.Ok(await customerService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerModel>> GetById(int id)
    {
        var customer = await customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return this.NotFound();
        }

        return this.Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerModel>> Create(CustomerModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname) ||
            model.BirthDate > DateTime.Now ||
            model.BirthDate.Year < 1900 ||
            model.DiscountValue < 0)
        {
            return this.BadRequest();
        }

        var created = await customerService.AddAsync(model);

        return this.CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CustomerModel model)
    {
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname) ||
            model.BirthDate > DateTime.Now ||
            model.BirthDate.Year < 1900 ||
            model.DiscountValue < 0)
        {
            return this.BadRequest();
        }

        await customerService.UpdateAsync(model);

        return this.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await customerService.DeleteAsync(id);

        return this.NoContent();
    }

    [HttpGet("by-product/{productId:int}")]
    public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomersByProductId(int productId)
    {
        var result = await customerService.GetCustomersByProductIdAsync(productId);

        return this.Ok(result);
    }
}
