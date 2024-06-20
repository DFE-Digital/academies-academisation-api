using System;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public abstract class TestAcademisationContext : IDisposable
{
	private readonly SqliteConnection _connection;

	private readonly DbContextOptions<AcademisationContext> _contextOptions;
	private readonly IMediator _mediator;
	protected TestAcademisationContext(IMediator mediator)
	{
		_connection = new("Filename=:memory:");
		_connection.Open();

		_contextOptions = new DbContextOptionsBuilder<AcademisationContext>()
			.UseSqlite(_connection)
			.Options;
		_mediator = mediator;
	}

	public AcademisationContext CreateContext()
	{
		return new(_contextOptions, _mediator);
	}

	protected void Seed()
	{
		SeedData();
	}

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
