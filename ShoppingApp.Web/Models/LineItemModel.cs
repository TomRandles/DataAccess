using System;

namespace ShoppingApp.Web.Models
{
    public class LineItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
