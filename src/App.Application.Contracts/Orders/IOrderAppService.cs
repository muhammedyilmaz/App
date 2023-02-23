using App.Application.Services;
using App.Orders.Dto;
using System.Collections.Generic;

namespace App.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        List<OrderGetForViewDto> GetAll(OrderGetAllInput input);
        void Create(OrderCreatedInput input);
        void Update(OrderUpdatedInput input);
        void Delete(int id);
    }
}
