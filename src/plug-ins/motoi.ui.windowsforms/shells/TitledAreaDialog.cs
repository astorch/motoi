using System;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.controls;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;
using motoi.ui.windowsforms.controls;
using motoi.ui.windowsforms.utils;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="ITitledAreaDialog"/>.
    /// </summary>
    public class TitledAreaDialog : DialogWindow, ITitledAreaDialog {
        private Label LblText;
        private Label LblTitle;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Separator separator2;
        private Separator separator1;
        private Panel ContentPanel;
        private GradientBackgroundPanel gradientBackgroundPanel1;

        private IWidget iContentPane;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TitledAreaDialog() {
            InitializeComponent();
        }

        #region ITitledAreaDialog

        /// <summary>
        /// Returns the description text of the dialog or does set it.
        /// </summary>
        string ITitledAreaDialog.Description {
            get { return LblText.Text; }
            set { LblText.Text = value; }
        }

        /// <summary>
        /// Returns the title of the dialog or does set it.
        /// </summary>
        string ITitledAreaDialog.Title {
            get { return LblTitle.Text; }
            set { LblTitle.Text = value; }
        }

        /// <summary>
        /// Sets the given <paramref name="viewPart"/> as view content.
        /// </summary>
        /// <param name="viewPart">View content to set</param>
        void IWindow.SetContent(IViewPart viewPart) {
            Control wdgCtrl = CastUtil.Cast<Control>(viewPart);
            ContentPanel.Controls.Add(wdgCtrl);
            wdgCtrl.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Adds a button to dialog button area.
        /// </summary>
        /// <param name="label">Label of the button</param>
        /// <param name="action">Action to be performed when the button is clicked</param>
        /// <returns>Handle for the button</returns>
        IButton ITitledAreaDialog.AddButton(string label, Action action) {
            Button btn = new Button {AutoSize = true};
            btn.Text = label;
            btn.Click += (sender, args) => action();
            flowLayoutPanel1.Controls.Add(btn);

            return new ButtonHandle(() => btn.Enabled, val => btn.Enabled = val, () => btn.Text, val => btn.Text = val);
        }

        #endregion

        private void InitializeComponent() {
            this.gradientBackgroundPanel1 = new motoi.ui.windowsforms.controls.GradientBackgroundPanel();
            this.LblText = new System.Windows.Forms.Label();
            this.LblTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.separator2 = new motoi.ui.windowsforms.controls.Separator();
            this.separator1 = new motoi.ui.windowsforms.controls.Separator();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.gradientBackgroundPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gradientBackgroundPanel1
            // 
            this.gradientBackgroundPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gradientBackgroundPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gradientBackgroundPanel1.BackColorStop = System.Drawing.SystemColors.ActiveCaption;
            this.gradientBackgroundPanel1.Controls.Add(this.LblText);
            this.gradientBackgroundPanel1.Controls.Add(this.LblTitle);
            this.gradientBackgroundPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientBackgroundPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientBackgroundPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.gradientBackgroundPanel1.Name = "gradientBackgroundPanel1";
            this.gradientBackgroundPanel1.Size = new System.Drawing.Size(486, 64);
            this.gradientBackgroundPanel1.TabIndex = 0;
            // 
            // LblText
            // 
            this.LblText.AutoSize = true;
            this.LblText.BackColor = System.Drawing.Color.Transparent;
            this.LblText.Location = new System.Drawing.Point(9, 30);
            this.LblText.Margin = new System.Windows.Forms.Padding(5);
            this.LblText.Name = "LblText";
            this.LblText.Size = new System.Drawing.Size(145, 25);
            this.LblText.TabIndex = 1;
            this.LblText.Text = "Description text";
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.BackColor = System.Drawing.Color.Transparent;
            this.LblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(9, 4);
            this.LblTitle.Margin = new System.Windows.Forms.Padding(5);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(58, 25);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Title";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gradientBackgroundPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.separator2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.separator1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ContentPanel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 404);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 359);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(486, 45);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // separator2
            // 
            this.separator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.separator2.Location = new System.Drawing.Point(0, 64);
            this.separator2.Margin = new System.Windows.Forms.Padding(0);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(486, 1);
            this.separator2.TabIndex = 2;
            // 
            // separator1
            // 
            this.separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.separator1.Location = new System.Drawing.Point(0, 358);
            this.separator1.Margin = new System.Windows.Forms.Padding(0);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(486, 1);
            this.separator1.TabIndex = 3;
            // 
            // ContentPanel
            // 
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Location = new System.Drawing.Point(0, 65);
            this.ContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(486, 293);
            this.ContentPanel.TabIndex = 4;
            // 
            // TitledAreaDialog
            // 
            this.ClientSize = new System.Drawing.Size(486, 404);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "TitledAreaDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.gradientBackgroundPanel1.ResumeLayout(false);
            this.gradientBackgroundPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}