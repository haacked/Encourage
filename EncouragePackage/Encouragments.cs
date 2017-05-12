﻿using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Haack.Encourage
{
    [Export(typeof(IEncouragements))]
    public class Encouragements : IEncouragements
    {
        const string CollectionPath = "Encouragements";
        const string PropertyName = "AllEncouragements";

        static readonly Random random = new Random();
        static readonly string[] defaultEncouragements = new[]
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
            "Nnnnailed it!",
            "much actions!",
            "wow!",
            "many algorithm!",
            "such array!",
            "so program!",
            "amaze!",
            "many bracket!",
            "such save!",
            "very code!",
            "very compile!",
            "amaze!",
            "much loops!",
            "very function!",
            "amaze condition!",
            "so logic!",
            "very class!",
            "such name!",
            "excite!",
            "many operand!",
            "so recurse!",
            "much syntax!",
            "very system!",
            "much smarts!"
        };

        readonly List<string> encouragements = new List<string>(defaultEncouragements);
        readonly WritableSettingsStore writableSettingsStore;

        public IEnumerable<string> AllEncouragements
        {
            get { return encouragements; }
            set
            {
                encouragements.Clear();
                encouragements.AddRange(value);
                if (encouragements.Count == 0)
                {
                    encouragements.AddRange(defaultEncouragements);
                }
                SaveSettings();
            }
        }

        [ImportingConstructor]
        public Encouragements(SVsServiceProvider vsServiceProvider)
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count);
            return encouragements[randomIndex];
        }

        void LoadSettings()
        {
            try
            {
                if (writableSettingsStore.PropertyExists(CollectionPath, PropertyName))
                {
                    string value = writableSettingsStore.GetString(CollectionPath, PropertyName);
                    AllEncouragements = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        void SaveSettings()
        {
            try
            {
                if (!writableSettingsStore.CollectionExists(CollectionPath))
                {
                    writableSettingsStore.CreateCollection(CollectionPath);
                }

                string value = string.Join(Environment.NewLine, encouragements);
                writableSettingsStore.SetString(CollectionPath, PropertyName, value);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}
