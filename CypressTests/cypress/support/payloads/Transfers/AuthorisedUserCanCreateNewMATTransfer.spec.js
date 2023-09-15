
const transferringAcademyUkprnAAndOutgoingTrustUkprn = 10066875
const transferringAcademyUkprnB = 10066884
const incomingTrustUkprn = 10066876
const AuthorisedUserCanCreateNewMATTransferPayload = 
{
    "transferringAcademyUkprns": [
        transferringAcademyUkprnAAndOutgoingTrustUkprn,
        transferringAcademyUkprnB
    ],
    "outgoingTrustUkprn": transferringAcademyUkprnAAndOutgoingTrustUkprn,
    "incomingTrustUkprn": incomingTrustUkprn
}
module.exports = {AuthorisedUserCanCreateNewMATTransferPayload}