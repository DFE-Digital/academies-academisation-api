using System.Diagnostics;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Seed;

// Set up Academisation context
AcademisationContext? context = DatabaseConfig.InitialiseDbContext();
if (context is null)
{
	throw new ArgumentNullException("Context");
}

// Set up Stopwatch
	var stopwatch = new Stopwatch();

// Get user input on number of projects to create
Console.WriteLine("Hello, please enter a number of projects you would like to seed.");
int numberOfProjects = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Generating new projects... ");
stopwatch.Start();

// Begin creation of projects and project notes for the input number

	await SeedProject.CreateProject(context, numberOfProjects);
	stopwatch.Stop();

	Console.WriteLine($"Done in {stopwatch.Elapsed.Minutes} minutes and {stopwatch.Elapsed.Seconds} seconds");



