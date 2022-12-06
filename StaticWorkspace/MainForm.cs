using System;
using System.Windows.Forms;
using VirtualDesktop;
using NonInvasiveKeyboardHookLibrary;

namespace StaticWorkspace
{
    public partial class MainForm : Form
    {
        private const int MAX_STATIC_VIRTUALDESKTOP = 10;
        private KeyboardHookManager keyboardHookManager;

        private Desktop currentDesktop;
        private Desktop previousDesktop;
        private Desktop[] staticDesktops;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeStaticVirtualDesktop();
            InitializeKeyboardHook();
            HideAppWindow();
        }

        private void InitializeStaticVirtualDesktop()
        {
            staticDesktops = new Desktop[MAX_STATIC_VIRTUALDESKTOP];

            int totalExistVirtualDesktop = Desktop.Count;
            int assignWorkspaceIndex = 0;

            bool isFinishAssignWorkspace = false;

            do
            {
                bool shouldCreate = ((assignWorkspaceIndex + 1) > totalExistVirtualDesktop);

                if (shouldCreate)
                {
                    staticDesktops[assignWorkspaceIndex] = Desktop.Create();
                }
                else
                {
                    staticDesktops[assignWorkspaceIndex] = Desktop.FromIndex(assignWorkspaceIndex);
                }

                staticDesktops[assignWorkspaceIndex].SetName($"Workspace:{assignWorkspaceIndex + 1}");

                isFinishAssignWorkspace = ((assignWorkspaceIndex + 1) == MAX_STATIC_VIRTUALDESKTOP);
                assignWorkspaceIndex += 1;
            }
            while (!isFinishAssignWorkspace);

            previousDesktop = Desktop.Current;
        }

        private void InitializeKeyboardHook()
        {
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();

            // Switch Virtual Desktop
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N1, () =>
            {
                SwitchVirtualDesktop(0);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N2, () =>
            {
                SwitchVirtualDesktop(1);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N3, () =>
            {
                SwitchVirtualDesktop(2);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N4, () =>
            {
                SwitchVirtualDesktop(3);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N5, () =>
            {
                SwitchVirtualDesktop(4);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N6, () =>
            {
                SwitchVirtualDesktop(5);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N7, () =>
            {
                SwitchVirtualDesktop(6);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N8, () =>
            {
                SwitchVirtualDesktop(7);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N9, () =>
            {
                SwitchVirtualDesktop(8);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.N0, () =>
            {
                SwitchVirtualDesktop(9);

            }, blocking: true);

            // Switch previous
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.OEMComma, () =>
            {
                SwitchVirtualDesktopPrevious();

            }, blocking: true);

            // Switch next
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.OEMPeriod, () =>
            {
                SwitchVirtualDesktopNext();

            }, blocking: true);

            // Switch current & previous
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey, (int)VirtualKeys.OEM3, () =>
            {
                SwitchVirtualDesktopBackAndForth();

            }, blocking: true);

            // Move Window to Virtual Desktop
            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N1, () =>
            {
                MoveFocusWindowToVirtualDesktop(0);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N2, () =>
            {
                MoveFocusWindowToVirtualDesktop(1);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N3, () =>
            {
                MoveFocusWindowToVirtualDesktop(2);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N4, () =>
            {
                MoveFocusWindowToVirtualDesktop(3);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N5, () =>
            {
                MoveFocusWindowToVirtualDesktop(4);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N6, () =>
            {
                MoveFocusWindowToVirtualDesktop(5);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N7, () =>
            {
                MoveFocusWindowToVirtualDesktop(6);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N8, () =>
            {
                MoveFocusWindowToVirtualDesktop(7);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N9, () =>
            {
                MoveFocusWindowToVirtualDesktop(8);

            }, blocking: true);

            keyboardHookManager.RegisterHotkey(NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift, (int)VirtualKeys.N0, () =>
            {
                MoveFocusWindowToVirtualDesktop(9);

            }, blocking: true);
        }

        private void HideAppWindow()
        {
            Visible = false;
            ShowInTaskbar = false;
            Hide();
        }

        private void SwitchVirtualDesktop(int index)
        {
            bool isIndexValid = (index >= 0) && (index < MAX_STATIC_VIRTUALDESKTOP);

            if (!isIndexValid)
            {
                return;
            }

            var desktop = Desktop.FromIndex(index);

            if (desktop.IsVisible)
            {
                return;
            }

            previousDesktop = Desktop.Current;
            currentDesktop = desktop;

            currentDesktop.MakeVisible();
            appNotifyIcon.Text = $"Workspace:{index + 1}";
        }

        private void SwitchVirtualDesktopNext()
        {
            int currentVirtualDesktopIndex = Array.IndexOf(staticDesktops, Desktop.Current);
            int nextVirtualDesktopIndex = (currentVirtualDesktopIndex + 1) >= MAX_STATIC_VIRTUALDESKTOP ? 0 : (currentVirtualDesktopIndex + 1);
            SwitchVirtualDesktop(nextVirtualDesktopIndex);
        }

        private void SwitchVirtualDesktopPrevious()
        {
            int currentVirtualDesktopIndex = Array.IndexOf(staticDesktops, Desktop.Current);
            int previousVirtualDesktopIndex = (currentVirtualDesktopIndex - 1) < 0 ? (MAX_STATIC_VIRTUALDESKTOP - 1) : (currentVirtualDesktopIndex - 1);
            SwitchVirtualDesktop(previousVirtualDesktopIndex);
        }

        private void SwitchVirtualDesktopBackAndForth()
        {
            currentDesktop = Desktop.Current;
            bool shouldChangeBackAndForth = (currentDesktop != previousDesktop);

            if (shouldChangeBackAndForth)
            {
                previousDesktop.MakeVisible();
                previousDesktop = currentDesktop;
            }
        }

        private void MoveFocusWindowToVirtualDesktop(int index)
        {
            try
            {
                var desktop = Desktop.FromIndex(index);
                desktop.MoveActiveWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // TODO : better name
        private void appNotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            bool shouldUpdateIconText = (Desktop.Current != currentDesktop);

            if (!shouldUpdateIconText)
            {
                return;
            }

            int virtualDesktopIndex = Array.IndexOf(staticDesktops, Desktop.Current);
            appNotifyIcon.Text = $"Workspace:{virtualDesktopIndex + 1}";

            currentDesktop = Desktop.Current;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

