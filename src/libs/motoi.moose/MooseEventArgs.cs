using System;

namespace motoi.moose {
    /// <summary>
    /// Describes a moose event.
    /// </summary>
    public class MooseEventArgs : EventArgs {
        /// <summary>Creates a new instance with the given <paramref name="symbolicPluginName"/>.</summary>
        public MooseEventArgs(string symbolicPluginName) {
            SymbolicPluginName = symbolicPluginName;
        }

        /// <summary>
        /// Returns the symbolic name of the affected plug-in.
        /// </summary>
        public string SymbolicPluginName { get; private set; }
    }
}