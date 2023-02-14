using smallApiExample.Model;

namespace smallApiExample.Interfaces
{
    public interface IDatabase
    {
        (CreateCustomerEnum status, Customer? customer) CreateCustomer(CreateCustomer createCustomer);

        bool RemoveCustomer(int customerId);

        IReadOnlyList<Customer> GetAllCustomers();
    }
}
