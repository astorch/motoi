using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Motoi.SharpFace.Wizards.Pages;
using Motoi.UI;
using Motoi.UI.Controls;
using Motoi.UI.Shells;
using Motoi.UI.Widgets;

namespace Motoi.SharpFace.Wizards
{
    /// <summary>
    /// Provides an abstract default implementation of <see cref="IWizard"/>.
    /// </summary>
    public abstract class AbstractWizard : IWizard {

        private readonly IList<IWizardPage> iWizardPages = new List<IWizardPage>(5);
        private readonly IDictionary<IWizardPage,IGridComposite> iPageToCompositeMap = new Dictionary<IWizardPage, IGridComposite>(5);
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
            Initialize();
        }

        public void Dispose() {
            OnDispose();

            iWizardPages.Clear();
            iPageToCompositeMap.Clear();
        }

        /// <summary>
        /// Adds the given page to the wizard.
        /// </summary>
        /// <param name="page">Page to add</param>
        public void AddWizardPage(IWizardPage page) {
            iWizardPages.Add(page);
            page.Wizard = this;
        }

        /// <summary>
        /// Will check the wizard state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="propertyChangedEventArgs"></param>
        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (sender != iCurrentWizardPage)
                return;
            CheckConditions();
        }

        /// <summary>
        /// Returns the count of pages the wizard owns.
        /// </summary>
        public int PageCount { get { return iWizardPages.Count; } }

        /// <summary>
        /// Returns all added wizard pages.
        /// </summary>
        public IWizardPage[] Pages { get { return iWizardPages.ToArray(); } }

        /// <summary>
        /// Returns the title of the wizard.
        /// </summary>
        public string Title {
            get { return iDialog.WindowTitle; }
            protected set { iDialog.WindowTitle = value; }
        }

        /// <summary>
        /// Shows the wizard.
        /// </summary>
        public void Open() {
            iDialog.Show();
        }

        /// <summary>
        /// Performs a backward navigation.
        /// </summary>
        private void NavigateBack() {
            if (iCurrentWizardPageIndex - 1 < 0)
                return;
            iCurrentWizardPageIndex--;
            AssignCurrentWizardPageContent();
        }

        /// <summary>
        /// Performs a forward navigation.
        /// </summary>
        private void NavigateNext() {
            if (iCurrentWizardPageIndex + 1 >= PageCount)
                return;
            iCurrentWizardPageIndex++;
            AssignCurrentWizardPageContent();
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
                iCurrentWizardPage.Initialize(gridComposite);
                iPageToCompositeMap.Add(iCurrentWizardPage, gridComposite);
            }

            iDialog.Title = iCurrentWizardPage.Title;
            iDialog.Description = iCurrentWizardPage.Description;
            iDialog.ContentPane = gridComposite;
            
            CheckConditions();
        }

        /// <summary>
        /// Tells the wizard to initialize itself.
        /// </summary>
        public void Initialize() {
            iDialog = UIFactory.NewViewPart<ITitledAreaDialog>();
            if (iDialog == null) // TODO Use throwHelper
                return;

            iDialog.AddButton("Cancel", Cancel);
            iBtnFinish = iDialog.AddButton("Finish", Finish);
            iBtnNext = iDialog.AddButton("Next >", NavigateNext);
            iBtnPrev = iDialog.AddButton("< Back", NavigateBack);

            iBtnPrev.IsEnabled = false;
            iBtnFinish.IsEnabled = false;
            iBtnNext.IsEnabled = false;

            OnInitialize();

            if (PageCount == 0)
                return;

            iCurrentWizardPageIndex = 0;
            AssignCurrentWizardPageContent();
        }

        /// <summary>
        /// Tells the wizard to cancel.
        /// </summary>
        public void Cancel() {
            OnCancel();
            iDialog.Close();
        }

        /// <summary>
        /// Tells the wizard to finish.
        /// </summary>
        public void Finish() {
            OnFinish();
            iDialog.Close();
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