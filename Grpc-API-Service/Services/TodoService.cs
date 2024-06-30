using Google.Protobuf.WellKnownTypes;
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

    public override async Task<ReadResponse> Read(ReadRequest request, ServerCallContext context)
    {
        if (request.Id == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id cannot be empty"));

        var todo = await _dbContext.ToDoItems.FindAsync(request.Id);

        if (todo == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Todo not found"));

        return await Task.FromResult(new ReadResponse
        {
            Title = todo.Title
        });
    }
    
    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        if (request.Id == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Id cannot be empty"));

        var todo = await _dbContext.ToDoItems.FindAsync(request.Id);

        if (todo == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Todo not found"));

        todo.Title = request.Title;
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateResponse
        {
            Title = todo.Title
        });
    }
}