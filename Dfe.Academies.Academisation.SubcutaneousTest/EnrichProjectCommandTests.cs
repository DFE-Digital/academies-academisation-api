﻿using System.Net;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Establishment;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.Establishment;
using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace Dfe.Academies.Academisation.SubcutaneousTest
{
	public class EnrichProjectCommandTests
	{
		private readonly AcademisationContext _context;
		private readonly Mock<IHttpClientFactory> _httpClientFactory;
		private readonly Mock<IAcademiesApiClientFactory> _academiesApiClientFactory;
		private readonly MockHttpMessageHandler _mockHttpMessageHandler = new MockHttpMessageHandler();
		private readonly EnrichProjectCommand _subject;

		private readonly Establishment _establishment;

		private readonly Fixture _fixture = new Fixture();

		public EnrichProjectCommandTests()
		{
			_context = new TestProjectContext().CreateContext();

			// mock establishment
			_establishment = _fixture.Create<Establishment>();
			_httpClientFactory = new Mock<IHttpClientFactory>();

			var httpClient = _mockHttpMessageHandler.ToHttpClient();
			httpClient.BaseAddress = new Uri("http://localhost");
			_httpClientFactory.Setup(m => m.CreateClient("AcademiesApi")).Returns(httpClient);

			var correlationContext = new CorrelationContext();
			correlationContext.SetContext(Guid.NewGuid());

			_academiesApiClientFactory = new Mock<IAcademiesApiClientFactory>();
			_academiesApiClientFactory.Setup(x => x.Create(correlationContext)).Returns(httpClient);


			// create command
			_subject = new EnrichProjectCommand(
				Mock.Of<ILogger<EnrichProjectCommand>>(),
				new IncompleteProjectsGetDataQuery(_context),
				new EstablishmentGetDataQuery(Mock.Of<ILogger<EstablishmentGetDataQuery>>(), _academiesApiClientFactory.Object, correlationContext),
				new ProjectUpdateDataCommand(_context));
		}

		[Fact]
		public async Task SomeProjectsAreIncomplete__ForKnownEstablishment__EnrichIncompleteProjectsWithMissingData()
		{
			// Arrange
			var (project1, project2, project3) = ( CreateProject(1), CreateProject(1), CreateProject(1, "Bristol", "South West") );
			_context.Projects.AddRange(project1, project2, project3);

			await _context.SaveChangesAsync();

			_mockHttpMessageHandler.When($"http://localhost/establishment/urn/{project1.Details.Urn}")
					.Respond("application/json", JsonConvert.SerializeObject(_establishment));
			var httpClient = _mockHttpMessageHandler.ToHttpClient();
			httpClient.BaseAddress = new Uri("http://localhost");
			_httpClientFactory.Setup(m => m.CreateClient("AcademiesApi")).Returns(httpClient);

			// Act
			await _subject.Execute();

			var updatedProject1 = await _context.Projects.FirstAsync(p => p.Id == project1.Id);
			var updatedProject2 = await _context.Projects.FirstAsync(p => p.Id == project2.Id);
			var updatedProject3 = await _context.Projects.FirstAsync(p => p.Id == project3.Id);

			//Assert
			Assert.Multiple(
				() => Assert.Equal(_establishment.LocalAuthorityName, updatedProject1.Details.LocalAuthority),
				() => Assert.Equal(_establishment.Gor.Name, updatedProject1.Details.Region),
				() => Assert.Equal(_establishment.LocalAuthorityName, updatedProject2.Details.LocalAuthority),
				() => Assert.Equal(_establishment.Gor.Name, updatedProject2.Details.Region),
				() => Assert.Equal("Bristol", updatedProject3.Details.Region)
			);
		}

		[Fact]
		public async Task SomeProjectsAreIncomplete__ForUnknownEstablishment__ProjectsRemainUnchanged()
		{
			// Arrange
			var (project1, project2) = (CreateProject(1, null, null), CreateProject(2, string.Empty, string.Empty));
			_context.Projects.AddRange(project1, project2);
			await _context.SaveChangesAsync();

			_mockHttpMessageHandler.When($"http://localhost/establishment/urn/*").Respond(HttpStatusCode.NotFound);
			var httpClient = _mockHttpMessageHandler.ToHttpClient();
			httpClient.BaseAddress = new Uri("http://localhost");
			_httpClientFactory.Setup(m => m.CreateClient("AcademiesApi")).Returns(httpClient);

			// Act
			await _subject.Execute();

			var updatedProject1 = await _context.Projects.FirstAsync(p => p.Id == project1.Id);
			var updatedProject2 = await _context.Projects.FirstAsync(p => p.Id == project2.Id);

			// Assert
			Assert.Multiple(
				() => Assert.Null(updatedProject1.Details.LocalAuthority),
				() => Assert.Null(updatedProject1.Details.Region),
				() => Assert.Equal(string.Empty, updatedProject2.Details.LocalAuthority),
				() => Assert.Equal(string.Empty, updatedProject2.Details.Region)
			);
		}

		private Project CreateProject(int? urn = 0, string? la = null, string? region = null)
		{
			return _fixture.Build<Project>()
							.With(p => p.Details.LocalAuthority, la)
							.With(p => p.Details.Urn, urn)
							.With(p => p.Details.Region, region)
							.Create();
		}
	}
}
