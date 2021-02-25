using Shared.Domain.Models;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.LazyLoading.VirtualProxy
{
    public class CustomerProxy : Customer
    {
        public override byte[] ProfilePicture { 
            get
            {
                if (base.ProfilePicture == null)
                {
                    base.ProfilePicture = ProfilePictureService.GetPictureFor(Name);
                }
                return base.ProfilePicture;
            }
        }
    }
}
