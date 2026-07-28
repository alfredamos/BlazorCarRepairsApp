using BlazorCarRepairsApp.Dto.Customers;
using BlazorCarRepairsApp.Models;

namespace BlazorCarRepairsApp.Mappers;

public static class CustomerMapper
{
    public static Customer MapCustomerCreateDtoToCustomer(CustomerCreateDto dto)
    {
        return new Customer
        {
            Address = dto.Address,
            Active = dto.Active,
            Notes = dto.Notes,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public static Customer MapCustomerEditDtoToCustomer(CustomerEditDto dto, Customer customer, DateTime createdAt)
    {
        customer.Id = dto.Id;
        customer.Address = dto.Address;
        customer.Active = dto.Active;
        customer.Notes = dto.Notes;
        customer.UserId = (Guid)dto.UserId!;
        customer.CreatedAt = createdAt;
        customer.UpdatedAt = DateTime.UtcNow;

        return customer;

    }
    
    public static CustomerResponse MapCustomerToCustomerResponse(Customer cst)
    {
        return new CustomerResponse
        {
            Id = cst.Id,
            Birthdate = cst.User?.Birthdate,
            Name = cst.User?.Name,
            Email = cst.User?.Email,
            Phone = cst.User?.PhoneNumber,
            Image = cst.User?.ImagePath,
            Gender = cst.User?.Gender,
            Address = cst.Address,
            Active = cst.Active,
            Notes = cst.Notes,
            UserId = cst.UserId!,
            CreatedAt = cst.CreatedAt,
            UpdatedAt = cst.UpdatedAt,
            
        };
    }
}