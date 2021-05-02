using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data;
using Avalentini.Expensi.Domain.Data.Repository;
using Avalentini.Expensi.Api.Data.Repository.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _repo;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _repo = new UsersRepository(context, mapper);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _repo.GetAll();
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (_repo.Exists(user))
                return Conflict();

            _repo.Add(user);

            return new JsonResult(user);
        }
    }
}
