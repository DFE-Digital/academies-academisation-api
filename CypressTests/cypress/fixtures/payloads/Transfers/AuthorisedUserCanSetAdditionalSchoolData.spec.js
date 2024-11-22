const transferringACADEMYOutgoingUkprn = 10034661
const incomingTrustUkprn = 10059669
const AuthorisedUserCanSetAdditionalSchoolDataPayload
= {
  urn: Cypress.env('URN'),
  transferringAcademyUkprn: transferringACADEMYOutgoingUkprn,
  latestOfstedReportAdditionalInformation: 'latestOfstedReportAdditionalInformation value',
  pupilNumbersAdditionalInformation: 'pupilNumbersAdditionalInformation value',
  keyStage2PerformanceAdditionalInformation: 'keyStage2PerformanceAdditionalInformation value',
  keyStage4PerformanceAdditionalInformation: 'KeyStage4PerformanceAdditionalInformation value',
  keyStage5PerformanceAdditionalInformation: 'keyStage5PerformanceAdditionalInformation value',
}
module.exports = { AuthorisedUserCanSetAdditionalSchoolDataPayload }
