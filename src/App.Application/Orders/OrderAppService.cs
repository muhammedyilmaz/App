using App.Application.Services;
using App.Domain.Repositories;
using App.Mapper;
using App.Orders.Dto;
using App.Products;
using App.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        #region Fields

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<OrderCustomer> _orderCustomerRepository;
        private readonly IRepository<Product> _productRepository;

        #endregion

        #region Ctor

        public OrderAppService(IRepository<Order> orderRepository,
            IRepository<OrderItem> orderItemRepository,
             IRepository<OrderCustomer> orderCustomerRepository,
             IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _orderCustomerRepository = orderCustomerRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Nested Classes

        private class GetOrderFilteredQto
        {
            public Order Order { get; set; }
            public OrderCustomer OrderCustomer { get; set; }
            public OrderItem OrderItem { get; set; }
            public Product Product { get; set; }
        }

        #endregion

        #region Utilities

        private IQueryable<GetOrderFilteredQto> GetOrderFilteredQuery(OrderGetAllInput input)
        {
            var query = from order in _orderRepository.GetAll()
                        join orderCustomer in _orderCustomerRepository.GetAll() on order.Id equals orderCustomer.OrderId
                        join orderItem in _orderItemRepository.GetAll() on order.Id equals orderItem.OrderId
                        join product in _productRepository.GetAll() on orderItem.ProductId equals product.Id
                        select new GetOrderFilteredQto
                        {
                            Order = order,
                            OrderCustomer = orderCustomer,
                            OrderItem = orderItem,
                            Product = product
                        };

            if (input.OrderId > 0)
                query = query.Where(x => x.Order.Id == input.OrderId);

            if (!string.IsNullOrEmpty(input.OrderSource))
                query = query.Where(x => x.Order.Source == input.OrderSource);

            if (!string.IsNullOrEmpty(input.CustomerName))
                query = query.Where(x => x.OrderCustomer.Name == input.CustomerName);

            return query;
        }

        #endregion

        #region Methods

        public List<OrderGetForViewDto> GetAll(OrderGetAllInput input)
        {
            #region Orders & Order Customers

            var query = GetOrderFilteredQuery(input);

            var results = query.ToList();

            var dtos = results.Select(qto => new OrderGetForViewDto
            {
                Order = qto.Order.ToModel<OrderDto>(),
                OrderCustomer = qto.OrderCustomer.ToModel<OrderCustomerDto>()
            }).ToList();

            var groupedList = (from data in dtos
                               group data by data.Order.Id into g
                               select new OrderGetForViewDto
                               {
                                   Order = g.FirstOrDefault().Order,
                                   OrderCustomer = g.FirstOrDefault().OrderCustomer
                               }).ToList();

            #endregion

            #region Order Items & Products

            foreach (var item in groupedList)
            {
                #region OrderItems

                var qtoOrderItems = results.Select(x => x.OrderItem).Where(y => y.OrderId == item.Order.Id).ToList();
                var orderItems = new List<OrderItemDto>();
                foreach (var qtoOrderItem in qtoOrderItems)
                {
                    var orderItem = qtoOrderItem.ToModel<OrderItemDto>();
                    orderItems.Add(orderItem);
                }

                item.OrderItems = orderItems;

                #endregion

                #region Products

                var productIds = qtoOrderItems.Select(x => x.ProductId).ToList();

                var qtoProducts = results.Where(c => c.Order.Id == item.Order.Id).Select(x => x.Product).Where(y => productIds.Contains(y.Id)).ToList();
                var products = new List<ProductDto>();
                foreach (var qtoOrderItem in qtoProducts)
                {
                    var product = qtoOrderItem.ToModel<ProductDto>();
                    products.Add(product);
                }

                item.Products = products;

                #endregion
            }

            #endregion

            return groupedList;
        }

        public void Create(OrderCreatedInput input)
        {
            #region Order Create

            var order = input.Order.ToEntity<Order>();
            order.CreationTime = DateTime.Now;
            _orderRepository.Insert(order);

            #endregion

            #region Order Customer Create

            var orderCustomer = input.OrderCustomer.ToEntity<OrderCustomer>();
            orderCustomer.OrderId = order.Id;
            orderCustomer.CreationTime = DateTime.Now;
            _orderCustomerRepository.Insert(orderCustomer);

            #endregion

            #region Order Items Create

            foreach (var item in input.OrderItems)
            {
                var orderItem = item.ToEntity<OrderItem>();
                orderItem.OrderId = order.Id;
                orderItem.CreationTime = DateTime.Now;

                var product = _productRepository.Get(item.ProductId);
                if (product == null)
                    throw new Exception($"Error occurred while adding new product. Error: Product not found! ProductId: {item.ProductId}");

                orderItem.ProductId = product.Id;

                _orderItemRepository.Insert(orderItem);
            }

            #endregion
        }

        public void Update(OrderUpdatedInput input)
        {
            #region Order Item Validation

            if (input.OrderItems.Count == 0)
                throw new Exception($"OrderItem can not be null!");

            #endregion

            #region Order Update

            var order = _orderRepository.Get(input.Id);
            if (order == null)
                throw new Exception("Order not fount!");

            order.LastModificationTime = DateTime.Now;
            _orderRepository.Update(order);

            #endregion

            #region Order Customer Update

            var orderCustomer = _orderCustomerRepository.FirstOrDefault(x => x.OrderId == input.Id);
            if (orderCustomer == null)
                throw new Exception("OrderCustomer not fount!");

            orderCustomer.Name = input.OrderCustomer.Name;
            orderCustomer.Address = input.OrderCustomer.Address;
            orderCustomer.LastModificationTime = DateTime.Now;
            _orderCustomerRepository.Update(orderCustomer);

            #endregion

            #region Order Items Update

            #region Prepare Data

            var dboOrderItems = _orderItemRepository.GetAllList(x => x.OrderId == order.Id);
            var dboProductIds = dboOrderItems.Select(x => x.ProductId).ToList();

            var inputProductIds = input.OrderItems.Select(x => x.ProductId).ToList();
            var filteredList = dboProductIds.Except(inputProductIds).ToList();

            #endregion

            #region Delete

            //Delete
            foreach (var id in filteredList)
            {
                var dboOrderItem = _orderItemRepository.FirstOrDefault(x => x.ProductId == id);
                if (dboOrderItem != null)
                    _orderItemRepository.Delete(dboOrderItem);
            }

            #endregion

            #region Create Or Update

            foreach (var item in input.OrderItems)
            {
                var dboOrderItem = dboOrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (dboOrderItem == null)
                {
                    //Create
                    var orderItem = item.ToEntity<OrderItem>();
                    orderItem.OrderId = order.Id;
                    orderItem.CreationTime = DateTime.Now;

                    var product = _productRepository.Get(item.ProductId);
                    if (product == null)
                        throw new Exception($"Error occurred while adding new product. Error: Product not found! ProductId: {item.ProductId}");

                    orderItem.ProductId = product.Id;

                    _orderItemRepository.Insert(orderItem);
                }
                else
                {
                    //Update
                    dboOrderItem.Quantity = item.Quantity;
                    dboOrderItem.Price = item.Price;
                    dboOrderItem.LastModificationTime = DateTime.Now;
                    _orderItemRepository.Update(dboOrderItem);
                }
            }

            #endregion

            #endregion
        }

        public void Delete(int id)
        {
            #region Get Order

            var order = _orderRepository.Get(id);
            if (order == null)
                throw new Exception("Order not fount!");

            #endregion

            #region Order Items Delete

            var orderItems = _orderItemRepository.GetAllList(x => x.OrderId == order.Id);
            foreach (var item in orderItems)
            {
                _orderItemRepository.Delete(item);
            }

            #endregion

            #region Order Customer Delete

            var orderCustomer = _orderCustomerRepository.FirstOrDefault(x => x.OrderId == order.Id);
            if (orderCustomer != null)
                _orderCustomerRepository.Delete(orderCustomer.Id);

            #endregion

            #region Order Delete

            _orderRepository.Delete(order.Id);

            #endregion
        }

        #endregion
    }
}
