using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Haack.Encourage
{
    [Export(typeof(IEncouragements))]
    public class Encouragements : IEncouragements
    {
        static readonly Random random = new Random();
        readonly List<string> encouragements = new List<string>
        {
            "Nice Job!",
            "Way to go!",
            "Wow, nice change!",
            "So good!"
        }; 

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count - 1);
            return encouragements[randomIndex];
        }
    }
}
