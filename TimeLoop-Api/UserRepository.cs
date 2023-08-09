using TimeLoop_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace TimeLoop_Api.UserBLL
{
    public class UserRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Users (Email, FirstName, LastName, HashedPassword, Birthdate, DateOfCreation, LastLogin) " +
                               "VALUES (@Email, @FirstName, @LastName, HASHBYTES('SHA2_256', @HashedPassword), @Birthdate, @DateOfCreation, @LastLogin)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@HashedPassword", user.HashedPassword);
                    command.Parameters.AddWithValue("@Birthdate", user.Birthdate);
                    command.Parameters.AddWithValue("@DateOfCreation", user.DateOfCreation);
                    command.Parameters.AddWithValue("@LastLogin", user.LastLogin);

                    command.ExecuteNonQuery();
                }
            }
        }



        public bool IsEmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = (int)reader["Id"],
                                Email = (string)reader["Email"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                HashedPassword = (string)reader["HashedPassword"],
                                Birthdate = (DateTime)reader["Birthdate"],
                                DateOfCreation = (DateTime)reader["DateOfCreation"],
                                LastLogin = (DateTime)reader["LastLogin"]
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public User GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                Id = (int)reader["Id"],
                                Email = (string)reader["Email"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                HashedPassword = (string)reader["HashedPassword"],
                                Birthdate = (DateTime)reader["Birthdate"],
                                DateOfCreation = (DateTime)reader["DateOfCreation"],
                                LastLogin = (DateTime)reader["LastLogin"]
                            };

                            return user;
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Users SET Email = @Email, FirstName = @FirstName, LastName = @LastName, " +
                               "HashedPassword = @HashedPassword, Birthdate = @Birthdate, " +
                               "DateOfCreation = @DateOfCreation, LastLogin = @LastLogin WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@HashedPassword", user.HashedPassword);
                    command.Parameters.AddWithValue("@Birthdate", user.Birthdate);
                    command.Parameters.AddWithValue("@DateOfCreation", user.DateOfCreation);
                    command.Parameters.AddWithValue("@LastLogin", user.LastLogin);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
        public User GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                Id = (int)reader["Id"],
                                Email = (string)reader["Email"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                HashedPassword = (string)reader["HashedPassword"], // Read hashed password as a string
                                Birthdate = (DateTime)reader["Birthdate"],
                                DateOfCreation = (DateTime)reader["DateOfCreation"],
                                LastLogin = (DateTime)reader["LastLogin"]
                            };

                            return user;
                        }
                    }
                }
            }

            return null;
        }
        public bool VerifyUserPassword(string userEmail, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("VerifyUserPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserEmail", userEmail);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.Add("@IsPasswordMatch", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    return (bool)command.Parameters["@IsPasswordMatch"].Value;
                }
            }
        }


    }
}
