const AuthorisedUserCanSetLegalRequirementsPayload =
{
    "urn": Cypress.env('URN'),
    "outgoingTrustConsent": "outgoing Trust Consent Value",
    "incomingTrustAgreement": "incoming Trust Consent Value",
    "diocesanConsent": "diocesanConsent Value",
    "isCompleted": true
  }
module.exports = { AuthorisedUserCanSetLegalRequirementsPayload }