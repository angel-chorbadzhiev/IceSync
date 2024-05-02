using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceSync.Services
{
    public interface IStorageService
    {
        string JwtToken{ get; set; }
    }
}