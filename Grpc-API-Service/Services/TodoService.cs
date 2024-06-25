using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grpc-API-Service.Services
{
    public class TodoService
    {
        private readonly AppDbContext _dbContext;
        public TodoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoModel> Create(CreateTodoModel model, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Title is required"));
            }

            var todo = new Todo
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted
            };

            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new TodoModel
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted
            });
        }
    }
}