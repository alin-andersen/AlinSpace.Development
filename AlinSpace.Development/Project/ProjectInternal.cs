using AlinSpace.Development.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace AlinSpace.Development
{
    /// <summary>
    /// Represents the implementation for <see cref="IProject"/>.
    /// </summary>
    internal class ProjectInternal : IProject
    {
        private XmlDocument document;

        private XmlNode projectNode;
        private XmlNode propertyGroupNode;

        private XmlNode versionNode;
        private XmlNode generatePackageOnBuildNode;

        public ProjectInternal(string pathToProjectFile)
        {
            PathToProjectFile = PathHelper.MakeRoot(pathToProjectFile);
            Name = Path.GetFileNameWithoutExtension(pathToProjectFile);
            document = new XmlDocument();
        }

        public static IProject Open(string pathToProjectFile)
        {
            var project = new ProjectInternal(pathToProjectFile);
            project.Init();

            return project;
        }

        private void Init()
        {
            document.Load(PathToProjectFile);

            projectNode = document.SelectSingleNode("/Project");
            propertyGroupNode = projectNode.SelectSingleNode("PropertyGroup");

            // Version
            versionHandler = new NodePropertyHandler<Version>(
                document,
                propertyGroupNode,
                "Version",
                v => Version.Parse(v),
                v => v.ToString());

            // GeneratePackageOnBuild
            generatePackageOnBuildHandler = new NodePropertyHandler<bool?>(
                document,
                propertyGroupNode,
                "GeneratePackageOnBuild",
                v => Convert.ToBoolean(v),
                v => v.GetValueOrDefault() ? "true" : "false");

            // Authors
            authorsHandler = new NodePropertyHandler<string>(
                document,
                propertyGroupNode,
                "Authors",
                v => v,
                v => v);

            // Copyright
            copyrightHandler = new NodePropertyHandler<string>(
                document,
                propertyGroupNode,
                "Copyright",
                v => v,
                v => v);

            // PackageProjectUrl
            packageProjectUrlHandler = new NodePropertyHandler<Uri>(
                document,
                propertyGroupNode,
                "PackageProjectUrl",
                v => new Uri(v),
                v => v.ToString());

            // PackageProjectUrl
            packageProjectUrlHandler = new NodePropertyHandler<Uri>(
                document,
                propertyGroupNode,
                "PackageProjectUrl",
                v => new Uri(v),
                v => v.ToString());

            // RepositoryUrl
            repositoryUrlHandler = new NodePropertyHandler<Uri>(
                document,
                propertyGroupNode,
                "RepositoryUrl",
                v => new Uri(v),
                v => v.ToString());

            // PackageTags
            packageTagsHandler = new NodePropertyHandler<string>(
                document,
                propertyGroupNode,
                "PackageTags",
                v => v,
                v => v);
        }

        public string PathToProjectFile { get; private set; }

        public string Name { get; private set; }

        public ProjectType Type { get; private set; }

        #region Version

        private NodePropertyHandler<Version> versionHandler;

        public Version Version
        {
            get => versionHandler.GetValue();
            set => versionHandler.SetValue(value);
        }

        void CheckSetVersion()
        {
            if (Version == null)
            {
                Version = new Version(0, 0, 0);
            }
        }

        public IProject VersionIncrementMajor()
        {
            CheckSetVersion();

            Version = new Version(
                Version.Major + 1,
                Version.Minor,
                Version.Build);

            return this;
        }

        public IProject VersionIncrementMinor()
        {
            CheckSetVersion();

            Version = new Version(
                Version.Major,
                Version.Minor + 1,
                Version.Build);

            return this;
        }

        public IProject VersionIncrementBuild()
        {
            CheckSetVersion();

            Version = new Version(
                Version.Major,
                Version.Minor,
                Version.Build + 1);
            
            return this;
        }

        #endregion

        #region GeneratePackageOnBuild

        private NodePropertyHandler<bool?> generatePackageOnBuildHandler;

        public bool? GeneratePackageOnBuild
        {
            get => generatePackageOnBuildHandler.GetValue();
            set => generatePackageOnBuildHandler.SetValue(value);
        }

        #endregion

        #region Dependencies

        public IProject AddOrUpdateDependency(string name, Version version)
        {
            var dependency = this
                .GetDependencies()
                .FirstOrDefault(x => x.Name == name);

            if (dependency != null)
            {
                dependency.Version = version;
            }
            else
            {
                var itemGroup = projectNode
                    .GetNodes()
                    .FirstOrDefault(x => x.Name == "ItemGroup");

                if (itemGroup == null)
                {
                    itemGroup = document.CreateElement("ItemGroup");
                    projectNode.AppendChild(itemGroup);
                }

                var packageReferenceNode = document.CreateElement("PackageReference");
                itemGroup.AppendChild(packageReferenceNode);

                #region Include

                var includeAttribute = document.CreateAttribute("Include");
                includeAttribute.Value = name;

                packageReferenceNode.Attributes.Append(includeAttribute);

                #endregion

                #region Version

                var versionAttribute = document.CreateAttribute("Version");
                versionAttribute.Value = version.ToString();

                packageReferenceNode.Attributes.Append(versionAttribute);

                #endregion
            }

            return this;
        }

        public IEnumerable<IDependency> GetDependencies()
        {
            foreach (var itemGroupNode in projectNode.GetNodes("ItemGroup"))
            {
                foreach(var packageReferenceNode in itemGroupNode.GetNodes("PackageReference"))
                {
                    yield return new DependencyInternal(itemGroupNode, packageReferenceNode);
                }
            }
        }

        public IProject SetDependency(string name, Version version)
        {
            var dependency = this
                .GetDependencies()
                .FirstOrDefault(x => x.Name == name);

            if (dependency != null)
            {
                dependency.Version = version;
            }
            else
            {
                // todo
            }

            return this;
        }

        #endregion

        #region Authors

        private NodePropertyHandler<string> authorsHandler;

        public string Authors
        {
            get => authorsHandler.GetValue();
            set => authorsHandler.SetValue(value);
        }

        #endregion

        #region Copyright

        private NodePropertyHandler<string> copyrightHandler;

        public string Copyright
        {
            get => copyrightHandler.GetValue();
            set => copyrightHandler.SetValue(value);
        }

        #endregion

        #region PackageProjectUrl

        private NodePropertyHandler<Uri> packageProjectUrlHandler;

        public Uri PackageProjectUrl
        {
            get => packageProjectUrlHandler.GetValue();
            set => packageProjectUrlHandler.SetValue(value);
        }

        #endregion

        #region RepositoryUrl

        private NodePropertyHandler<Uri> repositoryUrlHandler;

        public Uri RepositoryUrl
        {
            get => repositoryUrlHandler.GetValue();
            set => repositoryUrlHandler.SetValue(value);
        }

        #endregion

        #region PackageTags

        private NodePropertyHandler<string> packageTagsHandler;

        public string PackageTags
        {
            get => packageTagsHandler.GetValue();
            set => packageTagsHandler.SetValue(value);
        }

        #endregion

        public void Save()
        {
            document.Save(PathToProjectFile);
        }
    }
}
