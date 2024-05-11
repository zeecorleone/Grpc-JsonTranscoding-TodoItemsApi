using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TodoGrpc.Data;
using TodoGrpc.Models;

namespace TodoGrpc.Services;

[Authorize]
public class ToDoService : ToDoIt.ToDoItBase
{
    private readonly AppDbContext _dbContext;
    public ToDoService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is not valid. Please provide valid object"));

        var todoItem = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description,
        };

        await _dbContext.AddAsync(todoItem);
        await _dbContext.SaveChangesAsync();

        //return await Task.FromResult(new CreateToDoResponse()
        //{
        //    Id = todoItem.Id,
        //});

        var createResult = new CreateToDoResponse()
        {
            Id = todoItem.Id
        };

        return createResult;
    }

    public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
    {
        if (request is null || request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "resource index must be greater than zero"));

        var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == request.Id);

        if(toDoItem is not null)
        {
            return await Task.FromResult(new ReadToDoResponse()
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                ToDoStatus = toDoItem.ToDoStatus
            });
        }

        throw new RpcException(new Status(StatusCode.NotFound, $"Item not found with Id: {request.Id}"));

    }

    public override async Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        var response = new GetAllResponse();
        var todoItems = await _dbContext.ToDoItems.ToListAsync();

        foreach (var toDoItem in todoItems )
        {
            response.ToDo.Add(new ReadToDoResponse()
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                ToDoStatus = toDoItem.ToDoStatus
            });
        }

        return await Task.FromResult(response);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {
        if(request is null || request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "resource index must be greater than zero"));

        if (string.IsNullOrWhiteSpace(request.Title) &&
            string.IsNullOrWhiteSpace(request.Description) &&
            string.IsNullOrWhiteSpace(request.ToDoStatus))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request is not valid, please provide data to update"));

        var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (toDoItem is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"No Item with id {request.Id}"));

        if (!string.IsNullOrWhiteSpace(request.Title))
            toDoItem.Title = request.Title;
        if (!string.IsNullOrWhiteSpace(request.Description))
            toDoItem.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.ToDoStatus))
            toDoItem.ToDoStatus = request.ToDoStatus;

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateToDoResponse()
        {
            Id = toDoItem.Id
        });
    }

    public override async Task<DeleteToDoResponse> DeleteToDo(DeleteToDoRequest request, ServerCallContext context)
    {
        if (request is null || request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "resource index must be greater than zero"));

        var toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (toDoItem is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"No Item with id {request.Id}"));

        _dbContext.Remove(toDoItem);

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new DeleteToDoResponse() { Id = request.Id });
    }
}
