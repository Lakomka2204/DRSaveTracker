using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DRSaveTracker
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            settings = Settings.Current;
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
