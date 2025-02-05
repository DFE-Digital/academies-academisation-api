const AuthorisedUserCanSetBenefitsPayload
= {
  urn: Cypress.env('URN'),
  intendedTransferBenefits: {
    selectedBenefits: [
      'selectedBenefits value',
    ],
    otherBenefitValue: 'otherBenefitValue value',
  },
  otherFactorsToConsider: {
    highProfile: {
      shouldBeConsidered: true,
      furtherSpecification: 'highProfile furtherSpecification value',
    },
    complexLandAndBuilding: {
      shouldBeConsidered: true,
      furtherSpecification: 'complexLandAndBuildingFurtherSpecification Value',
    },
    financeAndDebt: {
      shouldBeConsidered: true,
      furtherSpecification: 'financeAndDebtFurtherSpecification value',
    },
    otherRisks: {
      shouldBeConsidered: true,
      furtherSpecification: 'otherRisksfurtherSpecification value',
    },
  },
  equalitiesImpactAssessmentConsidered: true,
  anyRisks: true,
  isCompleted: true,
}
module.exports = { AuthorisedUserCanSetBenefitsPayload }
