using System.Collections;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Data
{
    public interface IRepository
    {
        T GetById<T>(long id) where T : class;
        void Create<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void SaveOrUpdate<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        IList<T> FindAll<T>(params OrderBy[] orders) where T : class;
        int Count<T>(Expression<Func<T, bool>> whereExpression) where T : class;
        bool Exists<T>(Expression<Func<T, bool>> whereExpression) where T : class;

        T FindFirst<T>(Expression<Func<T, bool>> whereExpression, params OrderBy[] orders) where T : class;
        PaginatedResult<T> Find<T>(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize, params OrderBy[] orders) where T : class;

        void ExecuteNativeSQL(string sql);
    }
}
