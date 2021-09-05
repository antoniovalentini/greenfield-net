using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;

namespace SlnParser.Core
{
    public class Engine
    {
        public SolutionFile Solution { get; }
        public SolutionFolder? SolutionFolder { get; private set; }

        public IList<Project> Projects { get; }

        public Engine(string slnPath)
        {
            if (string.IsNullOrWhiteSpace(slnPath)) throw new ArgumentNullException(nameof(slnPath));
            Solution = SolutionFile.Parse(slnPath);
            Projects = PopulateProjects();
        }

        private List<Project> PopulateProjects()
        {
            var projects = new List<Project>();
            foreach (var project in Solution.ProjectsInOrder)
            {
                if (project.ProjectType == SolutionProjectType.SolutionFolder)
                {
                    SolutionFolder = new SolutionFolder(project.AbsolutePath, project.ProjectName);
                    continue;
                }

                var projRoot = ProjectRootElement.Open(project.AbsolutePath);
                if (projRoot is null) continue;

                projects.Add(new Project(
                    project.ProjectType,
                    project.AbsolutePath,
                    project.ProjectName,
                    Path.GetExtension(project.AbsolutePath),
                    projRoot.ItemGroups));
            }
            return projects;
        }

        public static PackageReferenceGroup GetPackageReferences(ProjectItemGroupElement itemGroup)
        {
            var projectReferences = new List<PackageReference>();

            var references = itemGroup.Children.Where(ig => ig.ElementName == "PackageReference").ToList();
            if (references.Count <= 0) return new PackageReferenceGroup(new List<PackageReference>(), "");

            foreach (var projectElement in references)
            {
                var reference = (ProjectItemElement)projectElement;
                var version = reference.Children.First(c => c.ElementName == "Version");
                var v = (ProjectMetadataElement)version;
                projectReferences.Add(new PackageReference(reference.Include, v.Value));
            }

            return new PackageReferenceGroup(projectReferences, itemGroup.Condition);
        }
    }

    public record PackageReferenceGroup(IList<PackageReference> References, string Condition);
    public record PackageReference(string Name, string Version);
    public record Project(SolutionProjectType Type, string AbsolutePath, string Name, string Extension, ICollection<ProjectItemGroupElement> ItemGroups);
    public record SolutionFolder(string AbsolutePath, string Name);
}
