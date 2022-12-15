/// <reference types="Cypress" />
import { AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload } from '../support/payloads/FAM-TrustKeyPerson/AuthorisedUserCanCreateNewFAMTrustKeyPerson.spec'
import { AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload } from '../support/payloads/FAM-TrustKeyPerson/AuthorisedUserCanUpdateNewFAMTrustKeyPerson.spec'
import { AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOBBodyPayload } from '../support/payloads/FAM-TrustKeyPerson/AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOB.spec'
describe('Academisation API Testing - FAM - Add Trust Key Person', () => {

    let apiKey = Cypress.env('apiKey');
    let applicationNumber = 177
    //let trustKeyPersonNumber = 11;
    //let getDateTimestampFormatRegex = /^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})Z?$/
   
    Cypress.config('baseUrl', 'https://s184d01-aca-aca-app.nicedesert-a691fec6.westeurope.azurecontainerapps.io')

    it('POST - Verify An UnAuthorised User Is Unable To Create New FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', () => {
      cy.request({
              method: 'POST',
              url: '/application/' + applicationNumber + '/form-trust/key-person',
              failOnStatusCode: false,
              headers: 
              {
                'x-api-key' : 'INVALIDAPIKEY'
              },
              body: 
              AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload
          }).then((response) => {
          //cy.log(JSON.stringify(response))
          expect(response).to.have.property('status', 401)

          })
  })

    it('POST - Verify An Authorised User Is Able To Create New FAM-Trust Key Person - Form-Trust Key-Person - 200 CREATED Expected', () => {
          cy.request({
                  method: 'POST',
                  url: '/application/' + applicationNumber + '/form-trust/key-person',
                  headers: 
                  {
                    'x-api-key' : apiKey
                  },
                  body: 
                  AuthorisedUserCanCreateNewFAMTrustKeyPersonBodyPayload
              }).then((response) => {
              //cy.log(JSON.stringify(response))
              expect(response).to.have.property('status', 200)

              })
      })

      it('POST - Verify An Authorised User Is Unable To Create New FAM-Trust Key Person - Form-Trust Key-Person WITH AN INVALID DOB - 400 BAD REQUEST Expected', () => {
        cy.request({
                method: 'POST',
                url: '/application/' + applicationNumber + '/form-trust/key-person',
                failOnStatusCode: false,
                headers: 
                {
                  'x-api-key' : apiKey
                },
                body: 
                AuthorisedUserCannotCreateNewFAMTrustKeyPersonWITHINVALIDDOBBodyPayload
            }).then((response) => {
            //cy.log(JSON.stringify(response))
            expect(response).to.have.property('status', 400)
  
            })
    })

      it('GET - Verify An UNAuthorised User Is UNAble To GET FAM-Trust Key PERSONS - Form-Trust Key-PersonS - 401 UNAUTHORISED Expected', () => {
        cy.request({
                method: 'GET',
                url: '/application/' + applicationNumber + '/form-trust/key-person',
                failOnStatusCode: false,
                headers: 
                {
                  'x-api-key' : 'INVALIDAPIKEY'
                },
            }).then((response) => {
                //cy.log(JSON.stringify(response))
                expect(response).to.have.property('status', 401)



    })
  })

      it('GET - Verify An Authorised User Is Able To GET FAM-Trust Key PERSONS - Form-Trust Key-PersonS - 200 OK Expected', () => {
        cy.request({
                method: 'GET',
                url: '/application/' + applicationNumber + '/form-trust/key-person',
                headers: 
                {
                  'x-api-key' : apiKey
                },
            }).then((response) => {
                //cy.log(JSON.stringify(response))
                expect(response).to.have.property('status', 200)
                expect(response.body[0]).to.have.property('id')
                Cypress.env('responseIDForRequest',response.body[0].id)


    })

  })

  it('GET - Verify An UNAuthorised User Is UNAble To GET FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', () => {
    cy.request({
            method: 'GET',
            url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 105,
            failOnStatusCode: false,
            headers: 
            {
              'x-api-key' : 'INVALIDAPIKEY'
            },
        }).then((response) => {
            //cy.log(JSON.stringify(response))
            expect(response).to.have.property('status', 401)
            //expect(response.body).to.have.property('id')
            //Cypress.env('responseIDForRequest',response.body[0].id)


})

})

  it('GET - Verify An Authorised User Is Able To GET FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {
    cy.request({
            method: 'GET',
            url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 105,
            headers: 
            {
              'x-api-key' : apiKey
            },
        }).then((response) => {
            //cy.log(JSON.stringify(response))
            expect(response).to.have.property('status', 200)
            expect(response.body).to.have.property('id')
            //Cypress.env('responseIDForRequest',response.body[0].id)


})

})

it('PUT - Verify An UNAuthorised User Is UNAble To UPDATE a FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', () => {

  //cy.contains('id', Id)
cy.log(JSON.stringify("Id = " + Cypress.env('responseIDForRequest')))
cy.request({
        method: 'PUT',
        url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 19,
        failOnStatusCode: false,
        headers: 
        {
          'x-api-key' : 'INVALIDAPIKEY'
        },
        body:
        {
          "applicationId": 177,
          "keyPersonId": 105,
          "roles": [
            {
              "id": 1,
              "role": 3,
              "timeInRole": "6 years"
            }
          ],
          "name": "Andrew Parsons",
          "dateOfBirth": "1945-12-12T14:08:05.835Z",
          "biography": "Wonderful Headmaster and astute businessman"
        }
    }).then((response) => {
    //cy.log(JSON.stringify(response))
    expect(response).to.have.property('status', 401)
  })




})

  it('PUT - Verify An Authorised User Is Able To UPDATE a FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {

      //cy.contains('id', Id)
   cy.log(JSON.stringify("Id = " + Cypress.env('responseIDForRequest')))
    cy.request({
            method: 'PUT',
            url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + 19,
            headers: 
            {
              'x-api-key' : apiKey
            },
            body:
            AuthorisedUserCanUpdateNewFAMTrustKeyPersonBodyPayload
        }).then((response) => {
        //cy.log(JSON.stringify(response))
        expect(response).to.have.property('status', 200)
      })




})

it('DELETE - Verify An UNAuthorised User Is UNable To DELETE FAM-Trust Key Person - Form-Trust Key-Person - 401 UNAUTHORISED Expected', () => {
  cy.request({
          method: 'DELETE',
          url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + Cypress.env('responseIDForRequest'),
          failOnStatusCode: false,
          headers: 
          {
            'x-api-key' : 'INVALID APIKEY'
          },
      }).then((response) => {
      //cy.log(JSON.stringify(response))
      expect(response).to.have.property('status', 401)

      })
    })

  it('DELETE - Verify An Authorised User Is Able To DELETE FAM-Trust Key Person - Form-Trust Key-Person - 200 OK Expected', () => {
    cy.request({
            method: 'DELETE',
            url: '/application/' + applicationNumber + '/form-trust/key-person' + '/' + Cypress.env('responseIDForRequest'),
            headers: 
            {
              'x-api-key' : apiKey
            },
        }).then((response) => {
        //cy.log(JSON.stringify(response))
        expect(response).to.have.property('status', 200)

        })
        


  })
  

})