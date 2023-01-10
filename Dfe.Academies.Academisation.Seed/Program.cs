using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// Set up Academisation context
AcademisationContext? context = InitialiseDbContext();
// Set up AutoFixture
var fixture = new Fixture();

// Get user input on number of projects to create
	Console.WriteLine("Hello, please enter a number of projects you would like to seed.");
	int numberOfProjects = Convert.ToInt32(Console.ReadLine());

	// Set up progress bar
	Console.WriteLine("Generating new projects... ");
	using (var progress = new ProgressBar())
	{
		for (int i = 1; i <= numberOfProjects; i++)
		{
			SeedProject.CreateProject(context);
			// Add Application
			// TODO: When the switch happens for application over to academisation
			
			// Report progress to update visual
			progress.Report((double)i / numberOfProjects);
			Thread.Sleep(2000);
		}
	}
	Console.WriteLine("Done.");


AcademisationContext? InitialiseDbContext()
{
	var services = new ServiceCollection();
	services.AddDbContext<AcademisationContext>(options =>
		options.UseSqlServer(
			connectionString: "Server=localhost,1433;Database=sip;User=sa;Password=StrongPassword905"));
	var serviceProvider = services.BuildServiceProvider();
	var academisationContext = serviceProvider.GetService<AcademisationContext>();
	return academisationContext;
}
