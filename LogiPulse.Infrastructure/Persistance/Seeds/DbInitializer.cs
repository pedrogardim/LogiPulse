using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Facilities;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
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

        var productCategory = ProductCategory.Create(tenant, "VAC", "Vaccines");

        productCategory.SetRule("TEMP", "C", 2, 3.4444m);

        var product = Product.Create(
            tenant,
            "REF-PFIZER-01",
            "VAC-PFIZER",
            "Pfizer Vaccine",
            productCategory);

        product.SetRule("TILT", "DEG", -5, 5);

        var facilityAddress = new Address("", "", "", "", "", "", "");

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

        var dispatch = Dispatch.Create("D-001-003", product, facilityWarehouse, deliveryPoint1);

        context.Tenants.Add(tenant);
        context.Products.Add(product);
        context.ProductCategories.Add(productCategory);
        context.Dispatches.Add(dispatch);
        context.Facilities.AddRange(facilityWarehouse, deliveryPoint1, deliveryPoint2);

        await context.SaveChangesAsync();
    }
}