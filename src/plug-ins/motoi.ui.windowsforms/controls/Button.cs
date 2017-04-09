using System;
using System.Drawing;
using motoi.platform.ui;
using motoi.platform.ui.actions;
using motoi.platform.ui.widgets;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides an implementation of <see cref="IButton"/>.
    /// </summary>
    public class Button : System.Windows.Forms.Button, IButton {
        /// <inheritdoc />
        public Button() {
            InitializeComponent();
        }

        #region IButton

        /// <inheritdoc />
        EVisibility IWidget.Visibility {
            get { return PButton.GetModelValue(this, PButton.VisibilityProperty); }
            set {
                PButton.SetModelValue(this, PButton.VisibilityProperty, value);
                Visible = (value == EVisibility.Visible);
            } 
        }

        /// <inheritdoc />
        bool IWidget.Enabled {
            get { return PButton.GetModelValue(this, PButton.EnabledProperty); }
            set {
                PButton.SetModelValue(this, PButton.EnabledProperty, value);
                Enabled = value;
            }
        }

        /// <inheritdoc />
        IActionHandler IButton.ActionHandler {
            get { return PButton.GetModelValue(this, PButton.ActionHandlerProperty); }
            set { PButton.SetModelValue(this, PButton.ActionHandlerProperty, value); } 
        }

        #endregion

        /// <summary>
        /// Performs an initialization of the used components.
        /// </summary>
        private void InitializeComponent() {
            MinimumSize = new Size(120, 32);
        }

        /// <inheritdoc />
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);

            IActionHandler actionHandler = ((IButton) this).ActionHandler;
            if (actionHandler == null) return;
            actionHandler.Run();
        }
    }
}