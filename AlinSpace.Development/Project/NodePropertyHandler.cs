using System;
using System.Xml;

namespace AlinSpace.Development.Internal
{
    /// <summary>
    /// Represents the node property handler.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    internal class NodePropertyHandler<T>
    {
        private readonly XmlDocument document;
        private readonly XmlNode parentNode;
        private readonly string name;
        private readonly Func<T, string> serialize;

        private XmlNode node;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="document">Xml document.</param>
        /// <param name="parentNode">Parent Xml node.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="deserialize">Deserialize func.</param>
        /// <param name="serialize">Serialize func.</param>
        public NodePropertyHandler(
            XmlDocument document,
            XmlNode parentNode,
            string name,
            Func<string, T> deserialize,
            Func<T, string> serialize)
        {
            this.document = document;
            this.parentNode = parentNode;
            this.name = name;
            this.serialize = serialize;

            #region Initial read

            node = parentNode.SelectSingleNode(name);

            if (node != null)
            {
                try
                {
                    value = deserialize(node.InnerText);
                }
                catch
                {
                    value = default;
                }
            }

            #endregion
        }

        private T value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>Value.</returns>
        public T GetValue()
        {
            return value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void SetValue(T value)
        {
            if (value == null)
            {
                if (node != null)
                {
                    parentNode.RemoveChild(node);
                    node = null;
                }
            }
            else
            {
                if (node == null)
                {
                    node = document.CreateElement(name);
                    parentNode.AppendChild(node);
                }

                try
                {
                    node.InnerText = serialize(value);
                    this.value = value;
                }
                catch
                {
                    node.InnerText = null;
                    this.value = default;
                }
            }
        }
    }
}
