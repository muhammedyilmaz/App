using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Products
{
    public class Product : Entity
    {
        [Required]
        [StringLength(ProductConsts.MaxBarcodeLength, MinimumLength = ProductConsts.MinBarcodeLength)]
        public string Barcode { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public string Description { get; set; }
    }
}
