using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SlnParser.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace SlnParser.ConsoleApp
{
    public static class Program
    {
        private const string SettingsFilePath = "appsettings.test.json";

        private static void Main()
        {
            AnsiConsole.MarkupLine("[green]Hello world![/]\n");

            var lastPath = ReadLastPath();
            var slnPath = AnsiConsole.Ask("[gray]Input [underline].sln[/] absolute path:[/]", lastPath);
            AnsiConsole.WriteLine();

            if (slnPath is not {Length: >0})
            {
                slnPath = GetTestSlnPath();
            }

            var engine = new Engine(slnPath);

            StoreLastPath(slnPath);

            var sln = engine.Solution;

            var projectsCount = sln.ProjectsInOrder.Count;
            var plural = projectsCount > 1 ? "s" : "";
            Console.WriteLine($"Selected solution has {projectsCount} project{plural}.");

            foreach (var proj in engine.Projects)
            {
                HandleMsBuildFormat(proj);
            }

            HandleSolutionFolder(engine.SolutionFolder);

            AnsiConsole.MarkupLine("\n[green]Bye world![/]");
        }

        private static void StoreLastPath(string slnPath)
        {
            File.WriteAllText(SettingsFilePath, slnPath);
        }

        private static string ReadLastPath()
        {
            return !File.Exists(SettingsFilePath) ? string.Empty : File.ReadAllText(SettingsFilePath);
        }

        private static void HandleSolutionFolder(SolutionFolder? slnFolder)
        {
            if (slnFolder is null) return;
            var grid = new Grid();
            grid.AddColumn();
            var panel = new Panel(grid)
            {
                Header = new PanelHeader($"[bold yellow]{slnFolder.Name}[/]")
            }.Expand();

            grid.AddRow(new Text(""));
            grid.AddRow(new Markup("[bold]Project Type: Solution folder[/]"));
            grid.AddRow(new Markup($"[bold]Absolute path: \"{slnFolder.AbsolutePath}[/]\""));
            grid.AddRow(new Text(""));

            AnsiConsole.Render(panel);
        }

        private static void HandleMsBuildFormat(Project project)
        {
            if (project.ItemGroups.Count <= 0) return;

            var grid = new Grid();
            grid.AddColumn();
            var panel = new Panel(grid)
            {
                Header = new PanelHeader($"[bold yellow]{project.Name}{project.Extension}[/]")
            }.Expand();

            grid.AddRow(new Text(""));
            grid.AddRow(new Markup("[bold]Project Type: MSBuild project[/]"));
            grid.AddRow(new Markup($"[bold]Absolute path: {project.AbsolutePath}[/]"));

            grid.AddRow(new Text(""));
            grid.AddRow(new Markup("[underline gray]PACKAGES[/]"));

            foreach (var itemGroup in project.ItemGroups)
            {
                var (references, condition) = Engine.GetPackageReferences(itemGroup);
                if (references is not {Count: >0}) continue;

                grid.AddRow(new Text(""));
                grid.AddRow(condition is { Length: >0 }
                    ? new Markup($"[yellow]Condition: {itemGroup.Condition}[/]")
                    : (IRenderable)new Markup("[yellow]No conditions[/]"));
                var table = CreateTable();
                foreach (var (name, version) in references)
                {
                    table.AddRow(name, version);
                }

                grid.AddRow(table);
            }

            AnsiConsole.Render(panel);
        }

        private static Table CreateTable() =>
            new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .AddColumn(new TableColumn("[u]Package[/]"))
                .AddColumn(new TableColumn("[u]Version[/]"));

        private static string GetTestSlnPath()
        {
            // i.e. [solution-folder]\SlnParser.ConsoleApp\bin\Debug\net5.0
            var location = Assembly.GetAssembly(typeof(Program))?.Location;
            if (location is null) throw new Exception("Unable to gather current working location.");
            var currentDir = Path.GetDirectoryName(location);
            if (string.IsNullOrWhiteSpace(currentDir)) throw new Exception("Unable to gather current working directory.");

            // var dir = Directory
            //     .GetParent(currentDir)?   // solution/project/bin/debug
            //     .Parent?                                    // solution/project/bin/
            //     .Parent?                                    // solution/project/
            //     .Parent;                                    // solution
            //
            // if (dir is null)
            // {
            //     throw new Exception("Unable to find solution directory.");
            // }

            // var slnPath = dir.GetFiles("*.sln").FirstOrDefault()?.FullName;
            var slnPath = $"{currentDir}\\TestFiles\\TestSolution.sln";
            return slnPath ?? throw new Exception($"Unable to find solution file '{slnPath}'");
        }
    }
}
