using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Drivers;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using LogiPulse.Domain.Entities.Users;
using LogiPulse.Domain.Entities.Vehicles;
using LogiPulse.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LogiPulse.Infrastructure.Persistance.Seeds;

public class DbInitializer
{
    public static async Task SeedAsync(LogiPulseDbContext context)
    {
        if (await context.Tenants.AnyAsync()) return;

        var tenant = Tenant.Create(
            "LogiPulse",
            "123456789",
            "LogiPulse Logistics CORP");

        var productCategory = ProductCategory.Create(tenant.Id, "VAC", "Vaccines");

        productCategory.SetRequirement("TEMP", "C", 2, 3.4444m);

        var product = Product.Create(
            tenant.Id,
            "REF-PFIZER-01",
            "VAC-PFIZER",
            "Pfizer Vaccine",
            productCategory.Id);

        product.SetRequirement("TILT", "DEG", -5, 5);
        
        var originAddr = new Address("Rod. Hélio Smidt", "s/n", "Guarulhos", "SP", "07190-100", "Brazil", "Aeroporto");
        var dest1Addr = new Address("Av. Dr. Enéas Carvalho de Aguiar", "255", "São Paulo", "SP", "05403-000", "Brazil",
            "Cerqueira César");
        var dest2Addr = new Address("Rua Dr. Washington Pedro Lanzzelotti", "140", "Osasco", "SP", "06142-000",
            "Brazil", "Jardim Novo Osasco");

        var facilityWarehouse = Facility.Create(
            tenant,
            "CD-GRU-01",
            "CD Pfizer Guarulhos",
            "CD-GRU",
            FacilityType.DistributionCenter,
            originAddr,
            new Point(-46.473, -23.430) { SRID = 4326 });

        var deliveryPoint1 = Facility.Create(
            tenant,
            "HOSP-HC-SP",
            "Hospital das Clínicas SP",
            "HC-SP",
            FacilityType.DeliveryPoint,
            dest1Addr,
            new Point(-46.665, -23.557)  { SRID = 4326 });

        var deliveryPoint2 = Facility.Create(
            tenant,
            "HOSP-MUNIC-OSZ",
            "Hospital Municipal Osasco",
            "HM-OSZ",
            FacilityType.DeliveryPoint,
            dest2Addr,
            new Point(-46.789, -23.562)  { SRID = 4326 });

        var vehicle = Vehicle.Create(tenant.Id, "V-001", "Peugeot 123", "ABC-1234", VehicleType.SemiTruck);
        
        vehicle.SetCapability("TEMP", "C", 2, 4);
        vehicle.AssignToFacility(facilityWarehouse.Id);
        vehicle.Activate();

        var driverUser = User.Create(tenant.Id, Email.Create("test@user.com"), "Driver User", null);

        var driver = Driver.Create(tenant.Id, driverUser.Id, "D-001", "Driver 1", "123456789X", new DateOnly(2030,1,1), "123-555-6789");
        
        var dispatch = Dispatch.Create("D-001-003", tenant.Id, product.Id, facilityWarehouse.Id, deliveryPoint1.Id);
        dispatch.AssignVehicle(vehicle.Id);
        dispatch.AssignDriver(driver.Id);

        context.Tenants.Add(tenant);
        context.Products.Add(product);
        context.ProductCategories.Add(productCategory);
        context.Dispatches.Add(dispatch);
        context.Facilities.AddRange(facilityWarehouse, deliveryPoint1, deliveryPoint2);
        context.Vehicles.Add(vehicle);
        context.Users.Add(driverUser);
        context.Drivers.Add(driver);

        await context.SaveChangesAsync();
    }
}