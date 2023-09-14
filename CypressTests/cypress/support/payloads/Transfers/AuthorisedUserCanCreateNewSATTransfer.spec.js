
const transferringAcademyUkprnAAndOutgoingTrustUkprn = 10066875
const incomingTrustUkprn = 10066876
const AuthorisedUserCanCreateNewSATTransferPayload = 
{
    "transferringAcademyUkprns": [
        transferringAcademyUkprnAAndOutgoingTrustUkprn,
    ],
    "outgoingTrustUkprn": transferringAcademyUkprnAAndOutgoingTrustUkprn,
    "incomingTrustUkprn": incomingTrustUkprn
}
module.exports = {AuthorisedUserCanCreateNewSATTransferPayload}