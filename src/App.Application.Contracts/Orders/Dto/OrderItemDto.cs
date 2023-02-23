using App.Application.Services.Dto;

namespace App.Orders.Dto
{
    public class OrderItemDto : EntityDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
