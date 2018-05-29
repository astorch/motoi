using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using NLog;

namespace motoi.platform.nls {
    /// <summary>
    /// Implements the localization support of plug-ins. The common pattern to define and load messages is as follows:
    /// <code>
    /// public class Messages : NLS&lt;Messages&gt; {
    ///     static Messages() {
    ///         LoadMessages();
    ///     }
    /// 
    ///     public static readonly string MyWindow_Title;
    ///     // ...
    /// }
    /// </code>
    /// Note, the localization contribution has to be registered using the extension point!
    /// </summary>
    /// <typeparam name="TObject">Type of subclass that provides the fields to set up</typeparam>
    public abstract class NLS<TObject> : NLS where TObject : NLS<TObject> {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Loads the messages according to the fields declared by the current class type. 
        /// The localization id is derived from the current subclass type <typeparamref name="TObject"/>.
        /// </summary>
        static protected void LoadMessages() {
            LoadMessages(null);
        }

        /// <summary>
        /// Loads the messages according to the fields declared by the current class type using the given 
        /// <paramref name="localizationId"/>. If <paramref name="localizationId"/> is NULL or empty, the 
        /// id is derived from the current subclass type <typeparamref name="TObject"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        static protected void LoadMessages(string localizationId) {
            Type nlsAccessType = typeof(TObject);
            localizationId = string.IsNullOrEmpty(localizationId)
                                    ? GetLocalizationId(nlsAccessType)
                                    : localizationId;

            try {
                // Grab fields to set up
                FieldInfo[] fields = nlsAccessType.GetFields(FieldBindingFlags);

                // Loads messages according to the NL specialization
                string[] nlSpecs = NL.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
                string majorNlSpec = nlSpecs[0];
                string minorNlSpec = nlSpecs.Length > 1 ? nlSpecs[1] : null;

                Dictionary<string, string> messageSet = new Dictionary<string, string>(fields.Length);

                // Load default messages
                string defaultNlFileContent = NLSRegistry.Instance.GetResourceFile(localizationId, null);
                PutMessages(defaultNlFileContent, messageSet);

                // Add major NL specialization
                if (!string.IsNullOrEmpty(majorNlSpec)) {
                    string majorNlFileContent = NLSRegistry.Instance.GetResourceFile(localizationId, majorNlSpec);
                    if (!string.IsNullOrEmpty(majorNlFileContent))
                        PutMessages(majorNlFileContent, messageSet);
                }

                // Add minor NL specialization
                if (!string.IsNullOrEmpty(minorNlSpec)) {
                    string minorNlFileContent = NLSRegistry.Instance.GetResourceFile(localizationId, NL);
                    if (!string.IsNullOrEmpty(minorNlFileContent))
                        PutMessages(minorNlFileContent, messageSet);
                }

                // Apply all messages to the fields
                for (int i = -1; ++i != fields.Length;) {
                    FieldInfo field = fields[i];
                    string fieldName = field.Name;
                    if (!messageSet.TryGetValue(fieldName, out string fieldValue)) {
                        _log.Warn($"{nlsAccessType} declares a message that has not been set by a NLS resource file");
                        continue;
                    }

                    messageSet.Remove(fieldName);
                    try {
                        field.SetValue(null, fieldValue);
                    } catch (Exception ex) {
                        _log.Error(ex, "Error on applying localized text");
                    }
                }

                // Log unused messages
                using (Dictionary<string, string>.Enumerator itr = messageSet.GetEnumerator()) {
                    while (itr.MoveNext()) {
                        KeyValuePair<string, string> entry = itr.Current;
                        string key = entry.Key;
                        _log.Warn($"Unused message found for key '{key}'");
                    }
                }
            } catch (Exception ex) {
                _log.Error(ex, $"Error on applying localized messages to '{nlsAccessType}'");
            }
        }

        /// <summary>
        /// Puts all messages of the resource file given by <paramref name="resourceFileContent"/> 
        /// into the given <paramref name="messageSet"/>. Already existing keys are overriden.
        /// </summary>
        /// <param name="resourceFileContent">Content of the resource file</param>
        /// <param name="messageSet">Set all found messages are inserted to</param>
        static private void PutMessages(string resourceFileContent, IDictionary<string, string> messageSet) {
            string[] messages = resourceFileContent.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = -1; ++i != messages.Length;) {
                string message = messages[i];
                string[] keyValueSet = message.Split(new[] {'='}, StringSplitOptions.None);
                string key = keyValueSet[0];
                string text = keyValueSet[1];
                messageSet[key] = text;
            }
        }
    }

    /// <summary> Provides non-generic access to NLS features. </summary>
    public abstract class NLS {
        internal const BindingFlags FieldBindingFlags = BindingFlags.Public | BindingFlags.Static;
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>  Set up NL property once </summary>
        static NLS() {
            // Set up platform language
            NL = (string)Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process)["motoi:nl"] ?? "en_US";
        }

        /// <summary> Returns the current set natural language of the platform. </summary>
        public static string NL { get; }

        /// <summary>
        /// Localizes the given <paramref name="key"/> by deriving the localization id from 
        /// the given <paramref name="assembly"/>. If the key doesn't start with a '%', 
        /// the key itself is returned.
        /// </summary>
        /// <param name="key">Key to localize. Should start with an '%'</param>
        /// <param name="assembly">Assembly the localization id is derived from</param>
        /// <returns>The key itself or the localized text</returns>
        public static string Localize(string key, Assembly assembly) 
            => LocalizeInternal(key, assembly, GetLocalizationId);

        /// <summary>
        /// Localizes the given <paramref name="key"/> by deriving the localization id from 
        /// the given <paramref name="type"/>. If the key doesn't start with a '%', 
        /// the key itself is returned.
        /// </summary>
        /// <param name="key">Key to localize. Should start with an '%'</param>
        /// <param name="type">Type the localization id is derived from</param>
        /// <returns>The key itself or the localized text</returns>
        static public string Localize(string key, Type type) 
            => LocalizeInternal(key, type, GetLocalizationId);

        /// <summary>
        /// Localizes the given <paramref name="key"/> by deriving the localization id from 
        /// the given <paramref name="obj"/>. If the key doesn't start with a '%', 
        /// the key itself is returned.
        /// </summary>
        /// <param name="key">Key to localize. Should start with an '%'</param>
        /// <param name="obj">Object the localization id is derived from</param>
        /// <returns>The key itself or the localized text</returns>
        static public string Localize(string key, object obj) 
            => LocalizeInternal(key, obj, GetLocalizationId);

        /// <summary>
        /// Returns the localized text of the <paramref name="key"/> that is associated with the given 
        /// <paramref name="localizationId"/>. If any argument is NULL or empty, the key is not mapped 
        /// or the key is not accessible through a missing field in the provider type, NULL is returned. 
        /// However, a waring log entry is written that describes the error in detail.
        /// </summary>
        /// <param name="localizationId">Localization id</param>
        /// <param name="key">NLS key</param>
        /// <returns>Localized text or NULL</returns>
        public static string GetText(string localizationId, string key) {
            if (string.IsNullOrEmpty(localizationId)) return null;
            if (string.IsNullOrEmpty(key)) return null;

            // Resolve provider
            Type providerType = NLSRegistry.Instance.GetProviderType(localizationId);
            if (providerType == null) {
                _log.Warn($"There is no NLS provider for the localization id '{localizationId}'");
                return null;
            }

            // Resolve provider field
            FieldInfo field = providerType.GetField(key, FieldBindingFlags);
            if (field == null) {
                _log.Warn($"NLS provider '{providerType}' defines no field for key '{key}'");
                return null;
            }

            // Return value
            return (string) field.GetValue(null);
        }

        /// <summary>
        /// Returns the localization id of the given <paramref name="assembly"/>. If the given 
        /// assembly is NULL, NULL is returned.
        /// </summary>
        /// <param name="assembly">Assembly the localization id is derived from</param>
        /// <returns>Localization id or NULL</returns>
        public static string GetLocalizationId(Assembly assembly) 
            => assembly?.GetName().Name;

        /// <summary>
        /// Returns the localization id based on the assembly of the given <paramref name="type"/>. 
        /// If the given type is NULL, NULL is returned.
        /// </summary>
        /// <param name="type">Type the localization id is derived from</param>
        /// <returns>Localization id or NULL</returns>
        public static string GetLocalizationId(Type type) 
            => GetLocalizationId(type?.Assembly);

        /// <summary>
        /// Returns the localization id based on the assembly that provides the given <paramref name="obj"/>. 
        /// If hte given object is NULL, NULL is returned.
        /// </summary>
        /// <param name="obj">Object the localization id is derived from</param>
        /// <returns>Localization id or NULL</returns>
        public static string GetLocalizationId(object obj) 
            => GetLocalizationId(obj?.GetType());

        /// <summary>
        /// Localizes the given <paramref name="key"/> with the help of the given <paramref name="getLocalizationId"/> 
        /// method. The key is checked if it's localized before. If the key is not localized, this method 
        /// returns the key itself.
        /// </summary>
        /// <typeparam name="TArg">Argument type of the <paramref name="getLocalizationId"/> method</typeparam>
        /// <param name="key">Key to localize</param>
        /// <param name="arg">Argument of the <paramref name="getLocalizationId"/> method</param>
        /// <param name="getLocalizationId">Method that provides the localization id based on the given <paramref name="arg"/></param>
        /// <returns>The key itself or the localized text</returns>
        private static string LocalizeInternal<TArg>(string key, TArg arg, Func<TArg, string> getLocalizationId) {
            if (!IsLocalized(key)) return key;
            string baseKey = key.Substring(1);
            string localizationId = getLocalizationId(arg);
            return GetText(localizationId, baseKey);
        }

        /// <summary>
        /// Returns TRUE if the given <paramref name="key"/> indicates a localization. This is the case 
        /// if the key starts with an '%' character.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>TRUE or FALSE</returns>
        private static bool IsLocalized(string key) {
            if (string.IsNullOrEmpty(key)) return false;
            if (key[0] != '%') return false;
            return true;
        }
    }
}