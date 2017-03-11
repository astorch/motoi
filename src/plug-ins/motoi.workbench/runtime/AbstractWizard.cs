using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using motoi.platform.ui;
using motoi.platform.ui.controls;
using motoi.platform.ui.shells;
using motoi.platform.ui.widgets;
using motoi.workbench.model;
using Xcite.Collections;
using Xcite.Csharp.lang;

namespace motoi.workbench.runtime {
    /// <summary>
    /// Provides an abstract implementation of <see cref="IWizard"/>.
    /// </summary>
    public abstract class AbstractWizard : AbstractDisposable, IWizard {
        private readonly IList<IWizardPage> iWizardPages = new List<IWizardPage>(5);
        private readonly IDictionary<IWizardPage, IGridComposite> iPageToCompositeMap = new Dictionary<IWizardPage, IGridComposite>(5);
        private ITitledAreaDialog iDialog;
        private IWizardPage iCurrentWizardPage;
        private int iCurrentWizardPageIndex;
        private IButton iBtnFinish;
        private IButton iBtnNext;
        private IButton iBtnPrev;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected AbstractWizard() { 
            Initialize(); // TODO Use a IInitializeExtension Pattern?
        }

        /// <summary>
        /// Tells the wizard to initialize itself.
        /// </summary>
        public virtual void Initialize() {
            iDialog = UIFactory.NewViewPart<ITitledAreaDialog>();
            if (iDialog == null) return; // TODO Use throwHelper

            iDialog.AddButton("Cancel", Cancel);
            iBtnFinish = iDialog.AddButton("Finish", Finish);
            iBtnNext = iDialog.AddButton("Next >", NavigateNext);
            iBtnPrev = iDialog.AddButton("< Back", NavigateBack);

            iBtnPrev.IsEnabled = false;
            iBtnFinish.IsEnabled = false;
            iBtnNext.IsEnabled = false;

            OnInitialize();

            if (PageCount == 0) return;

            iCurrentWizardPageIndex = 0;
            AssignCurrentWizardPageContent();
        }

        /// <summary>
        /// Is invoked when the instance is disposed. This method is intended to be overriden by clients. 
        /// But clients should call base.OnDispose().
        /// </summary>
        /// <param name="dispose">FALSE if the method is invoked from the GC</param>
        protected override void OnDispose(bool dispose) {
            try {
                OnDispose();

                iWizardPages.ForEach(page => { page.Wizard = null; page.Dispose(); });
                iWizardPages.Clear();
                iPageToCompositeMap.Clear();

                if (iCurrentWizardPage != null) {
                    iCurrentWizardPage.PropertyChanged -= OnPagePropertyChanged;
                    iCurrentWizardPage = null;
                }

                iBtnFinish = null;
                iBtnNext = null;
                iBtnPrev = null;

                iDialog.SetContent(null);
                iDialog = null;
            } catch {
                // Does not matter
            }
        }

        /// <summary>
        /// Adds the given page to the wizard.
        /// </summary>
        /// <param name="page">Page to add</param>
        public virtual void AddWizardPage(IWizardPage page) {
            iWizardPages.Add(page);
            page.Wizard = this;
        }

        /// <summary>
        /// Tells the wizard to cancel.
        /// </summary>
        public virtual void Cancel() {
            OnCancel();
            iDialog.Close();
        }

        /// <summary>
        /// Tells the wizard to finish.
        /// </summary>
        public virtual void Finish() {
            OnFinish();
            iDialog.Close();
        }

        /// <summary>
        /// Returns the count of pages the wizard owns.
        /// </summary>
        public virtual int PageCount { get { return iWizardPages.Count; } }

        /// <summary>
        /// Returns all added wizard pages.
        /// </summary>
        public virtual IWizardPage[] Pages { get { return Enumerable.ToArray(iWizardPages); } }

        /// <summary>
        /// Returns the title of the wizard.
        /// </summary>
        public virtual string Title {
            get { return iDialog.WindowTitle; }
            protected set { iDialog.WindowTitle = value; }
        }

        /// <summary>
        /// Shows the wizard.
        /// </summary>
        public virtual void Open() {
            iDialog.Show();
        }

        /// <summary>
        /// Updates the content pane of the wizard by assigning the content of 
        /// current wizard page.
        /// </summary>
        private void AssignCurrentWizardPageContent() {
            if (iCurrentWizardPage != null)
                iCurrentWizardPage.PropertyChanged -= OnPagePropertyChanged;

            iCurrentWizardPage = iWizardPages[iCurrentWizardPageIndex];
            iCurrentWizardPage.PropertyChanged += OnPagePropertyChanged;

            IGridComposite gridComposite;
            if (!iPageToCompositeMap.TryGetValue(iCurrentWizardPage, out gridComposite)) {
                gridComposite = UIFactory.NewWidget<IGridComposite>(iDialog);
                gridComposite.GridColumns = 1;
                gridComposite.GridRows = 1;
                iCurrentWizardPage.Initialize(gridComposite, FactoryProvider.GetWidgetFactory());
                iPageToCompositeMap.Add(iCurrentWizardPage, gridComposite);
            }

            iDialog.Title = iCurrentWizardPage.Title;
            iDialog.Description = iCurrentWizardPage.Description;
            iDialog.SetContent(gridComposite);

            CheckConditions();
        }

        /// <summary>
        /// Will check the wizard state after a page property has been changed.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="propertyChangedEventArgs">Event arguments</param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (sender != iCurrentWizardPage) return;
            CheckConditions();
        }

        /// <summary>
        /// Checks the state of the wizard and sets the behaviour of the wizard navigation buttons.
        /// </summary>
        private void CheckConditions() {
            iBtnFinish.IsEnabled = (iCurrentWizardPageIndex + 1) == PageCount && iCurrentWizardPage.CanLeave;
            iBtnNext.IsEnabled = !iBtnFinish.IsEnabled && iCurrentWizardPage.CanLeave;
            iBtnPrev.IsEnabled = (iCurrentWizardPageIndex - 1) > 0;
        }

        /// <summary>
        /// Performs a backward navigation.
        /// </summary>
        private void NavigateBack() {
            if (iCurrentWizardPageIndex - 1 < 0) return;
            iCurrentWizardPageIndex--;
            AssignCurrentWizardPageContent();
        }

        /// <summary>
        /// Performs a forward navigation.
        /// </summary>
        private void NavigateNext() {
            if (iCurrentWizardPageIndex + 1 >= PageCount) return;
            iCurrentWizardPageIndex++;
            AssignCurrentWizardPageContent();
        }

        /// <summary>
        /// Tells the subclass that initialize has been invoked.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Tells the subclass that cancel has been invoked.
        /// </summary>
        protected abstract void OnCancel();

        /// <summary>
        /// Tells the subclass that finish has been invoked.
        /// </summary>
        protected abstract void OnFinish();

        /// <summary>
        /// Tells the subclass that dispose has been invoked.
        /// </summary>
        protected abstract void OnDispose();
    }
}