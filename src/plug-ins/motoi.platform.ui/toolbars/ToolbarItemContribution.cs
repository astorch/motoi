using System.IO;
using motoi.platform.ui.actions;

namespace motoi.platform.ui.toolbars {
    /// <summary> Defines a toolbar group item contribution. </summary>
    public class ToolbarItemContribution {
        /// <summary> Creates a new instance. </summary>
        /// <param name="id">Id of the toolbar item</param>
        /// <param name="group">Parent group</param>
        /// <param name="actionHandler">Action handler</param>
        /// <param name="label">Label of the item</param>
        /// <param name="imageStream">Stream to an image</param>
        public ToolbarItemContribution(string id, string group, IActionHandler actionHandler, string label, Stream imageStream) {
            Id = id;
            Label = label;
            Group = group;
            ActionHandler = actionHandler ?? ActionHandlerFactory.NullActionHandler;
            ImageStream = imageStream;
        }

        /// <summary> Returns the id of the toolbar item. </summary>
        public string Id { get; }

        /// <summary> Returns the label of the toolbar item. </summary>
        public string Label { get; }

        /// <summary> Returns the id of the containing toolbar group. </summary>
        public string Group { get; }

        /// <summary> Returns the action handler. </summary>
        public IActionHandler ActionHandler { get; }

        /// <summary> Returns a stream to an image. </summary>
        public Stream ImageStream { get; }
    }
}