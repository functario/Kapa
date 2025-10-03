---
applyTo: "**/*.cs"
---

- Use `var` when the type is obvious from the right side of the assignment, otherwise use explicit types.
- Use expression-bodied members for simple properties and methods.
- Use ArgumentNullException.ThrowIfNull(argument) for argument validation.
- Split long method chains into multiple lines for better readability.
- Use 'is not null' instead of '!= null' for null checks.
- Prefer pattern matching over traditional type checks and casts.