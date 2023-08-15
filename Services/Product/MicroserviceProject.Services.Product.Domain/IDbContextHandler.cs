using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain
{
    public interface IDbContextHandler
    {
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
