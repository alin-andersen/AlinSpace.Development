using System.Xml;

namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the extensions for <see cref="XmlAttributeCollection"/>.
    /// </summary>
    public static class XmlAttributeCollectionExtensions
    {
        /// <summary>
        /// Get attribute.
        /// </summary>
        /// <param name="collection">Attribute collection.</param>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>Attribute.</returns>
        public static XmlAttribute GetAttribute(
            this XmlAttributeCollection collection, 
            string name)
        {
            foreach (var a in collection)
            {
                var attribute = a as XmlAttribute;

                if (attribute == null)
                    continue;

                if (attribute.Name == name)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}
