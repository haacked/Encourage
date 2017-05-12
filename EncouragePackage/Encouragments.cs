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
        private const string CollectionPath = "Encourage";
        private const string DiscouragementsPropertyName = "AllDiscouragements";
        private const string EncouragementsPropertyName = "AllEncouragements";

        private static readonly string[] defaultDiscouragements = new[] {
            // Thanks Phill
            "😱 Seriously?",
            "😷 That's a bad look.",
            "Burn it to the ground! 🔥",
            "😠 Torvalds frowns at you.",
            "🚶 Have you considered another career?",
            "You must hate your coworkers. 👹",
            "😡 You must hate yourself.",
            "Ha! Yeah, that'll work. 😄",
            "Are you just hitting keys at random?",
            "You code like a PM. 😐",
            "🍸Are you drinking?",
            "Who cares about uptime anyways, amirite?! 😏",
            "✨ YOLO! ✨"
        };

        private static readonly string[] defaultEncouragements = new[]
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

        private static readonly Random random = new Random();
        private readonly List<string> discouragements = new List<string>(defaultDiscouragements);
        private readonly List<string> encouragements = new List<string>(defaultEncouragements);
        private readonly WritableSettingsStore writableSettingsStore;
        private SVsServiceProvider _ServiceProvider;

        [ImportingConstructor]
        public Encouragements(SVsServiceProvider vsServiceProvider)
        {
            _ServiceProvider = vsServiceProvider;

            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public IEnumerable<string> AllDiscouragements
        {
            get { return discouragements; }
            set
            {
                discouragements.Clear();
                discouragements.AddRange(value);

                if (discouragements.Count == 0)
                {
                    discouragements.AddRange(defaultDiscouragements);
                }

                SaveSettings();
            }
        }

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

        public string GetRandomDiscouragement()
        {
            int randomIndex = random.Next(0, discouragements.Count);

            return discouragements[randomIndex];
        }

        public string GetRandomEncouragement()
        {
            int randomIndex = random.Next(0, encouragements.Count);
            return encouragements[randomIndex];
        }

        public SVsServiceProvider GetServiceProvider()
        {
            return this._ServiceProvider;
        }

        private void LoadSettings()
        {
            try
            {
                if (writableSettingsStore.PropertyExists(CollectionPath, EncouragementsPropertyName))
                {
                    string value = writableSettingsStore.GetString(CollectionPath, EncouragementsPropertyName);
                    AllEncouragements = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, DiscouragementsPropertyName))
                {
                    string value = writableSettingsStore.GetString(CollectionPath, DiscouragementsPropertyName);
                    AllDiscouragements = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private void SaveSettings()
        {
            try
            {
                if (!writableSettingsStore.CollectionExists(CollectionPath))
                {
                    writableSettingsStore.CreateCollection(CollectionPath);
                }

                writableSettingsStore.SetString(CollectionPath, EncouragementsPropertyName, string.Join(Environment.NewLine, encouragements));
                writableSettingsStore.SetString(CollectionPath, DiscouragementsPropertyName, string.Join(Environment.NewLine, discouragements));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}