using System.Runtime.Serialization;

namespace CustomersOrders.Models
{
    public class CustomerAdd
    {

        [DataMember]
        public virtual string FirstName { get; set; }

        [DataMember]
        public virtual string LastName { get; set; }

        [DataMember]
        public virtual string Email { get; set; }
    }
}
