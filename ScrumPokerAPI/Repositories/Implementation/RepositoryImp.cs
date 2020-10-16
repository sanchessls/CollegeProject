using ScrumPokerAPI.Context;
using ScrumPokerAPI.Repositories.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ScrumPokerAPI.Repositories.Implementation
{
    public class RepositoryImp<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public RepositoryImp(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> where)
        {
            return _context.Set<T>().Where(where);
        }
    }
}
