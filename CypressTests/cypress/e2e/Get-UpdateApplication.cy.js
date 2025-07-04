import { UnauthorisedUserCannotUpdatePayload } from '../fixtures/payloads/UnauthorisedUserCannotUpdateBody'
import { AuthorisedUserCanUpdatePayload } from '../fixtures/payloads/AuthorisedUserCanUpdate'
import { AuthorisedUserCannotUpdateTheirContributorsEmailToAnotherEmailBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeTheirContributorsEmailToAnotherEmail'
import { AuthorisedUserCannotUpdateTheirContributorsEmailToInvalidEmailBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeTheirContributorsEmailToInvalidEmail'
import { AuthorisedUserCannotUpdateWorksPlannedDateToInvalidDateBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeWorksPlannedDateToInvalidDate'
import { AuthorisedUserCannotUpdateSacreExemptionEndDateToInvalidDateBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeSacreExemptionEndDateToInvalidDate'
import { AuthorisedUserCannotUpdatePreviousFinancialYearEndDateToInvalidDateBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangePreviousFinancialYearEndDateToInvalidDate'
import { AuthorisedUserCannotUpdateCurrentFinancialYearEndDateToInvalidDateBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeCurrentFinancialYearEndDateToInvalidDate'
import { AuthorisedUserCannotUpdateNextFinancialYearEndDateToInvalidDateBodyPayload } from '../fixtures/payloads/AuthorisedUserCannotChangeNextFinancialYearEndDateToInvalidDate'

describe('Academisation API Testing', () => {
  let apiKey = Cypress.env('apiKey')
  let url = Cypress.env('url')
  let applicationNumber = url.includes('test') ? 281 : 10002;
  let emailRegex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/
  let getDateTimestampFormatRegex = /^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})Z?$/

  it('GET - Verify An Authorised User Can Retrieve A Respective Application - 200 OK EXPECTED', () => {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber,
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
      expect(response.body).to.have.property('contributors')
      expect(response.body.contributors[0]).to.have.property('firstName')
      expect(response.body.contributors[0]).to.have.property('lastName')
      expect(response.body.contributors[0]).to.have.property('emailAddress').to.match(emailRegex)
      expect(response.body.contributors[0]).to.have.property('role')

      expect(response.body).to.have.property('schools')

      // PREVIOUS FINANCIAL YEAR PROPERTIES
      expect(response.body.schools[0].previousFinancialYear).to.have.property('financialYearEndDate').to.match(getDateTimestampFormatRegex)
      expect(response.body.schools[0].previousFinancialYear).to.have.property('revenue')
      expect(response.body.schools[0].previousFinancialYear).to.have.property('revenueStatus')

      expect(response.body.schools[0].previousFinancialYear).to.have.property('capitalCarryForward')
      expect(response.body.schools[0].previousFinancialYear).to.have.property('capitalCarryForwardStatus')

      // CURRENT FINANCIAL YEAR PROPERTIES
      expect(response.body.schools[0].currentFinancialYear).to.have.property('financialYearEndDate').to.match(getDateTimestampFormatRegex)
      expect(response.body.schools[0].currentFinancialYear).to.have.property('revenue')
      expect(response.body.schools[0].currentFinancialYear).to.have.property('revenueStatus')
      expect(response.body.schools[0].currentFinancialYear).to.have.property('revenueStatusExplained')

      expect(response.body.schools[0].currentFinancialYear).to.have.property('capitalCarryForward')
      expect(response.body.schools[0].currentFinancialYear).to.have.property('capitalCarryForwardStatus')
      expect(response.body.schools[0].currentFinancialYear).to.have.property('capitalCarryForwardExplained')

      // NEXT FINANCIAL YEAR PROPERTIES
      expect(response.body.schools[0].nextFinancialYear).to.have.property('financialYearEndDate').to.match(getDateTimestampFormatRegex)
      expect(response.body.schools[0].nextFinancialYear).to.have.property('revenue')
      expect(response.body.schools[0].nextFinancialYear).to.have.property('revenueStatus')
      expect(response.body.schools[0].nextFinancialYear).to.have.property('revenueStatusExplained')

      expect(response.body.schools[0].nextFinancialYear).to.have.property('capitalCarryForward')
      expect(response.body.schools[0].nextFinancialYear).to.have.property('capitalCarryForwardStatus')
      expect(response.body.schools[0].nextFinancialYear).to.have.property('capitalCarryForwardExplained')
    })
  })

  it('GET - Verify An UNAUTHORISED USER CANNOT Retreive An Application - 401 UNAUTHORISED EXPECTED', function () {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })

  it('PUT - Verify An Authorised User Can Update An Application Correctly - 200 OK EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCanUpdatePayload,
    }).then((response) => {
      cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 200)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change Their Contributor\'s Email to ANOTHER EMAIL ADDRESS - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateTheirContributorsEmailToAnotherEmailBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change Their Contributor\'s Email to AN INVALID EMAIL ADDRESS - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateTheirContributorsEmailToInvalidEmailBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change The worksPlannedDate To An Invalid Date - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateWorksPlannedDateToInvalidDateBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change The sacreExemptionDate To An Invalid Date - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateSacreExemptionEndDateToInvalidDateBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change The previousFinancialYearEndDate To An Invalid Date - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdatePreviousFinancialYearEndDateToInvalidDateBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change The currentFinancialYearEndDate To An Invalid Date - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateCurrentFinancialYearEndDateToInvalidDateBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An Authorised User Is Unable To Change The nextFinancialYearEndDate To An Invalid Date - 400 BAD REQUEST EXPECTED', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotUpdateNextFinancialYearEndDateToInvalidDateBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('PUT - Verify An UNAUTHORISED USER CANNOT Update An Application - 401 UNAUTHORISED EXPECTED', function () {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber,
      failOnStatusCode: false,
      body:
        UnauthorisedUserCannotUpdatePayload,
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })
})
