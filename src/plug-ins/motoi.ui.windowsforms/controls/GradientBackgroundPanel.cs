using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides a panel which has a gradient background.
    /// </summary>
    public class GradientBackgroundPanel : Panel {
        /// <summary>
        /// Returns the gradient BackColorStop value or does set it.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "White")]
        public Color BackColorStop { get; set; }

        /// <summary>
        /// Zeichnet den Hintergrund des Steuerelements.
        /// </summary>
        /// <param name="e"><see cref="T:System.Windows.Forms.PaintEventArgs"/> mit den Ereignisdaten.</param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            Rectangle rc = new Rectangle(0, 0, Width, Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rc, BackColor, BackColorStop, 45F)) {
                e.Graphics.FillRectangle(brush, rc);
            }
        }

        /// <summary>
        /// Löst das Ereignis aus, das angibt, dass die Größe des Bereichs geändert wurde.Erbende Steuerelemente sollten diese Funktionalität verwenden anstatt das Ereignis direkt zu überwachen, aber dennoch base.onResize aufrufen, damit das Ereignis auch für externe Listener ausgelöst wird.
        /// </summary>
        /// <param name="eventargs">Eine Instanz der <see cref="T:System.EventArgs"/>-Klasse, die die Ereignisdaten enthält. </param>
        protected override void OnResize(EventArgs eventargs) {
            Refresh();
        }
    }
}