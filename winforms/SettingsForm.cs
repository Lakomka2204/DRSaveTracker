namespace DRSaveTracker
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            settings = Settings.Current;
            verLabel.Text = string.Format("Version {0}", Application.ProductVersion);
        }
        Settings settings;
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            startupCB.DataBindings.Add(
                nameof(CheckBox.Checked),
                settings,
                nameof(Settings.RunOnStartup),
                false,
                DataSourceUpdateMode.OnPropertyChanged);
            rmNameCB.DataBindings.Add(
                nameof(CheckBox.Checked),
                settings,
                nameof(Settings.FetchRoomNames),
                false,
                DataSourceUpdateMode.OnPropertyChanged);
            hideOnCloseCB.DataBindings.Add(
                nameof(CheckBox.Checked),
                settings,
                nameof(Settings.HideOnClose),
                false,
                DataSourceUpdateMode.OnPropertyChanged);
        }

    }
}
