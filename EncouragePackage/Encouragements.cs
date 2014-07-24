using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Haack.Encourage.Properties;

namespace Haack.Encourage
{
    [Export(typeof(IEncouragements))]
    public class Encouragements : IEncouragements
    {
        static readonly Random random = new Random();
        readonly List<string> encouragements = GetSettingsFromSettings();

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count);
            return encouragements[randomIndex];
        }

        static List<string> GetSettingsFromSettings()
        {
            return Settings.Default.Encouragements.Split(
                    new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
