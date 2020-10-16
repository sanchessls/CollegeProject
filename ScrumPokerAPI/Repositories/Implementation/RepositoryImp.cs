using ScrumPokerAPI.Context;
using ScrumPokerAPI.Repositories.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ScrumPokerAPI.Repositories.Implementation
{
    /*
 This class is our base Implementation for all repositories class that get information from the database

here we have the base implementation that is common to all of our repositories

 */
    public class RepositoryImp<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public RepositoryImp(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            //Set get a dynamic DbSet from an class
            return _context.Set<T>();
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> where)
        {
            //Set get a dynamic DbSet from an class
            //And the Where aplly the selected predicate
            return _context.Set<T>().Where(where);
        }
    }
}
