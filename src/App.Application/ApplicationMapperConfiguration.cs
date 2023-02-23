using App.Orders;
using App.Orders.Dto;
using App.Products;
using App.Products.Dto;
using AutoMapper;

namespace App
{
    public class ApplicationMapperConfiguration : Profile
    {
        public ApplicationMapperConfiguration()
        {
            CreateOrderMaps();
            CreateOrderItemMaps();
            CreateOrderCustomerMaps();
            CreateProductMaps();
        }

        private void CreateOrderMaps()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
        }

        private void CreateOrderItemMaps()
        {
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
        }

        private void CreateOrderCustomerMaps()
        {
            CreateMap<OrderCustomer, OrderCustomerDto>();
            CreateMap<OrderCustomerDto, OrderCustomer>();
        }

        private void CreateProductMaps()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}