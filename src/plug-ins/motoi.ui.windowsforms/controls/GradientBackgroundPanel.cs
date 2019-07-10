using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary> Provides a panel which has a gradient background. </summary>
    public class GradientBackgroundPanel : Panel {
        /// <summary> Returns the gradient BackColorStop value or does set it. </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "White")]
        public Color BackColorStop { get; set; }
        
        /// <inheritdoc />
        protected override void OnPaintBackground(PaintEventArgs e) {
            Rectangle rc = new Rectangle(0, 0, Width, Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rc, BackColor, BackColorStop, 45F)) {
                e.Graphics.FillRectangle(brush, rc);
            }
        }
        
        /// <inheritdoc />
        protected override void OnResize(EventArgs eventargs) {
            Refresh();
        }
    }
}