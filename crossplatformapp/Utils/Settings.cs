using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace crossplatformapp.Utils
{

    public sealed class Settings : INotifyPropertyChanged
    {
        private static readonly Lazy<Settings> _instance = new(Load);

        public static Settings Current => _instance.Value;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Helpers

        private void SetField<T>(ref T field, T value, string propertyName)
        {
            if (Equals(field, value))
                return;

            field = value;
            Save();

            PropertyChanged?.Invoke(this, new(propertyName));
        }

        #endregion

        #region Persisted settings

        private bool isCompact = false;
        public bool IsCompact
        {
            get => isCompact;
            set => SetField(ref isCompact, value, nameof(IsCompact));
        }

        private bool _hideOnClose = false;
        public bool HideOnClose
        {
            get => _hideOnClose;
            set => SetField(ref _hideOnClose, value, nameof(HideOnClose));
        }

        #endregion

        private static Settings Load()
        {
            try
            {
                if (!Directory.Exists(PlatformPaths.BaseDirectory))
                    Directory.CreateDirectory(PlatformPaths.BaseDirectory);

                if (!File.Exists(PlatformPaths.SettingsLocation))
                {
                    var s = new Settings();
                    s.Save();
                    return s;
                }

                var json = File.ReadAllText(PlatformPaths.SettingsLocation);

                var settings = JsonSerializer.Deserialize<Settings>(json, JsonOptions);

                if (settings == null)
                    return new Settings();

                return settings;
            }
            catch
            {
                return new Settings();
            }
        }

        public void Save()
        {
            if (!Directory.Exists(PlatformPaths.BaseDirectory))
                    Directory.CreateDirectory(PlatformPaths.BaseDirectory);

            var json = JsonSerializer.Serialize(this, JsonOptions);

            var temp = PlatformPaths.SettingsLocation + ".tmp";

            File.WriteAllText(temp, json);

            File.Move(temp, PlatformPaths.SettingsLocation, true);
        }
    }
}