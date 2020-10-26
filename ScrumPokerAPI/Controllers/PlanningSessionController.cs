using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Controllers
{
    [Route("api/PlanningSession")]
    public class PlanningSessionController : APIBaseController
    {
        private readonly IRepositoryPlanningSession _repository;
        public PlanningSessionController(IRepositoryPlanningSession repository)
        {
            _repository = repository;
        }

        //GET api/PlanningSession/
        [HttpGet]
        public ActionResult <IEnumerable<PlanningSession>> GetAll()
        {
            var obj = _repository.GetAll();
            return Ok(obj);
        }

        //GET api/PlanningSession/{id}
        [HttpGet("{id}")]
        public ActionResult <PlanningSession> GetById(int id)
        {
            var obj = _repository.GetWhere(x => x.Id == id);
            return Ok(obj);
        }


    }
}
