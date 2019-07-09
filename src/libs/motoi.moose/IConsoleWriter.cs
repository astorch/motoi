namespace motoi.moose {
    /// <summary> Writer to the console stream. </summary>
    public interface IConsoleWriter {
        /// <summary> Writes the given text to the console using the standard output stream. </summary>
        /// <param name="text">Text to write</param>
        void WriteMessage(string text);

        /// <summary> Writes the given text to the console using the error output stream </summary>
        /// <param name="text">Text to write</param>
        void WriteError(string text);
    }
}