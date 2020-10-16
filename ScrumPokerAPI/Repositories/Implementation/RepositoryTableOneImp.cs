using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Repositories.Implementation
{
    public class RepositoryTableOneImp : RepositoryImp<TableOne>, IRepositoryTableOne
    {
        public RepositoryTableOneImp(ApplicationContext context) : base(context)
        {
        }

    }
}
