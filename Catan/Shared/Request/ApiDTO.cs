using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Request
{
    public class ApiDTO<T>
    {
        public bool Success { get; set; }
        public T? Value { get; set; }
    }
}
