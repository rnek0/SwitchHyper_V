using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    /// <summary>
    /// ModifyTo defines starting states of hyper-v at windows 10 boot. See bcdedit cmd
    /// </summary>
    enum ModifyTo
    {
        auto,
        off
    }

    public partial class MainForm : Form
    {
        /// <summary>
        /// PATH bcdedit.exe 
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/bcdedit-command-line-options">bcdedit</a>
        /// <a href="https://docs.microsoft.com/en-us/windows-hardware/drivers/devtest/bcdedit--set">BCDEdit /set</a>
        /// </remarks>
        private string BCDEDIT = Environment.SystemDirectory + @"\bcdedit.exe"; // ex : @"C:\Windows\System32\bcdedit.exe"
        public MainForm()
        {
            InitializeComponent();
            ActualHyperVState();
        }

        /// <summary>
        /// SwitchHyperVState changes state of start hyper-v on windows 10 boot (v 18362.329)
        /// </summary>
        /// <param name="param">ModifyTo</param>
        private void SwitchHyperVState(ModifyTo param)
        {
            string requiredState = param.ToString();
            string setTo = "enable";
            if (param == ModifyTo.off)
            {
                setTo = "disable";
            }

            DialogResult result = MessageBox.Show($"Really {setTo} Hyper-V ?", $"Switching Hyper-V to {requiredState}", MessageBoxButtons.OKCancel);
            switch (result)
            {
                case DialogResult.OK:
                    {
                        ProcessStartInfo info = new ProcessStartInfo(BCDEDIT, $"/set hypervisorlaunchtype {requiredState}");
                        info.UseShellExecute = false;
                        info.RedirectStandardOutput = true;
                        string output = Process.Start(info).StandardOutput.ReadToEnd();
                        
                        DialogResult restart = MessageBox.Show("You need to reboot windows.", "System restart", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (restart == DialogResult.Yes)
                        {
                            RestartWindows();
                        }
                        break;
                    }
                case DialogResult.Cancel:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Is Hyper-V enabled ?
        /// </summary>
        private bool ActualHyperVState()
        {
            ProcessStartInfo info = new ProcessStartInfo(BCDEDIT, "");
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            string output = Process.Start(info).StandardOutput.ReadToEnd();
            string result;
            bool state;

            using (System.IO.StringReader readLines = new System.IO.StringReader(output)) { 
                
                while (true)
                {
                    string aLine = readLines.ReadLine();
                    if (aLine.Contains("hypervisorlaunchtype"))
                    {
                        if (aLine.Contains("Auto"))
                        {
                            result = "Hyper-V is ENABLED !";
                            label1.ForeColor = System.Drawing.Color.Gray;
                            btn_enable.Enabled = false;
                            btn_disable.Enabled = true;
                            state = true;
                            pictureBoxColor.Image = Properties.Resources.enabled;
                        }
                        else
                        {
                            result = "Hyper-V is DISABLED !";
                            label2.ForeColor = System.Drawing.Color.Gray;
                            btn_enable.Enabled = true;
                            btn_disable.Enabled = false;
                            state = false;
                            pictureBoxColor.Image = Properties.Resources.disabled;
                        }
                        break;
                    }
                }
            }
            MessageBox.Show(result, "Hyper-V STATE !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return state;
        }

        /// <summary>
        /// Enable Hyper-V
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_enable_Click(object sender, EventArgs e)
        {
            SwitchHyperVState(ModifyTo.auto);
        }

        /// <summary>
        /// Disable Hyper-V
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_disable_Click(object sender, EventArgs e)
        {
            SwitchHyperVState(ModifyTo.off);
        }

        /// <summary>
        /// Restart windows.
        /// </summary>
        private void RestartWindows()
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"cmd.exe /c shutdown -f -r -t 5",
                    Verb = "runas"
                };
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème de redemarrage windows :\r\n{ex.Message}");
            }
        }

        /// <summary>
        /// ESC key for exit
        /// </summary>
        private void ExitApp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)   
            {
                Application.Exit();
            }
        }
    }}
