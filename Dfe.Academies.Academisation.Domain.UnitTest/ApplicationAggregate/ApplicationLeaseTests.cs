using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate
{
	public class ApplicationLeaseTests
	{
		private readonly Fixture _fixture;

		public ApplicationLeaseTests()
		{
			_fixture = new Fixture();
		}
		
		[Fact]
		public void CreateLease_ReturnsNotFound()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Build<School>().With(x=> x.Id, 2).Create()
				},
				null,
				null,
				null);

			var result = subject.CreateLease( 1, "str", 1m, 1m, 1m,  "str", "str", "str");

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public void CreateLease_ReturnsCommandSuccess()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					_fixture.Build<School>().With(x=> x.Id, 2).Create()
				},
				null,
				null,
				null);

			var result = subject.CreateLoan(2, Decimal.One, "str", "str", Decimal.One, "str");

			Assert.IsType<CommandSuccessResult>(result);
		}
		
		[Fact]
		public void UpdateLease_ReturnsNotFound()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					new School(2, _fixture.Build<SchoolDetails>().Create(),
						new List<Loan>(), new List<Lease>() {new Lease(1, "str", 1m, 1m, 1m, "str", "str", "str")})
				},
				null,
				null,
				null);

			var result = subject.UpdateLease(1, 1, "str", 1m, 1m, 1m,  "str", "str", "str");

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public void UpdateLoan_ReturnsSuccessResult()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					new School(1, _fixture.Build<SchoolDetails>().Create(),
						new List<Loan>(), new List<Lease>() {new Lease(1, "str", 1m, 1m, 1m, "str", "str", "str")})
				},
				null,
				null,
				null);

			var result = subject.UpdateLease(1, 1, "str", 1m, 1m, 1m,  "str", "str", "str");

			Assert.IsType<CommandSuccessResult>(result);
		}
		
		[Fact]
		public void DeleteLease_ReturnsNotFound()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					new School(1, _fixture.Build<SchoolDetails>().Create(),
						new List<Loan>(), new List<Lease>() {new Lease(1, "str", 1m, 1m, 1m, "str", "str", "str")})
				},
				null,
				null,
				null);

			var result = subject. DeleteLease(1, 2);

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public void DeleteLease_ReturnsSuccessResult()
		{
			Application subject = new(
				1,
				DateTime.UtcNow,
				DateTime.UtcNow,
				ApplicationType.FormAMat,
				ApplicationStatus.InProgress,
				new Dictionary<int, ContributorDetails>(),
				new List<School>
				{
					new School(1, _fixture.Build<SchoolDetails>().Create(),
						new List<Loan>(), new List<Lease>() {new Lease(1, "str", 1m, 1m, 1m, "str", "str", "str")})
				},
				null,
				null,
				null);

			var result = subject.DeleteLease(1, 1);

			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
