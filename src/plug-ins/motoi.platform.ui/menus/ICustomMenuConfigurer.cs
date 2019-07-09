namespace motoi.platform.ui.menus {
    /// <summary>
    /// Provides methods that are hook by the platform
    /// to apply a custom menu configuration.
    /// </summary>
    public interface ICustomMenuConfigurer {
        /// <summary> Notifies the instance to (re-)configure the current menu contribution set. </summary>
        /// <param name="menuContributionSet">Current menu contribution set</param>
        /// <returns>A set of menu contributions to display</returns>
        MenuContribution[] Configure(MenuContribution[] menuContributionSet);
    }
}