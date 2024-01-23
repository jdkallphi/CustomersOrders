using System.Runtime.Serialization;

namespace CustomersOrders.Models.Orders
{
    public class OrderAdd
    {
        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public virtual decimal Price { get; set; }
    }
}
