using CatalogService.Domain.AggregateModels.CatalogAggregate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;

namespace CatalogService.Infrastructure.Context
{
    public class CatalogContextSeed
    {
        private static readonly List<CatalogBrand> catalogBrands = new List<CatalogBrand>(){
                    new CatalogBrand() { Brand = "Amd"},
                    new CatalogBrand() { Brand = "Intel" },
                    new CatalogBrand() { Brand = "Asus" },
                    new CatalogBrand() { Brand = "Kingston" },
                    new CatalogBrand() { Brand = "MSI" }
                };
        private static readonly List<CatalogType> catalogTypes = new List<CatalogType>()
                {
                    new CatalogType() { Type = "Cpu"},
                    new CatalogType() { Type = "Ram" },
                    new CatalogType() { Type = "Motherboard" },
                    new CatalogType() { Type = "Gpu" },
                    new CatalogType() { Type = "Case" },
                    new CatalogType() { Type = "Monitor" },

                };
        private static readonly List<CatalogItem> catalogItems = new List<CatalogItem>()
        {
            new CatalogItem()
            {
                Description="ASUS Prime B550-PLUS AMD AM4 Zen 3 Ryzen 5000 & 3rd Gen Ryzen ATX Motherboard (PCIe 4.0, ECC Memory, 1Gb LAN, HDMI 2.1, DisPlayPort 1.2 (4K@60HZ), Addressable Gen 2 RGB Header and Aura Sync).",
                Name="ASUS Prime B550-PLUS",
                AvailableStock=10,
                PictureUri="1.jpg",
                Price=114.99M,
                PictureFileName="1.jpg",
                CatalogBrandId=catalogBrands.FirstOrDefault(z=>z.Brand=="Asus").Id,
                CatalogTypeId=catalogTypes.FirstOrDefault(z=>z.Type=="Motherboard").Id
            },
             new CatalogItem()
            {
                Description="ASUS TUF Gaming GT501 Mid-Tower Computer Case for up to EATX Motherboards with USB 3.0 Front Panel Cases GT501/GRY/WITH Handle",
                Name="ASUS TUF Gaming GT501",
                AvailableStock=22,
                PictureUri="2.jpg",
                Price=134.99M,
                PictureFileName="2.jpg",
                CatalogBrandId=catalogBrands.FirstOrDefault(z=>z.Brand=="Asus").Id,
                CatalogTypeId=catalogTypes.FirstOrDefault(z=>z.Type=="Case").Id
            },
             new CatalogItem()
            {
                Description="Kingston Fury Beast 32GB (2x16GB) 3600MT/s DDR4 CL18 Desktop Memory Kit of 2 KF436C18BBK2/32",
                Name="Kingston Fury Beast 32GB",
                AvailableStock=12,
                PictureUri="3.jpg",
                Price=131.99M,
                PictureFileName="3.jpg",
                CatalogBrandId=catalogBrands.FirstOrDefault(z=>z.Brand=="Kingston").Id,
                CatalogTypeId=catalogTypes.FirstOrDefault(z=>z.Type=="Ram").Id
            }
             ,
             new CatalogItem()
            {
                Description="Kingston FURY Beast RGB 32GB (2x16GB) 3200MT/s DDR4 CL16 Desktop Memory Kit of 2 | Infrared Syncing | Intel XMP | AMD Ryzen | Plug n Play | KF432C16BBAK2/32",
                Name="Kingston FURY Beast RGB 32GB",
                AvailableStock=52,
                PictureUri="4.jpg",
                Price=145.99M,
                PictureFileName="4.jpg",
                CatalogBrandId=catalogBrands.FirstOrDefault(z=>z.Brand=="Kingston").Id,
                CatalogTypeId=catalogTypes.FirstOrDefault(z=>z.Type=="Ram").Id
            }
        };
        public async Task SeedAsync(CatalogContext context)
        {
            var policy = Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                    }
                );
            await policy.ExecuteAsync(() => ProcessSeeding(context));
        }


        private async Task ProcessSeeding(CatalogContext context)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(catalogBrands);

                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(catalogTypes);

                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(catalogItems);

                await context.SaveChangesAsync();

               
            }
        }

        
    }
}
