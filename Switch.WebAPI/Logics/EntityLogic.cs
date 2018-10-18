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
  
   
    public class EntityLogic<T> where T : class, new()
    {
        protected virtual ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext();
        }

        public void Insert(T entity)
        {
            using (var context = GetContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();

            }

        }

        public virtual void Update(params T[] items)
        {
            using (var context = GetContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public virtual void Delete(params T[] items)
        {
            using (var context = GetContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        public void Save()
        {
            var session = GetContext();
            session.SaveChanges();
        }

        public List<T> GetList()
        {

            var list = GetContext().Set<T>().ToList();
            return list;
        }

        public virtual List<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<T, object>(navigationProperty));

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<T, object>(navigationProperty));

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public long GetCount()
        {
            long count = GetContext().Set<T>().Count();
            return count;
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
    }
}