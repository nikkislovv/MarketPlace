using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasData(
            new Warehouse
            {
                Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                Address = "Minsk, L.Sapegi 5",
                
            },
            new Warehouse
            {
                Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                Address = "Grodno, P.Solim 35",
            }
            );
        }
    }
}
