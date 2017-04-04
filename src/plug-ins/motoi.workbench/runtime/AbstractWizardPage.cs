using motoi.platform.ui;
using motoi.platform.ui.bindings;
using motoi.platform.ui.factories;
using motoi.platform.ui.widgets;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IWizardPage"/>.
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
        /// Returns true if the page can be left.
        /// </summary>
        public bool CanLeave {
            get { return iCanLeave; }
            protected set {
                iCanLeave = value; 
                DispatchPropertyChanged(() => CanLeave);
            }
        }

        /// <summary>
        /// Returns the title of wizard page.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Returns the description of the wizard page.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Tells the page to initialize its content.
        /// </summary>
        /// <param name="gridComposite">Element container</param>
        /// <param name="widgetFactory">Factory to create widgets</param>
        public abstract void Initialize(IGridComposite gridComposite, IWidgetFactory widgetFactory);

        /// <summary>
        /// Notifies the instance to dispose any created or referenced resource.
        /// </summary>
        public abstract void Dispose();
    }
}