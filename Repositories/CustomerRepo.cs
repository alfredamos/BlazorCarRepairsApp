using System.Net;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Data;
using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Customers;
using BlazorCarRepairsApp.Exceptions;
using BlazorCarRepairsApp.Mappers;
using BlazorCarRepairsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCarRepairsApp.Repositories;

public class CustomerRepo(ApplicationDbContext context) : ICustomerRepo
{
    public Task<ResponseMessage> ChangeCustomerStatus(Guid id)
    {
        throw new NotImplementedException();
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
        existingCustomer.Id = customerEditDto.Id;
        existingCustomer.Address = customerEditDto.Address;
        existingCustomer.Active = customerEditDto.Active;
        existingCustomer.Notes =  customerEditDto.Notes;
        existingCustomer.UserId = customerEditDto.UserId;
        existingCustomer.UpdatedAt = customerEditDto.UpdatedAt;
        
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

    public async Task<List<CustomerResponse>> GetCustomers()
    {
        //----> Fetch all customers.
        var customers = await context.Customers.Include(cst => cst.User).AsNoTracking().ToListAsync();
        
        //----> Send back response.
        return [.. customers.Select(CustomerMapper.MapCustomerToCustomerResponse)];
    }

    public async Task<List<CustomerResponse>> GetActiveCustomers()
    {
        //----> Fetch all customers.
        var query = context.Customers.AsNoTracking();
        var customers = await query.Where(cst => cst.Active.HasValue).ToListAsync();
        
        //----> Send back response.
        return [.. customers.Select(CustomerMapper.MapCustomerToCustomerResponse)];
    }

    public async Task<CustomerResponse> GetCustomerByUserId(Guid userId)
    {
        //----> Fetch the customer with the given id.
        var query = context.Customers.AsNoTracking();
        var customer = await query.Where(cst => cst.UserId.Equals(userId)).FirstOrDefaultAsync();
        
        //----> Check for null customer.
        if (customer is null)
        {
            throw new CustomException("Customer not found", HttpStatusCode.NotFound);
        }
        
        //----> Send back response.
        return CustomerMapper.MapCustomerToCustomerResponse(customer);
    }

    public async Task<List<CustomerResponse>> GetInactiveCustomers()
    {
        //----> Fetch all customers.
        var query = context.Customers.AsNoTracking();
        var customers = await query.Where(cst => !cst.Active.HasValue).ToListAsync();
        
        //----> Send back response.
        return [.. customers.Select(CustomerMapper.MapCustomerToCustomerResponse)];
    }

    private async Task<Customer> GetOneCustomer(Guid id)
    {
        //----> Fetch the customer with giving id.
        var customer = await context.Customers.Include(cst => cst.User).AsNoTracking().FirstOrDefaultAsync(cst => cst.Id.Equals(id));
        
        //----> Send back response.
        return customer ?? throw new CustomException("Customer not found", HttpStatusCode.NotFound);
    }
}