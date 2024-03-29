﻿using System.Reflection;
using Autofac;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.Behaviours;
using Dfe.Academies.Academisation.Service.CommandValidations;
using FluentValidation;
using MediatR;

namespace Dfe.Academies.Academisation.WebApi.AutofacModules;

public class MediatorModule : Autofac.Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
			.AsImplementedInterfaces();
		
		// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
		builder.RegisterAssemblyTypes(typeof(UpdateLoanCommand).GetTypeInfo().Assembly)
			.AsClosedTypesOf(typeof(IRequestHandler<,>));

		// Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
	//	builder.RegisterAssemblyTypes(typeof(handlers here).GetTypeInfo().Assembly)
	//		.AsClosedTypesOf(typeof(INotificationHandler<>));

		// Register the Command's Validators (Validators based on FluentValidation library)
		var asm = typeof(CreateLoanCommandValidator).GetTypeInfo().Assembly;
		builder
			.RegisterAssemblyTypes(asm)
			.Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
			.AsImplementedInterfaces();

		builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
	//	builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

	}
}
