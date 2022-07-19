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
    public class DeliveryPointConfiguration : IEntityTypeConfiguration<DeliveryPoint>
    {
        public void Configure(EntityTypeBuilder<DeliveryPoint> builder)
        {
            builder.HasData(
            new DeliveryPoint
            {
                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                Address = "Minsk, T.Derginov 12",
            },
            new DeliveryPoint
            {
                Id = new Guid("80abbca8-664d-4b20-b7de-024715497d4a"),

                Address = "Minsk, P.Loginov 2",
            },
            new DeliveryPoint
            {
                Id = new Guid("80abbca1-664d-4b70-b5de-024705497d4a"),

                Address = "Minsk, A.Filimonov 74",
            }
            );
        }
    }
}
