using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.images;
using motoi.platform.ui.shells;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.shells
{
    /// <summary>
    /// Provides an implementation for <see cref="IWindow"/> based on 
    /// Windows Forms API.
    /// </summary>
    public class Window : Form, IWindow {

        /// <summary>
        /// Returns the current window content or does set it.
        /// </summary>
        protected IWidgetCompound WindowContent { get; set; }
        
        #region IWindow

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PWindow<IWindow>.GetModelValue(this, PWindow<IWindow>.VisibilityProperty); }
            set {
                PWindow<IWindow>.SetModelValue(this, PWindow<IWindow>.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            } 
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PWindow<IWindow>.GetModelValue(this, PWindow<IWindow>.EnabledProperty); }
            set {
                PWindow<IWindow>.SetModelValue(this, PWindow<IWindow>.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <inheritdoc />
        string IWindow.WindowTitle {
            get { return Text; }
            set { Text = value; }
        }

        /// <inheritdoc />
        int IWindow.WindowWidth {
            get { return Width; }
            set { Width = value; }
        }

        /// <inheritdoc />
        int IWindow.WindowHeight {
            get { return Height; }
            set { Height = value; }
        }

        /// <inheritdoc />
        int IWindow.WindowTopLocation {
            get { return Location.Y; } 
            set { Location = new Point(Location.X, value); }
        }

        /// <inheritdoc />
        public int WindowLeftLocation {
            get { return Location.X; }
            set { Location = new Point(value, Location.Y); }
        }

        /// <inheritdoc />
        EWindowState IWindow.WindowState {
            get { return ConvertToMotoiWindowState(WindowState); }
            set { WindowState = ConvertFromMotoiWindowState(value); }
        }

        /// <inheritdoc />
        ImageDescriptor IWindow.WindowIcon {
            get { throw new NotSupportedException(); }
            set { Icon = ConvertToIcon(value); }
        }

        /// <inheritdoc />
        void IShell.SetContent(IWidgetCompound viewPart) {
            Control control = CastUtil.Cast<Control>(viewPart);
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
            WindowContent = viewPart;
        }

        /// <inheritdoc />
        IWidgetCompound IShell.Content { get { return WindowContent; } }

        /// <summary>
        /// Converts a <see cref="FormWindowState"/> value to a <see cref="EWindowState"/> value.
        /// </summary>
        /// <param name="windowState">Value to convert</param>
        /// <returns>Converted value</returns>
        private EWindowState ConvertToMotoiWindowState(FormWindowState windowState) {
            if (windowState == FormWindowState.Maximized) return EWindowState.Maximized;
            if (windowState == FormWindowState.Minimized) return EWindowState.Minimized;
            return EWindowState.Normal;
        }

        /// <summary>
        /// Converts a <see cref="EWindowState"/> value to a <see cref="FormWindowState"/> value.
        /// </summary>
        /// <param name="windowState">Value to convert</param>
        /// <returns>Converted value</returns>
        private FormWindowState ConvertFromMotoiWindowState(EWindowState windowState) {
            if (windowState == EWindowState.Maximized) return FormWindowState.Maximized;
            if (windowState == EWindowState.Minimized) return FormWindowState.Minimized;
            return FormWindowState.Normal;
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
    }
}