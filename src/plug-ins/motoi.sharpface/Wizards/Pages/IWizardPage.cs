using System;
using System.ComponentModel;
using Motoi.UI.Widgets;

namespace Motoi.SharpFace.Wizards.Pages
{
    /// <summary>
    /// Defines the interface of a Wizard Page.
    /// </summary>
    public interface IWizardPage : INotifyPropertyChanged, IDisposable {

        /// <summary>
        /// Returns the wizard the page is currently added to or does set it.
        /// </summary>
        IWizard Wizard { get; set; }
        
        /// <summary>
        /// Returns true if the page is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Returns true if the page can be left.
        /// </summary>
        bool CanLeave { get; }

        /// <summary>
        /// Returns a data context object or does set it.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Returns the title of wizard page.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Returns the description of the wizard page.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Tells the page to initialize its content.
        /// </summary>
        /// <param name="gridComposite">Element container</param>
        void Initialize(IGridComposite gridComposite);
    }
}