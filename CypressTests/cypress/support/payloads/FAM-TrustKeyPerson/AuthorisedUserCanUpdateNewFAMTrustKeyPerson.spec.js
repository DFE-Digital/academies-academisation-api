const AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload = 
{
       "name": "Sean Cormac",
       "dateOfBirth": "1947-12-12T13:26:09.748",
       "biography": "Wonderful Deputy Head",
       "roles": [
        {
        "id": Cypress.env('responseIDForRequest'),
        "role": 0,
        "timeInRole": "4 years"
        }
       ]
}


module.exports = {AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload}
