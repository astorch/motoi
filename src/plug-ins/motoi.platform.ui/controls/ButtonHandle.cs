using System;

namespace motoi.platform.ui.controls
{
    // TODO Refactor
    public class ButtonHandle : IButton {

        private readonly Func<bool> getIsEnabled = () => true;
        private readonly Action<bool> setIsEnabled = b => { };

        private readonly Func<string> getText = () => string.Empty;
        private readonly Action<string> setText = str => { };

        public ButtonHandle(Func<bool> getIsEnabled, Action<bool> setIsEnabled, 
            Func<string> getText, Action<string> setText) {
            this.getIsEnabled = getIsEnabled;
            this.setIsEnabled = setIsEnabled;
            this.getText = getText;
            this.setText = setText;
            }

        public bool IsEnabled {
            get { return getIsEnabled(); }
            set { setIsEnabled(value); }
        }

        public string Text {
            get { return getText(); }
            set { setText(value); }
        }
    }
}