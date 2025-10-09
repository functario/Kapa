## Concept

Kapa is a framework to guide the implementation of a documented and self validating system,
built on top of functationalities from multiple domains.

Separating functationalities (`Capability`) from data (`Actor` and `State`). In a sens,
this is functional programming oriented with metadata that exposes the dependencies
to `Actors States`. 

This allows to compose a chain of functions from a graph. The dependencies in the chain can be validated before to be executed.
Chain can also be uses as an oracle to observe what will happen if the result from a Capability
was not `Ok` ('What if?'). Diagnostic can then be executed try to find the root cause of the problem.

## Weakness to try to resolve

1. When a Capability need multiple complex objects from from multiple Capabilities,
  the current mindset is to store this data in a Actor and expose via State.
  So there is duplication of object declaration like any DTO.
  The goal still remain to make visible only what is needed to make the system works.
  But it could remain a point of friction for adoption 
  (like who owns de Actors? What is the process to expose the need of information? But is different than today?
  Teams still need to talk when creating an API contract).
  It ask the developpers to think about the state of the system as a whole rather than individual components.

## Kapa workflow
1. Create a `Actor` with some `State`.
1. Create a `CapabilityType` with some `Capability`:
    - Add `Mutation`
1. Create another `CapabilityType` with some `Capability`:
    - Add `Mutation` and `Requirement` that will be resolved by other `Capability`.
1. Create a `ScenarioBuilder` passing a list of included and excluded `Capability`.
1. Build the `ScenarioBuilder` to get a list of `Scenarios` 
   resolved by `Requirement` and `Mutation`.


## User worflow

1. Get the full `Graph`.
1. Reduce the `Graph` with `IncludedNodes` and `ExcludedNodes`.

## Todo

- User actor pattern to change `Actor.State` (with logs of which method mutates, when, etc).
- How to expose some models from `State` that are collection? are they `Actor`?
- Validate if needs to support multiple statement in Requirement and Mutations:
  'x => x.Number > 0 && x.Number < 3' or 'x => x.Number > 0 and x.Boolean == true'.
  Or this should this 'x => x.Number > 0 && x.Number < 3' 
  should be 'x => x.NumberInRange == true'.
- Add `hints[]` for Outcome in case of failure (Or diagnostic could be enough?).


## Features

- Being able to test with the `Scenarios` by injecting controlled `Capability` during the build.
  This will allow to be able to simulate failures and `ContingencyPlans`.
- Being able to analyse `Scenario` failure by dependencies 
  (C failed because depends on B that failed because of A).
- Being able to run automated analysis on `Scenario` failure to investigate.
  For example, on CapabilityB failing, start different checks on the system (via logs, etc).
  Add new `Check(s)` each time a new type of failure is found. Eventually add a`ContingencyPlan`.
