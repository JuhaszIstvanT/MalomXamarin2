using System;
using System.Collections.Generic;

namespace MalomXamarin2.Persistence
{
    public interface IStore
    {
        IEnumerable<string> GetFiles();

        DateTime GetModifiedTime(string name);
    }
}
