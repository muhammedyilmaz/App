using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.EntityFrameworkCore
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;

        int SaveChanges();
    }
}