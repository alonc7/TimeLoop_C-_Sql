using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TimeLoop_Api.Models;

namespace TimeLoop_Api.TaskBLL
{
    public class TaskRepository
    {
        private const string Message = "User not found with the provided email.";
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int GetUserIdByEmail(string email)
        {
            int userId = -1; // Set a default value, or you can return null if appropriate.

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM Users WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }

            return userId;
        }

        public void AddTask(string userEmail, Task task)
        {
            // Get the user ID based on the provided email
            int userId = GetUserIdByEmail(userEmail);

            if (userId == -1)
            {
#pragma warning disable S112 
                throw new Exception(Message);
#pragma warning restore S112
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Tasks (UserId, Title, StartDate, DueDate, IsOnDate, IsOnTime, StartTime, DueTime, Priority, Status) " +
                               "VALUES (@UserId, @Title, @StartDate, @DueDate, @IsOnDate, @IsOnTime, @StartTime, @DueTime, @Priority, @Status)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set the UserId parameter to the obtained user ID
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@StartDate", task.StartDate);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate);
                    command.Parameters.AddWithValue("@IsOnDate", task.IsOnDate);
                    command.Parameters.AddWithValue("@IsOnTime", task.IsOnTime);
                    command.Parameters.AddWithValue("@StartTime", task.StartTime);
                    command.Parameters.AddWithValue("@DueTime", task.DueTime);
                    command.Parameters.AddWithValue("@Priority", task.Priority);
                    command.Parameters.AddWithValue("@Status", task.Status);

                    command.ExecuteNonQuery();
                }
            }
        }




        public List<Task> GetCompletedTasksByUserId(int userId)
        {
            List<Task> completedTasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tasks WHERE UserId = @UserId AND Status = 'Complete'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = (int)reader["Id"],
                                UserId = (int)reader["UserId"],
                                Title = (string)reader["Title"],
                                StartDate = (DateTime)reader["StartDate"],
                                DueDate = (DateTime)reader["DueDate"],
                                IsOnDate = (bool)reader["IsOnDate"],
                                IsOnTime = (bool)reader["IsOnTime"],
                                StartTime = (TimeSpan)reader["StartTime"],
                                DueTime = (TimeSpan)reader["DueTime"],
                                Priority = (string)reader["Priority"],
                                Status = (string)reader["Status"]
                            };

                            completedTasks.Add(task);
                        }
                    }
                }
            }

            return completedTasks;
        }
        public List<Task> GetPendingTasksByUserId(int userId)
        {
            List<Task> pendingTasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tasks WHERE UserId = @UserId AND Status = 'Pending'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = (int)reader["Id"],
                                UserId = (int)reader["UserId"],
                                Title = (string)reader["Title"],
                                StartDate = (DateTime)reader["StartDate"],
                                DueDate = (DateTime)reader["DueDate"],
                                IsOnDate = (bool)reader["IsOnDate"],
                                IsOnTime = (bool)reader["IsOnTime"],
                                StartTime = (TimeSpan)reader["StartTime"],
                                DueTime = (TimeSpan)reader["DueTime"],
                                Priority = (string)reader["Priority"],
                                Status = (string)reader["Status"]
                            };

                            pendingTasks.Add(task);
                        }
                    }
                }
            }

            return pendingTasks;
        }

        public List<Task> GetAllTasks()
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tasks";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = (int)reader["Id"],
                                UserId = (int)reader["UserId"],
                                Title = (string)reader["Title"],
                                StartDate = (DateTime)reader["StartDate"],
                                DueDate = (DateTime)reader["DueDate"],
                                IsOnDate = (bool)reader["IsOnDate"],
                                IsOnTime = (bool)reader["IsOnTime"],
                                StartTime = (TimeSpan)reader["StartTime"],
                                DueTime = (TimeSpan)reader["DueTime"],
                                Priority = (string)reader["Priority"],
                                Status = (string)reader["Status"]
                            };

                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public Task GetTaskById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Tasks WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = (int)reader["Id"],
                                    UserId = (int)reader["UserId"],
                                    Title = (string)reader["Title"],
                                    StartDate = (DateTime)reader["StartDate"],
                                    DueDate = (DateTime)reader["DueDate"],
                                    IsOnDate = (bool)reader["IsOnDate"],
                                    IsOnTime = (bool)reader["IsOnTime"],
                                    StartTime = (TimeSpan)reader["StartTime"],
                                    DueTime = (TimeSpan)reader["DueTime"],
                                    Priority = (string)reader["Priority"],
                                    Status = (string)reader["Status"]
                                };

                                return task;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetTaskById: " + ex.Message);
                return null;
            }

            return null; // Return null outside of the using block in case of any exception
        }

        public List<Task> GetAllTasksByUserId(int userId)
        {
            List<Task> tasks = new List<Task>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM Tasks WHERE UserId = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = (int)reader["Id"],
                                    UserId = (int)reader["UserId"],
                                    Title = (string)reader["Title"],
                                    StartDate = (DateTime)reader["StartDate"],
                                    DueDate = (DateTime)reader["DueDate"],
                                    IsOnDate = Convert.ToBoolean(reader["IsOnDate"]),
                                    IsOnTime = Convert.ToBoolean(reader["IsOnTime"]),
                                    StartTime = (TimeSpan)reader["StartTime"],
                                    DueTime = (TimeSpan)reader["DueTime"],
                                    Priority = (string)reader["Priority"],
                                    Status = (string)reader["Status"]
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                    return tasks;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading data: " + ex.Message);
                    return null;
                }
            }
        }


        public void UpdateTask(Task task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Tasks SET " +
                               "UserId = @UserId, " +
                               "Title = @Title, " +
                               "StartDate = @StartDate, " +
                               "DueDate = @DueDate, " +
                               "IsOnDate = @IsOnDate, " +
                               "IsOnTime = @IsOnTime, " +
                               "StartTime = @StartTime, " +
                               "DueTime = @DueTime, " +
                               "Priority = @Priority, " +
                               "Status = @Status " +
                               "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@UserId", task.UserId);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@StartDate", task.StartDate);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate);
                    command.Parameters.AddWithValue("@IsOnDate", task.IsOnDate);
                    command.Parameters.AddWithValue("@IsOnTime", task.IsOnTime);
                    command.Parameters.AddWithValue("@StartTime", task.StartTime);
                    command.Parameters.AddWithValue("@DueTime", task.DueTime);
                    command.Parameters.AddWithValue("@Priority", task.Priority);
                    command.Parameters.AddWithValue("@Status", task.Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Tasks WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
