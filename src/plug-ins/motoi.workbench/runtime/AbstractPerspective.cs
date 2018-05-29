using System;
using System.Collections.Generic;
using System.Linq;
using motoi.platform.resources.model.editors;
using motoi.platform.ui;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;
using motoi.workbench.events;
using motoi.workbench.model;
using motoi.workbench.registries;
using NLog;
using xcite.csharp.oop;

namespace motoi.workbench.runtime {
    /// <summary> Provides an abstract implementation of <see cref="IPerspective"/>. </summary>
    public abstract class AbstractPerspective : IPerspective {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary> Returns the underlying perspective event manager to dispatch events. </summary>
        protected AuxiliaryAudible<IPerspectiveListener> PerspectiveEventManager { get; } = new AuxiliaryAudible<IPerspectiveListener>();

        /// <summary> Returns the underlying collection of active data views. </summary>
        protected List<IDataView> ActiveDataViews { get; } = new List<IDataView>();

        /// <inheritdoc />
        public IEditor ActiveEditor { get; private set; }

        /// <inheritdoc />
        public IDataView[] ActiveViews 
            => ActiveDataViews.ToArray();

        /// <inheritdoc />
        public virtual IEditor OpenEditor(string editorId) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IEditor OpenEditor(IEditorInput editorInput) {
            if (editorInput == null) return null;

            string fileExtension = editorInput.Name.Split('.').Last();
            if (string.IsNullOrWhiteSpace(fileExtension)) return null;
            
            IEditor editor = EditorRegistry.Instance.GetEditorForExtension(fileExtension);
            if (editor == null) return null;

            ShowEditor(editor, editorInput);
            return editor;
        }

        /// <inheritdoc />
        public bool CloseEditor() {
            if (ActiveEditor == null) return false;
            return CloseEditor(ActiveEditor);
        }

        /// <summary>
        /// Advices the perspective to make the given editor visible.
        /// </summary>
        /// <param name="editor">Editor instance</param>
        /// <param name="editorInput">Input for the editor</param>
        public virtual void ShowEditor(IEditor editor, IEditorInput editorInput) {
            if (editor == null) return;

            try {
                // Close active editor if there is any
                IEditor activeEditor = ActiveEditor;
                if (activeEditor != null) {
                    if (!CanCloseEditor(activeEditor)) return;
                }

                // Set up new editor
                editor.WidgetFactory = FactoryProvider.Instance.GetWidgetFactory();
                editor.SetEditorInput(editorInput);

                // Make editor visible
                OnShowEditor(editor);

                // Update internal state and dispatch events
                ActiveEditor = editor;
                PerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartOpened(editor), OnDispatchWorkbenchEventException);
            } catch (Exception ex) {
                _log.Error(ex, $"Error on showing editor '{editor}' by perspective '{this}'.");
            }
        }

        /// <summary>
        /// Returns TRUE if the given editor instance can be closed. If the editor is dirty, a question dialog 
        /// is shown. If the user decides to save the modified content, <see cref="ISaveableWorkbenchPart.ExecuteSave"/> 
        /// is called.
        /// </summary>
        /// <param name="editor">Editor instance to check</param>
        /// <returns>TRUE or FALSE</returns>
        protected virtual bool CanCloseEditor(IEditor editor) {
            if (editor == null) return true;
            if (!editor.IsDirty) return true;

            EMessageDialogResult questionResult = MessageDialog.ShowQuestion(
                Messages.AbstractPerspective_CanCloseEditor_Dialog_Title, 
                Messages.AbstractPerspective_CanCloseEditor_Dialog_Header,
                Messages.AbstractPerspective_CanCloseEditor_Dialog_Text,
                new[] { EMessageDialogResult.Yes, EMessageDialogResult.No, EMessageDialogResult.Cancel });

            // User has canceled
            if (questionResult == EMessageDialogResult.Cancel) return false;

            // User wants to save
            if (questionResult == EMessageDialogResult.Yes) {
                try {
                    editor.ExecuteSave(null);
                } catch (Exception ex){
                    _log.Error(ex, "Error on execute save on active editor.");
                }
            }

            return true;
        }

        /// <inheritdoc />
        public virtual bool CloseEditor(IEditor editor) {
            return CloseEditor(editor, false);
        }

        /// <summary>
        /// Closes the editor referenced by the given instance. If none is open, nothing will happen. Note, there is no guarantee the editor is closed. 
        /// The user may cancel the process when the editor is dirty and the user refuses the save dialog.
        /// </summary>
        /// <param name="editor">Editor instance to close</param>
        /// <param name="silently">If TRUE the editor is closed silently</param>
        /// <returns></returns>
        protected virtual bool CloseEditor(IEditor editor, bool silently) {
            if (editor == null) return false;
            if (!Equals(ActiveEditor, editor)) return false;
            if (!silently && !CanCloseEditor(editor)) return false;

            try {
                OnCloseEditor(editor);
                ActiveEditor = null;
                PerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartClosed(editor), OnDispatchWorkbenchEventException);
                return true;
            } catch (Exception ex) {
                _log.Error(ex, $"Error on closing editor '{editor}' by perspective '{this}'.");
                return false;
            }
        }

        /// <summary>
        /// Is invoked when an exception during the dispatching of an perspective event occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="perspectiveListener">Listener the exception happened to</param>
        protected virtual void OnDispatchWorkbenchEventException(Exception exception, IPerspectiveListener perspectiveListener) {
            _log.Error(exception, $"Error on dispatching perspective event to '{perspectiveListener}'.");
        }

        /// <inheritdoc />
        public virtual void OpenView(string dataViewId, EViewPosition viewPosition) {
            IDataView dataView = DataViewRegistry.Instance.GetDataView(dataViewId);
            ShowView(dataView, viewPosition);
        }

        /// <inheritdoc />
        public virtual void OpenView<TDataView>(EViewPosition viewPosition) where TDataView : class, IDataView {
            TDataView dataView = DataViewRegistry.Instance.GetDataView<TDataView>();
            ShowView(dataView, viewPosition);
        }

        /// <summary>
        /// Advices the perspective to make the given <paramref name="dataView"/> visible at the <paramref name="viewPosition"/>.
        /// </summary>
        /// <param name="dataView">Data view to show</param>
        /// <param name="viewPosition">Target view position</param>
        public virtual void ShowView(IDataView dataView, EViewPosition viewPosition) {
            if (dataView == null) return;

            try {
                // Notify data view to init
                dataView.Init();

                // Set up new view
                IWidgetFactory widgetFactory = FactoryProvider.Instance.GetWidgetFactory();
                dataView.WidgetFactory = widgetFactory;

                // Make view visible
                OnShowDataView(dataView, viewPosition);

                // Update internal state and dispatch events
                ActiveDataViews.Add(dataView);
                PerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartOpened(dataView), OnDispatchWorkbenchEventException);
            } catch (Exception ex) {
                _log.Error(ex, $"Error on showing data view '{dataView}' by perspective '{this}'.");
            }
        }

        /// <inheritdoc />
        public virtual void AddPerspectiveListener(IPerspectiveListener listener) {
            PerspectiveEventManager.AddListener(listener);
        }

        /// <inheritdoc />
        public virtual void RemovePerspectiveListener(IPerspectiveListener listener) {
            PerspectiveEventManager.RemoveListener(listener);
        }

        /// <inheritdoc />
        public abstract IWidgetCompound GetPanel();

        /// <summary>
        /// Tells the instance to make the given <paramref name="editor"/> visible to the user.
        /// </summary>
        /// <param name="editor">Editor to show</param>
        protected abstract void OnShowEditor(IEditor editor);

        /// <summary>
        /// Tells the instance to close the given <paramref name="editor"/>.
        /// </summary>
        /// <param name="editor">Editor to close</param>
        protected abstract void OnCloseEditor(IEditor editor);

        /// <summary>
        /// Tells the instance to make the given <paramref name="dataView"/> visible to the user at the given 
        /// <paramref name="viewPosition"/>.
        /// </summary>
        /// <param name="dataView">Data view to show</param>
        /// <param name="viewPosition">Target view position</param>
        protected abstract void OnShowDataView(IDataView dataView, EViewPosition viewPosition);

        
    }
}