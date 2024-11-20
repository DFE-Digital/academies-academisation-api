const transferringAcademyUkprnAAndOutgoingTrustUkprn = 10066875
const incomingTrustUkprn = 10066876
const AuthorisedUserCanCreateNewSATTransferPayload
= {
  transferringAcademies: [{
    outgoingAcademyUkprn: transferringAcademyUkprnAAndOutgoingTrustUkprn,
    incomingTrustUkprn: incomingTrustUkprn,
    incomingTrustName: 'PLYMOUTH CAST',
    region: 'South West',
    localAuthority: 'Plymouth',
    pupilNumbersAdditionalInformation: 'No additional pupilNumbersAdditionalInformation',
    latestOfstedReportAdditionalInformation: 'Good school',
    keyStage2PerformanceAdditionalInformation: 'not applicable',
    keyStage4PerformanceAdditionalInformation: 'Outstanding',
    keyStage5PerformanceAdditionalInformation: 'Outstanding',
    pfiScheme: 'The PFI Scheme',
    pfiSchemeDetails: 'The PFI Scheme details',
    viabilityIssues: 'Viability issues',
    financialDeficit: '£99,999.99',
    mpNameAndParty: 'Gary Streeter, Conservative',
    distanceFromAcademyToTrustHq: '1 miles',
    distanceFromAcademyToTrustHqDetails: 'Distance from academy to HQ trust is nearby.',
    publishedAdmissionNumber: '999',
  },
  ],
  outgoingTrustUkprn: transferringAcademyUkprnAAndOutgoingTrustUkprn,
  outgoingTrustName: 'Learning Academies Trust',
  isFormAMat: false,
}

module.exports = { AuthorisedUserCanCreateNewSATTransferPayload }
