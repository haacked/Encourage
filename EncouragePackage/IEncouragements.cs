using System.Collections.Generic;
namespace Haack.Encourage
{
    public interface IEncouragements
    {
        IEnumerable<string> AllEncouragements { get; set; }

        string GetRandomEncouragement();
    }
}