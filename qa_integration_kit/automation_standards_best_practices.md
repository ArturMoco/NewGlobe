# Automation Standards & Best Practices – NewGlobe

## Purpose
Ensure consistency and maintainability of automated tests across all products.

---

## Standards
- Use a clear folder structure (`tests/unit`, `tests/integration`, `tests/e2e`).
- Follow naming conventions (`test_<feature>_<scenario>`).
- Keep tests independent and idempotent.
- Use explicit waits and selectors to reduce flakiness in E2E tests.

## Best Practices
- Reuse code through helpers and fixtures.
- Keep test data separate from test logic.
- Integrate tests into CI/CD pipelines with approval gates.
- Review and refactor tests regularly.

---

*Author: Artur Felipe Albuquerque Portela – QA Engineer/SDET*
