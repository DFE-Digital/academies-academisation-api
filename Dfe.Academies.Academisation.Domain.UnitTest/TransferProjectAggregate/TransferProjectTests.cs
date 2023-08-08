using AutoMapper;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using FluentAssertions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.TransferProjectAggregate
{
	public class TransferProjectTests
	{
		public TransferProjectTests()
		{
		}

		[Fact]
		public void GenerateUrn_WithMultipleAcademies_GivesAMATBasedReference()
		{
			// Arrange
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
				createdOn);

			// Act
			result.GenerateUrn(10101020);

			// Assert
			result.ProjectReference.Should().Be("MAT-10101020");
		}

		[Fact]
		public void GenerateUrn_WithOneAcademy_GivesASATBasedReference()
		{
			// Arrange
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
				createdOn);

			// Act
			result.GenerateUrn(10101020);

			// Assert
			result.ProjectReference.Should().Be("SAT-10101020");
		}

		[Fact]
		public void CreateTransferProject_WithValidParameters_CreatesTransferProject()
		{
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
				createdOn);

			// Assert
			result.OutgoingTrustUkprn.Should().Be(outgoingTrustUkprn);
			result.CreatedOn.Should().Be(createdOn);
			result.TransferringAcademies.Count.Should().Be(2);
			result.TransferringAcademies.SingleOrDefault(x => x.IncomingTrustUkprn == incomingTrustUkprn && x.OutgoingAcademyUkprn == "22221111").Should().NotBeNull();
			result.TransferringAcademies.SingleOrDefault(x => x.IncomingTrustUkprn == incomingTrustUkprn && x.OutgoingAcademyUkprn == "33331111").Should().NotBeNull();
		}

		[Fact]
		public void CreateTransferProject_WithNullOutgoingTrustUkprn_ThrowsArgumentNullException()
		{
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			 Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
				null,
				incomingTrustUkprn,
				academyUkprns,
				createdOn));
		}

		[Theory]
		[ClassData(typeof(CreationArgumentExceptionTestData))]
		public void CreateTransferProject_WithTestData_ThrowsArgumentExceptions(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> academyUkprns, DateTime createdOn, Type exType)
		{
			// Arrange      
			dynamic exception;
			// Act
			if (exType == typeof(ArgumentException))
			{
				Assert.Throws<ArgumentException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}

			if (exType == typeof(ArgumentNullException))
			{
				Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}

			if (exType == typeof(ArgumentOutOfRangeException))
			{
				Assert.Throws<ArgumentOutOfRangeException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}
		}

		public class CreationArgumentExceptionTestData : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				yield return new object[] { null, "11110000", new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentNullException)};
				yield return new object[] { string.Empty, "11110000", new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentException)};

				yield return new object[] { "11112222", null, new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { "11112222", string.Empty, new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "11110000", null, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { "11112222", "11110000", new List<string>(), DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, DateTime.MinValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, DateTime.MaxValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, null, typeof(ArgumentOutOfRangeException) };
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
