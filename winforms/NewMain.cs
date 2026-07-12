using DRSTCore;
using System.Data;

namespace DRSaveTracker
{
    public partial class NewMain : Form
    {
        public NewMain()
        {
            InitializeComponent();
            rmi = new(cacheFolder);
            settings = Settings.Current;
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-s"))
                _silent = true;
        }
        protected override void SetVisibleCore(bool value)
        {
            if (_silent)
            {
                notifyIcon1.Visible = true;
                value = false;
                if (!IsHandleCreated)
                    CreateHandle();
            }
            base.SetVisibleCore(value);
        }
        private bool _silent = false;
        private readonly Settings settings;
        private readonly RoomMapper rmi;
        private static readonly string saveFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "DELTARUNE");
        private static readonly string backupFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            Program.AppName,
            "Backups");
        private static readonly string cacheFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            Program.AppName,
            "Cache");
        private static void UpdateListViewItem(ListViewItem lvi, SaveFileInfo svi)
        {
            lvi.Text = svi.Slot.ToString();
            if (lvi.SubItems.Count > 1)
                lvi.SubItems[1].Text = svi.Name;
            else
                lvi.SubItems.Add(svi.Name);
            if (lvi.SubItems.Count > 2)
                lvi.SubItems[2].Text = svi.PlayTime.ToString();
            else
                lvi.SubItems.Add(svi.PlayTime.ToString());
            if (lvi.SubItems.Count > 3)
                lvi.SubItems[3].Text = svi.RoomName ?? svi.Room.ToString();
            else
                lvi.SubItems.Add(svi.RoomName ?? svi.Room.ToString());
            if (lvi.SubItems.Count > 4)
                lvi.SubItems[4].Text = svi.LastWrite.ToString();
            else
                lvi.SubItems.Add(svi.LastWrite.ToString());
        }

        private void UpdateFileInfo(string filename)
        {
            var lvItem = listView1.Items
                .Cast<ListViewItem>()
                .FirstOrDefault(i => (string?)i.Tag == filename);
            if (lvItem == null)
                return;
            var svi = new SaveFileInfo(filename, rmi);
            UpdateListViewItem(lvItem, svi);
            if (svi.FileExists)
                svi.MakeBackup(backupFolder);
        }
        private void InitSaveInfoLV()
        {
            mainWatcher.EnableRaisingEvents = false;
            listView1.Groups.Clear();
            listView1.Items.Clear();
            var saves = Directory.GetFiles(saveFolder, "filech*")
                .Select(s => new SaveFileInfo(s, rmi))
                .Where(s => s.FileExists);
            if (!Directory.Exists(backupFolder))
            {
                toolStripStatusLabel1.Text = "Making initial backups...";
                foreach (var save in saves)
                {
                    save.MakeBackup(backupFolder);
                }
            }
            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);
            var backups = Directory
                .GetDirectories(
                    backupFolder,
                    "filech*",
                    SearchOption.TopDirectoryOnly)
                .Select(s => new SaveFileInfo(s, rmi));
            var allSaves = saves
                .Concat(backups)
                .GroupBy(g => g.OriginalFileName)
                .Select(g => g.First());
            var chapters = allSaves.GroupBy(s => s.Chapter).ToArray();
            toolStripStatusLabel1.Text = "Updating UI";
            foreach (var chapter in chapters)
            {
                ListViewGroup lvg = new(string.Format("Chapter {0}", chapter.Key));

                listView1.Groups.Add(lvg);
                foreach (var slot in chapter)
                {
                    ListViewItem lvi = new(lvg)
                    {
                        Tag = slot.FileName,
                        ToolTipText = slot.FileName,
                    };
                    UpdateListViewItem(lvi, slot);
                    listView1.Items.Add(lvi);

                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            mainWatcher.EnableRaisingEvents = true;
        }
        private async void NewMain_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("-s"))
                Visible = false;
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            toolStripStatusLabel1.Text = "Loading room info...";
            if (settings.FetchRoomNames)
                await rmi.LoadRoomInfo(null);
            toolStripStatusLabel1.Text = "Reading save files...";
            if (!Directory.Exists(saveFolder))
            {
                MessageBox.Show("You don't even have save information about DELTARUNE, at least launch it once...");
                Application.Exit();
                return;
            }
            mainWatcher.Path = saveFolder;
            InitSaveInfoLV();
            toolStripStatusLabel1.Text = "Done";
        }

        private void mainWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => mainWatcher_Changed(sender, e));
                return;
            }
            var f = (sender as FileSystemWatcher)!;
            try
            {
                f.EnableRaisingEvents = false;
                //toolStripStatusLabel1.Text = string.Format("Updated via: {0};{1}", e.ChangeType.ToString(), e.FullPath);
                //if (e.ChangeType == WatcherChangeTypes.Created)
                //    RefreshSaveInfo();
                if ((e.ChangeType & (WatcherChangeTypes.Created | WatcherChangeTypes.Deleted)) != 0)
                    InitSaveInfoLV();
                UpdateFileInfo(e.FullPath);
                var lv2Tag = (string?)listView2.Tag;
                if (lv2Tag == null)
                    return;
                if (lv2Tag != e.FullPath)
                    return;
                UpdateBackupInfo(lv2Tag);
            }
            finally
            {
                f.EnableRaisingEvents = true;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listView1.SelectedItems
                .Cast<ListViewItem>()
                .Select(i =>
                {
                    if (i.Tag != null)
                        return (string)i.Tag;
                    else return null;
                }).FirstOrDefault();
            if (selected == null) return;
            UpdateBackupInfo(selected);
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {


        }
        private void UpdateBackupInfo(string filename)
        {
            listView2.Items.Clear();
            listView2.Groups.Clear();
            listView2.Tag = filename;
            var svi = new SaveFileInfo(filename, rmi);
            svi.GetBackups(backupFolder);
            if (svi.Backups.Length == 0)
            {
                listView2.Items.Add("No backups");
                return;
            }
            ListViewGroup latestVG = new("Latest");
            ListViewGroup previousVG = new("Previous");
            listView2.Groups.AddRange([latestVG, previousVG]);
            foreach (var backup in svi.Backups)
            {
                ListViewGroup group = svi.LastWrite == backup.LastWrite ? latestVG : previousVG;
                var lvi = new ListViewItem(backup.Name, group)
                {
                    Tag = backup.FileName,
                    ToolTipText = backup.FileName,
                };
                lvi.SubItems.Add(backup.PlayTime.ToString());
                lvi.SubItems.Add(backup.RoomName ?? backup.Room.ToString());
                lvi.SubItems.Add(backup.LastWrite.ToString());
                listView2.Items.Add(lvi);
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void listView2_ItemActivate(object sender, EventArgs e)
        {
            var selected = listView2.SelectedItems
                .Cast<ListViewItem>()
                .Select(i =>
                {
                    if (i?.Group?.Header != "Previous")
                        return null;
                    if (i.Tag != null)
                        return (string)i.Tag;
                    else return null;
                }).FirstOrDefault();
            if (selected == null) return;
            var svi = new SaveFileInfo(selected, rmi);
            var r = MessageBox.Show("Restore backup?",
                string.Format("Chapter {0} {1} slot", svi.Chapter, svi.Slot.ToString()),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Yes)
            {
                mainWatcher.EnableRaisingEvents = false;
                var resFileName = svi.RestoreToOriginal(saveFolder);
                UpdateFileInfo(resFileName);
                UpdateBackupInfo(resFileName);
                mainWatcher.EnableRaisingEvents = true;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _silent = false;
            Show();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void NewMain_VisibleChanged(object sender, EventArgs e)
        {
            Form f = (Form)sender;
            if (f == null) return;
            notifyIcon1.Visible = !f.Visible;
            if (!f.Visible)
            {
                if (!settings.SeenToolTip)
                {
                    notifyIcon1.ShowBalloonTip(1000, "DR Save Tracker", "The program is working in the background", ToolTipIcon.Info);
                    settings.SeenToolTip = true;
                }
            }
        }

        private void NewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!settings.HideOnClose) return;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
