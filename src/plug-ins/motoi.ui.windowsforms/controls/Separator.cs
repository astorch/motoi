using System.Windows.Forms;

namespace motoi.ui.windowsforms.controls {
    /// <summary>
    /// Provides a label which looks like a separator.
    /// </summary>
    public class Separator : Label {
        /// <summary>
        /// Ruft die Rahmenart des Steuerelements ab oder legt diese fest.
        /// </summary>
        /// <returns>
        /// Einer der <see cref="T:System.Windows.Forms.BorderStyle"/>-Werte.Der Standardwert ist BorderStyle.None.
        /// </returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Der zugewiesene Wert ist keiner der <see cref="T:System.Windows.Forms.BorderStyle"/>-Werte. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override BorderStyle BorderStyle { get { return BorderStyle.Fixed3D; } }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob die Größe des Steuerelements automatisch an dessen Inhalt angepasst wird, oder legt diesen fest.
        /// </summary>
        /// <returns>
        /// true, wenn das Steuerelement seine Breite dem Inhalt möglichst genau anpasst, andernfalls false.Der Standardwert ist true.
        /// </returns>
        /// <filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override bool AutoSize { get { return false; } }

        /// <returns>
        /// Der diesem Steuerelement zugeordnete Text.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Text { get { return string.Empty; } }
    }
}