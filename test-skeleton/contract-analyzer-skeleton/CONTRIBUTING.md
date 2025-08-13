# Contributing

- Keep the skeleton minimal and readable.
- Any new LLM/GPT providers must implement `IGptClient` and be injected via `Program.cs`.
- Do not commit secrets. Use CI variables/secrets.
- Keep `samples/` diffs small and stable.
