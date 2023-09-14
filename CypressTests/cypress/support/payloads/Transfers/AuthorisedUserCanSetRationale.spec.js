
const AuthorisedUserCanSetRationalePayload = 
{
    "urn": Cypress.env('URN'),
    "projectRationale": "projectRationale",
    "trustSponsorRationale": "trustSponsorRationale",
    "isCompleted": true
  
}
module.exports = {AuthorisedUserCanSetRationalePayload}