## NEXT
* Added ability to edit PFI schemes within a Transfer project for their requisite Transferring Academies
* Added Region and Local authority to the transfering academy record for filtering purposes
* Added ability to have 'DAO Revoked' decision recorded against a Sponsored conversion project
* Auto-Populate the `Proposed conversion date` when `Preferred conversion date` is set during conversion project creation for the school.
## 9.2.0
* Added transfer projects
* Added fields required to a Conversion Project to allow for sponsored grants

## 9.1.0
* Added correlation id middleware package. Logs and outgoing http requsts will now provide correlation id to make debugging easier.

## 9.0.1
* Fixed Bug 132060 : Added Migration to remove the 'Opening Date' field from projects. The 'ProposedAcademyOpeningDate' field will now be used throughout the api.
* Added migration to change projects with status of "APPROVED WITH CONDITIONS" to "Approved with conditions"

---
## 9.0.0
* Refactoring the command/command handlers to be consistent

## 8.0.0
* The term 'Involuntary Conversion' has been replaced with 'Sponsored Conversion'. This includes all Api endpoints. Note this release is required for the corresponding changes in the prepare-conversions application to work.
* User Story 129594 : The Local Authority and Region for a school is now passed up to the Academisation Api during involuntary project creation. This avoids the delay in populating the two values within the Academisation Api as they're already known during the conversion process.
* LA Dates can be set back to null
---