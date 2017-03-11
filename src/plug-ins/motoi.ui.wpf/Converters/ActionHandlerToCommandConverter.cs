using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using motoi.platform.ui.actions;
using Motoi.UI.WPF.Commands;

namespace Motoi.UI.WPF.Converters
{
    /// <summary>
    /// Provides an implementation of <see cref="IValueConverter"/> to bind delegates 
    /// to commands.
    /// </summary>
    [ValueConversion(typeof(IActionHandler), typeof(ICommand))]
    public class ActionHandlerToCommandConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            IActionHandler actionHandler = value as IActionHandler;
            if (actionHandler == null)
                return null;

            ICommand command = new RelayCommand(o => actionHandler.Run(), o => actionHandler.IsEnabled);
            return command;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}