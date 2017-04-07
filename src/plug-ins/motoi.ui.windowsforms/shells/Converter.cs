using System.Windows.Forms;
using motoi.platform.ui.shells;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides access to various implementations of <see cref="IConverter{TIn,TOut}"/>.
    /// </summary>
    public static class Converter {
        /// <summary> Window state converter </summary>
        public static readonly WindowStateConverter WindowState = new WindowStateConverter();
        
        /// <summary> Window startup location converter </summary>
        public static readonly StartPositionConverter WindowStartupLocation = new StartPositionConverter();

        /// <summary>
        /// Implements a converter between <see cref="FormBorderStyle"/> and <see cref="EWindowStyle"/>.
        /// </summary>
        public class WindowStyleConverter : IConverter<FormBorderStyle, EWindowStyle> {
            /// <inheritdoc />
            public EWindowStyle ConvertFrom(FormBorderStyle obj) {
                if (obj == FormBorderStyle.None) return EWindowStyle.BlankWindow;
                if (obj == FormBorderStyle.SizableToolWindow) return EWindowStyle.ToolWindow;
                return EWindowStyle.DefaultWindow;
            }

            /// <inheritdoc />
            public FormBorderStyle ConvertFrom(EWindowStyle obj) {
                if (obj == EWindowStyle.BlankWindow) return FormBorderStyle.None;
                if (obj == EWindowStyle.ToolWindow) return FormBorderStyle.SizableToolWindow;
                return FormBorderStyle.Sizable;
            }
        }

        /// <summary>
        /// Implements a converter between <see cref="FormStartPosition"/> and <see cref="EWindowStartupLocation"/>.
        /// </summary>
        public class StartPositionConverter : IConverter<FormStartPosition, EWindowStartupLocation> {
            /// <inheritdoc />
            public EWindowStartupLocation ConvertFrom(FormStartPosition obj) {
                if (obj == FormStartPosition.CenterParent) return EWindowStartupLocation.CenterOwner;
                if (obj == FormStartPosition.CenterScreen) return EWindowStartupLocation.CenterScreen;
                return EWindowStartupLocation.Manual;
            }

            /// <inheritdoc />
            public FormStartPosition ConvertFrom(EWindowStartupLocation obj) {
                if (obj == EWindowStartupLocation.CenterOwner) return FormStartPosition.CenterParent;
                if (obj == EWindowStartupLocation.CenterScreen) return FormStartPosition.CenterScreen;
                return FormStartPosition.Manual;
            }
        }

        /// <summary>
        /// Implements a converter between <see cref="FormWindowState"/> and <see cref="EWindowState"/>.
        /// </summary>
        public class WindowStateConverter : IConverter<FormWindowState, EWindowState> {
            /// <inheritdoc />
            public EWindowState ConvertFrom(FormWindowState windowState) {
                if (windowState == FormWindowState.Maximized) return EWindowState.Maximized;
                if (windowState == FormWindowState.Minimized) return EWindowState.Minimized;
                return EWindowState.Normal;
            }

            /// <inheritdoc />
            public FormWindowState ConvertFrom(EWindowState windowState) {
                if (windowState == EWindowState.Maximized) return FormWindowState.Maximized;
                if (windowState == EWindowState.Minimized) return FormWindowState.Minimized;
                return FormWindowState.Normal;
            }
        }
    }
}