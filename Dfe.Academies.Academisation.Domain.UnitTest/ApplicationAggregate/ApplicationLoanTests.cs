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
	public class ApplicationLoanTests
	{
		private readonly Fixture _fixture;

		public ApplicationLoanTests()
		{
			_fixture = new Fixture();
		}
		
		[Fact]
		public void CreateLoan_ReturnsNotFound()
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

			var result = subject.CreateLoan(1, Decimal.One, "str", "str", Decimal.One, "str");

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public void CreateLoan_ReturnsCommandSuccess()
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
		public void UpdateLoan_ReturnsNotFound()
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
					new School(2, 
						"",
						"",
						"",
						"",
						"",
						"",
						"",
						true,
						"",
						"",
						DateTimeOffset.Now, 
						"",
						"",
						null,
						"",
						_fixture.Build<SchoolDetails>().Create(),new List<Loan>
					{
						new Loan(1,Decimal.One, "str", "str", Decimal.One, "str")
					}, new List<Lease>(), true, false)
				},
				null,
				null,
				null);

			var result = subject.UpdateLoan(1, 2, Decimal.One, "str", "str", Decimal.One, "str");

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
					new School(1, 			"",
						"",
						"",
						"",
						"",
						"",
						"",
						true,
						"",
						"",
						DateTimeOffset.Now, 
						"",
						"",
						null,
						"",
						_fixture.Build<SchoolDetails>().Create(),new List<Loan>
					{
						new Loan(1,Decimal.One, "str", "str", Decimal.One, "str")
					}, new List<Lease>(), true, false)
				},
				null,
				null,
				null);

			var result = subject.UpdateLoan(1, 1, Decimal.One, "str", "str", Decimal.One, "str");

			Assert.IsType<CommandSuccessResult>(result);
		}
		
		[Fact]
		public void DeleteLoan_ReturnsNotFound()
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
					new School(1, 			"",
						"",
						"",
						"",
						"",
						"",
						"",
						true,
						"",
						"",
						DateTimeOffset.Now, 
						"",
						"",
						null,
						"",
						_fixture.Build<SchoolDetails>().Create(),new List<Loan>
					{
						new Loan(1,Decimal.One, "str", "str", Decimal.One, "str")
					}, new List<Lease>(), true, null )
				},
				null,
				null,
				null);

			var result = subject.DeleteLoan(1, 2);

			Assert.IsType<NotFoundCommandResult>(result);
		}

		[Fact]
		public void DeleteLoan_ReturnsSuccessResult()
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
					new School(1, 			"",
						"",
						"",
						"",
						"",
						"",
						"",
						true,
						"",
						"",
						DateTimeOffset.Now, 
						"",
						"",
						null,
						"",
						_fixture.Build<SchoolDetails>().Create(),new List<Loan>
					{
						new Loan(1,Decimal.One, "str", "str", Decimal.One, "str")
					}, new List<Lease>(), true, null )
				},
				null,
				null,
				null);

			var result = subject.DeleteLoan(1, 1);

			Assert.IsType<CommandSuccessResult>(result);
		}
	}
}
