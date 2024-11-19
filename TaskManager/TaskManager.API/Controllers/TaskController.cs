using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Services;
using TaskManager.Domain.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUserIdAsync(int userId)
        {
            try
            {
                var tasks = await _taskService.GetAllByUserIdAsync(userId);

                if (tasks == null)
                {
                    return NotFound(new ApiResponse<IEnumerable<TaskItem>>(false, "No tasks found."));
                }

                return Ok(new ApiResponse<IEnumerable<TaskItem>>(true, "Tasks retrieved successfully.", tasks));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An error occurred while retrieving tasks.", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid task ID."));
                }

                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                {
                    return NotFound(new ApiResponse<string>(false, "Task not found."));
                }

                return Ok(new ApiResponse<TaskItem>(true, "Task retrieved successfully.", task));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An error occurred while retrieving the task.", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItem taskItem)
        {
            try
            {
                if (taskItem == null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Task data is required."));
                }

                var result = await _taskService.CreateAsync(taskItem);
                if (result == null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Failed to create the task."));
                }

                return CreatedAtAction(nameof(Get), new { id = result.Id },
                    new ApiResponse<TaskItem>(true, "Task created successfully.", result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An error occurred while creating the task.", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskItem taskItem)
        {
            try
            {
                if (id <= 0 || taskItem == null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid input data."));
                }

                var result = await _taskService.UpdateAsync(id, taskItem);
                if (result == 0)
                {
                    return NotFound(new ApiResponse<string>(false, "Task not found or update failed."));
                }

                return Ok(new ApiResponse<string>(true, "Task updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An error occurred while updating the task.", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid task ID."));
                }

                var result = await _taskService.DeleteAsync(id);
                if (result == 0)
                {
                    return NotFound(new ApiResponse<string>(false, "Task not found or already deleted."));
                }

                return Ok(new ApiResponse<string>(true, "Task deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>(false, "An error occurred while deleting the task.", ex.Message));
            }
        }

    }
}
