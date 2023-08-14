using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.Entitites
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }

        // Sütun sırasını belirler. Sona atmak için 200'lü değerleri veriyoruz.
        [Column(Order =200)]
        public DateTime CreatedDate { get; set; }

        [Column(Order =201)]
        public DateTime ModifiedDate { get; set; }

        [Column(Order =202)]
        public bool IsActive { get; set; }

        public void SetIsActive(bool value)
        {
            IsActive = value;
        }

    }
}
