﻿using Bootcamp.Clean.Domain.Entities;
using Bootcamp.Clean.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bootcamp.Clean.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        public DbSet<T> DbSet { get; set; }
        protected AppDbContext Context;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }


        public async Task<IReadOnlyList<T>> GetAll()
        {
            var list = await DbSet.ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<IReadOnlyList<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var list = await DbSet.Where(predicate).ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<IReadOnlyList<T>> GetAllByPage(int page, int pageSize)
        {
            var list = await DbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return list.AsReadOnly();
        }

        public Task Update(T entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<T> Create(T entity)
        {
            await DbSet.AddAsync(entity);

            return entity;
        }

        public async Task<T?> GetById(int id)
        {
            var result = await DbSet.FindAsync(id);
            return result;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);

            DbSet.Remove(entity!);
        }

        public Task<bool> HasExist(int id)
        {
            return DbSet.AnyAsync(x => x.Id == id);

            //var entity = await GetById(id);

            //return entity is not null;
        }
    }
}
