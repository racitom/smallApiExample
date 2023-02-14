using smallApiExample.Interfaces;
using smallApiExample.Model;

namespace smallApiExample.Core
{
    public class Database : IDatabase
    {
        private readonly object _lock;
        private readonly IIdGenerator _idGenerator;
        private readonly Dictionary<int, Customer> _customers;

        public Database(IIdGenerator idGenerator) 
        {
            _lock = new();
            _idGenerator = idGenerator;
            _customers = new Dictionary<int, Customer>();            
        }

        public (CreateCustomerEnum status, Customer? customer) CreateCustomer(CreateCustomer createCustomer)
        {
            lock (_lock)
            {
                int id = _idGenerator.Get();
                var existCustomer = _customers.FirstOrDefault(x =>
                    x.Value.FirstName.Equals(createCustomer.FirstName) &&
                    x.Value.SurName.Equals(createCustomer.SurName)).Value;

                if (existCustomer != null)
                    return (CreateCustomerEnum.CustomerExist, existCustomer);

                var newCustomer = new Customer()
                {
                    Id = id,
                    FirstName = createCustomer.FirstName,
                    SurName = createCustomer.SurName
                };

                if (_customers.TryAdd(id, newCustomer))
                    return (CreateCustomerEnum.Created, newCustomer);

                return (CreateCustomerEnum.Error, null);
            }
        }

        public IReadOnlyList<Customer> GetAllCustomers()
        {
            lock (_lock)
            {
                return _customers.Values.ToList();
            }
        }

        public bool RemoveCustomer(int customerId)
        {
            lock (_lock)
            {
                return _customers.Remove(customerId);
            }
        }   
    }
}
