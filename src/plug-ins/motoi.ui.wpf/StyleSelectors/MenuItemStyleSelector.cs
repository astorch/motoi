using System.Windows;
using System.Windows.Controls;
using motoi.platform.ui.menus;

namespace Motoi.UI.WPF.StyleSelectors
{
    /// <summary>
    /// Implements a <see cref="StyleSelector"/> for setting a style to menu items 
    /// where were defined by extension points.
    /// </summary>
    public class MenuItemStyleSelector : StyleSelector {

        public override Style SelectStyle(object item, DependencyObject container) {
            FrameworkElement frameworkElement = container as FrameworkElement;
            if (frameworkElement == null)
                return null;

            if (item is MenuContribution) {
                object style = frameworkElement.TryFindResource("MenuStyle");
                return style as Style;
            }

            if (item is MenuItemContribution) {
                object style = frameworkElement.TryFindResource("MenuItemStyle");
                return style as Style;
            }

            return base.SelectStyle(item, container);
        }
       
    }
}