using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public interface IOrderRepository
    {
        Order Create();
        Task<Order> CreateAsync();
        Order GetById(int id);
        Task<Order> GetByIdAsync(int orderId);
        void Update(Order order);
        Task UpdateAsync(Order order);
    }
}
