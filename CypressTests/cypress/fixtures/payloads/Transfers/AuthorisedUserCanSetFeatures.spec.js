const AuthorisedUserCanSetFeaturesPayload
= {
  urn: Cypress.env('URN'),
  typeOfTransfer: 'typeOfTransfer Value',
  whoInitiatedTheTransfer: 'whoInitiatedTheTransfer Value',
  specificReasonsForTransfer: ['specificReasonsForTransfer Value'],
  isCompleted: true,
}
module.exports = { AuthorisedUserCanSetFeaturesPayload }
