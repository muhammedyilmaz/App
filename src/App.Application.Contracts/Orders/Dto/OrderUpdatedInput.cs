using System.Collections.Generic;

namespace App.Orders.Dto
{
    public class OrderUpdatedInput
    {
        public int Id { get; set; }
        public OrderCustomerDto OrderCustomer { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
