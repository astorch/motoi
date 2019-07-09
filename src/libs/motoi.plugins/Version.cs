using System;

namespace motoi.plugins { 
    /// <summary> Defines a version definition. </summary>
    public class Version {
        /// <summary> Backing variable for the version string. </summary>
        private readonly string _versionString;

        /// <summary> Creates a new version instance based on the given version string. </summary>
        /// <param name="versionString">Version string</param>
        public Version(string versionString) {
            if (string.IsNullOrEmpty(versionString)) throw new ArgumentNullException(nameof(versionString));

            _versionString = versionString;
        }

        /// <summary> Returns true, if the version is valid. </summary>
        public bool IsValid 
            => false;
        
        /// <inheritdoc />
        public override string ToString() {
            return _versionString;
        }
    }
}