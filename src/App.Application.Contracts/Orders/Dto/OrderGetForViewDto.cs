using App.Products.Dto;
using System.Collections.Generic;

namespace App.Orders.Dto
{
    public class OrderGetForViewDto
    {
        public OrderDto Order { get; set; }
        public OrderCustomerDto OrderCustomer { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
