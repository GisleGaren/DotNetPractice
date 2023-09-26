using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using Microsoft.EntityFrameworkCore;
using MyShop.DAL;

namespace MyShop.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IActionResult> Table()
    {
        var customers = await _customerRepository.GetAll();
        return View(customers);
    }

}
