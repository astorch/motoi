using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui.actions;
using motoi.platform.ui.menus;
using motoi.platform.ui.messaging;
using motoi.platform.ui.shells;
using motoi.platform.ui.toolbars;
using motoi.ui.windowsforms.controls;
using motoi.ui.windowsforms.jobs;
using xcite.collections;
using ToolBar = motoi.ui.windowsforms.toolbars.ToolBar;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="IMainWindow"/> based on Windows Forms 
    /// API.
    /// </summary>
    public class MainWindow : Window, IMainWindow {
        private ToolBar iApplicationToolBar;
        private readonly LinearList<Control> iTopControlAddQueue = new LinearList<Control>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public MainWindow() {
            InitializeComponents();
            StartPosition = FormStartPosition.Manual;
        }

        /// <summary>
        /// Tells the window to the add the given menu.
        /// </summary>
        /// <param name="menu">Menu and its item to add</param>
        public void AddMenu(MenuContribution menu) {
            if (MainMenuStrip == null) {
                MainMenuStrip = new MenuStrip();
                iTopControlAddQueue.Add(MainMenuStrip); // Add it to the queue to handle it later (cf. topmost control problem)
            }

            ToolStripMenuItem subMenu = new ToolStripMenuItem(menu.Label);
            MainMenuStrip.Items.Add(subMenu);

            using (IEnumerator<MenuItemContribution> enmtor = menu.MenuItems.GetEnumerator()) {
                while (enmtor.MoveNext()) {
                    MenuItemContribution menuItemContribution = enmtor.Current;

                    // Support separator
                    if (menuItemContribution.IsSeparator) {
                        ToolStripSeparator separator = new ToolStripSeparator();
                        subMenu.DropDownItems.Add(separator);
                        continue;
                    }

                    // Add normal menu items
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(menuItemContribution.Label);
                    subMenu.DropDownItems.Add(menuItem);
                    menuItem.Enabled = menuItemContribution.ActionHandler.IsEnabled;
                    menuItem.Click += (sender, args) => menuItemContribution.ActionHandler.Run();

                    if (menuItemContribution.ImageStream != null)
                        menuItem.Image = new Bitmap(menuItemContribution.ImageStream);

                    Keys shortcutKeys;
                    if (Enum.TryParse(menuItemContribution.Shortcut, true, out shortcutKeys)) {
                        menuItem.ShortcutKeys = shortcutKeys;
                        menuItem.ShowShortcutKeys = true;
                    }

                    menuItemContribution.ActionHandler.PropertyChanged += (sender, args) => {
                        if (args.PropertyName != nameof(IActionHandler.IsEnabled)) return;

                        // Get current value
                        bool isEnabled = ((IActionHandler) sender).IsEnabled;

                        // Delegate to UI thread
                        BeginInvoke(new Action(() => menuItem.Enabled = isEnabled));
                    };
                }
            }
        }

        /// <summary>
        /// Tells the window to add the given group to the toolbar.
        /// </summary>
        /// <param name="group"></param>
        public void AddToolbarGroup(ToolbarGroupContribution group) {
            bool groupExisting = true;

            if (iApplicationToolBar == null) {
                ToolStripContainer toolStripContainer = new ToolStripContainer {Dock = DockStyle.Top};
                Size oldSize = toolStripContainer.Size;
                toolStripContainer.Size = new Size(oldSize.Width, 32);
                Controls.Add(toolStripContainer);

                // All all remaining Controls
                AddTopControlsFromQueue();

                iApplicationToolBar = new ToolBar();
                toolStripContainer.ContentPanel.Controls.Add(iApplicationToolBar);
                groupExisting = false;
            }

            // If there has been added groups before we have to put a separator on it
            if (groupExisting)
                iApplicationToolBar.Items.Add(new ToolStripSeparator());

            using (IEnumerator<ToolbarItemContribution> itr = group.GroupItems.GetEnumerator()) {
                while (itr.MoveNext()) {
                    ToolbarItemContribution toolbarItem = itr.Current;
                    iApplicationToolBar.AddButton(null, toolbarItem.ImageStream, toolbarItem.ActionHandler,
                        toolbarItem.Label);
                }
            }
        }

        /// <summary>
        /// Adds all items of the top control add queue to the controls collection in the order they  
        /// have been queued before.
        /// </summary>
        private void AddTopControlsFromQueue() {
            iTopControlAddQueue.ForEach(Controls.Add);
            iTopControlAddQueue.Clear();
        }

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponents() {
            // Reduce flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            IsMdiContainer = true;

            // Perspective
//            iPerspectivePane = new Panel {Dock = DockStyle.Fill, Margin = new Padding(0)};
//            Controls.Add(iPerspectivePane);

            // Status strip
            StatusStrip statusStrip = new StatusStrip {Dock = DockStyle.Bottom};
            Controls.Add(statusStrip);

            // Left filler
            statusStrip.Items.Add(new ToolStripStatusLabel {Spring = true});

            // Progress bar area
            ToolStripLabelledProgressBar toolStripProgressBar = new ToolStripLabelledProgressBar {ProgressVisible = false};
            statusStrip.Items.Add(toolStripProgressBar);

            ProgressMonitor.ToolStripProgressBar = toolStripProgressBar;
        }

        #region IUIInvoker

        /// <summary>
        /// Invokes the given <paramref name="action"/> on the UI thread. It blocks the current thread until 
        /// the action has been executed.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        void IUIInvoker.Invoke(Action action) {
            base.Invoke(action);
        }

        /// <summary>
        /// Invokes the given <paramref name="action"/> on the UI thread using the given <paramref name="value"/>. 
        /// It blocks the current thread until the action has been executed.
        /// </summary>
        /// <typeparam name="TObject">Type of the action parameter</typeparam>
        /// <param name="action">Action to be executed</param>
        /// <param name="value">Action argument</param>
        void IUIInvoker.Invoke<TObject>(Action<TObject> action, TObject value) {
            base.Invoke(action, value);
        }

        /// <summary>
        /// Invokes the given <paramref name="action"/> asynchronously on the UI thread. It doesn't block the current thread.
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of the operation</returns>
        IAsyncResult IUIInvoker.InvokeAsync(Action action) {
            return base.BeginInvoke(action);
        }

        /// <summary>
        /// Invokes the given <paramref name="action"/> asynchronously on the UI thread. It doesn't block the current thread.
        /// </summary>
        /// <typeparam name="TObject">Type of the action parameter</typeparam>
        /// <param name="action">Action to be executed</param>
        /// <param name="value">Action argument</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of the operation</returns>
        IAsyncResult IUIInvoker.InvokeAsync<TObject>(Action<TObject> action, TObject value) {
            return base.BeginInvoke(action, value);
        }

        /// <summary>
        /// Returns TRUE if current thread is not the UI thread and therefore an <see cref="IUIInvoker.Invoke"/> is required 
        /// to execute an action by it.
        /// </summary>
        /// <returns>TRUE if the current thread is not the UI thread</returns>
        bool IUIInvoker.RequiresInvoke() {
            return InvokeRequired;
        }

        #endregion
    }
}