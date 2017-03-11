using System;
using motoi.platform.application.model;

namespace motoi.platform.application.annotations
{
    /// <summary>
    /// Tags an implementation of <see cref="IMotoiApplication"/> as global available 
    /// application that can be used for Motoi.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MotoiApplicationAttribute : Attribute {
        /// <summary>
        /// Returns the id of the application.
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="id">Id of the application</param>
        public MotoiApplicationAttribute(string id) {
            Id = id;
        }
    }
}