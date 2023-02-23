using System.Collections.Generic;

namespace App.Orders.Dto
{
    public class OrderCreatedInput
    {
        public OrderDto Order { get; set; }
        public OrderCustomerDto OrderCustomer { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
