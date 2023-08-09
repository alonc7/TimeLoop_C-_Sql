using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Configuration;
using TimeLoop_Api.Models;
using TimeLoop_Api.TaskBLL;

namespace TimeLoop_Api.Controllers
{
    public class TasksController : ApiController
    {
        private readonly TaskRepository _taskRepository;

        public TasksController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            _taskRepository = new TaskRepository(connectionString);
        }

        private void FormatDateAndTimeProperties(IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                task.StartDate = task.StartDate.Date;
                task.DueDate = task.DueDate.Date;
            }
        }
        [HttpGet]
        [Route("api/Tasks")]
        public IHttpActionResult Get()
        {
            try
            {
                var tasks = _taskRepository.GetAllTasks();
                FormatDateAndTimeProperties(tasks);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting all tasks: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult GetTaskById(int id)
        {
            try
            {
                Task task = _taskRepository.GetTaskById(id);
                if (task == null)
                {
                    return NotFound();
                }

                FormatDateAndTimeProperties(new List<Task> { task });
                return Ok(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting task by ID: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Tasks/GetAllTasksByUserId/{userId}")]
        public IHttpActionResult GetAllTasksByUserId(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetAllTasksByUserId(userId);
                FormatDateAndTimeProperties(tasks);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting all tasks by user ID: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Tasks/CompletedTasksByUserId/{userId}")]
        public IHttpActionResult GetCompletedTasksByUserId(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetCompletedTasksByUserId(userId);
                FormatDateAndTimeProperties(tasks);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting completed tasks by user ID: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Tasks/PendingTasksByUserId/{userId}")]
        public IHttpActionResult GetPendingTasksByUserId(int userId)
        {
            try
            {
                var tasks = _taskRepository.GetPendingTasksByUserId(userId);
                FormatDateAndTimeProperties(tasks);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting pending tasks by user ID: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        // POST: api/Tasks
        [HttpPost]
        [Route("api/Tasks/AddTask")]
        public IHttpActionResult AddTask([FromBody] TaskRequest taskRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                // Call the AddTask method with the userEmail
                _taskRepository.AddTask(taskRequest.UserEmail, taskRequest.Task);

                // Return a simple response indicating success (e.g., "Ok" result)
                return Ok("Task added successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class TaskRequest
        {
            public string UserEmail { get; set; }
            public Task Task { get; set; }
        }
        // PUT: api/Tasks/5
        [HttpPut]
        [Route("api/Tasks/updateTask/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Task task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                Task existingTask = _taskRepository.GetTaskById(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                task.Id = id;
                _taskRepository.UpdateTask(task);
                return Ok(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating the task: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Tasks/5
        [HttpDelete]
        [Route("api/Tasks/RemoveTask/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Task existingTask = _taskRepository.GetTaskById(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                _taskRepository.DeleteTask(id);
                return Ok("Task deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting the task: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Tasks/CompleteTask/5
        [HttpPut]
        [Route("api/Tasks/CompleteTask/{id}")]
        public IHttpActionResult CompleteTask(int id)
        {
            try
            {
                Task existingTask = _taskRepository.GetTaskById(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                // Update the task status to "Complete" in the repository
                existingTask.Status = "Complete";
                _taskRepository.UpdateTask(existingTask);

                return Ok(existingTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error completing the task: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
