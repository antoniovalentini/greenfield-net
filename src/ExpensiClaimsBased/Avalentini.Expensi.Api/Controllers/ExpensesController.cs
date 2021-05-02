using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Domain.Data.Repository;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Api.Data.Repository.Expenses;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "mustBeUser")]
    public class ExpensesController : ControllerBase
    {
        private readonly IMongoCollection<ExpensesPerUser> _collection;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        private IRepository<Expense> _repo;

        public IRepository<Expense> Repo
        {
            get
            {
                if (_repo != null)
                    return _repo;

                var userId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
                _repo = new ExpensesMongoRepository(_collection, _mapper, userId);

                return _repo;
            }
        }

        public ExpensesController(IMongoCollection<ExpensesPerUser> collection, IMapper mapper, ILog logger)
        {
            _collection = collection;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<Expense>> Get()
        {
            _logger.Info($"Requesting all expenses for {User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
            return await Repo.GetAll();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var expense = await Repo.Get(id);

            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        // POST: api/Expenses
        [HttpPost]
        public IActionResult Post([FromBody] Expense expense)
        {
            Repo.Add(expense);

            return CreatedAtAction("Get", new { id = expense.Id }, expense);
        }

        // PUT: api/expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Expense expense)
        {
            if (id != expense.Id)
                return BadRequest();

            var oldExpense = await Repo.Get(id);
            if (oldExpense == null)
                return NotFound();

            Repo.Edit(id, expense);

            return NoContent();
        }

        // DELETE: api/expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var expense = await Repo.Get(id);
            if (expense == null)
                return NotFound();

            Repo.Remove(expense.Id);

            return Ok(expense);
        }
    }
}
