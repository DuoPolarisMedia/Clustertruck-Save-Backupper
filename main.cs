using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

class SaveData : Form
{
    private Button btnBackup;
    private Button btnRestore;
    private Label lblTitle;

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new SaveData());
    }

    public SaveData()
    {
        this.Text = "Clustertruck Backupper";
        this.ClientSize = new Size(360, 160);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        lblTitle = new Label()
        {
            Text = "Backup and Restore your Clustertruck save",
            AutoSize = true,
            Location = new Point(20, 20)
        };

        btnBackup = new Button()
        {
            Text = "Backup Save",
            Location = new Point(20, 60),
            Size = new Size(140, 30)
        };
        btnBackup.Click += new EventHandler(Backup_Click);

        btnRestore = new Button()
        {
            Text = "Restore Save",
            Location = new Point(180, 60),
            Size = new Size(140, 30)
        };
        btnRestore.Click += new EventHandler(Restore_Click);

        this.Controls.Add(lblTitle);
        this.Controls.Add(btnBackup);
        this.Controls.Add(btnRestore);
    }

    private void Backup_Click(object sender, EventArgs e)
    {
        SaveFileDialog save = new SaveFileDialog();
        save.Filter = "Registry File (*.reg)|*.reg";
        save.FileName = "ClustertruckSave.reg";
        if (save.ShowDialog() == DialogResult.OK)
        {
            string regPath = "\"HKCU\\Software\\Landfall\\Clustertruck\"";
            string outputFile = "\"" + save.FileName + "\"";

            ProcessStartInfo psi = new ProcessStartInfo("reg.exe", "export " + regPath + " " + outputFile + " /y");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            try
            {
                Process.Start(psi).WaitForExit();
                MessageBox.Show("Backup successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failed to backup registry key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void Restore_Click(object sender, EventArgs e)
    {
        OpenFileDialog open = new OpenFileDialog();
        open.Filter = "Registry File (*.reg)|*.reg";
        if (open.ShowDialog() == DialogResult.OK)
        {
            string inputFile = "\"" + open.FileName + "\"";

            ProcessStartInfo psi = new ProcessStartInfo("reg.exe", "import " + inputFile);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            try
            {
                Process.Start(psi).WaitForExit();
                MessageBox.Show("Restore successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failed to import registry file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
