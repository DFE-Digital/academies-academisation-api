﻿using System.Net;
using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.CorrelationIdMiddleware;
using MediatR;
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
		private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
		private readonly EnrichProjectCommand _subject;

		private readonly EstablishmentDto _establishment;
		private readonly TrustDto _trust;

		private readonly Fixture _fixture = new();
		public EnrichProjectCommandTests()
		{
			_context = new TestProjectContext(new Mock<IMediator>().Object).CreateContext();

			// mock establishment
			_establishment = _fixture.Create<EstablishmentDto>();
			_trust = _fixture.Build<TrustDto>().With(x => x.Ukprn, "101101").Create();
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
				new ConversionProjectRepository(_context),
				new AcademiesQueryService(Mock.Of<ILogger<AcademiesQueryService>>(), _academiesApiClientFactory.Object, correlationContext),
				new ProjectUpdateDataCommand(_context));
		}

		[Fact]
		public async Task SomeProjectsAreIncomplete__ForKnownEstablishment__EnrichIncompleteProjectsWithMissingData()
		{
			// Arrange
			var (project1, project2, project3) = (CreateProject(), CreateProject(), CreateProject("Bristol", "South West", 101101));
			_context.Projects.AddRange(project1, project2, project3);

			await _context.SaveChangesAsync();

			_mockHttpMessageHandler.When($"http://localhost/v4/establishment/urn/{project1.Details.Urn}")
					.Respond("application/json", JsonConvert.SerializeObject(_establishment));

			_mockHttpMessageHandler.When($"http://localhost/v4/establishment/urn/{project2.Details.Urn}")
					.Respond("application/json", JsonConvert.SerializeObject(_establishment));

			_mockHttpMessageHandler.When($"http://localhost/v4/trust/trustReferenceNumber/{project1.Details.TrustReferenceNumber}")
		.Respond("application/json", JsonConvert.SerializeObject(_trust));

			_mockHttpMessageHandler.When($"http://localhost/v4/trust/trustReferenceNumber/{project2.Details.TrustReferenceNumber}")
					.Respond("application/json", JsonConvert.SerializeObject(_trust));

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
				() => Assert.Equal("Bristol", updatedProject3.Details.LocalAuthority),
				() => Assert.Equal("South West", updatedProject3.Details.Region)
			);
		}

		[Fact]
		public async Task SomeProjectsAreIncomplete__ForUnknownEstablishment__ProjectsRemainUnchanged()
		{
			// Arrange
			var (project1, project2) = (CreateProject(null, null), CreateProject(string.Empty, string.Empty));
			_context.Projects.AddRange(project1, project2);
			await _context.SaveChangesAsync();

			_mockHttpMessageHandler.When($"http://localhost/v4/establishment/urn/*").Respond(HttpStatusCode.NotFound);

			_mockHttpMessageHandler.When($"http://localhost/v4/trust/trustReferenceNumber/*").Respond(HttpStatusCode.NotFound);


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

		private Project CreateProject(string? la = null, string? region = null, int? trustUkprn = null)
		{
			var project = _fixture.Create<Project>();

			var projectDetails = _fixture.Build<ProjectDetails>()
							.With(p => p.LocalAuthority, la)
							.With(p => p.Urn, project.Details.Urn)
							.With(p => p.Region, region)
							.With(p => p.TrustUkprn, trustUkprn)
							.Create();

			project.Update(projectDetails);

			return project;
		}
	}
}
