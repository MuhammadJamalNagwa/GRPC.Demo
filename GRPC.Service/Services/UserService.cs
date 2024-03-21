using Grpc.Core;
using GRPC.Service.Data;
using GRPC.Service.Models;
using GRPC.Service.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GRPC.Service.Services;


public class UserService : IUserService.IUserServiceBase
{
    private readonly ILogger<UserService> _logger;
    private readonly ApplicationDbContext _dbContext;
    public UserService(ApplicationDbContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Age <= default(int))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Bad Request the you provide is Empty or Not Valid"));

        UserEntity newUser = new()
        {
            Name = request.Name,
            Age = request.Age,
            Email = request.Email,
            Address = request.Address
        };
        await _dbContext.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new CreateUserResponse()
        {
            Id = newUser.Id
        });
    }
    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        if (request.Id <= default(int))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id Must Be Greater Than Zero"));

        UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

        if (user is not null)
        {
            return await Task.FromResult(new GetUserResponse()
            {
                Id = user.Id,
                Address = user.Address,
                Age = user.Age,
                Email = user.Email,
                Name = user.Name
            });
        }
        throw new RpcException(new Status(StatusCode.NotFound, "User Not Exists"));
    }

    [Authorize]
    public override async Task<GetUsersListResponse> GetUsersList(GetUsersListRequest request, ServerCallContext context)
    {
        GetUsersListResponse usersListResponse = new();

        int totalUsersCount = await _dbContext.Users.CountAsync();

        int totalPagesCount = (int)Math.Ceiling(totalUsersCount / (double)request.PageSize);

        usersListResponse.UserList.AddRange((await _dbContext.Users.Select(u => new GetUserResponse()
        {
            Id = u.Id,
            Name = u.Name,
            Age = u.Age,
            Email = u.Email,
            Address = u.Address
        })
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .ToListAsync()));

        usersListResponse.TotalCount = totalUsersCount;

        usersListResponse.TotalPages = totalPagesCount;

        usersListResponse.CurrentPage = request.PageNumber;

        return await Task.FromResult(usersListResponse);
    }
    public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        if (request.Id <= default(int))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id Must Be Greater Than Zero"));

        UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

        if (user is null)
            throw new RpcException(new Status(StatusCode.NotFound, "User Not Exists"));

        user.Email = request.Email;
        user.Address = request.Address;
        user.Age = request.Age;
        user.Name = request.Name;

        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateUserResponse()
        {
            Id = user.Id
        });
    }
    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        if (request.Id <= default(int))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User Id Must Be Greater Than Zero"));

        UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

        if (user is null)
            throw new RpcException(new Status(StatusCode.NotFound, "User Not Exists"));

        _dbContext.Remove(user);

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new DeleteUserResponse()
        {
            Id = user.Id
        });
    }


}
