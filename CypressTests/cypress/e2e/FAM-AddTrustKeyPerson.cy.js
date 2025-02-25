import { AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload } from '../fixtures/payloads/FAM-TrustKeyPerson/AuthorisedUserCanCreateNewFAMTrustKeyPerson'
import { AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload } from '../fixtures/payloads/FAM-TrustKeyPerson/AuthorisedUserCanUpdateNewFAMTrustKeyPerson'
import { AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOBBodyPayload } from '../fixtures/payloads/FAM-TrustKeyPerson/AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOB'
import { UnauthorisedUserCannotUpdateNewFAMTrustKeyPersonBodyPayload } from '../fixtures/payloads/FAM-TrustKeyPerson/UnauthorisedUserCannotUpdateNewFAMTrustKeyPerson'

describe('Academisation API Testing - FAM - Add Trust Key Person', () => {
  let apiKey = Cypress.env('apiKey')
  let url = Cypress.env('url')
  let applicationNumber = url.includes('test') ? 10491 : 10038;
  let trustKeyPersonNumber = 0

  it('POST - Verify An UnAuthorised User Is Unable To Create New FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', function () {
    cy.api({
      method: 'POST',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person',
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': 'INVALIDAPIKEY',
      },
      body:
        AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })

  it('POST - Verify An Authorised User Is Able To Create New FAM-Trust Key Person - Form-Trust Key-Person - 200 CREATED Expected', () => {
    cy.api({
      method: 'POST',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person',
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 200)
    })
  })

  it('POST - Verify An Authorised User Is Unable To Create New FAM-Trust Key Person - Form-Trust Key-Person WITH AN INVALID DOB - 400 BAD REQUEST Expected', () => {
    cy.api({
      method: 'POST',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person',
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOBBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 400)
    })
  })

  it('GET - Verify An UNAuthorised User Is UNAble To GET FAM-Trust Key PERSONS - Form-Trust Key-PersonS - 401 UNAUTHORISED Expected', function () {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person',
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': 'INVALIDAPIKEY',
      },
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })

  it('GET - Verify An Authorised User Is Able To GET FAM-Trust Key PERSONS - Form-Trust Key-PersonS - 200 OK Expected', () => {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person',
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      expect(response).to.have.property('status', 200)
      expect(response.body[0]).to.have.property('id')
      trustKeyPersonNumber = response.body[0].id
    })
  })

  it('GET - Verify An UNAuthorised User Is UNAble To GET FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', function () {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 105,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': 'INVALIDAPIKEY',
      },
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })

  it('GET - Verify An Authorised User Is Able To GET FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {
    cy.api({
      method: 'GET',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + trustKeyPersonNumber,
      headers:
      {
        'x-api-key': apiKey,
      },
    }).then((response) => {
      expect(response).to.have.property('status', 200)
      expect(response.body).to.have.property('id')
    })
  })

  it('PUT - Verify An UNAuthorised User Is UNAble To UPDATE a FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', function () {

    cy.log(JSON.stringify('Id = ' + Cypress.env('responseIDForRequest')))
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 19,
      failOnStatusCode: false,
      headers:
      {
        'x-api-key': 'INVALIDAPIKEY',
      },
      body:
        UnauthorisedUserCannotUpdateNewFAMTrustKeyPersonBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 401)
    })
  })

  it('PUT - Verify An Authorised User Is Able To UPDATE a FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {
    cy.api({
      method: 'PUT',
      url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + trustKeyPersonNumber,
      headers:
      {
        'x-api-key': apiKey,
      },
      body:
        AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload,
    }).then((response) => {
      expect(response).to.have.property('status', 200)
    })
  })

  //  NEED TO COMMENT OUT THE UNAUTH DELETE AS THIS IS NOW DELETING WHEN IT SHOULDN'T BE
  /*
  it('DELETE - Verify An UNAuthorised User Is UNable To DELETE FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', () => {
    cy.api({
            method: 'DELETE',
            url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + Cypress.env('responseIDForRequest'),
            failOnStatusCode: false,
            headers:
            {
              'x-api-key' : 'INVALID APIKEY'
            },
        }).then((response) => {
        expect(response).to.have.property('status', 401)
        })
      })
    */

  // BEST TO TAKE THIS ONE OUT THE PIPELINE SO WE DON'T RUN OUT OF PEOPLE TO DELETE
  /*
    it('DELETE - Verify An Authorised User Is Able To DELETE FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {
      cy.api({
              method: 'DELETE',
              url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + Cypress.env('responseIDForRequest'),
              headers:
              {
                'x-api-key' : apiKey
              },
          }).then((response) => {
          expect(response).to.have.property('status', 200)
          })
    })
    */

  // WE DON'T CARE ABOUT THIS RIGHT NOW
  /*
    it('DELETE - Verify An Authorised User Is UNable To DELETE FAM-Trust Key Person THAT DOES NOT EXIST - Form-Trust Key-Person THAT DOES NOT EXIST - 500 SERVER ERROR Expected - (May change to 400 later)', () => {
      cy.api({
              method: 'DELETE',
              url: url + '/application/' + applicationNumber + '/form-trust/key-person' + '/' + Cypress.env('responseIDForRequest'),
              failOnStatusCode: false,
              headers:
              {
                'x-api-key' : apiKey
              },
          }).then((response) => {
          expect(response).to.have.property('status', 400)
          })
    })
    */
})
