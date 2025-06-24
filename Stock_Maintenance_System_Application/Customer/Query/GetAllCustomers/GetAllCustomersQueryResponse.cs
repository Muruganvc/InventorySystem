namespace Stock_Maintenance_System_Application.Customer.Query.GetAllCustomers;
public record GetAllCustomersQueryResponse(int CustomerId, string CustomerName, string Phone, string? Address);
