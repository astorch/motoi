using motoi.platform.ui.bindings;
using motoi.platform.ui.factories;
using motoi.platform.ui.widgets;
using motoi.workbench.model;

namespace motoi.workbench.runtime {
    /// <summary> Provides an abstract implementation of <see cref="IWizardPage"/>. </summary>
    public abstract class AbstractWizardPage : PropertyChangedDispatcher, IWizardPage {
        private bool _canLeave;

        /// <summary> Returns the wizard the page is currently added to or does set it. </summary>
        public IWizard Wizard { get; set; }

        /// <summary> Returns a data context object or does set it. </summary>
        public object DataContext { get; set; }

        /// <summary> Returns TRUE, if the page is enabled. </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary> Returns TRUE, if the page can be left. </summary>
        public bool CanLeave {
            get { return _canLeave; }
            protected set {
                _canLeave = value; 
                DispatchPropertyChanged(() => CanLeave);
            }
        }
        
        /// <inheritdoc />
        public string Title { get; protected set; }
        
        /// <inheritdoc />
        public string Description { get; protected set; }
        
        /// <inheritdoc />
        public abstract void Initialize(IGridPanel gridComposite, IWidgetFactory widgetFactory);
        
        /// <inheritdoc />
        public abstract void Dispose();
    }
}