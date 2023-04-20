const AuthorisedUserCanUpdatePayload = 
{
    "applicationId": 10002,
    "applicationType": "joinAMat",
    "applicationStatus": "inProgress",
    "contributors": [
        {
            "contributorId": 0,
            "firstName": "Dan",
            "lastName": "Good",
            "emailAddress": "Dan.GOOD@education.gov.uk",
            "role": "chairOfGovernors"
        }
    ],
    "schools": [
        {
            "id": 0,
            "urn": 136568,
            "schoolName": "Plymstock School",
            "landAndBuildings": {},
            "safeguarding": false,
            "partOfFederation": false,
            "previousFinancialYear": {
                "financialYearEndDate": "2022-03-31T00:00:00",
                "revenue": 4999.99,
                "revenueStatus": "surplus",
                "capitalCarryForward": 4998.98,
                "capitalCarryForwardStatus": "surplus"
            },
            "currentFinancialYear": {
                "financialYearEndDate": "2023-03-31T00:00:00",
                "revenue": 5999.99,
                "revenueStatus": "deficit",
                "revenueStatusExplained": "Explain the reason for the deficit, how the school plan to deal with it, and the recovery plan.\r\nProvide details of the financial forecast and/or the deficit recovery plan agreed with the local authori",
                "capitalCarryForward": 5998.98,
                "capitalCarryForwardStatus": "deficit",
                "capitalCarryForwardExplained": "Explain the reason for the deficit, how the school plan to deal with it, and the recovery plan.\r\nProvide details of the financial forecast and/or the deficit recovery plan agreed with the local authori"
            },
            "nextFinancialYear": {
                "financialYearEndDate": "2024-03-31T00:00:00",
                "revenue": 99999.99,
                "revenueStatus": "deficit",
                "revenueStatusExplained": "Explain the reason for the deficit, how the school plan to deal with it, and the recovery plan.\r\nProvide details of the financial forecast and/or the deficit recovery plan agreed with the local authori",
                "capitalCarryForward": 99999.98,
                "capitalCarryForwardStatus": "deficit",
                "capitalCarryForwardExplained": "Explain the reason for the deficit, how the school plan to deal with it, and the recovery plan.\r\nProvide details of the financial forecast and/or the deficit recovery plan agreed with the local authori"
            },
            "loans": [],
            "leases": [],
            "schoolConversionContactHeadName": "Paul Lockwood",
            "schoolConversionContactHeadEmail": "paul.lockwood@education.gov.uk",
            "schoolConversionContactHeadTel": "01752404930",
            "schoolConversionContactChairName": "Dan Good",
            "schoolConversionContactChairEmail": "dan.good@education.gov.uk",
            "schoolConversionContactChairTel": "01752404000",
            "schoolConversionContactRole": "ChairOfGoverningBody",
            "schoolConversionApproverContactName": "Nicky Price",
            "schoolConversionApproverContactEmail": "nicky.price@aol.com",
            "schoolConversionTargetDateSpecified": false,
            "conversionChangeNamePlanned": false,
            "applicationJoinTrustReason": "Why does the school want to join this trust in particular? Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
            "financeOngoingInvestigations": false,
            "hasLoans": false,
            "hasLeases": false,
            "entityId": "2540a351-0ef6-4ced-ae62-2485c9a4d6db"
        }
    ],
    "joinTrustDetails": {
        "id": 67,
        "ukprn": 10060481,
        "trustName": "THE MINERVA LEARNING TRUST",
        "trustReference": "TR03169",
        "changesToTrust": "unknown",
        "changesToLaGovernance": false
    },
    "applicationSubmittedDate": "2023-03-10T11:09:22.5191374",
    "applicationReference": "A2B_10002",
    "entityId": "e6ffff6e-e785-4c4f-a920-0fd32ead237e"
}
module.exports = {AuthorisedUserCanUpdatePayload}
