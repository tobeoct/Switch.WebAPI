using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PusherServer;
using Switch.WebAPI.Models;

namespace Switch.WebAPI.Logics
{
    public interface IEntity<TId>
    {
//        TId Id { get; set; }
        TId Name { get; set; }
    }
    public interface UEntity<TId>
    {
        //        TId Id { get; set; }
        TId Scheme { get; set; }
        TId SourceNode { get; set; }
        TId Route { get; set; }
        TId SinkNode { get; set; }
        TId Channel { get; set; }
    }
    public class EntityLogic<T> where T : class, new()
    {
        protected virtual ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }

        public void Save()
        {
            var session = GetContext();
            session.SaveChanges();
        }

        //        public T RetrieveByID(int id)
        //        {
        //            T entity = new T();
        //
        //            entity = GetContext().Set<T>().SingleOrDefault(entity.Id);
        //
        //            return entity;
        //        }

        public virtual TEntity RetrieveSingle<TEntity, TId>(TId id) where TEntity : class, IEntity<TId>
        {
            return GetContext().Set<TEntity>().SingleOrDefault(e => e.Name.Equals(id));
        }
        public T RetrieveByName(string id)
        {
            T entity = new T();

            entity = GetContext().Set<T>().Find(id);

            return entity;
        }
        public virtual T GetSingle(Func<T, bool> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public List<T> GetList()
        {
            
            List<T> list = GetContext().Set<T>().ToList();
            return list;
        }
        //        public IQueryable<T> IncludeMultiple<T>(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        //            where T : class
        //        {
        //            if (includes != null)
        //            {
        //                query = includes.Aggregate(query,
        //                    (current, include) => current.Include(include));
        //            }
        //
        //            return query;
        //        }
        public virtual List<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public void Insert(T entity)
        {
            using (var context = GetContext())
            {
               context.Set<T>().Add(entity);
                context.SaveChanges();

            }

        }
        public void Pusher(T entity)
        {
            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);
           

        }
//        public void Update(T entity)
//        {
//            GetContext();
//        }
        public virtual void Update(params T[] items)
        {
            using (var context = GetContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public long GetCount()
        {
            long count = GetContext().Set<T>().Count();
            return count;
        }
        //        public void Delete(T entity)
        //        {
        //            GetContext().Set<T>().Remove(entity);
        //        }
        public virtual void Delete(params T[] items)
        {
            using (var context = GetContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}