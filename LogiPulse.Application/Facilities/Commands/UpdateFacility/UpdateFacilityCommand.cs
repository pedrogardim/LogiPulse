using System.Text.Json.Serialization;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Shared;
using MediatR;

namespace LogiPulse.Application.Facilities.Commands.UpdateFacility;

public record UpdateFacilityCommand : IRequest<Facility>
{
    [JsonIgnore] public Guid Id;
    public string? Name;
    public string? Code;
    public FacilityType? FacilityType;
    public double? Latitude;
    public double? Longitude;
    public Address? Address;
}