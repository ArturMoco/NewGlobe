
# Estratégia de Testes e Monitoramento – Feature Toggle API  

*Esta estratégia foi elaborada em alinhamento direto com as expectativas descritas na seção “Overview” do pré-trabalho (pre-work) e com o Framework de Integração de Produto. Ela vai além de transformar requisitos em scripts, cobrindo ambiguidades reais, prevenindo falhas e garantindo confiança em produção. Inclui medidas preventivas e reativas, testes funcionais e não funcionais, e o uso de agentes GPT como assistentes para acelerar tarefas repetitivas, sempre sob supervisão humana.*

---

## 1. Contexto e Objetivo

A **Feature Toggle API** é utilizada por praticamente todos os serviços e aplicações da organização, funcionando como um **ponto único de falha**.  
Qualquer indisponibilidade ou alteração incompatível pode afetar serviços críticos em produção e consumidores downstream.

**Objetivos principais:**

- Garantir **segurança e previsibilidade** nas entregas.  
- **Prevenir regressões** e alterações incompatíveis para consumidores.  
- Ganhar **agilidade e consistência** com agentes GPT como apoio.  
- **Transparência e rastreabilidade** para todos os envolvidos, mesmo fora da equipe de QA.  
- **Escalabilidade** para aplicar a mesma abordagem a outros produtos.

---

## 2. Sequência e Priorização

Execução seguindo a Pirâmide de Testes:

1. **Testes Unitários** – validar lógica e tipos em isolamento.  
2. **Testes de Integração** – validar endpoints reais, autenticação e persistência.  
3. **Testes de Contrato (CDC)** – identificar mudanças incompatíveis antes do merge.  
4. **Fluxos E2E Críticos** – validar cenários mais relevantes para o negócio.  
5. **Testes de Performance** – garantir SLAs de latência e throughput.  
6. **Testes de Segurança** – validar contra OWASP Top 10.  
7. **Smoke Tests em Produção** – verificação rápida pós-deploy.  
8. **Monitoramento Contínuo** – detectar e agir sobre incidentes.

**Critérios de priorização:** risco + impacto:
- Alta prioridade: fluxos críticos, áreas de alta frequência de mudança ou histórico de falhas.  
- Média: fluxos secundários de uso moderado.  
- Baixa: funcionalidades raramente utilizadas.

---

## 3. Pirâmide de Testes + Agentes GPT

| Camada de Teste    | Objetivo                                         | Ferramentas                          | Gatilho/Etapa                              | Agente GPT (Função)                                              | Saída do Agente                  | Critérios de Aprovação |
| ------------------ | ------------------------------------------------ | ------------------------------------- | -------------------------------------------| ---------------------------------------------------------------- | --------------------------------- | ---------------------- |
| **Unitário**       | Validar regras e tipos                           | xUnit (.NET), FluentAssertions        | Build CI – Push/PR                         | **Gerador de Testes** – cria stubs unitários                      | Stubs C#                          | Cobertura mínima e verde |
| **Integração**     | Validar endpoints e autenticação (Basic/OAuth)   | HttpClient (.NET), Postman+Newman     | Build CI – Pós-unitário                    | **Gerador de Testes** – cria stubs de integração                  | Stubs C# / coleção Postman        | Todos passando          |
| **Contrato (CDC)** | Evitar mudanças incompatíveis multi-consumidor  | OpenAPI Diff (`oasdiff`), Pact        | Validação PR – alteração no openapi.json   | **Guardião de Contrato** – valida alterações, classifica riscos   | Relatório + Pact por consumidor   | Sem breaking sem mitigação |
| **E2E Crítico**    | Validar UI↔API e App VueJS                       | Playwright .NET / Selenium C#         | Staging – deploy + seeds                   | **Consultor de Estabilidade E2E** – sugere melhorias de locators  | Recomendações de ajustes          | Flakiness < alvo         |
| **Performance**    | Garantir latência/throughput                     | k6                                    | Staging – pós-E2E                           | **Analista de Relatórios** – sumariza resultados                  | Sumário executivo + ações         | P95/erros dentro do SLA  |
| **Segurança**      | Cobrir OWASP Top 10                              | OWASP ZAP                             | Staging – pós-E2E                           | **Analista de Relatórios** – prioriza riscos                      | Sumário executivo + prioridades   | Sem findings críticos    |
| **Smoke Prod**     | Checar API e UI após deploy                      | HttpClient (.NET)                     | Produção – pós-deploy                       | *(sem agente)*                                                 | —                                 | Todos verdes            |
| **Monitoramento**  | Detectar incidentes                              | Healthchecks, Logs, APM               | Produção – execução                         | **Analista de Monitoramento** – interpreta métricas e sugere ação | Plano inicial de ação              | MTTR no alvo             |

---

## 4. Cobertura de Domínio Específica

- **Variações e Alvos:** criar matriz de testes para `Environment`, `Country` e `AcademyId`, cobrindo conflitos e prioridades de resolução.  
- **Tipos de Valor:** incluir casos para boolean, inteiro e JSON (validar esquema).  
- **Autenticação:** testar fluxos com Basic Auth e OAuth (credenciais válidas/expiradas, escopos).  
- **UI VueJS:** smoke test leve de login, listagem de projetos, alteração de toggle e verificação na API.

---

## 5. Gestão de Ambientes e Promoção

- **DEV:** testes rápidos, feedback inicial.  
- **TEST:** integração completa, suites automatizadas.  
- **STAGING:** igual à produção, testes E2E, performance, segurança.  
- **PROD:** deploy canário + smoke tests.

**Gates de promoção:**  
- Sem breaking no CDC.  
- P95 < 500ms.  
- ZAP sem risco “High”.

---

## 6. Inicialização de Estado

- **Seeds idempotentes** para dados consistentes.  
- **Isolamento de execução** via header `X-Test-Run`.  
- **Cleanup seguro** apenas para dados de teste.

---

## 7. Pós-Deploy e Monitoramento Contínuo

- Healthchecks a cada 5 min em `/projects` e `/features/status`.  
- Alertas para:
  - P95 > 500ms.
  - Picos 4xx/5xx.
  - Quebra de contrato.  
- Runbooks com rollback canário.

---

## 8. Resiliência e Chaos Test

- Testar degradação do backend de toggles simulando latência e falhas.  
- Verificar fallback do consumidor e disparo correto de alertas.

---

## 9. O que Evitar

- E2E excessivos fora dos fluxos críticos.  
- Testes destrutivos em produção.  
- Dados sensíveis em ambientes não produtivos.  
- Over-mocking que esconde problemas reais.

---

## 10. Integração com o Framework de Produto

- **Passo 1:** Avaliar arquitetura, stack e maturidade de testes.  
- **Passo 2:** Mapear riscos e fluxos críticos.  
- **Passo 3:** Adaptar pirâmide e papéis de agentes GPT.  
- **Passo 4:** Integrar no CI/CD com gates e monitoramento.  
- **Passo 5:** Revisão contínua mensal.

---

## Sumário Executivo

Combina **boas práticas de QA** com **automação assistida por GPT** para:

- Criar testes rapidamente e de forma padronizada.  
- Reduzir risco de regressões e incidentes.  
- Dar visibilidade de qualidade a todo o time.  
- Escalar para outros produtos com mínimo ajuste.

O **QA Lead** mantém a decisão final, garantindo que automação **apoie, mas nunca substitua** o julgamento humano.
