using System.ComponentModel;
using System.Text.Json;

namespace DRSaveTracker
{

    public sealed class Settings : INotifyPropertyChanged
    {
        private static readonly Lazy<Settings> _instance = new(Load);

        public static Settings Current => _instance.Value;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        private static readonly string DirectoryPath =
            Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Program.AppName
                );

        private static readonly string FilePath =
            Path.Combine(DirectoryPath, "settings.json");

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

        private bool _seenToolTip = false;
        public bool SeenToolTip
        {
            get => _seenToolTip;
            set => SetField(ref _seenToolTip, value, nameof(SeenToolTip));
        }
        private bool fetchRoomNames = true;
        public bool FetchRoomNames
        {
            get => fetchRoomNames;
            set => SetField(ref fetchRoomNames, value, nameof(FetchRoomNames));
        }

        private bool _hideOnClose = false;
        public bool HideOnClose
        {
            get => _hideOnClose;
            set => SetField(ref _hideOnClose, value, nameof(HideOnClose));
        }

        #endregion

        #region Dynamic settings

        public bool RunOnStartup
        {
            get => StartupManager.IsEnabled();

            set
            {
                StartupManager.SetEnabled(value);
                PropertyChanged?.Invoke(this, new(nameof(RunOnStartup)));
            }
        }

        #endregion

        private static Settings Load()
        {
            try
            {
                Directory.CreateDirectory(DirectoryPath);

                if (!File.Exists(FilePath))
                {
                    var s = new Settings();
                    s.Save();
                    return s;
                }

                var json = File.ReadAllText(FilePath);

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
            Directory.CreateDirectory(DirectoryPath);

            var json = JsonSerializer.Serialize(this, JsonOptions);

            var temp = FilePath + ".tmp";

            File.WriteAllText(temp, json);

            File.Move(temp, FilePath, true);
        }
    }
}