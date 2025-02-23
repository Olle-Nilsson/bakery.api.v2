using Bakery.Interfaces;
using Bakery.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    [HttpPost()]
    public async Task<IActionResult> AddOrder(OrderPostViewModel model)
    {
        try
        {
            if (await _unitOfWork.OrderRepository.Add(model))
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
    public async Task<IActionResult> ListOrders()
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.OrderRepository.List() });
        }
        catch (Exception ex)
        {
            return NotFound($"We did not find anything {ex.Message}");
        }
    }

    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> FindOrder(string orderNumber)
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.OrderRepository.Find(orderNumber) });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("date/{orderDate}")]
    public async Task<IActionResult> FindOrderByDate(string orderDate)
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.OrderRepository.Find(DateTime.Parse(orderDate)) });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }
}
