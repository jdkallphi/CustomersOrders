using AutoMapper;
using CustomersOrders.Models.Customers;
using CustomersOrders.Models.Orders;

namespace CustomersOrders.Models
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<CustomerAdd, Customer>().ForMember(x=>x.Id, opt=>opt.Ignore()).ForMember(x => x.Orders, opt => opt.Ignore()).ForMember(x => x.NumberOfOrders, opt => opt.Ignore());
            CreateMap<OrderAdd, Order>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.CreationDate, opt => opt.Ignore()).ForMember(x => x.IsCancelled, opt => opt.Ignore()).ForMember(x => x.CustomerId, opt => opt.Ignore()).ForMember(x => x.Customer, opt => opt.Ignore());
        }

    }
}
