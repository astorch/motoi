using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary> Provides a label which looks like a separator. </summary>
    public class Separator : Label {
        /// <inheritdoc />
        public override BorderStyle BorderStyle 
            => BorderStyle.Fixed3D;
        
        /// <inheritdoc />
        public override bool AutoSize 
            => false;
        
        /// <inheritdoc />
        public override string Text 
            => string.Empty;
    }
}