using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ScrumPokerPlanning.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T,bool>> where);
    }
}
