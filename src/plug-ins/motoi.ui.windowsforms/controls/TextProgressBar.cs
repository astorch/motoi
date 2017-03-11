using System;
using System.Drawing;
using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Represents a Windows progress bar control that shows additional text 
    /// above the progress bar.
    /// </summary>
    public class TextProgressBar : ProgressBar {

        /// <inheritdoc />
        protected override CreateParams CreateParams {
            get {
                CreateParams result = base.CreateParams;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT
                    && Environment.OSVersion.Version.Major >= 6) {
                    result.ExStyle |= 0x02000000; // WS_EX_COMPOSITED 
                }

                return result;
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (m.Msg == 0x000F) {
                using (Graphics graphics = CreateGraphics())
                using (SolidBrush brush = new SolidBrush(ForeColor)) {
                    SizeF textSize = graphics.MeasureString(Text, Font);
                    graphics.DrawString(Text, Font, brush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2);
                }
            }
        }

        /// <inheritdoc />
        public override string Text {
            get { return base.Text; }
            set { 
                base.Text = value; 
                Refresh();
            }
        }

        /// <inheritdoc />
        public override Font Font {
            get { return base.Font; }
            set {
                base.Font = value;
                Refresh();
            }
        }

        /// <summary>
        /// Returns TRUE when the progress is indetermine or does set it.
        /// </summary>
        public bool IsIndetermine {
            get { return Style == ProgressBarStyle.Marquee; }
            set { Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous; }
        }
    }
}