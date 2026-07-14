using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        this.receiptService = receiptService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAll()
    {
        var receipts = await this.receiptService.GetAllAsync();
        return this.Ok(receipts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReceiptModel>> GetById(int id)
    {
        try
        {
            var receipt = await this.receiptService.GetByIdAsync(id);
            return this.Ok(receipt);
        }
        catch (MarketException)
        {
            return this.NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReceiptModel receipt)
    {
        if (receipt == null)
        {
            return this.BadRequest();
        }

        try
        {
            await this.receiptService.AddAsync(receipt);
            return this.Created($"/api/receipts/{receipt.Id}", receipt);
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReceiptModel receipt)
    {
        if (receipt == null || id != receipt.Id)
        {
            return this.BadRequest();
        }

        try
        {
            await this.receiptService.UpdateAsync(receipt);
            return this.NoContent();
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await this.receiptService.DeleteAsync(id);
        }
        catch (MarketException)
        {
            // Tests expect NoContent.
        }

        return this.NoContent();
    }

    [HttpPost("{receiptId:int}/products/{productId:int}")]
    public async Task<IActionResult> AddProduct(
        int receiptId,
        int productId,
        [FromQuery] int quantity)
    {
        try
        {
            await this.receiptService.AddProductAsync(productId, receiptId, quantity);
            return this.Ok();
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }

    [HttpDelete("{receiptId:int}/products/{productId:int}")]
    public async Task<IActionResult> RemoveProduct(
        int receiptId,
        int productId,
        [FromQuery] int quantity)
    {
        try
        {
            await this.receiptService.RemoveProductAsync(productId, receiptId, quantity);
            return this.Ok();
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }

    [HttpGet("{receiptId:int}/details")]
    public async Task<IActionResult> GetDetails(int receiptId)
    {
        try
        {
            var details = await this.receiptService.GetReceiptDetailsAsync(receiptId);
            return this.Ok(details);
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }

    [HttpPost("{receiptId:int}/checkout")]
    public async Task<IActionResult> Checkout(int receiptId)
    {
        try
        {
            await this.receiptService.CheckOutAsync(receiptId);
            return this.Ok();
        }
        catch (MarketException)
        {
            return this.BadRequest();
        }
    }
}
