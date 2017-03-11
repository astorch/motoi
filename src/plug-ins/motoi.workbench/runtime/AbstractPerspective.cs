using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using motoi.platform.resources.model.editors;
using motoi.platform.ui;
using motoi.platform.ui.shells;
using motoi.workbench.events;
using motoi.workbench.exceptions;
using motoi.workbench.model;
using motoi.workbench.registries;
using Xcite.Csharp.oop;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IPerspective"/>.
    /// </summary>
    public abstract class AbstractPerspective : IPerspective {
        private readonly ILog iLog;
        private readonly AuxiliaryAudible<IPerspectiveListener> iPerspectiveEventManager = new AuxiliaryAudible<IPerspectiveListener>();

        private readonly List<IDataView> iActiveDataViews = new List<IDataView>();

        /// <summary>Constructor</summary>
        protected AbstractPerspective() {
            iLog = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Returns the currently active editor. May be NULL.
        /// </summary>
        public IEditor ActiveEditor { get; private set; }

        /// <summary>
        /// Returns the collection of currently active data views. May be empty.
        /// </summary>
        public IDataView[] ActiveViews { get { return iActiveDataViews.ToArray(); } }

        /// <summary>
        /// Returns the underlying perspective event manager to dispatch events.
        /// </summary>
        protected AuxiliaryAudible<IPerspectiveListener> PerspectiveEventManager { get { return iPerspectiveEventManager; } }

        /// <summary>
        /// Returns the underlying collection of active data views.
        /// </summary>
        protected List<IDataView> ActiveDataViews { get { return iActiveDataViews; } }

        /// <summary>
        /// Advices the perspective to make the editor with the given <paramref name="editorId"/> visible.
        /// </summary>
        /// <param name="editorId">Id of the editor</param>
        public virtual void OpenEditor(string editorId) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens an editor within the perspective that can handle the given <paramref name="editorInput"/>.
        /// </summary>
        /// <param name="editorInput">Input for the editor</param>
        /// <exception cref="WorkbenchPartInitializationException">If an error during the process occurs</exception>
        public virtual void OpenEditor(IEditorInput editorInput) {
            if (editorInput == null) return;

            string fileExtension = editorInput.Name.Split('.').Last();
            if (string.IsNullOrWhiteSpace(fileExtension)) return;
            
            IEditor editor = EditorRegistry.Instance.GetEditorForExtension(fileExtension);
            if (editor == null) return;

            ShowEditor(editor, editorInput);
        }

        /// <summary>
        /// Closes the currently opened editor. If none is open, nothing will happen. Note, there is no guarantee the editor is closed. 
        /// The user may cancel the process when the editor is dirty and the user refuses the save dialog.
        /// </summary>
        public void CloseEditor() {
            if (ActiveEditor == null) return;
            CloseEditor(ActiveEditor);
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
                    if (!CloseEditor(activeEditor)) return;
                }

                // Set up new editor
                editor.WidgetFactory = FactoryProvider.GetWidgetFactory();
                editor.SetEditorInput(editorInput);

                // Make editor visible
                OnShowEditor(editor);

                // Update internal state and dispatch events
                ActiveEditor = editor;
                iPerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartOpened(editor), OnDispatchWorkbenchEventException);
            } catch (Exception ex) {
                iLog.ErrorFormat("Error on showing editor '{0}' by perspective '{1}'. Reason: {2}", editor, this, ex);
            }
        }

        /// <summary>
        /// Advices the perspective to close the given <paramref name="editor"/>.
        /// </summary>
        /// <param name="editor">Editor to close</param>
        public virtual bool CloseEditor(IEditor editor) {
            if (editor == null) return true;
            if (!Equals(ActiveEditor, editor)) return true;

            if (editor.IsDirty) {
                EMessageDialogResult questionResult = MessageDialog.ShowQuestion("Hinweis", "Änderungen speichern?",
                    "Das Dokument besitzt noch ungespeicherte Änderungen. Wenn Sie den Vorgang fortsetzen, gehen " +
                    "diese möglicherweise verloren.",
                    new[] {EMessageDialogResult.Yes, EMessageDialogResult.No, EMessageDialogResult.Cancel});

                // User has canceled
                if (questionResult == EMessageDialogResult.Cancel) return false;

                // User wants to save
                if (questionResult == EMessageDialogResult.Yes) {
                    try {
                        editor.ExecuteSave(null);
                    } catch (Exception ex) {
                        iLog.ErrorFormat("Error on execute save on active editor. Reason: {0}", ex);
                    }
                }
            }

            try {
                OnCloseEditor(editor);
                ActiveEditor = null;
                iPerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartClosed(editor), OnDispatchWorkbenchEventException);
            } catch (Exception ex) {
                iLog.ErrorFormat("Error on closing editor '{0}' by perspective '{1}'. Reason: {2}", editor, this, ex);
            }

            return true;
        }

        /// <summary>
        /// Is invoked when an exception during the dispatching of an perspective event occurred.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="perspectiveListener">Listener the exception happened to</param>
        private void OnDispatchWorkbenchEventException(Exception exception, IPerspectiveListener perspectiveListener) {
            iLog.ErrorFormat("Error on dispatching perspective event to '{0}'. Reason: {1}", perspectiveListener, exception);
        }

        /// <summary>
        /// Shows the view with the given <paramref name="dataViewId"/> at the given <see cref="viewPosition"/>, but there is no guarantee that 
        /// the view will be visible. For example, the <paramref name="dataViewId"/> must not be known by the framework. In that case, no instance 
        /// can be created. Therefore, this method is more a hint than a reliable operation.
        /// A new instance of the data view will not be created at any time. For instance, if the view already exists, but it's not visible to the 
        /// user, it will be brought to top.
        /// </summary>
        /// <param name="dataViewId">Id of the data view to (re)open</param>
        /// <param name="viewPosition">View position</param>
        public virtual void OpenView(string dataViewId, EViewPosition viewPosition) {
            IDataView dataView = DataViewRegistry.Instance.GetDataView(dataViewId);
            ShowView(dataView, viewPosition);
        }

        /// <summary>
        /// Shows the view with the given <typeparamref name="TDataView"/> type at the given <paramref name="viewPosition"/>, but there is no guarantee that 
        /// the view will be visible. For example, the <typeparamref name="TDataView"/> type must not be known by the framework. In that case, no instance 
        /// can be created. Therefore, this method is more a hint than a reliable operation.
        /// A new instance of the data view will not be created at any time. For instance, if the view already exists, but it's not visible to the 
        /// user, it will be brought to top.
        /// </summary>
        /// <typeparam name="TDataView">Type of data view to (re)open</typeparam>
        /// <param name="viewPosition">View position</param>
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
                IWidgetFactory widgetFactory = FactoryProvider.GetWidgetFactory();
                dataView.WidgetFactory = widgetFactory;

                // Make view visible
                OnShowDataView(dataView, viewPosition);

                // Update internal state and dispatch events
                iActiveDataViews.Add(dataView);
                iPerspectiveEventManager.Dispatch(lstnr => lstnr.OnWorkbenchPartOpened(dataView), OnDispatchWorkbenchEventException);
            } catch (Exception ex) {
                iLog.ErrorFormat("Error on showing data view '{0}' by perspective '{1}'. Reason: {2}", dataView, this, ex);
            }
        }

        /// <summary>
        /// Subscribes the given <paramref name="listener"/> to perspective events.
        /// </summary>
        /// <param name="listener">Listener to subscribe</param>
        public virtual void AddPerspectiveListener(IPerspectiveListener listener) {
            iPerspectiveEventManager.AddListener(listener);
        }

        /// <summary>
        /// Unsubscribes the given <paramref name="listener"/> from perspective events.
        /// </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        public virtual void RemovePerspectiveListener(IPerspectiveListener listener) {
            iPerspectiveEventManager.RemoveListener(listener);
        }

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