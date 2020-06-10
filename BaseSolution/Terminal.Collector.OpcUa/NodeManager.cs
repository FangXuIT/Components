using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.OpcUa
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class NodeManager : CustomNodeManager2
    {
        private ServerDataSource DataSource;

        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public NodeManager(IServerInternal server, ApplicationConfiguration configuration)
        :
            base(server, configuration, Namespaces.OpcUA)
        {
            SystemContext.NodeIdFactory = this;

            // get the configuration for the node manager.
            m_configuration = configuration.ParseExtension<OpcUAServerConfiguration>();

            // use suitable defaults if no configuration exists.
            if (m_configuration == null)
            {
                m_configuration = new OpcUAServerConfiguration();
            }
            DataSource = new ServerDataSource();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TBD
            }
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        public override NodeId New(ISystemContext context, NodeState node)
        {
            return node.NodeId;
        }
        #endregion

        #region INodeManager Members
        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                BaseObjectState trigger = new BaseObjectState(null);

                trigger.NodeId = new NodeId(1, NamespaceIndex);
                trigger.BrowseName = new QualifiedName("Trigger", NamespaceIndex);
                trigger.DisplayName = trigger.BrowseName.Name;
                trigger.TypeDefinitionId = ObjectTypeIds.BaseObjectType;

                // ensure trigger can be found via the server object. 
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                trigger.AddReference(ReferenceTypeIds.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypeIds.Organizes, false, trigger.NodeId));

                //var manager = new TargetsManager();
                //var targetDir = manager.GetAllTargets();
                //foreach (var item in targetDir)
                //{
                //    var targetName = item.Key;
                //    var targetSimpleName = targetName.Substring(targetName.LastIndexOf('.') + 1, targetName.Length - targetName.LastIndexOf('.') - 1);
                //    var dataTypeId = DataTypeHelper.GetDataTypeId(item.Value);

                //    PropertyState property = BuildPropertyState(trigger, targetName, targetSimpleName, dataTypeId, manager);
                //    trigger.AddChild(property);
                //}

                // save in dictionary. 
                AddPredefinedNode(SystemContext, trigger);

                ReferenceTypeState referenceType = new ReferenceTypeState();

                referenceType.NodeId = new NodeId(3, NamespaceIndex);
                referenceType.BrowseName = new QualifiedName("IsTriggerSource", NamespaceIndex);
                referenceType.DisplayName = referenceType.BrowseName.Name;
                referenceType.InverseName = new LocalizedText("IsSourceOfTrigger");
                referenceType.SuperTypeId = ReferenceTypeIds.NonHierarchicalReferences;

                if (!externalReferences.TryGetValue(ObjectIds.Server, out references))
                {
                    externalReferences[ObjectIds.Server] = references = new List<IReference>();
                }

                trigger.AddReference(referenceType.NodeId, false, ObjectIds.Server);
                references.Add(new NodeStateReference(referenceType.NodeId, true, trigger.NodeId));

                DataSource.NamespaceIndex = NamespaceIndex;
                DataSource.Trigger = trigger;
                DataSource.SystemContext = this.SystemContext;
                //DataSource.StartFlashValue();

                // save in dictionary. 
                AddPredefinedNode(SystemContext, referenceType);
            }
        }

        private PropertyState BuildPropertyState(BaseObjectState trigger, string targetName, string targetSimpleName, NodeId dataTypeId)
        {
            PropertyState property = new PropertyState(trigger);

            property.NodeId = new NodeId(targetName, NamespaceIndex);
            property.BrowseName = new QualifiedName(targetSimpleName, NamespaceIndex);
            property.DisplayName = property.BrowseName.Name;
            property.TypeDefinitionId = VariableTypeIds.PropertyType;
            property.ReferenceTypeId = ReferenceTypeIds.HasProperty;
            property.WriteMask = AttributeWriteMask.None;
            property.UserWriteMask = AccessLevels.None;
            property.AccessLevel = (byte)AccessLevels.CurrentReadOrWrite;
            property.UserAccessLevel = (byte)AccessLevels.CurrentReadOrWrite;
            property.MinimumSamplingInterval = MinimumSamplingIntervals.Continuous;
            property.Historizing = false;
            property.AccessRestrictions = AccessRestrictionType.None;
            property.ValueRank = ValueRanks.Scalar;
            property.DataType = dataTypeId;
            property.OnWriteValue = Property_NodeValueEventHandler;

            //object value = DataSource.ParseTargetValue(dataTypeId, manager.GetTargetValue(string.Format("ns=2;s={0}", targetName)));
            //if (value != null) property.Value = value;

            return property;
        }

        public ServiceResult Property_NodeValueEventHandler(ISystemContext context, NodeState node, NumericRange indexRange, QualifiedName dataEncoding, ref object value, ref StatusCode statusCode, ref DateTime timestamp)
        {
            ServiceResult result = new ServiceResult(statusCode);
            //var manager = new TargetsManager();
            //manager.SetTargetValue(node.NodeId.Identifier.ToString(), value);

            return result;
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            lock (Lock)
            {
                // TBD
            }
        }

        /// <summary>
        /// Returns a unique handle for the node.
        /// </summary>
        protected override NodeHandle GetManagerHandle(ServerSystemContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            lock (Lock)
            {
                // quickly exclude nodes that are not in the namespace. 
                if (!IsNodeIdInNamespace(nodeId))
                {
                    return null;
                }

                NodeState node = null;

                if (!PredefinedNodes.TryGetValue(nodeId, out node))
                {
                    return null;
                }

                NodeHandle handle = new NodeHandle();

                handle.NodeId = nodeId;
                handle.Node = node;
                handle.Validated = true;

                return handle;
            }
        }

        /// <summary>
        /// Verifies that the specified node exists.
        /// </summary>
        protected override NodeState ValidateNode(
            ServerSystemContext context,
            NodeHandle handle,
            IDictionary<NodeId, NodeState> cache)
        {
            // not valid if no root.
            if (handle == null)
            {
                return null;
            }

            // check if previously validated.
            if (handle.Validated)
            {
                return handle.Node;
            }

            // TBD

            return null;
        }
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        private OpcUAServerConfiguration m_configuration;
        #endregion
    }
}
