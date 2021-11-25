using System.Collections.Generic;

namespace Haack.Encourage.Shared
{
    public interface IEncouragements
    {
        IEnumerable<string> AllEncouragements { get; set; }

        string GetRandomEncouragement();
    }
}