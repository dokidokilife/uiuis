using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace uiui
{

    public partial class FrmMain : Form
    {
        private FrmMain Par = null;
        private bool _IsRunning = false;
        public bool IsRunning        {            get { return _IsRunning; } }

        [DllImport("winmm", CharSet = CharSet.Auto)]
        private static extern int mciSendString( string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public FrmMain(object _par)
        {
            InitializeComponent();
            if(_par != null) { Par = (FrmMain)_par; }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            bool run = true;
            if (Par != null)
            {
                if (Par.IsRunning)
                {
                    run = false;
                }
            }
            this.timer1.Enabled = run;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            FrmMain newFrm = new FrmMain(this);
            newFrm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (string drive in Environment.GetLogicalDrives())
            {
                DriveInfo di = new DriveInfo(drive);
                if (di.DriveType == DriveType.CDRom)
                {
                    int ret = int.MinValue;
                    try
                    {
                        ret = mciSendString("open " + "\"" + drive  + "\"" + " type cdaudio alias orator", null, 0, IntPtr.Zero);
                        ret = mciSendString("set orator door open", null, 0, IntPtr.Zero);
                        ret = mciSendString("close orator", null, 0, IntPtr.Zero);
                        //ret = mciSendString("Set CDAudio Door Open Wait", null, 0, IntPtr.Zero);
                    }
                    catch(Exception ex)
                    {
                    }
                }
            }
        }
    }
}
