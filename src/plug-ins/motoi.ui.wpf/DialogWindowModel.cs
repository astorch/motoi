using System.Collections.Generic;
using motoi.platform.ui.controls;
using motoi.platform.ui.shells;

namespace Motoi.UI.WPF {
    /// <summary>
    /// Provides a MVVM-conform implementation of <see cref="IDialogWindow"/>.
    /// </summary>
    public class DialogWindowModel : WindowModel, IDialogWindow {

        private readonly List<IButton> iButtons = new List<IButton>();

        public IList<IButton> Buttons
        {
            get { return iButtons; }
        }

        public void Show(bool modal)
        {
            throw new System.NotImplementedException();
        }
    }
}