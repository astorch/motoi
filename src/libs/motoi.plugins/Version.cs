using System;

namespace motoi.plugins { 
    /// <summary>
    /// Defines a version definition.
    /// </summary>
    public class Version {
        /// <summary>
        /// Backing variable for the version string.
        /// </summary>
        private readonly string iVersionString;

        /// <summary>
        /// Creates a new version instance based on the given version string.
        /// </summary>
        /// <param name="versionString">Version string</param>
        public Version(string versionString) {
            if (string.IsNullOrEmpty(versionString)) throw new NullReferenceException("Version string is null or empty!");

            iVersionString = versionString;
        }

        /// <summary>
        /// Returns true if the version is valid.
        /// </summary>
        public bool IsValid {
            get { return false; }
        }

        /// <summary>
        /// Gibt einen <see cref="T:System.String"/> zurück, der das aktuelle <see cref="T:System.Object"/> darstellt.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.String"/>, der das aktuelle <see cref="T:System.Object"/> darstellt.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString() {
            return iVersionString;
        }
    }
}