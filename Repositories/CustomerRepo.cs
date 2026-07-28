using System.Net;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Customers;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using Microsoft.EntityFrameworkCore;
using CustomerResponse = BlazorCarRepairsApp.Dto.Customers.CustomerResponse;

namespace BlazorCarRepairsApp.Repositories;

public class CustomerRepo(ApplicationDbContext context) : ICustomerRepo
{
    public async Task<ResponseMessage> ChangeCustomerStatus(Guid id)
    {
        //----> Get customer from db.
        var customer = await GetOneCustomer(id);
        
        //----> Change customer status.
        customer.Active = !customer.Active;
        customer.UpdatedAt = DateTime.UtcNow;
        
        //----> Update customer.
        context.Update(customer);
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Customer status changed successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> CreateCustomer(CustomerCreateDto customerDto)
    {
        //----> Map customer-create-dto to customer.
        var customer = CustomerMapper.MapCustomerCreateDtoToCustomer(customerDto);
        
        //----> Insert the new customer into db.
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Customer created successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.Created
        };
    }

    public async Task<ResponseMessage> DeleteCustomerById(Guid id)
    {
        //----> Check for existence of customer.
        var customer = await GetOneCustomer(id);
        
        //----> Delete the customer with the given id.
        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Customer deleted successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<ResponseMessage> EditCustomerById(Guid id, CustomerEditDto customerEditDto)
    {
        if (!id.Equals(customerEditDto.Id))
        {
            throw new CustomException("Invalid customer id", HttpStatusCode.BadRequest);
        }
        //----> Check for the existence of customer.
        var existingCustomer = await GetOneCustomer(id);
        
        //----> Map customer-edit-dto to customer.
        existingCustomer =
            CustomerMapper.MapCustomerEditDtoToCustomer(customerEditDto, existingCustomer, existingCustomer.CreatedAt);
        //----> Update the customer info in db.
        context.Customers.Update(existingCustomer);
        await context.SaveChangesAsync();
        
        //----> Send back response.
        return new ResponseMessage
        {
            Message = "Customer edited successfully!",
            Status = "Success",
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<CustomerResponse> GetCustomerById(Guid id)
    {
        //----> Fetch the customer with the giving id.
        var customer = await GetOneCustomer(id);
        
        //----> Send back response.
        return CustomerMapper.MapCustomerToCustomerResponse(customer);
    }

    public async Task<List<CustomerResponse>> GetCustomers(string? searchItem = "")
    {
        var query = context.Customers.Include(cst => cst.User).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            var search = searchItem.Trim().ToLower();
        
            query = query.Where(cst => 
                (cst.Address != null && cst.Address.ToLower().Contains(search)) ||
                (cst.User != null && (
                    (!string.IsNullOrEmpty(cst.User.Name) && cst.User.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.Email) && cst.User.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.PhoneNumber) && cst.User.PhoneNumber.Contains(search)) ||
                    (!string.IsNullOrEmpty(cst.User.Gender) && cst.User.Gender.ToLower().Contains(search))
                ))
            );
        }

        var customers = await query.ToListAsync();

        return [.. customers.Select(CustomerMapper.MapCustomerToCustomerResponse)];
    }

    public async Task<List<CustomerResponse>> GetActiveCustomers(string? searchItem = "")
    {
        //----> Fetch all active customers.
        var customers = (await GetCustomers(searchItem)).Where(cst => cst.Active).ToList();
        
        //----> Send back response.
        return customers;
    }

    public async Task<CustomerResponse> GetCustomerByUserId(Guid userId)
    {
        //----> Fetch the customer with the given id.
        var query = context.Customers.AsNoTracking();
        var customer = await query.Where(cst => cst.UserId.Equals(userId)).FirstOrDefaultAsync();

        //----> Check for null customer and send back response.
        return customer is null ? throw new CustomException("Customer not found", HttpStatusCode.NotFound) :
            CustomerMapper.MapCustomerToCustomerResponse(customer);
    }

    public async Task<List<CustomerResponse>> GetInactiveCustomers(string? searchItem = "")
    {
        //----> Fetch all active customers.
        var customers = (await GetCustomers(searchItem)).Where(cst => !cst.Active).ToList();
        
        //----> Send back response.
        return customers;
    }

    private async Task<Customer> GetOneCustomer(Guid id)
    {
        //----> Fetch the customer with giving id.
        var customer = await context.Customers.Include(cst => cst.User).AsNoTracking().FirstOrDefaultAsync(cst => cst.Id.Equals(id));
        
        //----> Send back response.
        return customer ?? throw new CustomException("Customer not found", HttpStatusCode.NotFound);
    }
}