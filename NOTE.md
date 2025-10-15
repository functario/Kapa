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


## ScenarioBuilder

A scenario is a sequence of Steps (wrapping Capabilities) realised by one or many Actors in concurrence.
Step.Capabilities can change the Actor properties states (called State).
All Capability returns an IOutcome with a OutcomeStatus (ok, fail, etc)
Step.Capabilities and Actor.States are owned by one Actor. For example the Actor1.LoginCapability change the Actor1.IsLoggedState, but it will not change the Actor2.IsLoggedState.
Still the sequence of Step.Capabilities of each actor is synchronise by events. For Example, Actor2.GiveHelpCapability can only happen once the Actor1.AskForHelpCapability has been run and HelpAskedActorEvent raised.
There can be no deadlock (exception is raised in deadlock detection).

A Scenario is build from a ScenarioBuilder in a fluent BBD style. The 'And' depends of the previous keyword (Given, When, Then).


## Example





!!! Capability ne sont pas attaché à un Actor donc "x => x.LoginCapability()" ne marchera pas a moins de faire une method d'extension.
Voir si il n'y a pas un autre type d'écriture. Probablement par:
Given(user, (AuthenticationCapabilities c) => c.LoginCapability("user@email.com", "Password1"))
ou 
Given<AuthenticationCapabilities>(user, c => c.LoginCapability("user@email.com", "Password1")) // This will have advantage of scoping where is the capability

- Aussi pour avoir un AuthenticationCapabilities, il faut que l'object soit passer par DI 
ou factory dans le scenarioBuilder (public ScenarioBuilder(ICollection<ICapabilityType>))
- Peut on valider qu'une seule capability est appellé dans la lambda et qu'elle a l'attribu "Capability"?


**La meilleur option serait d'avoir des méthodes d'extension prenant le ScenarioBuilder et wrappant les méthodes.
Possible avec un source generator. 
**





!!! Ne pas oublier qu'il faut supporter la création de scénario par API!!!
La validation du scenarioBuild devrait se faire dans une méthode à part pour simplifier
l'implementation API en ayant la même validation (l'API ne ferait que construire un scenarioBuild depuis un json body)



// Note: Cette écriture est explicite. Si une capability est manquante cela fera une erreur sur le scenario.Validate().
// Le dev doit faire appel au graph avant pour voir quelles sont les routes possibles.

```csharp
// The actors
var user = new User();
var admin = new Admin();
var scenario = ScenarioBuilder
		.AddActor(user)
		.WithSetup(new UserSetupCability()) // Setup capability defined the State and initial Mutations.
		.AddActor(admin)
		.WithSetup(new AdminSetupCability())
		.Given( // Both Steps are done in concurrence (not sequencially)
			{ user, x => x.LoginCapability("user@email.com", "Password1") },
			{ admin, x => x.LoginCapability("admin@email.com", "PasswordAdmin") }
		)
		.And(user, x => x.HasAProblem(true)) // Will change the User.HasAProblem state to true
		.When<HelpAskedActorEvent>(user, x => x.AskForHelpCapability(), [admin]) // will raise event HelpAskedActorEvent to admin (will not wait for answer from Admin to avoid deadlock). Note that AskForHelpCapability has a Requirement on Mutation "HelpAsked", provided by HasAProblem**
		.And<HelpAskedActorEvent>(admin, (x, e) => x.Help()) // Can only help once HelpAskedActorEvent is sent to Admin.DispatchEvents().
		.Then(user, x => x.HasAProblem(false)) // Will change the User.HasAProblem state to false
		.Build();

// The scenario.Validate() will returns Errors if:
// - there are any Capability Requirements not resolved by Mutation.
// - An event is expected but no Actor raised it before.

var errors = scenario.Validate();
if (errors.Count() > 0)
{
    throw new Exception(errors);
}

var scenario = scenarioBuild.Scenario;

var outcomes = scenario.Run();
```

** Voir si c'est réalisable! Et comment visualiser cela. Est-ce que cela pourrait être un EventReceivedCapability qui serait silencieusement ajouter
par le builder sur le HasAProblem?


journey
    title Concurrent Users Example
    section Login
      User A logs in: 5: User A, User B
    section View Dashboard
      User A opens dashboard: 4: User A, User B