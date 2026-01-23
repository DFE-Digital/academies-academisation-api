# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/). To see an example from a mature product in the program [see the Complete products changelog that follows the same methodology](https://github.com/DFE-Digital/complete-conversions-transfers-changes/blob/main/CHANGELOG.md).

---
 
## [Unreleased](https://github.com/DFE-Digital/academies-academisation-api/compare/production-2026-01-19.560...HEAD)
Note: remember to update unrelease section when creating a new release.

--- 

## [9.46.0][9.46.0] - 2026-01-19

### Added
- 236757 - Sets PFI Scheme type details
- Set viability issue flag if number of pupil percent is more than 84%
- Consultation response pre-populated

---

## [9.45.0][9.45.0] - 2026-01-07

---

## [9.44.0][9.44.0] - 2026-01-06

---

## [9.43.0][9.43.0] - 2026-01-06

---

## [9.42.0][9.42.0] - 2026-01-06

### Added
- 237053 - Approve with conditions

---

## [9.41.0][9.41.0] - 2025-12-18

### Changed
- Replaced ruby complete API with .NET API

---

## [9.40.0][9.40.0] - 2025-12-04

### Added
- Auto-populate proposed conversion date while creating conversion project in A2B

---

## [9.39.0][9.39.0] - 2025-11-28

### Added
- Added a new property to the CreateTransferProject endpoint

### Changed
- Package Validator policy update

---

## [9.38.0][9.38.0] - 2025-08-13

### Added
- Add form a mat to summary endpoint

### Changed
- Replace Advisory Board date by Proposed Decision Date
- Reverting AB Changes

---

## [9.37.0][9.37.0] - 2025-07-03

### Added
- Added summary endpoint

### Changed
- Conversion Support Grant Removal - School Application Form
- School Support grant set to null when application submitted after deadline during PUT request
- Support grant set default to zero rather than null
- Set Support grant amount to zero when Voluntary Conversion and Post deadline

---

## [9.36.0][9.36.0] - 2025-06-13

### Fixed
- Excel exports fail if too many UKPRNs are queried

---

## [9.35.0][9.35.0] - 2025-05-23

### Fixed
- 213830 and 213699 - Bug fixes
- Eliminate AB dates duplicates

### Changed
- Reverse excel download changes for Advisory board column names

---

## [9.34.0][9.34.0] - 2025-04-28

### Added
- 200233 - Add PSED task to prepare
- PSED Preview project document screen

---

## [9.33.0][9.33.0] - 2025-02-04

---

## [9.32.0][9.32.0] - 2024-11-06

### Changed
- Hosted service delay and log information changed to log error

---

## [9.31.0][9.31.0] - 2024-11-05

### Added
- Create complete projects
- Send transfers to complete
- Send form a mat projects to complete
- Add manual trust reference number to form a mat transfer
- Auto generate TRN for form a mat A2B applications

### Fixed
- Fixes after dev testing for sending projects to complete
- Fix to only bring back projects to enrich which have a trust reference number
- Fix for project enrichment - projects without a trust reference
- Date conversions to UK date to avoid server culture settings clash
- Generate TRN await fix

---

## [9.30.0][9.30.0] - 2024-10-09

---

## [9.29.0][9.29.0] - 2024-09-25

### Added
- Setup User Role permission
- Assign user with role capabilities
- Add mock server
- New DAO revoked reason for 'policy change'

### Fixed
- Fixed error if no role is defined

---

## [9.28.0][9.28.0] - 2024-09-06

### Added
- Added two columns in export sheet for conversion and transfer

---

## [9.27.0][9.27.0] - 2024-08-22

### Added
- Create Project Group Endpoint
- Project groups endpoints
- 173335 - Grouping projects
- API Integration Test Setup for Integration tests
- 176671 - Delete project group endpoint
- Transfers by outgoing trust ukprn endpoint

### Fixed
- Fix for swagger UI with CSP

### Changed
- Split Set Project Group endpoint into Assign user and update Project Group's Conversion Project Endpoints
- Update Conversion projects with project group Id on adding them into a project

---

## [9.26.0][9.26.0] - 2024-07-15

### Added
- Opening date event
- 169468 - Amend advisory board date
- DAO Revoked export

### Fixed
- Bugfix - enum DeputyDirector and Grade6 wrong way round
- Proposed conversion date migration fix

---

## [9.25.0][9.25.0] - 2024-07-04

### Added
- School improvement plans

---

## [9.24.0][9.24.0] - 2024-06-24

### Added
- 160874 - DAO revoked
- SharePoint files in prepare

### Changed
- Kim changes for transferring academies
- Cypress endpoints

---

## [9.23.0][9.23.0] - 2024-06-04

### Added
- Transfers missing filter fields
- Cypress endpoints

### Fixed
- Small bug fix

### Changed
- Export moved to school level for transfers

---

## [9.22.0][9.22.0] - 2024-05-20

### Added
- Additional fields onto filter
- Delegate decision
- PFI added to Transferring Academies

### Changed
- Refactoring - remove data commands
- 162020 - Update incoming trust

---

## [9.21.0][9.21.0] - 2024-05-08

### Changed
- 162626 - Update project list and filter
- Transfer project export optimisation

### Fixed
- V4 endpoint fix

---

## [9.20.0][9.20.0] - 2024-05-02

---

## [9.19.0][9.19.0] - 2024-05-02

---

## [9.18.0][9.18.0] - 2024-05-02

### Added
- Transfer filters

### Fixed
- Form a mat flag reset fix

### Changed
- Refactoring - advisory board decision remove state objects
- Updated date formats to be sortable

---

## [9.17.0][9.17.0] - 2024-05-01

---

## [9.16.0][9.16.0] - 2024-04-23

### Added
- 162020 - Preferred trust
- Filter transfers

---

## [9.15.0][9.15.0] - 2024-04-16

### Fixed
- Bugfix to return transfer project Id in the API response

---

## [9.14.0][9.14.0] - 2024-04-15

### Added
- Search for FAM based on A2B ref, FAM ref or trust name
- Ability to assign a project to a FAM
- Is form a mat flag
- Soft delete project

### Fixed
- Update Project Controller AddConversion to use correct createResult
- Enrichment not overriding isformamat now
- EF version docker fix

### Changed
- Dotnet 8 upgrade, nuget packages upgraded to latest version

---

## [9.13.0][9.13.0] - 2024-03-13

### Added
- 157115 - No trust sponsored
- Adding FAM references
- Hosted service
- 157116 - Change trust name
- Ability to add FAM from conversions
- Allowing for sponsored FAM

### Fixed
- EF can't interpret NullOrEmpty
- Using correct advisory board date

### Removed
- Redundant transfers background service

---

## [9.12.0][9.12.0] - 2024-03-07

### Added
- Add PFI scheme to field
- 156787 - Export transfer projects endpoint
- AO Date to Record a decision
- Transfers form a mat

### Changed
- Add service unit tests to coverage

---

## [9.11.0][9.11.0] - 2024-02-26

### Added
- 155112 - Clear transfer routes

### Fixed
- Form a mat paging fix

### Changed
- Remove AO required for conversions
- Update transfer project status

---

## [9.10.0][9.10.0] - 2024-02-21

---

## [9.9.0][9.9.0] - 2024-02-21

### Added
- FormAMAT API
- 155114 - Record a decision tab
- 153461 - Form a mat project list
- Conversions form a mat
- Withdrawn decision status for the API

### Fixed
- Corrected PerformanceData Endpoint name

---

## [9.8.0][9.8.0] - 2024-01-30

### Added
- Set performance data

---

## [9.7.0][9.7.0] - 2024-01-24

### Added
- MOJ project filter
- SEN task list
- 147236 - PRU task list

### Fixed
- Add mapping so PRU groups stop getting overwritten

### Changed
- Spike spreadsheet filter
- Introduced .AsSplitQuery() on default includes for application object
- Added unit tests to cover v2 project search

---

## [9.6.0][9.6.0] - 2023-12-15

---

## [9.5.0][9.5.0] - 2023-12-15

---

## [9.4.0][9.4.0] - 2023-12-12

### Added
- External application form - allows for an application form saved in SharePoint to be linked to voluntary conversions

### Changed
- 140633 - Refactor of project state
- Refactor naming from Sponsored to New

---

## [9.3.0][9.3.0] - 2023-09-27

### Changed
- 140396 - Pull trust names in background task
  
--- 
 
## [9.2.0][9.2.0] - 2023-09-21
* Added transfer projects
* Added fields required to a Conversion Project to allow for sponsored grants

---
 
## 9.1.0
* Added correlation id middleware package. Logs and outgoing http requsts will now provide correlation id to make debugging easier.

---
 
## 9.0.1
* Fixed Bug 132060 : Added Migration to remove the 'Opening Date' field from projects. The 'ProposedAcademyOpeningDate' field will now be used throughout the api.
* Added migration to change projects with status of "APPROVED WITH CONDITIONS" to "Approved with conditions"

---
 
## 9.0.0
* Refactoring the command/command handlers to be consistent

---
 
## 8.0.0
* The term 'Involuntary Conversion' has been replaced with 'Sponsored Conversion'. This includes all Api endpoints. Note this release is required for the corresponding changes in the prepare-conversions application to work.
* User Story 129594 : The Local Authority and Region for a school is now passed up to the Academisation Api during involuntary project creation. This avoids the delay in populating the two values within the Academisation Api as they're already known during the conversion process.
* LA Dates can be set back to null
---


[9.46.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2026-01-19.560
[9.45.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2026-01-07.551
[9.44.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2026-01-06.548
[9.43.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2026-01-06.546
[9.42.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2026-01-06.545
[9.41.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-12-18.542
[9.40.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-12-04.531
[9.39.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-11-28.527
[9.38.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-08-13.520
[9.37.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-07-03.514
[9.36.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-06-13.503
[9.35.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-05-23.501
[9.34.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-04-28.494
[9.33.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2025-02-04.456
[9.32.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-11-06.412
[9.31.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-11-05.409
[9.30.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-10-09.391
[9.29.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-09-25.382
[9.28.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-09-06.365
[9.27.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-08-22.353
[9.26.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-07-15.334
[9.25.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-07-04.326
[9.24.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-06-24.301
[9.23.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-06-04.291
[9.22.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-20.278
[9.21.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-08.267
[9.20.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-02.263
[9.19.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-02.262
[9.18.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-02.259
[9.17.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-05-01.257
[9.16.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-04-23.245
[9.15.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-04-16.234
[9.14.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-04-15.229
[9.13.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-03-13.209
[9.12.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-03-07.201
[9.11.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-02-26.189
[9.10.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-02-21.183
[9.9.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-02-21.181
[9.8.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-01-30.166
[9.7.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2024-01-24.163
[9.6.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2023-12-15.150
[9.5.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2023-12-15.148
[9.4.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2023-12-12.145
[9.3.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2023-09-27.116
[9.2.0]: https://github.com/DFE-Digital/academies-academisation-api/releases/tag/production-2023-09-21.103


