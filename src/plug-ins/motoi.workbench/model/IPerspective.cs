using motoi.platform.resources.model.editors;
using motoi.platform.ui;
using motoi.workbench.events;
using motoi.workbench.exceptions;
using Xcite.Csharp.lang;

namespace motoi.workbench.model {
    /// <summary>
    /// Defines the content of a window.
    /// </summary>
    public interface IPerspective : IUIService {
        /// <summary>
        /// Returns the currently active editor. May be NULL.
        /// </summary>
        IEditor ActiveEditor { get; }

        /// <summary>
        /// Returns the collection of currently active data views. May be empty.
        /// </summary>
        IDataView[] ActiveViews { get; }

        /// <summary>
        /// Advices the perspective to make the editor with the given <paramref name="editorId"/> visible.
        /// </summary>
        /// <param name="editorId">Id of the editor</param>
        /// <returns>The opened editor instance or NULL</returns>
        /// <exception cref="WorkbenchPartInitializationException">If an error during the process occurs</exception>
        IEditor OpenEditor(string editorId);

        /// <summary>
        /// Opens an editor within the perspective that can handle the given <paramref name="editorInput"/>.
        /// </summary>
        /// <param name="editorInput">Input for the editor</param>
        /// <returns>The opened editor instance or NULL</returns>
        /// <exception cref="WorkbenchPartInitializationException">If an error during the process occurs</exception>
        IEditor OpenEditor(IEditorInput editorInput);

        /// <summary>
        /// Closes the currently opened editor. If none is open, nothing will happen. Note, there is no guarantee the editor is closed. 
        /// The user may cancel the process when the editor is dirty and the user refuses the save dialog.
        /// </summary>
        /// <returns>TRUE if the editor has been closed</returns>
        bool CloseEditor();

        /// <summary>
        /// Closes the editor referenced by the given instance. If none is open, nothing will happen. Note, there is no guarantee the editor is closed. 
        /// The user may cancel the process when the editor is dirty and the user refuses the save dialog.
        /// </summary>
        /// <param name="editor">Editor instance to close</param>
        /// <returns>TRUE if the editor has been closed</returns>
        bool CloseEditor(IEditor editor);

        /// <summary>
        /// Shows the view with the given <paramref name="dataViewId"/> at the given <paramref name="viewPosition"/>, but there is no guarantee that 
        /// the view will be visible. For example, the <paramref name="dataViewId"/> must not be known by the framework. In that case, no instance 
        /// can be created. Therefore, this method is more a hint than a reliable operation.
        /// A new instance of the data view will not be created at any time. For instance, if the view already exists, but it's not visible to the 
        /// user, it will be brought to top.
        /// </summary>
        /// <param name="dataViewId">Id of the data view to (re)open</param>
        /// <param name="viewPosition">View position</param>
        void OpenView(string dataViewId, EViewPosition viewPosition);

        /// <summary>
        /// Shows the view with the given <typeparamref name="TDataView"/> type at the given <paramref name="viewPosition"/>, but there is no guarantee that 
        /// the view will be visible. For example, the <typeparamref name="TDataView"/> type must not be known by the framework. In that case, no instance 
        /// can be created. Therefore, this method is more a hint than a reliable operation.
        /// A new instance of the data view will not be created at any time. For instance, if the view already exists, but it's not visible to the 
        /// user, it will be brought to top.
        /// </summary>
        /// <typeparam name="TDataView">Type of data view to (re)open</typeparam>
        /// <param name="viewPosition">View position</param>
        void OpenView<TDataView>(EViewPosition viewPosition) where TDataView : class, IDataView;

        /// <summary>
        /// Subscribes the given <paramref name="listener"/> to perspective events.
        /// </summary>
        /// <param name="listener">Listener to subscribe</param>
        void AddPerspectiveListener(IPerspectiveListener listener);

        /// <summary>
        /// Unsubscribes the given <paramref name="listener"/> from perspective events.
        /// </summary>
        /// <param name="listener">Listener to unsubscribe</param>
        void RemovePerspectiveListener(IPerspectiveListener listener);

        /// <summary>
        /// Returns the panel all controls of the perspective are drawn onto.
        /// </summary>
        /// <returns>Panel that contains all controls of the perspective</returns>
        IWidgetCompound GetPanel();
    }

    /// <summary>
    /// Defines kinds of view positions.
    /// </summary>
    public class EViewPosition : XEnum<EViewPosition> {
        /// <summary>
        /// Indicates that the view shall be placed left of document position.
        /// </summary>
        public static readonly EViewPosition Left = new EViewPosition("Left");

        /// <summary>
        /// Indicates that the view shall be placed top of document position.
        /// </summary>
        public static readonly EViewPosition Top = new EViewPosition("Top");

        /// <summary>
        /// Indicates that the view shall be placed right of document position.
        /// </summary>
        public static readonly EViewPosition Right = new EViewPosition("Right");

        /// <summary>
        /// Indicates that the view shall be placed bottom of document position.
        /// </summary>
        public static readonly EViewPosition Bottom = new EViewPosition("Bottom");

        /// <summary>
        /// Indicates that the view shall be placed collapsed left of the document position.
        /// </summary>
        public static readonly EViewPosition LeftCollapsed = new EViewPosition("LeftCollapsed");

        /// <summary>
        /// Indicates that the view shall be placed collapsed right of the document position.
        /// </summary>
        public static readonly EViewPosition RightCollapsed = new EViewPosition("RightCollapsed");

        /// <summary>
        /// Indicates that the view shall be placed collapsed bottom of the document position.
        /// </summary>
        public static readonly EViewPosition BottomCollapsed = new EViewPosition("BottomCollapsed");

        /// <summary>
        /// Indicates that the view shall be placed collapsed top of the document position.
        /// </summary>
        public static readonly EViewPosition TopCollapsed = new EViewPosition("TopCollapsed");

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="uniqueReference">Unique reference to identify this instance</param>
        private EViewPosition(object uniqueReference) : base(uniqueReference) {
            // Currently nothing to do here
        }
    }
}