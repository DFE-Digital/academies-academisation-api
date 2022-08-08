using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public abstract class TestAcademisationContext : IDisposable
{
	private readonly SqliteConnection _connection;

	private readonly DbContextOptions<AcademisationContext> _contextOptions;

	protected TestAcademisationContext()
	{
		_connection = new("Filename=:memory:");
		_connection.Open();
		
		_contextOptions = new DbContextOptionsBuilder<AcademisationContext>()
			.UseSqlite(_connection)
			.Options;
	}

	public AcademisationContext CreateContext() => new(_contextOptions);

	protected void Seed() => SeedData();

	protected abstract void SeedData();

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			_connection.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
