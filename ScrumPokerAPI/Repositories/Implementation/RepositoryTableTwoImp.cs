using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Repositories.Implementation
{
    public class RepositoryTableTwoImp : RepositoryImp<TableTwo>, IRepositoryTableTwo
    {
        public RepositoryTableTwoImp(ApplicationContext context) : base(context)
        {
        }

    }
}
