using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Repositories.Implementation
{
    public class RepositoryTableTwoImp : RepositoryImp<TableTwo>, IRepositoryTableTwo
    {
        public RepositoryTableTwoImp(ApplicationContext context) : base(context)
        {
        }

    }
}
