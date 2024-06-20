using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoGrpc.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
    }
}