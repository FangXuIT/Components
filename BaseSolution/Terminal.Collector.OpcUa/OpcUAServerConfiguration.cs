using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Terminal.Collector.OpcUa
{
    /// <summary>
    /// Stores the configuration the data access node manager.
    /// </summary>
    [DataContract(Namespace = Namespaces.OpcUA)]
    public class OpcUAServerConfiguration
    {
        #region Constructors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public OpcUAServerConfiguration()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the object during deserialization.
        /// </summary>
        [OnDeserializing()]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        /// <summary>
        /// Sets private members to default values.
        /// </summary>
        private void Initialize()
        {
        }
        #endregion

        #region Public Properties
        #endregion

        #region Private Members
        #endregion
    }
}
