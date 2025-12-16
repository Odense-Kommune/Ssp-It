using System;
using System.ComponentModel.DataAnnotations;

namespace Dk.Odense.SSP.Core
{
    public class Entity : IEntity
    {
        public Entity()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
    }
}
