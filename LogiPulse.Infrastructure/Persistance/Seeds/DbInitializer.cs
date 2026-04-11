using LogiPulse.Domain.Entities.Dispatches;
using LogiPulse.Domain.Entities.Products;
using LogiPulse.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

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

        var dispatch = Dispatch.Create("D-001-003", product);

        context.Tenants.Add(tenant);
        context.Products.Add(product);
        context.ProductCategories.Add(productCategory);
        context.Dispatches.Add(dispatch);

        await context.SaveChangesAsync();
    }
}