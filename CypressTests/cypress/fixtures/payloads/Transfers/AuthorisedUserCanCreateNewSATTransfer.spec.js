const transferringAcademyUkprnAAndOutgoingTrustUkprn = 10066875
const incomingTrustUkprn = 10066876
const AuthorisedUserCanCreateNewSATTransferPayload
= {
  transferringAcademies: [{
    outgoingAcademyUkprn: incomingTrustUkprn,
    incomingTrustUkprn: 'string',
    incomingTrustName: 'string',
    region: 'string',
    localAuthority: 'string',
    pupilNumbersAdditionalInformation: 'string',
    latestOfstedReportAdditionalInformation: 'string',
    keyStage2PerformanceAdditionalInformation: 'string',
    keyStage4PerformanceAdditionalInformation: 'string',
    keyStage5PerformanceAdditionalInformation: 'string',
    pfiScheme: 'string',
    pfiSchemeDetails: 'string',
    viabilityIssues: 'string',
    financialDeficit: 'string',
    mpNameAndParty: 'string',
    distanceFromAcademyToTrustHq: 'string',
    distanceFromAcademyToTrustHqDetails: 'string',
    publishedAdmissionNumber: 'string',
  },
  ],
  outgoingTrustUkprn: transferringAcademyUkprnAAndOutgoingTrustUkprn,
  outgoingTrustName: 'Test',
  isFormAMat: false,
}

module.exports = { AuthorisedUserCanCreateNewSATTransferPayload }
