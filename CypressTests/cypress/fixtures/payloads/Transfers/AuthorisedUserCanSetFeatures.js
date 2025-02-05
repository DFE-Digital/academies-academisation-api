const AuthorisedUserCanSetFeaturesPayload
= {
  urn: Cypress.env('URN'),
  typeOfTransfer: 'SAT',
  whoInitiatedTheTransfer: 'whoInitiatedTheTransfer Value',
  specificReasonsForTransfer: ['specificReasonsForTransfer Value'],
  isCompleted: true,
}
module.exports = { AuthorisedUserCanSetFeaturesPayload }
