namespace motoi.platform.ui.data {
    /// <summary> Provides a default implementation of <see cref="ITreeLabelProvider"/>. </summary>
    public class DefaultTreeLabelProvider : DefaultLabelProvider, ITreeLabelProvider {
        /// <summary> Default instance. </summary>
         public new static readonly DefaultTreeLabelProvider Instance = new DefaultTreeLabelProvider();
    }
}