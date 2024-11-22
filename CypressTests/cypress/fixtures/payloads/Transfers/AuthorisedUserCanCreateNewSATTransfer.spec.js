const transferringACADEMYOutgoingUkprn = 10034661
const outgoingTRUSTUkprn = 10059016
const incomingTrustUkprn = 10059669
const AuthorisedUserCanCreateNewSATTransferPayload
= {
  transferringAcademies: [{
    outgoingAcademyUkprn: transferringACADEMYOutgoingUkprn,
    incomingTrustUkprn: incomingTrustUkprn,
    incomingTrustName: 'THE LANGTREE SCHOOL ACADEMY TRUST COMPANY',
    region: 'London',
    localAuthority: 'Harrow',
    pupilNumbersAdditionalInformation: 'No additional pupil numbers information.',
    latestOfstedReportAdditionalInformation: 'Good school, latest inspection: 2022',
    keyStage2PerformanceAdditionalInformation: 'Good performance at key stage 2',
    keyStage4PerformanceAdditionalInformation: 'Outstanding performance at key stage 4',
    keyStage5PerformanceAdditionalInformation: 'Outstanding A-level results',
    pfiScheme: 'No PFI scheme',
    pfiSchemeDetails: 'N/A',
    viabilityIssues: 'None',
    financialDeficit: '£99,999.99',
    mpNameAndParty: 'Gary Streeter, Conservative',
    distanceFromAcademyToTrustHq: '3 miles',
    distanceFromAcademyToTrustHqDetails: 'Located in the city centre, very close to HQ.',
    publishedAdmissionNumber: '999',
  },
  ],
  outgoingTrustUkprn: outgoingTRUSTUkprn,
  outgoingTrustName: 'CANONS HIGH SCHOOL',
  isFormAMat: false,
}

module.exports = { AuthorisedUserCanCreateNewSATTransferPayload }
