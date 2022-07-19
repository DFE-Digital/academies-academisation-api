using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Academies.Academisation.Data.ExtensionMethods;

public static class EntityTypeBuilderExtensions
{
	public static void HasEnum<TEntity, TProperty>(
		this EntityTypeBuilder<TEntity> entityBuilder, 
		Expression<Func<TEntity, TProperty>> propertyExpression)
		where TEntity : class
		where TProperty : Enum
	{
		entityBuilder
			.Property(propertyExpression)
			.HasConversion(
				p => p.ToString(),
				p => (TProperty)Enum.Parse(typeof(TProperty), p)
			);
	}
}
