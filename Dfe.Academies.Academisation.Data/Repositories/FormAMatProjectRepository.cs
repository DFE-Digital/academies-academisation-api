using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class FormAMatProjectRepository : GenericRepository<FormAMatProject>, IFormAMatProjectRepository
	{
		private readonly AcademisationContext _context;

		public IUnitOfWork UnitOfWork => _context;
		public FormAMatProjectRepository(AcademisationContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IFormAMatProject?> GetByApplicationReference(string? applicationReferenceNumber, CancellationToken cancellationToken)
		{
			return await this.dbSet.SingleOrDefaultAsync(x => x.ApplicationReference == applicationReferenceNumber, cancellationToken).ConfigureAwait(false);
		}

		public async Task<List<IFormAMatProject>> GetByIds(IEnumerable<int?> formAMatProjectIds, CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => formAMatProjectIds.Contains(x.Id)).Cast<IFormAMatProject>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}
		public async Task<List<IFormAMatProject>> GetProjectsWithoutReference(CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => x.ReferenceNumber == null || x.ReferenceNumber == "").Cast<IFormAMatProject>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Searches projects by a single search term across application reference, proposed trust name, and reference number.
		/// </summary>
		/// <param name="searchTerm">The search term to apply across multiple fields.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A list of projects where the search term matches any of the specified fields.</returns>
		public async Task<IEnumerable<FormAMatProject>> SearchProjectsByTermAsync(string searchTerm, CancellationToken cancellationToken)
		{
			return await this.dbSet
				.Where(x => EF.Functions.Like(x.ApplicationReference, $"%{searchTerm}%")
							|| EF.Functions.Like(x.ProposedTrustName, $"%{searchTerm}%")
							|| EF.Functions.Like(x.ReferenceNumber, $"%{searchTerm}%"))
				.ToListAsync(cancellationToken);
		}

		public async Task CreateFormAMatProjectWithTrustReferenceNumber(IFormAMatProject formAMatProject)
		{
			string? trustReferenceNumber;

			// This has been written to allow the integration tests to run as they use sqlite
			if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
			{
				trustReferenceNumber = $"TR{formAMatProject.Id:D5}"; ;
			}
			else
			{
				var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
				p.Direction = System.Data.ParameterDirection.Output;

				await context.Database.ExecuteSqlRawAsync($"set @result = NEXT VALUE FOR {AcademisationContext.DEFAULT_SCHEMA}.TrustReferenceNumberSeq", p);
				trustReferenceNumber = $"TR{(int)p.Value:D5}";
			}

			formAMatProject.SetTrustReferenceNumber(trustReferenceNumber);

			Insert(formAMatProject as FormAMatProject);
		}

		public async Task<IEnumerable<FormAMatProject>> GetByEmail(string? deliveryOfficerEmail, CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => x.AssignedUser.EmailAddress == deliveryOfficerEmail).ToListAsync(cancellationToken);
		}
	}
}

