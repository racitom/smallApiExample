using Microsoft.AspNetCore.Mvc;
using smallApiExample.Interfaces;
using smallApiExample.Model;

namespace smallApiExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IDatabase _data;

        public CustomerController(ILogger<CustomerController> logger, IDatabase data)
        {
            _logger = logger;
            _data = data;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _data.GetAllCustomers();
                _logger.LogInformation("Get {customerCount} customers.", result.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot get all customers.");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCustomer customer)
        {
            try
            {
                var result = _data.CreateCustomer(customer);

                switch (result.status)
                {
                    case CreateCustomerEnum.Created:
                        _logger.LogInformation("Added new customer. Customer details: {@CustomerData}.", result);
                        return StatusCode(StatusCodes.Status201Created, result.customer);                
                    case CreateCustomerEnum.CustomerExist:
                        _logger.LogWarning("Cannot create new customer. This customer exist in with id: {customerId}. Customer details: {@CustomerData}.", result.customer.Id, result.customer);
                        return BadRequest("Customer exist");
                    case CreateCustomerEnum.Error:
                        _logger.LogWarning("Cannot create new customer. Customer details: {@CustomerData}.", customer);
                        return BadRequest();
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot add customers. Customer details: {@CustomerData}.", customer);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)        
        {
            try
            {
                var result = _data.RemoveCustomer(id);

                if (result)
                {
                    _logger.LogInformation("Removed customer with id: {CustomerId}.", id);
                    return Ok(id);
                }

                _logger.LogWarning("Cannot removed customer with id: {CustomerId}. Customer not exist.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot delete customers. Customer id: {CustomerId}.", id);
                return StatusCode(500);
            }
        }
    }
}