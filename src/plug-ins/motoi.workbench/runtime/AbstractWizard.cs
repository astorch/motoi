using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using motoi.platform.ui.actions;
using motoi.platform.ui.factories;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using xcite.csharp;

namespace motoi.workbench.runtime {
    /// <summary> Provides an abstract implementation of <see cref="IWizard"/>. </summary>
    public abstract class AbstractWizard : AbstractDisposable, IWizard {
        private readonly IList<IWizardPage> _wizardPages = new List<IWizardPage>(5);
        private readonly IDictionary<IWizardPage, IGridPanel> _pageToCompositeMap = new Dictionary<IWizardPage, IGridPanel>(5);
        private ITitledAreaDialog _dialog;
        private IWizardPage _currentWizardPage;
        private int _currentWizardPageIndex;
        private IButton _btnFinish;
        private IButton _btnNext;
        private IButton _btnPrev;
        
        /// <inheritdoc />
        protected AbstractWizard() { 
            Initialize(); // TODO Use a IInitializeExtension Pattern?
        }
        
        /// <inheritdoc />
        public virtual void Initialize() {
            _dialog = UIFactory.NewShell<ITitledAreaDialog>();
            if (_dialog == null) return; // TODO Use throwHelper

            _dialog.AddButton(Messages.AbstractWizard_Button_Cancel, new ActionHandlerDelegate(Cancel));
            _btnFinish = _dialog.AddButton(Messages.AbstractWizard_Button_Finish, new ActionHandlerDelegate(Finish));
            _btnNext = _dialog.AddButton(Messages.AbstractWizard_Button_Next, new ActionHandlerDelegate(NavigateNext));
            _btnPrev = _dialog.AddButton(Messages.AbstractWizard_Button_Back, new ActionHandlerDelegate(NavigateBack));

            _btnPrev.Enabled = false;
            _btnFinish.Enabled = false;
            _btnNext.Enabled = false;

            OnInitialize();

            if (PageCount == 0) return;

            _currentWizardPageIndex = 0;
            AssignCurrentWizardPageContent();
        }
        
        /// <inheritdoc />
        protected override void OnDispose(bool dispose) {
            try {
                OnDispose();

                for (int i = -1; ++i != _wizardPages.Count;) {
                    IWizardPage page = _wizardPages[i];
                    page.Wizard = null;
                    page.Dispose();
                }
                
                _wizardPages.Clear();
                _pageToCompositeMap.Clear();

                if (_currentWizardPage != null) {
                    _currentWizardPage.PropertyChanged -= OnPagePropertyChanged;
                    _currentWizardPage = null;
                }

                _btnFinish = null;
                _btnNext = null;
                _btnPrev = null;

                _dialog.SetContent(null);
                _dialog = null;
            } catch {
                // Does not matter
            }
        }

        /// <inheritdoc />
        public virtual void AddWizardPage(IWizardPage page) {
            _wizardPages.Add(page);
            page.Wizard = this;
        }

        /// <inheritdoc />
        public virtual void Cancel() {
            OnCancel();
            _dialog.Close();
        }
        
        /// <inheritdoc />
        public virtual void Finish() {
            OnFinish();
            _dialog.Close();
        }
        
        /// <inheritdoc />
        public virtual int PageCount 
            => _wizardPages.Count;


        /// <inheritdoc />
        public virtual IWizardPage[] Pages 
            => _wizardPages.ToArray();
        
        /// <inheritdoc />
        public virtual string Title {
            get { return _dialog.WindowTitle; }
            protected set { _dialog.WindowTitle = value; }
        }

        /// <inheritdoc />
        public virtual void Open() {
            _dialog.Show();
        }

        /// <summary>
        /// Updates the content pane of the wizard by assigning the content of 
        /// current wizard page.
        /// </summary>
        private void AssignCurrentWizardPageContent() {
            if (_currentWizardPage != null)
                _currentWizardPage.PropertyChanged -= OnPagePropertyChanged;

            _currentWizardPage = _wizardPages[_currentWizardPageIndex];
            _currentWizardPage.PropertyChanged += OnPagePropertyChanged;

            if (!_pageToCompositeMap.TryGetValue(_currentWizardPage, out IGridPanel gridComposite)) {
                gridComposite = UIFactory.NewWidget<IGridPanel>(_dialog);
                gridComposite.GridColumns = 1;
                gridComposite.GridRows = 1;
                _currentWizardPage.Initialize(gridComposite, FactoryProvider.Instance.GetWidgetFactory());
                _pageToCompositeMap.Add(_currentWizardPage, gridComposite);
            }

            _dialog.Title = _currentWizardPage.Title;
            _dialog.Description = _currentWizardPage.Description;
            _dialog.SetContent(gridComposite);

            CheckConditions();
        }

        /// <summary> Will check the wizard state after a page property has been changed. </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="propertyChangedEventArgs">Event arguments</param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (sender != _currentWizardPage) return;
            CheckConditions();
        }

        /// <summary> Checks the state of the wizard and sets the behaviour of the wizard navigation buttons. </summary>
        private void CheckConditions() {
            _btnFinish.Enabled = (_currentWizardPageIndex + 1) == PageCount && _currentWizardPage.CanLeave;
            _btnNext.Enabled = !_btnFinish.Enabled && _currentWizardPage.CanLeave;
            _btnPrev.Enabled = (_currentWizardPageIndex - 1) > 0;
        }

        /// <summary> Performs a backward navigation. </summary>
        private void NavigateBack() {
            if (_currentWizardPageIndex - 1 < 0) return;
            _currentWizardPageIndex--;
            AssignCurrentWizardPageContent();
        }

        /// <summary> Performs a forward navigation. </summary>
        private void NavigateNext() {
            if (_currentWizardPageIndex + 1 >= PageCount) return;
            _currentWizardPageIndex++;
            AssignCurrentWizardPageContent();
        }

        /// <summary> Tells the subclass that initialize has been invoked. </summary>
        protected abstract void OnInitialize();

        /// <summary> Tells the subclass that cancel has been invoked. </summary>
        protected abstract void OnCancel();

        /// <summary> Tells the subclass that finish has been invoked. </summary>
        protected abstract void OnFinish();

        /// <summary> Tells the subclass that dispose has been invoked. </summary>
        protected abstract void OnDispose();
    }
}