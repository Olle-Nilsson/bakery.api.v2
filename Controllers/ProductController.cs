using Bakery.Interfaces;
using Bakery.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace Bakery.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpPost()]
    public async Task<IActionResult> AddProduct(ProductPostViewModel model)
    {
        try
        {
            if (await _unitOfWork.ProductRepository.Add(model))
            {
                if (await _unitOfWork.Complete())
                {
                    return StatusCode(201);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> ListAllProducts()
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.ProductRepository.List() });
        }
        catch (Exception ex)
        {
            return NotFound($"We did not find anything {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindProduct(int id)
    {
        try
        {
            return Ok(new { succes = true, data = await _unitOfWork.ProductRepository.Find(id) });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePrice(int id, [FromQuery] decimal newPrice)
    {
        try
        {
            if (await _unitOfWork.ProductRepository.Update(id, newPrice))
            {
                if (_unitOfWork.HasChanges())
                {
                    await _unitOfWork.Complete();
                    return NoContent();
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return StatusCode(500);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> PurchaseHistory()
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.ProductRepository.History() });
        }
        catch (Exception ex)
        {
            return NotFound($"We did not find anything {ex.Message}");
        }
    }
}
