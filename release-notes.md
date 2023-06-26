## 9.0.1
* Bug 132060 Added migration to move OpeningDate field from projects
* Added migration to change projects with status of "APPROVED WITH CONDITIONS" to "Approved with conditions"

---
## 9.0.0
* Refactoring the command/command handlers to be consistent

## 8.0.0
* The term 'Involuntary Conversion' has been replaced with 'Sponsored Conversion'. This includes all Api endpoints. Note this release is required for the corresponding changes in the prepare-conversions application to work.
* User Story 129594 : The Local Authority and Region for a school is now passed up to the Academisation Api during involuntary project creation. This avoids the delay in populating the two values within the Academisation Api as they're already known during the conversion process.
* LA Dates can be set back to null
---