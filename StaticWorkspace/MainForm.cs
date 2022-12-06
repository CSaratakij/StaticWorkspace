using System;
using System.Windows.Forms;
using VirtualDesktop;
using NonInvasiveKeyboardHookLibrary;

namespace StaticWorkspace
{
    public partial class MainForm : Form
    {
        private const int MAX_STATIC_VIRTUALDESKTOP = 10;
        private const string WORKSPACE_NAME_FORMAT = "Workspace:{0}";

        private struct Message
        {
            public const string SWITCH_WORKSPACE = "SwitchWorkspace";
            public const string SWITCH_WORKSPACE_NEXT = "SwitchWorkspaceNext";
            public const string SWITCH_WORKSPACE_PREVIOUS = "SwitchWorkspacePrevious";
            public const string SWITCH_WORKSPACE_BACK_AND_FORTH = "SwitchWorkspaceBackAndForth";
            public const string MOVE_WINDOW_TO_WORKSPACE = "MoveWindowToWorkspace";
            public const string MOVE_WINDOW_TO_WORKSPACE_NEXT = "MoveWindowToWorkspaceNext";
            public const string MOVE_WINDOW_TO_WORKSPACE_PREVIOUS = "MoveWindowToWorkspacePrevious";
        }

        private struct KeyMapping
        {
            public ModifierKeys modifierKeys;
            public VirtualKeys virtualKeys;
            public string message;
            public object data;
            public bool isBlocking;
        }

        private Desktop currentDesktop;
        private Desktop previousDesktop;
        private Desktop[] staticDesktops;

        private KeyboardHookManager keyboardHookManager;

        private KeyMapping[] keyMappings = new KeyMapping[]
        {
            // Switch Virtual Desktop
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N1,
                message = Message.SWITCH_WORKSPACE,
                data = 0,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N2,
                message = Message.SWITCH_WORKSPACE,
                data = 1,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N3,
                message = Message.SWITCH_WORKSPACE,
                data = 2,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N4,
                message = Message.SWITCH_WORKSPACE,
                data = 3,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N5,
                message = Message.SWITCH_WORKSPACE,
                data = 4,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N6,
                message = Message.SWITCH_WORKSPACE,
                data = 5,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N7,
                message = Message.SWITCH_WORKSPACE,
                data = 6,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N8,
                message = Message.SWITCH_WORKSPACE,
                data = 7,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N9,
                message = Message.SWITCH_WORKSPACE,
                data = 8,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.N0,
                message = Message.SWITCH_WORKSPACE,
                data = 9,
                isBlocking = true
            },

            // Switch Next
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.OEMPeriod,
                message = Message.SWITCH_WORKSPACE_NEXT,
                data = null,
                isBlocking = true
            },

            // Switch Previous
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.OEMComma,
                message = Message.SWITCH_WORKSPACE_PREVIOUS,
                data = null,
                isBlocking = true
            },

            // Switch Back and Forth
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey,
                virtualKeys = VirtualKeys.OEM3,
                message = Message.SWITCH_WORKSPACE_BACK_AND_FORTH,
                data = null,
                isBlocking = true
            },

            // Move window to workspace
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N1,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 0,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N2,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 1,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N3,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 2,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N4,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 3,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N5,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 4,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N6,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 5,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N7,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 6,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N8,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 7,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N9,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 8,
                isBlocking = true
            },
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.N0,
                message = Message.MOVE_WINDOW_TO_WORKSPACE,
                data = 9,
                isBlocking = true
            },

            // Move window to workspace next
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.OEMPeriod,
                message = Message.MOVE_WINDOW_TO_WORKSPACE_NEXT,
                data = null,
                isBlocking = true
            },

            // Move window to workspace previous
            new KeyMapping()
            {
                modifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys.WindowsKey | NonInvasiveKeyboardHookLibrary.ModifierKeys.Shift,
                virtualKeys = VirtualKeys.OEMComma,
                message = Message.MOVE_WINDOW_TO_WORKSPACE_PREVIOUS,
                data = null,
                isBlocking = true
            },
        };

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

        private void OnKeyMappingMessage(string message, object data)
        {
            switch (message)
            {
                case Message.SWITCH_WORKSPACE:
                {
                    int virtualDesktopIndex = (int)data;
                    SwitchVirtualDesktop(virtualDesktopIndex);
                }
                break;

                case Message.SWITCH_WORKSPACE_NEXT:
                {
                    SwitchVirtualDesktopNext();
                }
                break;

                case Message.SWITCH_WORKSPACE_PREVIOUS:
                {
                    SwitchVirtualDesktopPrevious();
                }
                break;

                case Message.SWITCH_WORKSPACE_BACK_AND_FORTH:
                {
                    SwitchVirtualDesktopBackAndForth();
                }
                break;

                case Message.MOVE_WINDOW_TO_WORKSPACE:
                {
                    int virtualDesktopIndex = (int)data;
                    MoveFocusWindowToVirtualDesktop(virtualDesktopIndex);
                }
                break;

                case Message.MOVE_WINDOW_TO_WORKSPACE_NEXT:
                {
                    MoveFocusWindowToVirtualDesktopNext();
                }
                break;

                case Message.MOVE_WINDOW_TO_WORKSPACE_PREVIOUS:
                {
                    MoveFocusWindowToVirtualDesktopPrevious();
                }
                break;

                default:
                    break;
            }
        }

        private void appNotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            bool shouldUpdateIconText = (Desktop.Current != currentDesktop);

            if (!shouldUpdateIconText)
            {
                return;
            }

            int virtualDesktopIndex = Desktop.FromDesktop(Desktop.Current);
            appNotifyIcon.Text = IndexToWorkspaceName(virtualDesktopIndex);

            currentDesktop = Desktop.Current;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

                string assignWorkspaceName = IndexToWorkspaceName(assignWorkspaceIndex);
                staticDesktops[assignWorkspaceIndex].SetName(assignWorkspaceName);

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

            for (int i = 0; i < keyMappings.Length; ++i)
            {
                var mapping = keyMappings[i];

                keyboardHookManager.RegisterHotkey(mapping.modifierKeys, (int)mapping.virtualKeys, () =>
                {
                    OnKeyMappingMessage(mapping.message, mapping.data);

                }, mapping.isBlocking);
            }
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
            appNotifyIcon.Text = IndexToWorkspaceName(index);
        }

        private void SwitchVirtualDesktopNext()
        {
            int nextVirtualDesktopIndex = GetNextVirtualDesktopIndex(Desktop.Current);
            SwitchVirtualDesktop(nextVirtualDesktopIndex);
        }

        private void SwitchVirtualDesktopPrevious()
        {
            int previousVirtualDesktopIndex = GetPreviousVirtualDesktopIndex(Desktop.Current);
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
                //MessageBox.Show(ex.Message);
            }
        }

        private void MoveFocusWindowToVirtualDesktopNext()
        {
            int nextVirtualDesktopIndex = GetNextVirtualDesktopIndex(Desktop.Current);
            MoveFocusWindowToVirtualDesktop(nextVirtualDesktopIndex);
        }

        private void MoveFocusWindowToVirtualDesktopPrevious()
        {
            int previousVirtualDesktopIndex = GetPreviousVirtualDesktopIndex(Desktop.Current);
            MoveFocusWindowToVirtualDesktop(previousVirtualDesktopIndex);
        }

        private int GetNextVirtualDesktopIndex(Desktop current)
        {
            int currentVirtualDesktopIndex = Desktop.FromDesktop(current);
            return (currentVirtualDesktopIndex + 1) >= MAX_STATIC_VIRTUALDESKTOP ? 0 : (currentVirtualDesktopIndex + 1);
        }

        private int GetPreviousVirtualDesktopIndex(Desktop current)
        {
            int currentVirtualDesktopIndex = Desktop.FromDesktop(current);
            return (currentVirtualDesktopIndex - 1) < 0 ? (MAX_STATIC_VIRTUALDESKTOP - 1) : (currentVirtualDesktopIndex - 1);
        }

        private string IndexToWorkspaceName(int index)
        {
            return string.Format(WORKSPACE_NAME_FORMAT, (index + 1));
        }
    }
}

