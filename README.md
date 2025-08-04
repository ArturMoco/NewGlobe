# 🧪 Professional Test Guide — STLC | NewGlobe Interview

**Author:** Artur Felipe Albuquerque Portela  
**Position:** QA Engineer Candidate  
**Company:** NewGlobe (Remote - Amsterdam Team)  
**GitHub:** [github.com/ArturMoco](https://github.com/ArturMoco)

---

## ✅ Overview

This guide outlines my practical approach to the Software Testing Life Cycle (STLC), based on real-world experience with educational platforms and scalable software products. It reflects the quality standards expected by NewGlobe, with a focus on traceability, efficiency, and continuous improvement.

---

## 🔄 Software Testing Life Cycle (STLC)

### 1. Requirement Analysis

* Review of user stories, business rules, and critical mobile/web flows.
* Identification of risks: synchronization, offline mode, security, and accessibility.
* Tools used: Jira, Confluence, GitHub Projects.
* Occasional support from language-based tools to accelerate technical interpretation (\~20% average time saved).

### 2. Test Planning

* Strategic planning to cover functional, regression, integration, security, and scalability testing.
* Initial structure of reusable plans aided by supporting tools, resulting in \~30% time reduction during preparation.
* Environment segmentation, CI/CD integration, and monitoring defined early.

### 3. Test Case Design

* Writing of structured scenarios based on defined criteria, covering positive, negative, and exploratory tests.
* flow validation, usability, accessibility, and sync behavior.
* Clear organization by functionality and risk-based prioritization (impact × likelihood).
* Use of complementary tools to speed up repetitive tasks and improve consistency (\~25% time savings).  
* Inclusion of basic security validation patterns aligned with OWASP Top 10 risks, such as input validation and authentication checks.

### 4. Environment Setup

* Environment separation (QA, Dev, Staging) with flexible configuration using `.env` files.
* Support for both local and remote execution, leveraging Docker for realistic simulations.
* CI/CD pipelines via Jenkins and GitHub Actions.

### 5. Test Execution

* Manual testing
* Automated testing:

  * Cypress: E2E and regression coverage.
  * Selenium WebDriver: multi-browser and cross-device testing.
  * Postman/Newman: REST API validation with mocked data.
* Occasional use of technical tools to support debugging and data generation, reducing fix times by up to 30%.

### 6. Monitoring and Troubleshooting

* Continuous tracking of errors and failures during test runs.
* Cross-validation with logs, error messages, and alerting tools.
* Support for the development team in investigating environment-specific issues.

### 7. Test Closure and Reporting

* Final evaluation based on predefined KPIs (time, success rate, coverage).
* Organized documentation for release decisions and evidence handoff.
* Occasional writing support to improve structure and clarity of final reports (\~15–20% effort reduction).

---

## 📂 Project Structure

```
project-root/
├── cypress/
├── selenium/
├── api/
├── evidencias/
├── screenshots/
├── videos/
├── logs/
├── reports/
├── docker/
├── .env.qa
├── jenkinsfile
├── README.md
├── test-plan.md
├── test-cases.md
├── final-report.md
└── package.json
```

---

## 💻 Technologies & Tools

* Cypress, Selenium WebDriver, Postman, Newman
* Jenkins, GitHub Actions, Docker
* Allure Reports, automatic logging, `.env` configuration per environment
* Android emulators, OWASP Top 10 coverage, accessibility testing

---

*This guide reflects my hands-on QA experience applied to scalable educational platforms. It is adaptable to various systems and demonstrates measurable gains in both productivity and quality across the entire test process.*

*More than a fixed template, this guide reflects a QA approach that values clarity, continuous improvement, and the ability to learn quickly in the face of new technical challenges.*
