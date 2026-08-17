using AzurePractice.Application;
using AzurePractice.Domain;
using AzurePractice.Web.Dtos.Customers;
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
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomers()
    {
        var customers = await _customerService.GetAllAsync();

        var response = customers
            .Select(ToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(customer));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(
        CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };

        var createdCustomer =
            await _customerService.CreateAsync(customer);

        var response = ToResponse(createdCustomer);

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = response.Id },
            response);
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            CreatedUtc = customer.CreatedUtc
        };
    }
}