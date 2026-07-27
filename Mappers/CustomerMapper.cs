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
    
    public static Customer MapCustomerEditDtoToCustomer(CustomerEditDto dto)
    {
        return new Customer
        {
            Id = dto.Id,
            Address = dto.Address,
            Active = dto.Active,
            Notes = dto.Notes,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public static CustomerResponse MapCustomerToCustomerResponse(Customer cst)
    {
        return new CustomerResponse
        {
            Id = cst.Id,
            Name = cst?.User?.Name,
            Email = cst?.User?.Email,
            Phone = cst?.User?.PhoneNumber,
            Image = cst?.User?.ImagePath,
            Gender = cst?.User?.Gender,
            Address = cst?.Address,
            Active = cst?.Active,
            Notes = cst?.Notes,
            UserId = (Guid)cst?.UserId!,
            CreatedAt = cst.CreatedAt,
            UpdatedAt = cst.UpdatedAt,
            
        };
    }
}