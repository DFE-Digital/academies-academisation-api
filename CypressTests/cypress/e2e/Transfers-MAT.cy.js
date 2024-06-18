import { AuthorisedUserCanCreateNewMATTransferPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanCreateNewMATTransfer.spec'
import { AuthorisedUserCanSetRationalePayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetRationale.spec'
import { AuthorisedUserCanSetTransferDatesPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetTransferDates.spec'
import { AuthorisedUserCanSetLegalRequirementsPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetLegalRequirements.spec'
import { AuthorisedUserCanSetFeaturesPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetFeatures.spec'
import { AuthorisedUserCanSetBenefitsPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetBenefits.spec'
import { AuthorisedUserCanSetAdditionalSchoolDataPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetAdditionalSchoolData.spec'
import { AuthorisedUserCanSetTrustInfoAndProjectDatesPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanSetTrustInfoAndProjectsDates.spec'
import { AuthorisedUserCanAssignUserDataPayload } from '../fixtures/payloads/Transfers/AuthorisedUserCanAssignUser.spec'

describe('Academisation API Testing - Transfers MAT Projects', () => {
  let apiKey = Cypress.env('apiKey')
  let url = Cypress.env('url')
  let URN = Cypress.env('URN')

  let getTransferDateTimeFormatRegex = /^\d{2}\/\d{2}\/\d{4}$/

  // VERIFY AN UNAUTH USER CANNOT GET ALL TRANSFERPROJECTS REGARDLESS OF MAT OR SAT TYPE
  it('GET - Verify An UNAuthorised User CANNOT GET ALL MAT AND SAT Transfer Projects - 401 UNAUTH EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/GetTransferProjects',
      failOnStatusCode: false,
      method: 'GET',
      headers:
      {
        'x-api-key': 'INCORRECT API-KEY LOOKING FOR 401 RESPONSE',
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
    })
  })

  // GET ALL TRANSFER PROJECTS REGARDLESS OF MAT OR SAT TYPE

  it('GET - Verify An Authorised User Can GET ALL MAT AND SAT Transfer Projects - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/GetTransferProjects',
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      const totalCount = response.body.totalCount
      expect(totalCount).to.be.greaterThan(0)
    })
  })

  // TRY TO CREATE A NEW TRANSFER AS AN UNAUTH USER AND HOPE FOR 401 UNAUTH
  it('POST - Verify An UN-Authorised User CANNOT Create A New MAT Transfer - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project',
      failOnStatusCode: false,
      method: 'POST',
      headers:
      {
        'x-api-key': 'INCORRECTAPIKEY FOR 401 UNAUTH RESPONSE',
      },
      body:
        AuthorisedUserCanCreateNewMATTransferPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
    })
  })

  // TRY TO CREATE A NEW MAT TRANSFER AS AN AUTH USER AND HOPE WE GET A 201 CREATED RESPONSE AND EXPECTED FIELDS
  it('POST - Verify An Authorised User Can Create A New MAT Transfer - 201 CREATED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project',
      method: 'POST',
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCanCreateNewMATTransferPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 201)

      expect(response.body).to.have.property('projectUrn')

      URN = response.body.projectUrn
      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      cy.log(URN)
    })
  })

  // TRY TO GET THE NEW MAT TRANSFER WE CREATED AS AN AUTH USER AND HOPE WE GET A 200 OK RESPONSE AND EXPECTED FIELDS
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      cy.log(URN)
    })
  })

  // VERIFY AN UNAUTHORISED USER IS UNABLE TO GET ACCESS TO THE NEW TRANSFER PROJECT
  it('GET - Verify An UNAuthorised User CANNOT GET THE New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      failOnStatusCode: false,
      method: 'GET',
      headers:
      {
        'x-api-key': 'INVALID API KEY FOR 401 RESPONSE',
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
    })
  })
  // VERIFY AN UNAUTHORISED USER GETS A 401 UNAUTH RESPONSE WHEN TRYING TO SET RATIONALE
  it('PUT - Verify An UN-authorised User CANNOT SET-RATIONALE On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-rationale/',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT API KEY - 401 UNAUTHORISED EXPECTED',
      },
      body: AuthorisedUserCanSetRationalePayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })
  // TRY TO SET RATIONALE ON THE NEW MAT PROJECT AND HOPE WE GET A 200 OK RESPONSE
  it('PUT - Verify An Authorised User Can SET-RATIONALE On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-rationale/',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetRationalePayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // VERIFY SET-RATIONALE STUFF COMES BACK CORRECTLY IN GET RESPONSE
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED RATIONALE - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      // OK LET'S CHECK OUR RATIONALE STUFF HERE
      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)
      cy.log(URN)
    })
  })

  // TRY FOR AN UNAUTH USER TO SET TRANSFER DATES IN PUT REQUEST
  it('PUT - Verify An UNAuthorised User CANNOT SET-TRANSFER DATES On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-transfer-dates',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT APIKEY 401 EXPECTED',
      },
      body: AuthorisedUserCanSetTransferDatesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO SET TRANSFER DATES IN PUT REQUEST
  it('PUT - Verify An Authorised User Can SET-TRANSFER DATES On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-transfer-dates',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetTransferDatesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED TRANSFER DATES - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      // CHECK OUR SET TRANSFER DATES COME BACK CORRECTLY HERE
      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)
      cy.log(URN)
    })
  })

  // TRY FOR AN UNAUTH USER TO SET LEGAL REQUIREMENTS IN PUT REQUEST
  it('PUT - Verify An UNAuthorised User CANNOT SET-LEGAL REQUIREMENTS On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-legal-requirements',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT API KEY 401 UNAUTH EXPECTED',
      },
      body: AuthorisedUserCanSetLegalRequirementsPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO SET LEGAL REQUIREMENTS IN PUT REQUEST
  it('PUT - Verify An Authorised User Can SET-LEGAL REQUIREMENTS On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-legal-requirements',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetLegalRequirementsPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK UPDATED LEGAL REQUIREMENTS COME BACK CORRECTLY IN GET RESPONSE
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED LEGAL REQUIREMENTS - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      // CHECK LEGAL REQUIREMENTS IN GET RESPONSE
      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      cy.log(URN)
    })
  })

  // TRY TO SET FEATURES IN PUT REQUEST AS AN UNAUTH USER
  it('PUT - Verify An UNAuthorised User CANNOT SET-FEATURES On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-features',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT API-KEY FOR 401 UNAUTH',
      },
      body: AuthorisedUserCanSetFeaturesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO SET FEATURES IN PUT REQUEST
  it('PUT - Verify An Authorised User Can SET-FEATURES On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-features',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetFeaturesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK UPDATED FEATURES COME BACK CORRECTLY IN GET RESPONSE
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED FEATURES - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      //  NOW TO CHECK FEATURES COME BACK CORRECTLY IN THE GET RESPONSE
      expect(response.body.features).to.have.property('whoInitiatedTheTransfer', 'whoInitiatedTheTransfer Value')
      expect(response.body.features).to.have.property('typeOfTransfer', 'typeOfTransfer Value')
      expect(response.body.features).to.have.property('isCompleted', true)

      cy.log(URN)
    })
  })

  // AN UNAUTH USER CANNOT SET BENEFITS IN PUT REQUEST
  it('PUT - Verify An UNAuthorised User CANNOT SET-BENEFITS On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-benefits',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT APIKEY FOR UNAUTH',
      },
      body: AuthorisedUserCanSetBenefitsPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO SET BENEFITS IN PUT REQUEST
  it('PUT - Verify An Authorised User Can SET-BENEFITS On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-benefits',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetBenefitsPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK SET BENEFITS IN GET RESPONSE...
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED BENEFITS - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      expect(response.body.features).to.have.property('whoInitiatedTheTransfer', 'whoInitiatedTheTransfer Value')
      expect(response.body.features).to.have.property('typeOfTransfer', 'typeOfTransfer Value')
      expect(response.body.features).to.have.property('isCompleted', true)

      // CHECK OUR SETBENEFITS STUFF IS COMING BACK OK IN RESPONSE BELOW
      expect(response.body.benefits.intendedTransferBenefits.selectedBenefits[0]).to.equal('selectedBenefits value')
      expect(response.body.benefits.intendedTransferBenefits.otherBenefitValue).to.equal('otherBenefitValue value')
      expect(response.body.benefits.otherFactorsToConsider.highProfile.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.highProfile.furtherSpecification).to.equal('highProfile furtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.furtherSpecification).to.equal('complexLandAndBuildingFurtherSpecification Value')
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.furtherSpecification).to.equal('financeAndDebtFurtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.furtherSpecification).to.equal('otherRisksfurtherSpecification value')
      expect(response.body.benefits.equalitiesImpactAssessmentConsidered).to.be.true
      expect(response.body.benefits.isCompleted).to.be.true
      expect(response.body.benefits.anyRisks).to.be.true

      cy.log(URN)
    })
  })

  // AN UNAUTHORISED USER TRIES TO SET ADDITIONAL SCHOOL DATA IN PUT REQUEST
  it('PUT - Verify An UNAuthorised User CANNOT SET-SCHOOL-ADDITIONAL-DATA On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-school-additional-data',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT APIKEY - 401 UNAUTH EXPECTED',
      },
      body: AuthorisedUserCanSetAdditionalSchoolDataPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO SET ADDITIONAL SCHOOL DATA IN PUT REQUEST
  it('PUT - Verify An Authorised User Can SET-SCHOOL-ADDITIONAL-DATA On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-school-additional-data',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetAdditionalSchoolDataPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK SET SCHOOL ADDITIONAL DATA IN GET RESPONSE...
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED SCHOOL ADDITIONAL DATA - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      expect(response.body.features).to.have.property('whoInitiatedTheTransfer', 'whoInitiatedTheTransfer Value')
      expect(response.body.features).to.have.property('typeOfTransfer', 'typeOfTransfer Value')
      expect(response.body.features).to.have.property('isCompleted', true)

      expect(response.body.benefits.intendedTransferBenefits.selectedBenefits[0]).to.equal('selectedBenefits value')
      expect(response.body.benefits.intendedTransferBenefits.otherBenefitValue).to.equal('otherBenefitValue value')
      expect(response.body.benefits.otherFactorsToConsider.highProfile.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.highProfile.furtherSpecification).to.equal('highProfile furtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.furtherSpecification).to.equal('complexLandAndBuildingFurtherSpecification Value')
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.furtherSpecification).to.equal('financeAndDebtFurtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.furtherSpecification).to.equal('otherRisksfurtherSpecification value')
      expect(response.body.benefits.equalitiesImpactAssessmentConsidered).to.be.true
      expect(response.body.benefits.isCompleted).to.be.true
      expect(response.body.benefits.anyRisks).to.be.true

      // CHECK SCHOOL ADDITIONAL DETAILS RETURNING CORRECTLY IN RESPONSE
      expect(response.body.transferringAcademies[0].pupilNumbersAdditionalInformation).to.equal('pupilNumbersAdditionalInformation value')
      expect(response.body.transferringAcademies[0].latestOfstedReportAdditionalInformation).to.equal('latestOfstedReportAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage2PerformanceAdditionalInformation).to.equal('keyStage2PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage4PerformanceAdditionalInformation).to.equal('KeyStage4PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage5PerformanceAdditionalInformation).to.equal('keyStage5PerformanceAdditionalInformation value')
      cy.log(URN)
    })
  })

  // AN UNAUTH USER TRIES TO SET TRUST INFO GENERAL INFO
  // TRY TO SET TRUST GENERAL INFO
  it('PUT - Verify An Authorised User Can SET-SCHOOL-TRUST-GENERAL-INFORMATION On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-general-information',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT APIKEY FOR 401 RESPONSE',
      },
      body: AuthorisedUserCanSetTrustInfoAndProjectDatesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })
  // TRY TO SET TRUST GENERAL INFO
  it('PUT - Verify An Authorised User Can SET-SCHOOL-GENERAL-INFORMATION On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/set-general-information',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanSetTrustInfoAndProjectDatesPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK SET TRUST INFO IN GET RESPONSE...
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE UPDATED SCHOOL ADDITIONAL DATA - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      expect(response.body.features).to.have.property('whoInitiatedTheTransfer', 'whoInitiatedTheTransfer Value')
      expect(response.body.features).to.have.property('typeOfTransfer', 'typeOfTransfer Value')
      expect(response.body.features).to.have.property('isCompleted', true)

      expect(response.body.benefits.intendedTransferBenefits.selectedBenefits[0]).to.equal('selectedBenefits value')
      expect(response.body.benefits.intendedTransferBenefits.otherBenefitValue).to.equal('otherBenefitValue value')
      expect(response.body.benefits.otherFactorsToConsider.highProfile.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.highProfile.furtherSpecification).to.equal('highProfile furtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.furtherSpecification).to.equal('complexLandAndBuildingFurtherSpecification Value')
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.furtherSpecification).to.equal('financeAndDebtFurtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.furtherSpecification).to.equal('otherRisksfurtherSpecification value')
      expect(response.body.benefits.equalitiesImpactAssessmentConsidered).to.be.true
      expect(response.body.benefits.isCompleted).to.be.true
      expect(response.body.benefits.anyRisks).to.be.true

      expect(response.body.transferringAcademies[0].pupilNumbersAdditionalInformation).to.equal('pupilNumbersAdditionalInformation value')
      expect(response.body.transferringAcademies[0].latestOfstedReportAdditionalInformation).to.equal('latestOfstedReportAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage2PerformanceAdditionalInformation).to.equal('keyStage2PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage4PerformanceAdditionalInformation).to.equal('KeyStage4PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage5PerformanceAdditionalInformation).to.equal('keyStage5PerformanceAdditionalInformation value')

      // CHECKING THE NEW SET TRUST RETURNS HERE
      expect(response.body.generalInformation).to.have.property('recommendation', 'recommendationString value')
      expect(response.body.generalInformation).to.have.property('author', 'authorString value')
      cy.log(URN)
    })
  })

  // UNAUTH USER TRIES TO ASSIGN A USER TO A PROJECT
  it('PUT - Verify An UNAuthorised User CANNOT ASSIGN-USER to project On New MAT Transfer We Created - 401 UNAUTHORISED EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/assign-user',
      failOnStatusCode: false,
      method: 'PUT',
      headers:
      {
        'x-api-key': 'INCORRECT APIKEY - LOOKING FOR 401',
      },
      body: AuthorisedUserCanAssignUserDataPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)
      cy.log(URN)
    })
  })

  // TRY TO ASSIGN A USER TO A PROJECT
  it('PUT - Verify An Authorised User Can ASSIGN-USER to project On New MAT Transfer We Created - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN + '/assign-user',
      method: 'PUT',
      headers:
      {
        'x-api-key': apiKey,
      },
      body: AuthorisedUserCanAssignUserDataPayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      cy.log(URN)
    })
  })

  // CHECK SET ASSIGN USER DETAILS COME BACK IN GET RESPONSE...
  it('GET - Verify An Authorised User Can GET THE New MAT Transfer We Created WITH THE USER ASSIGNED TO THE PROJECT - 200 OK EXPECTED', () => {
    cy.api({
      url: url + '/transfer-project/' + URN,
      method: 'GET',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)

      expect(response.body).to.have.property('projectUrn')

      expect(response.body).to.have.property('projectReference', 'MAT-' + URN)

      expect(response.body.transferringAcademies[0]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[0]).to.have.property('incomingTrustUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('outgoingAcademyUkprn')
      expect(response.body.transferringAcademies[1]).to.have.property('incomingTrustUkprn')

      expect(response.body.rationale).to.have.property('projectRationale', 'projectRationale')
      expect(response.body.rationale).to.have.property('trustSponsorRationale', 'trustSponsorRationale')
      expect(response.body.rationale).to.have.property('isCompleted', true)

      expect(response.body.dates).to.have.property('targetDateForTransfer').to.match(getTransferDateTimeFormatRegex)
      expect(response.body.dates).to.have.property('htbDate').to.match(getTransferDateTimeFormatRegex)

      expect(response.body.legalRequirements).to.have.property('incomingTrustAgreement', 'incoming Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('diocesanConsent', 'diocesanConsent Value')
      expect(response.body.legalRequirements).to.have.property('outgoingTrustConsent', 'outgoing Trust Consent Value')
      expect(response.body.legalRequirements).to.have.property('isCompleted', true)

      expect(response.body.features).to.have.property('whoInitiatedTheTransfer', 'whoInitiatedTheTransfer Value')
      expect(response.body.features).to.have.property('typeOfTransfer', 'typeOfTransfer Value')
      expect(response.body.features).to.have.property('isCompleted', true)

      expect(response.body.benefits.intendedTransferBenefits.selectedBenefits[0]).to.equal('selectedBenefits value')
      expect(response.body.benefits.intendedTransferBenefits.otherBenefitValue).to.equal('otherBenefitValue value')
      expect(response.body.benefits.otherFactorsToConsider.highProfile.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.highProfile.furtherSpecification).to.equal('highProfile furtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.complexLandAndBuilding.furtherSpecification).to.equal('complexLandAndBuildingFurtherSpecification Value')
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.financeAndDebt.furtherSpecification).to.equal('financeAndDebtFurtherSpecification value')
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.shouldBeConsidered).to.be.true
      expect(response.body.benefits.otherFactorsToConsider.otherRisks.furtherSpecification).to.equal('otherRisksfurtherSpecification value')
      expect(response.body.benefits.equalitiesImpactAssessmentConsidered).to.be.true
      expect(response.body.benefits.isCompleted).to.be.true
      expect(response.body.benefits.anyRisks).to.be.true

      expect(response.body.transferringAcademies[0].pupilNumbersAdditionalInformation).to.equal('pupilNumbersAdditionalInformation value')
      expect(response.body.transferringAcademies[0].latestOfstedReportAdditionalInformation).to.equal('latestOfstedReportAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage2PerformanceAdditionalInformation).to.equal('keyStage2PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage4PerformanceAdditionalInformation).to.equal('KeyStage4PerformanceAdditionalInformation value')
      expect(response.body.transferringAcademies[0].keyStage5PerformanceAdditionalInformation).to.equal('keyStage5PerformanceAdditionalInformation value')

      expect(response.body.generalInformation).to.have.property('recommendation', 'recommendationString value')
      expect(response.body.generalInformation).to.have.property('author', 'authorString value')

      // CHECK ASSIGNED USER WE SET COMES BACK IN GET RESPONSE
      expect(response.body.assignedUser).to.have.property('fullName', 'userFullName value')
      expect(response.body.assignedUser).to.have.property('emailAddress', 'userEmail@useremailval.com')
      expect(response.body.assignedUser).to.have.property('id', '3fa85f64-5717-4562-b3fc-2c963f66afa6')

      cy.log(URN)
    })
  })
})
