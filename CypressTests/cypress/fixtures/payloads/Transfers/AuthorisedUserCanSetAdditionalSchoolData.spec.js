const transferringAcademyUkprnAAndOutgoingTrustUkprn = 10066875
const incomingTrustUkprn = 10066876
const AuthorisedUserCanSetAdditionalSchoolDataPayload
= {
  urn: Cypress.env('URN'),
  transferringAcademyUkprn: transferringAcademyUkprnAAndOutgoingTrustUkprn,
  latestOfstedReportAdditionalInformation: 'latestOfstedReportAdditionalInformation value',
  pupilNumbersAdditionalInformation: 'pupilNumbersAdditionalInformation value',
  keyStage2PerformanceAdditionalInformation: 'keyStage2PerformanceAdditionalInformation value',
  keyStage4PerformanceAdditionalInformation: 'KeyStage4PerformanceAdditionalInformation value',
  keyStage5PerformanceAdditionalInformation: 'keyStage5PerformanceAdditionalInformation value',
}
module.exports = { AuthorisedUserCanSetAdditionalSchoolDataPayload }
