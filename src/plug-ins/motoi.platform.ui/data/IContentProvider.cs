namespace motoi.platform.ui.data {
    /// <summary>
    /// Defines the interface of content provider. Those providers 
    /// are used by data viewers to collect the data which shall be viewed.
    /// </summary>
    public interface IContentProvider {
        /// <summary> Return all elements that derive from the given input element. </summary>
        /// <param name="input">Input element</param>
        /// <returns>Set of elements</returns>
        object[] GetElements(object input);
    }
}