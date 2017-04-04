using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using motoi.platform.ui;
using motoi.platform.ui.images;
using motoi.ui.windowsforms.controls;
using motoi.workbench.model;
using motoi.workbench.runtime;
using WeifenLuo.WinFormsUI.Docking;
using ToolBar = motoi.ui.windowsforms.toolbars.ToolBar;

namespace motoi.ui.windowsforms.shells {
    /// <summary>
    /// Provides an implementation of <see cref="ISingleViewPerspective"/>.
    /// </summary>
    public class SingleViewPerspective : AbstractPerspective, ISingleViewPerspective, IDockableControl {
        private DockPanel iDockPanel;
        private DockContent iDocumentDockContent;
        private ToolBar iCurrentToolBar;

        /// <summary>
        /// Tells the control that it is going to be attached to the given <paramref name="dockPanel"/>.
        /// </summary>
        /// <param name="dockPanel">Dock panel this is instance is attached to</param>
        void IDockableControl.Attach(DockPanel dockPanel) {
            iDockPanel = dockPanel;
            iDockPanel.DocumentStyle = GetDocumentStyle(dockPanel);
        }

        /// <inheritdoc />
        public override IWidgetCompound GetPane() {
            return new WidgetCompoundAdapter(this);
        }

        /// <summary>
        /// Returns the <see cref="DocumentStyle"/> for the given <paramref name="dockPanel"/> based on the type 
        /// of this class. This class is intended to be overridden.
        /// </summary>
        /// <param name="dockPanel">Dock panel</param>
        /// <returns>Document style</returns>
        protected virtual DocumentStyle GetDocumentStyle(DockPanel dockPanel) {
            return DocumentStyle.DockingSdi;
        }

        /// <summary>
        /// Tells the instance to make the given <paramref name="editor"/> visible to the user.
        /// </summary>
        /// <param name="editor">Editor to show</param>
        protected override void OnShowEditor(IEditor editor) {
            if (editor == null) return;

            // If there is an open editor, close it before
            if (iDocumentDockContent != null) {
                iDocumentDockContent.Close();
                iDocumentDockContent = null;
            }
            
//            iDockPanel.SuspendLayout();

            // Add ToolStripContainer to the top of the perspective
            ToolStripContainer toolStripContainer = new ToolStripContainer { Dock = DockStyle.Top };

            // Let the editor configure the tool bar
            iCurrentToolBar = new ToolBar();
            editor.ConfigureToolBar(iCurrentToolBar);

            // Add the tool bar to the controls
            toolStripContainer.TopToolStripPanel.Controls.Add(iCurrentToolBar);
            iDockPanel.Controls.Add(iCurrentToolBar);

            // Let the editor create its content
            CompositePanel editorPanel = new CompositePanel { Dock = DockStyle.Fill };
            editor.CreateContents(editorPanel);

            // Place into a dock content
            iDocumentDockContent = new DockContent {Tag = editor};
            iDocumentDockContent.Controls.Add(editorPanel);
            iDocumentDockContent.Show(iDockPanel, DockState.Document);

            iDocumentDockContent.TabText = editor.EditorTabText;
            iDocumentDockContent.Closed += OnDockContentClosed;
            
            editor.PropertyChanged += OnEditorPropertyChanged;
            editor.DirtyChanged += OnEditorDirtyChanged;

//            iDockPanel.ResumeLayout(true);
        }

        /// <summary>
        /// Is invoked when a property of the currently opened editor has been changed.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="args">Event arguments</param>
        private void OnEditorPropertyChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName != "EditorTabText") return;

            IEditor editor = sender as IEditor;
            if (editor == null) return;

            string editorTabText = editor.EditorTabText;
            iDocumentDockContent.TabText = editorTabText;
        }

        /// <summary>
        /// Is invoked when the dirty state of the currently opened editor has been changed.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnEditorDirtyChanged(object sender, EventArgs eventArgs) {
            IEditor editor = sender as IEditor;
            if (editor == null) return;

            bool isDirty = editor.IsDirty;
            string tabName = string.Format("{1}{0}", editor.EditorTabText, (isDirty ? "*" : string.Empty));
            iDocumentDockContent.TabText = tabName;
        }

        /// <summary>
        /// Is invoked when the user closes the editor by closing the dock content.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="eventArgs">Event arguments</param>
        private void OnDockContentClosed(object sender, EventArgs eventArgs) {
            DockContent dockContent = sender as DockContent;
            if (dockContent == null) return;
            CloseEditor(dockContent.Tag as IEditor);
        }

        /// <summary>
        /// Tells the instance to close the given <paramref name="editor"/>.
        /// </summary>
        /// <param name="editor">Editor to close</param>
        protected override void OnCloseEditor(IEditor editor) {
            if (editor == null) return;

            // If the editor isn't currently open
            if (!Equals(iDocumentDockContent.Tag, editor)) return;

            editor.PropertyChanged -= OnEditorPropertyChanged;
            editor.DirtyChanged -= OnEditorDirtyChanged;

            iDocumentDockContent.Closed -= OnDockContentClosed;

            if (iCurrentToolBar != null) {
                iCurrentToolBar.Dispose();
                iCurrentToolBar = null;
            }
        }

        /// <summary>
        /// Tells the instance to make the given <paramref name="dataView"/> visible to the user at the given 
        /// <paramref name="viewPosition"/>.
        /// </summary>
        /// <param name="dataView">Data view to show</param>
        /// <param name="viewPosition">Target view position</param>
        protected override void OnShowDataView(IDataView dataView, EViewPosition viewPosition) {
            // Visual root element
            DockContent dataViewDockContent = new DockContent();

            // Let the view create its content
            CompositePanel compositePanel = new CompositePanel();
            compositePanel.Dock = DockStyle.Fill;
            dataView.CreateContents(compositePanel);

            // Place into a dock content
            dataViewDockContent.Controls.Add(compositePanel);

            // Provide a tool bar
            ToolBar dataViewToolBar = new ToolBar();
            dataView.ConfigureToolBar(dataViewToolBar);

            // Only add the tool bar if any item has been added
            if (dataViewToolBar.Items.Count != 0) {
                // Add ToolStripContainer to the top of the perspective
                ToolStripContainer toolStripContainer = new ToolStripContainer { Dock = DockStyle.Top };
                toolStripContainer.TopToolStripPanel.Controls.Add(dataViewToolBar);

                // Add the tool bar to the controls
                dataViewDockContent.Controls.Add(dataViewToolBar);
            }

            // Set up properties
            dataViewDockContent.TabText = dataView.Name;
            ImageDescriptor imageDescriptor = dataView.Image;
            if (imageDescriptor != null) {
                using (Bitmap tmpBitmap = new Bitmap(imageDescriptor.ImageStream)) {
                    IntPtr intPtr = tmpBitmap.GetHicon();
                    using (Icon tmpIcon = Icon.FromHandle(intPtr)) {
                        dataViewDockContent.Icon = tmpIcon;
                        dataViewDockContent.ShowIcon = true;
                    }
                }
            }

            // Make it visible
            DockState dockState = ConvertToDockState(viewPosition);
            dataViewDockContent.Show(iDockPanel, dockState);
        }

        /// <summary>
        /// Converts the given <paramref name="viewPosition"/> into the corresponding <see cref="DockState"/>. 
        /// If there is no corresponding element, <see cref="DockState.DockLeftAutoHide"/> is returned.
        /// </summary>
        /// <param name="viewPosition">View position to convert</param>
        /// <returns>Corresponding <see cref="DockState"/></returns>
        protected virtual DockState ConvertToDockState(EViewPosition viewPosition) {
            return DockState.Document;
        }

        /// <summary>
        /// Implements an adapter between <see cref="IDockableControl"/> and <see cref="IWidgetCompound"/>.
        /// </summary>
        class WidgetCompoundAdapter : IDockableControl, IWidgetCompound {
            private readonly IDockableControl iDockableControl;

            /// <inheritdoc />
            public WidgetCompoundAdapter(IDockableControl dockableControl) {
                if (dockableControl == null) throw new ArgumentNullException("dockableControl");
                iDockableControl = dockableControl;
            }

            /// <inheritdoc />
            void IDockableControl.Attach(DockPanel dockPanel) {
                iDockableControl.Attach(dockPanel);
            }

            /// <inheritdoc />
            bool IWidget.Enabled {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            /// <inheritdoc />
            EVisibility IWidget.Visibility {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }
    }
}