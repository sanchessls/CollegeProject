using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Repositories.Implementation
{
    public class RepositoryPlanningSessionImp : RepositoryImp<PlanningSession>, IRepositoryPlanningSession
    {
        public RepositoryPlanningSessionImp(ApplicationContext context) : base(context)
        {            
        }

    }
}
