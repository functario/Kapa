## Kapa workflow
1. Create a `Prototype` with some `Trait`.
1. Create a `CapabilityType` with some `Capability`:
    - Add `Mutation`
1. Create another `CapabilityType` with some `Capability`:
    - Add `Mutation` and `Requirement` that will be resolved by other `Capability`.
1. Create a `ScenarioBuilder` passing a list of included and excluded `Capability`.
1. Build the `ScenarioBuilder` to get a list of `Scenarios` 
   resolved by `Requirement` and `Mutation`.


## Todo

- Add `hints[]` for Outcome in case of failure.
- Add `Priority` on Capability to set a score on the scenarios.
- Add `Options` type to configure the dependency resolution of `Requirement` (by value or statement).


## Features

- Being able to test with the `Scenarios` by injecting controlled `Capability` during the build.
  This will allow to be able to simulate failures and `ContingencyPlans`.
- Being able to analyse `Scenario` failure by dependencies 
  (C failed because depends on B that failed because of A).
- Being able to run automated analysis on `Scenario` failure to investigate.
  For example, on CapabilityB failing, start different checks on the system (via logs, etc).
  Add new `Check(s)` each time a new type of failure is found. Eventually add a`ContingencyPlan`.