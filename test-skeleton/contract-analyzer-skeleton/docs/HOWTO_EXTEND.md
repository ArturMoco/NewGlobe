# How-To: Extend the Contract Analyzer

- Add real GPT provider under `Services/Gpt` implementing `IGptClient`.
- Customize risk heuristic in `ContractReportService`.
- Adjust CI gates in `ci/` to match your organization's policies.
- Keep sample diffs under `samples/` for regression tests.
