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
            "So good!",
            "Bravo!",
            "You rock!",
            "Well done!",
            "I see what you did there!",
            "Genius work!",
            "Thumbs up!",
            "Coding win!",
            "FTW!",
            "Yep!",
            "Nnnnailed it!"
        }; 

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count);
            return encouragements[randomIndex];
        }
    }
}
