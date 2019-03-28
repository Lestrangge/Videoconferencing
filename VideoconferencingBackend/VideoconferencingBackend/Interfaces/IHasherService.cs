using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoconferencingBackend.Interfaces
{
    public interface IHasherService
    {
        string Hash(string toHash);
    }
}
