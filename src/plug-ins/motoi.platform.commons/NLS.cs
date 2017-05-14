using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;
using motoi.platform.application;

namespace motoi.platform.commons {
    /// <summary>
    /// Implements the localization support of plug-ins. The common pattern to define and load messages is as follows:
    /// <code>
    /// public class Messages : NLS&lt;Messages&gt; {
    ///     static Messages() {
    ///         LoadMessages("resources/texts/messages");
    ///     }
    /// 
    ///     public static readonly string MyWindow_Title;
    ///     // ...
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="TObject">Type of subclass that provides the fields to set up</typeparam>
    public abstract class NLS<TObject> where TObject : NLS<TObject> {

        static private readonly ILog fLogWriter = LogManager.GetLogger(typeof(TObject));
        
        /// <summary>
        /// Loads the messages according to the fields declared by the current class type from the given 
        /// resource path. Note, the targeted resources have to be embedded resources.
        /// </summary>
        /// <param name="resourcePath">Path to the (embedded) resources</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        static protected void LoadMessages(string resourcePath) {
            Type nlsAccessType = typeof(TObject);

            try {
                // Set up platform language
                NL = PlatformSettings.Instance.Get("nl") ?? "en_US";

                // Ensure a valid resource file path 
                if (string.IsNullOrEmpty(resourcePath)) throw new LocalizationException($"Resource file path of '{nlsAccessType}' is NULL or empty");

                // Grab fields to set up
                FieldInfo[] fields = nlsAccessType.GetFields(BindingFlags.Public | BindingFlags.Static);

                // Loads messages according to the NL specialization
                string[] nlSpecs = NL.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string majorNlSpec = nlSpecs[0];
                string minorNlSpec = nlSpecs.Length > 1 ? nlSpecs[1] : null;

                Dictionary<string, string> messageSet = new Dictionary<string, string>(fields.Length);
                string defaultNlsResourceFile = $"{resourcePath}.txt";
                PutMessages(defaultNlsResourceFile, true, messageSet);

                // Add major NL specialization
                if (!string.IsNullOrEmpty(majorNlSpec)) {
                    string majorNlsResourceFile = $"{resourcePath}_{majorNlSpec}.txt";
                    PutMessages(majorNlsResourceFile, false, messageSet);
                }
                
                // Add minor NL specialization
                if (!string.IsNullOrEmpty(minorNlSpec)) {
                    string minorNlsResourceFile = $"{resourcePath}_{majorNlSpec}_{minorNlSpec}.txt";
                    PutMessages(minorNlsResourceFile, false, messageSet);
                }
                
                // Apply all messages to the fields
                for (int i = -1; ++i != fields.Length;) {
                    FieldInfo field = fields[i];
                    string fieldName = field.Name;
                    string fieldValue;
                    if (!messageSet.TryGetValue(fieldName, out fieldValue)) {
                        fLogWriter.WarnFormat("{0} declares a message that has not been set by a NLS resource file", nlsAccessType);
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
        /// Puts all messages of the resource file specified by the given <paramref name="resourcePath"/> 
        /// into the given <paramref name="messageSet"/>. Already existing keys are overriden. If there is no 
        /// resource file under the given <paramref name="resourcePath"/>, an exception is thrown if 
        /// <paramref name="mandatory"/> is TRUE. Otherwise, nothing will happen.
        /// </summary>
        /// <param name="resourcePath">Path to the resource file</param>
        /// <param name="mandatory">If TRUE an exception is thrown if the resource file does not exist</param>
        /// <param name="messageSet">Set all found messages are inserted to</param>
        /// <exception cref="LocalizationException">If the <paramref name="resourcePath"/> is <paramref name="mandatory"/> but does not exist</exception>
        static private void PutMessages(string resourcePath, bool mandatory, IDictionary<string, string> messageSet) {
            string fileContent;
            using (Stream stream = ResourceLoader.OpenStream(typeof(TObject).Assembly, resourcePath)) {
                if (stream == null) {
                    if (mandatory) throw new LocalizationException($"Could not open stream to '{resourcePath}'");
                    return;
                }
                using (StreamReader streamReader = new StreamReader(stream)) {
                    fileContent = streamReader.ReadToEnd();
                }
            }

            string[] messages = fileContent.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
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
}