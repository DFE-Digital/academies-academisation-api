const AuthorisedUserCannotUpdateTheirContributorsEmailToInvalidEmailBodyPayload = {
    "applicationId": 24,
    "applicationType": "JoinAMat",
    "applicationStatus": "InProgress",
    "contributors": [
        {
            "contributorId": 24,
            "firstName": "Dan",
            "lastName": "Good",
            "emailAddress": "POTATO",
            "role": "ChairOfGovernors",
            "otherRoleName": null
        }
    ],
    "schools": [
        {
            "id": 20,
            "urn": 113537,
            "schoolName": "Plymstock School",
            "landAndBuildings": {
                "ownerExplained": "Andrew Parsons",
                "worksPlanned": false,
                "worksPlannedDate": "2023-02-28T15:06:55.953Z",
                "worksPlannedExplained": "Andrew Parsons has got in contractors to build the new classrooms with a business deal.",
                "facilitiesShared": true,
                "facilitiesSharedExplained": "facilitiesSharedExplained string",
                "grants": true,
                "grantsAwardingBodies": "grantsAwardingBodies String",
                "partOfPfiScheme": false,
                "partOfPfiSchemeType": null,
                "partOfPrioritySchoolsBuildingProgramme": true,
                "partOfBuildingSchoolsForFutureProgramme": true
            },
            "performance": {
                "inspectedButReportNotPublished": false,
                "inspectedButReportNotPublishedExplain": "inspectedButReportNotPublishedExplain string",
                "ongoingSafeguardingInvestigations": false,
                "ongoingSafeguardingDetails": "ongoingSafeguardingDetails string"
            },
            "localAuthority": {
                "partOfLaReorganizationPlan": false,
                "laReorganizationDetails": "laReorganizationDetails string",
                "partOfLaClosurePlan": false,
                "laClosurePlanDetails": "laClosurePlanDetails string"
            },
            "partnershipsAndAffliations": {
                "isPartOfFederation": false,
                "isSupportedByFoundation": false,
                "supportedFoundationName": "supportedFoundationName string",
                "supportedFoundationEvidenceDocumentLink": "supportedFoundationEvidenceDocumentLink string",
                "feederSchools": "feederSchools string"
            },
            "religiousEducation": {
                "faithSchool": false,
                "faithSchoolDioceseName": "faithSchoolDioceseName string",
                "diocesePermissionEvidenceDocumentLink": "diocesePermissionEvidenceDocumentLink string",
                "hasSACREException": false,
                "sacreExemptionEndDate": "2022-09-12T15:06:55.953Z"
            },
                "previousFinancialYear": {
                "financialYearEndDate": "2023-02-28T15:06:55.953",
                "revenue": 50000.00,
                "revenueStatus": 1,
                "revenueStatusExplained": "revenueExplained String",
                "revenueStatusFileLink": "revenueStatusFileLink",
                "capitalCarryForward": 20000.00,
                "capitalCarryForwardStatus": 1,
                "capitalCarryForwardExplained": "capital CarryForwardExplained String",
                "capitalCarryForwardFileLink": "capitalCarryForwardFileLink String"
            },
            "currentFinancialYear": {
                "financialYearEndDate": "2022-09-12T15:06:55.953Z",
                "revenue": 500000.99,
                "revenueStatus": "Surplus",
                "revenueStatusExplained": "revenueStatusExplained string",
                "revenueStatusFileLink": "revenueStatusFileLink string",
                "capitalCarryForward": 0.00,
                "capitalCarryForwardStatus": "Surplus",
                "capitalCarryForwardExplained": "capitalCarryForwardExplained string",
                "capitalCarryForwardFileLink": "capitalCarryForwardFileLink string"
            },
            "nextFinancialYear": {
                "financialYearEndDate": null,
                "revenue": null,
                "revenueStatus": null,
                "revenueStatusExplained": null,
                "revenueStatusFileLink": null,
                "capitalCarryForward": null,
                "capitalCarryForwardStatus": null,
                "capitalCarryForwardExplained": null,
                "capitalCarryForwardFileLink": null
            },
        "loans": [
        {
          "loanId": 23,
          "amount": 500000.00,
          "purpose": "The purpose of this is to totally revitalise Plymstock School as an even better",
          "provider": "string",
          "interestRate": 3.25,
          "schedule": "string"
        }
        ],
            "schoolContributionToTrust": "string",
            "governingBodyConsentEvidenceDocumentLink": "string",
            "additionalInformationAdded": false,
            "additionalInformation": "string",
            "equalitiesImpactAssessmentCompleted": "ConsideredUnlikely",
            "equalitiesImpactAssessmentDetails": "string",
            "schoolConversionContactHeadName": "string",
            "schoolConversionContactHeadEmail": "andrew.parsons@plymstock.plym.sch.uk",
            "schoolConversionContactHeadTel": "string",
            "schoolConversionContactChairName": "string",
            "schoolConversionContactChairEmail": "lee.fisher@plymouth.gov.uk",
            "schoolConversionContactChairTel": "string",
            "schoolConversionContactRole": "string",
            "schoolConversionMainContactOtherName": "string",
            "schoolConversionMainContactOtherEmail": "andrea.hamley@plymstock.sch.uk",
            "schoolConversionMainContactOtherTelephone": "string",
            "schoolConversionMainContactOtherRole": "string",
            "schoolConversionApproverContactName": "string",
            "schoolConversionApproverContactEmail": "dangood84@me.com",
            "schoolConversionTargetDateSpecified": false,
            "schoolConversionTargetDate": "2022-09-12T15:06:55.953Z",
            "schoolConversionTargetDateExplained": "string",
            "conversionChangeNamePlanned": true,
            "proposedNewSchoolName": "string",
            "applicationJoinTrustReason": "string",
            "projectedPupilNumbersYear1": 1000,
            "projectedPupilNumbersYear2": 1250,
            "projectedPupilNumbersYear3": 1500,
            "schoolCapacityAssumptions": "string",
            "schoolCapacityPublishedAdmissionsNumber": 0,
            "schoolSupportGrantFundsPaidTo": "School",
            "confirmPaySupportGrantToSchool": false
        }
    ]
}
   module.exports = {AuthorisedUserCannotUpdateTheirContributorsEmailToInvalidEmailBodyPayload}