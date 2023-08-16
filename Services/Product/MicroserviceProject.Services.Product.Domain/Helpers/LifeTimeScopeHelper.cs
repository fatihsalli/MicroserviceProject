using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Domain.Helpers
{
    public static class LifeTimeScopeHelper
    {
        public static Autofac.ILifetimeScope Scope { get; set; }
    }
}
