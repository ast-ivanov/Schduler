using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Entities;
using Scheduler.Services;

namespace Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IRepository<WorkTask> _taskRepository;

        public TaskController(IRepository<WorkTask> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpPut]
        public async Task<IActionResult> Create(WorkTask task)
        {
            await _taskRepository.CreateAsync(task);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<WorkTask>> Get(int id)
        {
            var task = await _taskRepository.GetAsync(id);

            return Ok(task);
        }

        [HttpPatch]
        public async Task<ActionResult<WorkTask>> Update(WorkTask task)
        {
            var result = await _taskRepository.UpdateAsync(task);

            return Ok(result);
        }

        [HttpDelete]

        public async Task<ActionResult<WorkTask>> Delete(int id)
        {
            await _taskRepository.DeleteAsync(id);

            return Ok();
        }
    }
}