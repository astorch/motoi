using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.bindings;
using motoi.platform.ui.images;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation for <see cref="IWindow"/> based on 
    /// Windows Forms API.
    /// </summary>
    public class Window : Form, IWindow {
        private EWindowResizeMode _windowResizeMode;
        private EWindowStyle _windowStyle;

        /// <inheritdoc />
        public Window() {
            InitializeComponents();
        }

        /// <summary> Returns the current window content or does set it. </summary>
        protected IWidgetCompound WindowContent { get; set; }
        
        #region IWindow

        /// <inheritdoc />
        IShell IShell.Owner {
            get { return (IShell) Owner; } 
            set { Owner = (Form) value; } 
        }

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PWindow.GetModelValue(this, PWindow.VisibilityProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            } 
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PWindow.GetModelValue(this, PWindow.EnabledProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <inheritdoc />
        string IWindow.WindowTitle {
            get { return Text; }
            set { Text = value; }
        }

        /// <inheritdoc />
        int IShell.Width {
            get { return PWindow.GetModelValue(this, PWindow.WidthProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.WidthProperty, value);
                Width = value;
            }
        }

        /// <inheritdoc />
        int IShell.Height {
            get { return PWindow.GetModelValue(this, PWindow.HeightProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.HeightProperty, value);
                Height = value;
            }
        }

        /// <inheritdoc />
        int IShell.TopLocation {
            get { return PWindow.GetModelValue(this, PWindow.TopLocationProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.TopLocationProperty, value);
                Location = new Point(Location.X, value);
            }
        }

        /// <inheritdoc />
        int IShell.LeftLocation {
            get { return PWindow.GetModelValue(this, PWindow.LeftLocationProperty); }
            set {
                PWindow.SetModelValue(this, PWindow.LeftLocationProperty, value);
                Location = new Point(value, Location.Y);
            }
        }

        /// <inheritdoc />
        EWindowState IWindow.WindowState {
            get { return Converter.WindowState.ConvertFrom(WindowState); }
            set { WindowState = Converter.WindowState.ConvertFrom(value); }
        }

        /// <inheritdoc />
        ImageDescriptor IWindow.WindowIcon {
            get { throw new NotSupportedException(); }
            set { Icon = ConvertToIcon(value); }
        }

        /// <inheritdoc />
        void IShell.SetContent(IWidgetCompound widgetCompound) {
            using (this.DeferLayout()) {
                // Remove the old content 
                if (WindowContent != null) {
                    Control oldContentControl = CastUtil.Cast<Control>(WindowContent);
                    Controls.Remove(oldContentControl);
                }

                // Store current window content
                WindowContent = widgetCompound;
                if (widgetCompound == null) return;

                Control newContentControl = CastUtil.Cast<Control>(widgetCompound);
                newContentControl.Dock = DockStyle.Fill;

                // Add the new content
                Controls.Add(newContentControl);

                // Due to a bug of WeifenLou DockPanel the content panel should be at first position
                Controls.SetChildIndex(newContentControl, 0);
            }
        }

        /// <inheritdoc />
        IWidgetCompound IShell.Content { get { return WindowContent; } }

        /// <inheritdoc />
        EWindowStartupLocation IWindow.WindowStartupLocation {
            get { return Converter.WindowStartupLocation.ConvertFrom(StartPosition); }
            set { StartPosition = Converter.WindowStartupLocation.ConvertFrom(value); }
        }

        /// <inheritdoc />
        public EWindowStyle WindowStyle {
            get { return _windowStyle; }
            set {
                _windowStyle = value;
                UpdateFormBorderStyle();
            }
        }

        /// <inheritdoc />
        public EWindowResizeMode WindowResizeMode {
            get { return _windowResizeMode; }
            set {
                _windowResizeMode = value;
                UpdateFormBorderStyle();
            } 
        }

        /// <summary>
        /// Updates the form border style according to the current model state based on 
        /// <see cref="WindowResizeMode"/> and <see cref="WindowStyle"/>.
        /// </summary>
        private void UpdateFormBorderStyle()  {
            EWindowStyle windowStyle = _windowStyle;
            if (windowStyle == EWindowStyle.BlankWindow) {
                FormBorderStyle = FormBorderStyle.None;
                return;
            }

            EWindowResizeMode resizeMode = _windowResizeMode;
            if (resizeMode == EWindowResizeMode.NoResize) {
                MinimizeBox = false;
                MaximizeBox = false;

                FormBorderStyle = windowStyle == EWindowStyle.DialogWindow
                    ? FormBorderStyle.FixedDialog
                    : FormBorderStyle.FixedSingle
                    ;
            } else if (resizeMode == EWindowResizeMode.CanMinimize) {
                MinimizeBox = true;
                MaximizeBox = false;

                FormBorderStyle = windowStyle == EWindowStyle.DialogWindow
                    ? FormBorderStyle.FixedDialog
                    : FormBorderStyle.FixedSingle
                    ;
            } else { // CanResize
                MinimizeBox = true;
                MaximizeBox = true;

                FormBorderStyle = windowStyle == EWindowStyle.DialogWindow
                    ? FormBorderStyle.SizableToolWindow
                    : FormBorderStyle.Sizable
                    ;
            }
        }

        /// <summary>
        /// Converts the given <paramref name="imageDescriptor"/> to an <see cref="Icon"/>.
        /// </summary>
        /// <param name="imageDescriptor">Image descriptor to convert</param>
        /// <returns>Instance of <see cref="Icon"/></returns>
        private Icon ConvertToIcon(ImageDescriptor imageDescriptor) {         
            using (Stream stream = imageDescriptor.ImageStream) {
                using (Bitmap bitmap = (Bitmap) Image.FromStream(stream)) {
                    IntPtr hicon = bitmap.GetHicon();
                    return Icon.FromHandle(hicon);
                }
            }
        }

        #endregion

        /// <summary> Performs an initialization of the used components. </summary>
        private void InitializeComponents() {
            WindowStyle = EWindowStyle.DefaultWindow;
            WindowResizeMode = EWindowResizeMode.CanResize;
            Closing += OnClosing;
            Closed += OnClosed;
        }

        /// <summary> Is invoked when the form has been closed. </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnClosed(object sender, EventArgs eventArgs) {
            OnWindowClosed();

            // Clean up
            Closing -= OnClosing;
            Closed -= OnClosed;
            Controls.Clear();
        }

        /// <summary> Is invoked before the form will be closed. </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="cancelEventArgs">Event arguments</param>
        private void OnClosing(object sender, CancelEventArgs cancelEventArgs) {
            cancelEventArgs.Cancel = !OnWindowClosing();
        }

        /// <inheritdoc />
        protected override void OnLocationChanged(EventArgs e) {
            base.OnLocationChanged(e);

            PWindow.SetModelValue(this, PWindow.TopLocationProperty, Location.Y, EBindingSourceUpdateReason.PropertyChanged);
            PWindow.SetModelValue(this, PWindow.LeftLocationProperty, Location.X, EBindingSourceUpdateReason.PropertyChanged);
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);

            PWindow.SetModelValue(this, PWindow.WidthProperty, Width, EBindingSourceUpdateReason.PropertyChanged);
            PWindow.SetModelValue(this, PWindow.HeightProperty, Height, EBindingSourceUpdateReason.PropertyChanged);
        }

        /// <summary>
        /// Is invoked when a window close request has been received. Return TRUE if 
        /// window can be closed. This method is intended to be overriden by clients. 
        /// The default implementation returns TRUE.
        /// </summary>
        /// <returns>TRUE if the window can be closed</returns>
        protected virtual bool OnWindowClosing() {
            // Clients may override
            return true;
        }

        /// <summary>
        /// Is invoked when the window has been closed. This method is intended 
        /// to be overriden by clients.
        /// </summary>
        protected virtual void OnWindowClosed() {
            // Clients may override
        }
    }
}