using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Controllers
{
    [ApiController]
    [Route("api/TableOne")]
    public class TableOneController : ControllerBase
    {
        private readonly IRepositoryTableOne _repository;
        public TableOneController(IRepositoryTableOne repository)
        {
            _repository = repository;
        }


        //GET api/TableOne/
        [HttpGet]
        public ActionResult <IEnumerable<TableOne>> GetAll()
        {
            var obj = _repository.GetAll();
            return Ok(obj);
        }

        //GET api/TableOne/{id}
        [HttpGet("{id}")]
        public ActionResult <TableOne> GetById(int id)
        {
            var obj = _repository.GetWhere(x => x.Id == id);
            return Ok(obj);
        }


    }
}
