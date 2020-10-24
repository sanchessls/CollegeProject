using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Repositories.Implementation
{
    public class RepositoryPlanningSessionImp : RepositoryImp<PlanningSession>, IRepositoryPlanningSession
    {
        public RepositoryPlanningSessionImp(ApplicationContext context) : base(context)
        {            
        }

    }
}
