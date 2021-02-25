using Shared.LazyLoading.Lazy;
using System;

namespace Shared.Domain.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public virtual string Name { get; set; }
        public virtual string ShippingAddress { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }

        // private byte[] profilePicture; 
        // public IValueHolder<byte[]> ProfilePictureValueHolder { get; set; }

        //VirtualProxy example
        public virtual byte[] ProfilePicture
        {
            get
            {
                // Lazy loading pattern. Tightly coupled solution
                //if (profilePicture == null)
                //    profilePicture = ProfilePictureService.GetPictureFor(Name);
                // return profilePicture;

                // ValueHolder example
                // ProfilePictureValueHolder constructor initializes the Func used to load the picture
                // return ProfilePictureValueHolder.GetValue(Name);
                return ProfilePicture;
            }
            set
            {
            }
        }

        public Customer()
        {
            CustomerId = Guid.NewGuid();
        }
    }
}
