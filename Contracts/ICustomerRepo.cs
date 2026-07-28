using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Customers;
using CustomerResponse = BlazorCarRepairsApp.Dto.Customers.CustomerResponse;

namespace BlazorCarRepairsApp.Contracts;

public interface ICustomerRepo
{
    Task<ResponseMessage> ChangeCustomerStatus(Guid id);
    Task<ResponseMessage> CreateCustomer(CustomerCreateDto customer);
    Task<ResponseMessage> DeleteCustomerById(Guid id);
    Task<ResponseMessage> EditCustomerById(Guid id, CustomerEditDto customer);
    Task<CustomerResponse> GetCustomerById(Guid id);
    Task<List<CustomerResponse>> GetCustomers(string? searchItem = "");
    Task<List<CustomerResponse>> GetActiveCustomers(string? searchItem = "");
    
    Task<CustomerResponse> GetCustomerByUserId(Guid userId);
    Task<List<CustomerResponse>> GetInactiveCustomers(string? searchItem = "");
}