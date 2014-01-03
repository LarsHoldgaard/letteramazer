using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Data
{
    public class EFRepository : IRepository
    {
        private IDBFactory databaseFactory;
        private LetterAmazerContext dataContext;

        public EFRepository(IDBFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected LetterAmazerContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public T GetById<T>(long id) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            return dbset.Find(id);
        }

        public void Create<T>(T entity) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            dbset.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void SaveOrUpdate<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            dbset.Remove(entity);
        }

        public IList<T> FindAll<T>(params OrderBy[] orders) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            var query = dbset.Where(t => true);
            query = ApplyOrders<T>(query, orders);
            return query.ToList<T>();
        }

        public int Count<T>(Expression<Func<T, bool>> whereExpression) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            return dbset.Count<T>(whereExpression);
        }

        public bool Exists<T>(Expression<Func<T, bool>> whereExpression) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            return dbset.Count<T>(whereExpression) != 0;
        }

        public T FindFirst<T>(Expression<Func<T, bool>> whereExpression, params OrderBy[] orders) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            var query = dbset.Where(whereExpression);
            query = ApplyOrders<T>(query, orders);
            return query.SingleOrDefault<T>();
        }

        public PaginatedResult<T> Find<T>(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize, params OrderBy[] orders) where T : class
        {
            IDbSet<T> dbset = DataContext.Set<T>();
            PaginatedResult<T> results = new PaginatedResult<T>();
            var query = dbset.Where(whereExpression);
            query = ApplyOrders<T>(query, orders);
            results.Results = query.Skip<T>(pageIndex * pageSize).Take<T>(pageSize).ToList<T>();
            results.TotalItems = dbset.LongCount(whereExpression);
            return results;
        }

        public void ExecuteNativeSQL(string sql)
        {
            DataContext.Database.ExecuteSqlCommand(sql);
        }

        private IQueryable<T> ApplyOrders<T>(IQueryable<T> query, params OrderBy[] orders)
        {
            if (orders == null || orders.Length == 0) return query;
            foreach (var order in orders)
            {
                query = query.OrderBy(order.ToString());
            }
            return query;
        }
    }
}
