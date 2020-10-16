using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ScrumPokerAPI.Repositories.Interface
{
    /*
     This class is our base Interface for all repositories class that get information from the database

    all of those class must have AT least this basic methods    
     
     */
    public interface IRepository<T> where T : class
    {
        //Gets all registry from a type of object
        IQueryable<T> GetAll();

        //Gets all registry from a type of object
        //using an expression to create a where expression
        IQueryable<T> GetWhere(Expression<Func<T,bool>> where);
    }
}
