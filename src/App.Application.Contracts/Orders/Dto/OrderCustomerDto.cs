using App.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace App.Orders.Dto
{
    public class OrderCustomerDto : EntityDto
    {
        [Required]
        [StringLength(OrderCustomerConsts.MaxNameLength, MinimumLength = OrderCustomerConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(OrderCustomerConsts.MaxAddressLength, MinimumLength = OrderCustomerConsts.MinAddressLength)]
        public string Address { get; set; }
    }
}
