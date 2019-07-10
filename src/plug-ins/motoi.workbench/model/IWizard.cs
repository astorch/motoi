using System;

namespace motoi.workbench.model {
    /// <summary> Defines a wizard. </summary>
    public interface IWizard : IDisposable {
        /// <summary> Tells the wizard to initialize itself. </summary>
        void Initialize();

        /// <summary> Adds the given page to the wizard. </summary>
        /// <param name="page">Page to add</param>
        void AddWizardPage(IWizardPage page);

        /// <summary> Tells the wizard to cancel. </summary>
        void Cancel();

        /// <summary> Tells the wizard to finish. </summary>
        void Finish();

        /// <summary> Returns the count of pages the wizard owns. </summary>
        int PageCount { get; }

        /// <summary> Returns all added wizard pages. </summary>
        IWizardPage[] Pages { get; }

        /// <summary> Returns the title of the wizard. </summary>
        string Title { get; }

        /// <summary> Shows the wizard. </summary>
        void Open();
    }
}