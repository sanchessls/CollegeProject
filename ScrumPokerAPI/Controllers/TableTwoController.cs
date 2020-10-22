﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Controllers
{
    [ApiController]
    [Route("api/TableTwo")]
    public class TableTwoController : APIBaseController
    {
        private readonly IRepositoryTableTwo _repository;
        public TableTwoController(IRepositoryTableTwo repository)
        {
            _repository = repository;
        }


        //GET api/TableTwo/
        [HttpGet]
        public ActionResult <IEnumerable<TableTwo>> GetAll()
        {
            var obj = _repository.GetAll();
            return Ok(obj);
        }

        //GET api/TableTwo/{id}
        [HttpGet("{id}")]
        public ActionResult <TableTwo> GetById(int id)
        {
            var obj = _repository.GetWhere(x => x.Id == id);
            return Ok(obj);
        }


    }
}
