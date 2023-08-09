using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLoop_Api.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool IsOnDate { get; set; }
        public bool IsOnTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan DueTime { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
    }
}