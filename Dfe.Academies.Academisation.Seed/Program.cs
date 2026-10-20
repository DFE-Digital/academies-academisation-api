using System.Diagnostics;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Seed;

// Set up Academisation context
AcademisationContext? context = DatabaseConfig.InitialiseDbContext();

// Set up Stopwatch
var stopwatch = new Stopwatch();

// Get user input on number of projects to create
Console.WriteLine("Enter the number of projects to seed for each type (conversion, transfer, significant change).");
int projectsPerType = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Generating new projects...");
stopwatch.Start();

// Begin creation of projects and project notes for the input number

await SeedProject.CreateProject(context ?? throw new InvalidOperationException(), projectsPerType);
stopwatch.Stop();

Console.WriteLine($"Seeded {projectsPerType} conversion projects, {projectsPerType} transfer projects, and {projectsPerType} significant change projects.");
Console.WriteLine($"Done in {stopwatch.Elapsed.Minutes} minutes and {stopwatch.Elapsed.Seconds} seconds");
