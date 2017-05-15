using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;

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
        static private readonly ILog fLogWriter = LogManager.GetLogger(typeof(TObject));

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
                // Set up platform language
                NL = (string) Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process)["motoi:nl"] ?? "en_US";

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
                    string fieldValue;
                    if (!messageSet.TryGetValue(fieldName, out fieldValue)) {
                        fLogWriter.WarnFormat("{0} declares a message that has not been set by a NLS resource file",
                            nlsAccessType);
                        continue;
                    }

                    messageSet.Remove(fieldName);
                    try {
                        field.SetValue(null, fieldValue);
                    } catch (Exception ex) {
                        fLogWriter.Error("Error on applying localized text", ex);
                    }
                }

                // Log unused messages
                using (Dictionary<string, string>.Enumerator itr = messageSet.GetEnumerator()) {
                    while (itr.MoveNext()) {
                        KeyValuePair<string, string> entry = itr.Current;
                        string key = entry.Key;
                        fLogWriter.WarnFormat("Unused message found for key '{0}'", key);
                    }
                }
            } catch (Exception ex) {
                fLogWriter.Error($"Error on applying localized messages to '{nlsAccessType}'", ex);
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

        /// <summary> Returns the current set natural language of the platform. </summary>
        static public string NL { get; private set; }
    }

    /// <summary> Provides non-generic access to NLS features. </summary>
    public abstract class NLS {
        internal const BindingFlags FieldBindingFlags = BindingFlags.Public | BindingFlags.Static;
        static private readonly ILog fLogWriter = LogManager.GetLogger(typeof(NLS));

        /// <summary>
        /// Returns the localized text of the <paramref name="key"/> that is associated with the given 
        /// <paramref name="localizationId"/>. If any argument is NULL or empty, the key is not mapped 
        /// or the key is not accessible through a missing field in the provider type, NULL is returned. 
        /// However, a waring log entry is written that describes the error in detail.
        /// </summary>
        /// <param name="localizationId">Localization id</param>
        /// <param name="key">NLS key</param>
        /// <returns>Localized text or NULL</returns>
        static public string GetText(string localizationId, string key) {
            if (string.IsNullOrEmpty(localizationId)) return null;
            if (string.IsNullOrEmpty(key)) return null;

            // Resolve provider
            Type providerType = NLSRegistry.Instance.GetProviderType(localizationId);
            if (providerType == null) {
                fLogWriter.WarnFormat("There is no NLS provider for the localization id '{0}'", localizationId);
                return null;
            }

            // Resolve provider field
            FieldInfo field = providerType.GetField(key, FieldBindingFlags);
            if (field == null) {
                fLogWriter.WarnFormat("NLS provider '{0}' defines no field for key '{1}'", providerType, key);
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
        static public string GetLocalizationId(Assembly assembly) {
            return assembly?.GetName().Name;
        }

        /// <summary>
        /// Returns the localization id based on the assembly of the given <paramref name="type"/>. 
        /// If the given type is NULL, NULL is returned.
        /// </summary>
        /// <param name="type">Type the localization id is derived from</param>
        /// <returns>Localization id or NULL</returns>
        static public string GetLocalizationId(Type type) {
            return GetLocalizationId(type?.Assembly);
        }

        /// <summary>
        /// Returns the localization id based on the assembly that provides the given <paramref name="obj"/>. 
        /// If hte given object is NULL, NULL is returned.
        /// </summary>
        /// <param name="obj">Object the localization id is derived from</param>
        /// <returns>Localization id or NULL</returns>
        static public string GetLocalizationId(object obj) {
            return GetLocalizationId(obj?.GetType());
        }
    }
}