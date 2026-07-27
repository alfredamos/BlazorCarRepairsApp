using BlazorCarRepairsApp.Dto;
using BlazorCarRepairsApp.Dto.Customers;

namespace BlazorCarRepairsApp.Contracts;

public interface ICustomerRepo
{
    Task<ResponseMessage> ChangeCustomerStatus(Guid id);
    Task<ResponseMessage> CreateCustomer(CustomerCreateDto customer);
    Task<ResponseMessage> DeleteCustomerById(Guid id);
    Task<ResponseMessage> EditCustomerById(Guid id, CustomerEditDto customer);
    Task<CustomerResponse> GetCustomerById(Guid id);
    Task<List<CustomerResponse>> GetCustomers();
    Task<List<CustomerResponse>> GetActiveCustomers();
    
    Task<CustomerResponse> GetCustomerByUserId(Guid userId);
    Task<List<CustomerResponse>> GetInactiveCustomers();
}