using AzurePractice.Application;
using AzurePractice.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AzurePractice.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetCustomers()
    {
        var customers = await _customerService.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        var createdCustomer =
            await _customerService.CreateAsync(customer);

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = createdCustomer.Id },
            createdCustomer);
    }
}