const AuthorisedUserCannotUpdateCurrentFinancialYearEndDateToInvalidDateBodyPayload = {
    "applicationId": 24,
    "applicationType": "JoinAMat",
    "applicationStatus": "InProgress",
    "contributors": [
        {
            "contributorId": 24,
            "firstName": "Dan",
            "lastName": "Good",
            "emailAddress": "Dan.GOOD@education.gov.uk",
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
                "ownerExplained": "Jared Moore",
                "worksPlanned": false,
                "worksPlannedDate": "2023-11-15T15:06:55.953",
                "worksPlannedExplained": "Jared Moore has got in contractors to build the new classrooms with a business deal.",
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
                "sacreExemptionEndDate": "2023-02-28T15:06:55.953"
            },
            "previousFinancialYear": {
                "financialYearEndDate": "2023-11-15T15:06:55.953",
                "revenue": 50000.00,
                "revenueStatus": "Surplus",
                "revenueStatusExplained": "revenueExplained String",
                "revenueStatusFileLink": "revenueStatusFileLink",
                "capitalCarryForward": 20000.00,
                "capitalCarryForwardStatus": "Surplus",
                "capitalCarryForwardExplained": "capital CarryForwardExplained String",
                "capitalCarryForwardFileLink": "capitalCarryForwardFileLink String"
            },
            "currentFinancialYear": {
                "financialYearEndDate": "POTATO",
                "revenue": 500000.99,
                "revenueStatus": "Surplus",
                "revenueStatusExplained": "revenueStatusExplained string",
                "revenueStatusFileLink": "revenueStatusFileLink string",
                "capitalCarryForward": 30000.00,
                "capitalCarryForwardStatus": "Surplus",
                "capitalCarryForwardExplained": "capitalCarryForwardExplained string",
                "capitalCarryForwardFileLink": "capitalCarryForwardFileLink string"
            },
            "nextFinancialYear": {
                "financialYearEndDate": "2023-11-15T15:06:55.953",
                "revenue": 60000.00,
                "revenueStatus": "Deficit",
                "revenueStatusExplained": "revenueStatusExplained string Next Finanical Year",
                "revenueStatusFileLink": "revenueStatusFileLink string Next Financial Year",
                "capitalCarryForward": 60000.00,
                "capitalCarryForwardStatus": "Deficit",
                "capitalCarryForwardExplained": "capitalCarryForwardExplained string next Financial Year",
                "capitalCarryForwardFileLink": "capitalCarryForwardFileLink string"
            },
            "schoolContributionToTrust": "string",
            "governingBodyConsentEvidenceDocumentLink": "string",
            "additionalInformationAdded": false,
            "additionalInformation": "string",
            "equalitiesImpactAssessmentCompleted": "ConsideredUnlikely",
            "equalitiesImpactAssessmentDetails": "string",
            "schoolConversionContactHeadName": "string",
            "schoolConversionContactHeadEmail": "Hipolito.Bednar@example.net",
            "schoolConversionContactHeadTel": "string",
            "schoolConversionContactChairName": "string",
            "schoolConversionContactChairEmail": "Arno_Block@example.org",
            "schoolConversionContactChairTel": "string",
            "schoolConversionContactRole": "string",
            "schoolConversionMainContactOtherName": "string",
            "schoolConversionMainContactOtherEmail": "Lafayette9@example.org",
            "schoolConversionMainContactOtherTelephone": "string",
            "schoolConversionMainContactOtherRole": "string",
            "schoolConversionApproverContactName": "string",
            "schoolConversionApproverContactEmail": "Keven_Schinner@example.com",
            "schoolConversionTargetDateSpecified": false,
            "schoolConversionTargetDate": "2023-02-28T15:06:55.953",
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
   module.exports = {AuthorisedUserCannotUpdateCurrentFinancialYearEndDateToInvalidDateBodyPayload}