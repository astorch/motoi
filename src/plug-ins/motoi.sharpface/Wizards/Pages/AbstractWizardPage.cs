using Motoi.Commons;
using Motoi.UI.Widgets;

namespace Motoi.SharpFace.Wizards.Pages
{
    /// <summary>
    /// Provides an abstract default implementation of <see cref="IWizardPage"/>.
    /// </summary>
    public abstract class AbstractWizardPage : PropertyChangedDispatcher, IWizardPage {

        private bool iCanLeave;

        /// <summary>
        /// Returns the wizard the page is currently added to or does set it.
        /// </summary>
        public IWizard Wizard { get; set; }

        /// <summary>
        /// Returns a data context object or does set it.
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        /// Returns true if the page is enabled.
        /// </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary>
        /// Returns the title of wizard page.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Returns the description of the wizard page.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Returns true if the page can be left.
        /// </summary>
        public bool CanLeave
        {
            get { return iCanLeave; }
            protected set {
                iCanLeave = value;
                DispatchPropertyChanged(() => CanLeave);
            }
        }

        public void Dispose() {
            OnDispose();
        }

        /// <summary>
        /// Tells the page to initialize its content.
        /// </summary>
        /// <param name="gridComposite">Element container</param>
        public abstract void Initialize(IGridComposite gridComposite);

        /// <summary>
        /// Tells subclass that dispose has been invoked.
        /// </summary>
        protected abstract void OnDispose();
    }
}