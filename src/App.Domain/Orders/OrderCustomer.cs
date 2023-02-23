using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Orders
{
    public class OrderCustomer : Entity
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(OrderCustomerConsts.MaxNameLength, MinimumLength = OrderCustomerConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(OrderCustomerConsts.MaxAddressLength, MinimumLength = OrderCustomerConsts.MinAddressLength)]
        public string Address { get; set; }
    }
}
