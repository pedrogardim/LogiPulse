using MediatR;

namespace LogiPulse.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest;