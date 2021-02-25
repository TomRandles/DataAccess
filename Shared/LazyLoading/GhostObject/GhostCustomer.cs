using Shared.Domain.Models;
using Shared.LazyLoading.VirtualProxy;
using System;

namespace Shared.LazyLoading.GhostObject
{
    public enum LoadState
    {
        Ghost,
        Loading,
        Loaded
    };

    public class GhostCustomer : CustomerProxy
    {
        private LoadState loadStatus;
        private readonly Func<Customer> load;

        public bool IsGhost => loadStatus == LoadState.Ghost;

        public bool IsLoaded => loadStatus == LoadState.Loaded;

        // Need a mechanism to load the rest of the data when a property is 
        // accessed.
        // This loading function is injected into the entity constructor
        public GhostCustomer(Func<Customer> load) : base()
        {
            this.load = load;

            // Initialize state - starts at Ghost
            loadStatus = LoadState.Ghost;
        }

        public override string Name
        {
            get
            {
                Load();
                return base.Name;
            }
            set
            {
                Load();
                base.Name = value;
            }
        }

        public override string City
        {
            get
            {
                Load();
                return base.City;
            }
            set
            {
                Load();
                base.City = value;
            }
        }

        public override string Country
        {
            get
            {
                Load();
                return base.Country;
            }
            set
            {
                Load();
                base.Country = value;
            }
        }

        public override string PostalCode
        {
            get
            {
                Load();
                return base.PostalCode;
            }
            set
            {
                Load();
                base.PostalCode = value;
            }
        }

        public override string ShippingAddress 
        {
            get
            {
                Load();
                return base.ShippingAddress;
            }
            set
            {
                Load();
                base.ShippingAddress = value;
            }
        }

        public void Load()
        {
            if (IsGhost)
            {
                loadStatus = LoadState.Loading;
                var customer = load();

                // Initialise ghost object
                base.Name = customer.Name;
                base.City = customer.City;
                base.PostalCode = customer.PostalCode;
                base.ShippingAddress = customer.ShippingAddress;
                base.Country = customer.Country;

                loadStatus = LoadState.Loaded;
            }
        }
    }
}