using App.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace App.Products.Dto
{
    public class ProductDto : EntityDto
    {
        [Required]
        [StringLength(ProductConsts.MaxBarcodeLength, MinimumLength = ProductConsts.MinBarcodeLength)]
        public string Barcode { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxDescriptionLength, MinimumLength = ProductConsts.MinDescriptionLength)]
        public string Description { get; set; }
    }
}
