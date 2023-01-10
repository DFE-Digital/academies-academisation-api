using System.Diagnostics;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Seed;

// Set up Academisation context
AcademisationContext? context = DatabaseConfig.InitialiseDbContext();

// Set up Stopwatch
var stopwatch = new Stopwatch();

// Get user input on number of projects to create
Console.WriteLine("Hello, please enter a number of projects you would like to seed.");
int numberOfProjects = Convert.ToInt32(Console.ReadLine());

// Set up progress bar
Console.WriteLine("Generating new projects... ");
using (var progress = new ProgressBar())
{
	stopwatch.Start();
	// Begin creation of projects and project notes for the input number
	for (int i = 1; i <= numberOfProjects; i++)
	{
		SeedProject.CreateProject(context);
		// Add Application
		// TODO: When the switch happens for application over to academisation

		// Report progress to update visual
		progress.Report((double)i / numberOfProjects);
	}
	stopwatch.Stop();
}
Console.WriteLine($"Done in {stopwatch.Elapsed.Minutes} minutes and {stopwatch.Elapsed.Seconds} seconds");
