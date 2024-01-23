using System.Runtime.Serialization;

namespace CustomersOrders.Models.Customers
{
    [DataContract]
    public class CustomerDTO
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string FirstName { get; set; }

        [DataMember]
        public virtual string LastName { get; set; }

        [DataMember]
        public virtual string Email { get; set; }

        [DataMember]
        public virtual int NumberOfOrders { get; set; }
    }
}
