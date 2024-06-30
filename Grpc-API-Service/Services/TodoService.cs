using Grpc.Core;
using TodoGrpc.Data;
using TodoGrpc.Models;

namespace Grpc_API_Service.Services;

public class TodoService : TodoIt.TodoItBase
{
    private readonly AppDbContext _dbContext;
    
    public TodoService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        if (request.Title == string.Empty)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Title cannot be empty"));
        }
        
        var todo = new ToDoItem
        {
            Title = request.Title
        };
        
        await _dbContext.AddAsync(todo);
        await _dbContext.SaveChangesAsync();
        
        return await Task.FromResult(new CreateResponse
        {
            Id = todo.Title
        });
    }
    
}