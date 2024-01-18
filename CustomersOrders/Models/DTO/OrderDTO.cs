using CustomersOrders.Classes;
using System.Runtime.Serialization;

namespace CustomersOrders.Models.DTO
{
    [DataContract]
    public class OrderDTO
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
     
        [DataMember]
        public virtual decimal Price { get; set; }
    }
}
