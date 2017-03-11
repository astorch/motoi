using System;
using motoi.platform.ui.actions;

namespace DemoApplication
{
    /// <summary>
    /// Provides an action handler for the menu item 'New'.
    /// </summary>
    public class FileMenuNewHandler : AbstractActionHandler {

        /// <summary>
        /// Tells the handler to invoke his action.
        /// </summary>
        public override void Run() {
            Console.WriteLine("File menu new has been selected!");            
        }
    }
}