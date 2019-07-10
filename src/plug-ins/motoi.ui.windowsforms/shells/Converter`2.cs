namespace motoi.ui.windowsforms.shells {
    /// <summary> Defines an object converter. </summary>
    /// <typeparam name="TIn">Type of object to convert from</typeparam>
    /// <typeparam name="TOut">Type of object to convert to</typeparam>
    public interface IConverter<TIn, TOut> {
        /// <summary> Converts the given value of the incoming object type to the declared return type. </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Corresponding return type object</returns>
        TOut ConvertFrom(TIn obj);

        /// <summary> Converts the given value of the incoming object type to the declared return type. </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Corresponding return type object</returns>
        TIn ConvertFrom(TOut obj);
    }
}