using System.Text.Json.Serialization;
using LogiPulse.Domain.Entities.Dispatches;
using MediatR;

namespace LogiPulse.Application.Dispatches.Commands.UpdateDispatch;

public record UpdateDispatchCommand : IRequest<Dispatch>
{
    [JsonIgnore] public Guid Id;
}