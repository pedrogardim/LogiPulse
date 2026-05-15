using LogiPulse.Domain.Entities.Vehicles;

namespace LogiPulse.Application.Vehicles.DTOs;

public record CreateVehicleRequest(
    string ExternalId,
    string Name,
    string LicensePlate,
    VehicleType VehicleType
);