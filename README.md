# Thomas Elmore Submission


## Overview

My approach was:
 - Extract concrete data access / config as simply as possible
 - Cover existing service in tests
 - Refactor responsibilities to new collaborators along logical boundaries
 - Add test coverage for new collaborators
 - Refactor service tests to remove duplication and mock out new collaborators
 - Add helper factory to build the service since the constructor is now more complex
 
## Notes

 - There was a bug where the success state was never set so always defaulted to false. I fixed this and added test coverage.
 - There is an edge case when FasterPayments is used and both balance and payment amount are 0. This validates as true and so the account is updated unnecessarily.
 - I added a custom exception for the case when the requested payment scheme does not have a validator (eg the enum has been extended without extending the validation code). In the provided implementation this would have validated as successful and payment could have proceded. I think failing out is the correct thing to do since the situation cannot be handled.
 - The validators inherit from an abstract base class in order to share the null check. Normally I would "prefer composition over inheritance" but in this instance I think it gives safety to future extensions; the abstract method provides a template which guarantees the account is instantiated.
 - The service has its own null check since I feel that "no account found" is a different case to "account not in valid state" so the service should make that check itself (and log if it fails).
 - The duplication in null account checks is justified because the validators can now be re-used and so have some responsibility to their own correctness.
 
## Follow up work

If this was production code I would look into the following:

 - Logging / metrics : most importantly at the points where result is set to false.
 - Validation of account number before fetching the account from the store.
 - PaymentService tests could have the duplicate system-under-test construction extracted to a builder method to emphasise what is different.
 - Integration testing; it would be good to have a happy path integration test to show the parts working together.
 - Validation tests could have more test cases for multiple allowed payment schemes.