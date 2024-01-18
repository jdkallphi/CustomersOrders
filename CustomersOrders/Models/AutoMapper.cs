using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Models.DTO;

namespace CustomersOrders.Models
{
    public class AutoMapperProfile: Profile
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
