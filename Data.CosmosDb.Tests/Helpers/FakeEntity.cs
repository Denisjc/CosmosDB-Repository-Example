using System;
using Domain.Core.Model;

namespace Data.CosmosDb.Tests.Helpers
{
    public class FakeEntity : Entity<string>
    {
        public FakeEntity() : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Object = "Fake";
        }
    }
}
