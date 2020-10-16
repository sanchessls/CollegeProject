using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Repositories.Implementation
{
    public class RepositoryTableOneImp : RepositoryImp<TableOne>, IRepositoryTableOne
    {
        public RepositoryTableOneImp(ApplicationContext context) : base(context)
        {
        }

    }
}
