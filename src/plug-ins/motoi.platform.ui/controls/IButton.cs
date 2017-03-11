namespace motoi.platform.ui.controls
{
    /// <summary>
    /// Defines the properties of a button.
    /// </summary>
    public interface IButton {
        
        /// <summary>
        /// Returns the state of the button or does set it.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Returns the text of the button or does set it.
        /// </summary>
        string Text { get; set; }
    }
}