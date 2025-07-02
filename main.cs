using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

class SaveData : Form
{
    private Button btnExport;
    private Button btnLoad;
    private Label lblTitle;

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new SaveData());
    }

    public SaveData()
    {
        this.Text = "TruckClusterer";
        this.ClientSize = new Size(360, 160);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        lblTitle = new Label()
        {
            Text = "Convert Clustertruck saves between Registry \nEntries and portable .reg files.",
            AutoSize = true,
            Location = new Point(20, 20)
        };

        btnExport = new Button()
        {
            Text = "Export Save",
            Location = new Point(20, 60),
            Size = new Size(140, 30)
        };
        btnExport.Click += new EventHandler(Export_Click);

        btnLoad = new Button()
        {
            Text = "Load Save",
            Location = new Point(180, 60),
            Size = new Size(140, 30)
        };
        btnLoad.Click += new EventHandler(Load_Click);

        this.Controls.Add(lblTitle);
        this.Controls.Add(btnExport);
        this.Controls.Add(btnLoad);
    }

    private void Export_Click(object sender, EventArgs e)
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
                MessageBox.Show("Export successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Export failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void Load_Click(object sender, EventArgs e)
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
                MessageBox.Show("Load successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failed to import registry file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
