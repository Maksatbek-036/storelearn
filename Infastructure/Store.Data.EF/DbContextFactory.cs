using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    public class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor= httpContextAccessor;
        }
        public StoreDbContext Create(Type repositoryType)
        {
            var service = httpContextAccessor.HttpContext.RequestServices;
            var dbContexts = service.GetService<Dictionary<Type, StoreDbContext>>();
            if (!dbContexts.ContainsKey(repositoryType))
                dbContexts[repositoryType] = service.GetService<StoreDbContext>();

            return dbContexts[repositoryType];
        }

    }
}
