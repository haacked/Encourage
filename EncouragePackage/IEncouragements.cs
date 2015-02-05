using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;

namespace Haack.Encourage
{
    public interface IEncouragements
    {
        IEnumerable<string> AllDiscouragements { get; set; }

        IEnumerable<string> AllEncouragements { get; set; }

        string GetRandomDiscouragement();

        string GetRandomEncouragement();

        SVsServiceProvider GetServiceProvider();
    }
}