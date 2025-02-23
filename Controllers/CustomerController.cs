using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakery.Interfaces;
using Bakery.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace Bakery.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    [HttpPost()]
    public async Task<IActionResult> AddCustomer(CustomerPostViewModel model)
    {
        try
        {
            if (await _unitOfWork.CustomerRepository.Add(model))
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
    public async Task<IActionResult> ListCustomers()
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.CustomerRepository.List() });
        }
        catch (Exception ex)
        {
            return NotFound($"We did not find anything {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindCustomer(int id)
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.CustomerRepository.Find(id) });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpPatch("{id}/contactPerson")]
    public async Task<IActionResult> UpdateContactPerson(int id, [FromQuery] string newContactPerson)
    {
        try
        {
            if (await _unitOfWork.CustomerRepository.Update(id, newContactPerson))
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
}
