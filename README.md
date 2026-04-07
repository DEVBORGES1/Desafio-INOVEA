# Sistema Gerencial de Filmes (Kaggle Dataset)

Um portal administrativo moderno construído na robusta infraestrutura **ASP.NET WebForms + C#/VB.NET** focado em altíssima performance estrutural e experiência de usuário (UX). A aplicação eleva componentes clássicos à arquitetura "Enterprise Analytics".

## O Desafio

Este repositório atende a um robusto Desafio Técnico, centralizado em processar e manipular um _Dataset Real_ imenso da plataforma **Kaggle** (`Movies.csv`) contendo dezenas de milhares de registros, executando as engrenagens principais sob a suíte corporativa **Telerik UI for ASP.NET AJAX**.

## Stack Tecnológica

- **Banco de Dados Relacional:** Microsoft SQL Server (T-SQL)
- **Back-end:** VB.NET interagindo ativamente com ADO.NET Seguro (Blindado via Params)
- **Front-end UI:** Componentização Inteligente do 'Telerik RadGrid' envelopado sob CSS puro de _Dark Glassmorphism_
- **APIs de Terceiros:** Consumo Vanilla Javascript injetado de Front-end para a _OMDb (Open Movie Database)_

---

## Features Exclusivas e Resolução Avançada

- **Paginação Customizada Profissional (`OFFSET / FETCH`)**: Desabilitamos o cache na memória local da aplicação e ordenamos cálculos de "Pulo" nativamente dentro do banco de dados, aliviando mais de 99% da banda por clique.
- **Resiliência a Anomalias de Base de Dados (`TRY_CAST(id AS INT)`):** IDs gigantescos ou forçados em texto (comuns em DataSets impuros como o KAGGLE) que destruíam a indexação foram neutralizados de forma matemática via "Cast" nativo na requisição SQL.
- **Escudo Regex na Interceptação Booleana:** RadFilterExpressions baseados em colunas Checkbox explodiam instâncias SQL por enviar "True/False" literais do C#. Criamos Regex Expressions focadas para intervir no tempo de viagem e formatar o conteúdo para a gramática exigida pelo T-SQL `LIKE`.
- **API de Mídia Nativa (Fetch JS):** Roteamento mestre-detalhe onde uma segunda tela recebe o Pôster Oficial do registro acoplado consumindo um WebService externo sem onerar processos base da Microsoft.
- **Zero Falhas L10N (Internacionalização Pura):** Todas as instâncias da Telerik foram injetadas dinamicamente via Code-Behind `Culture="pt-BR"` garantindo consistência em Paginadores, Agrupamentos, e Sortings!

---

## Como Instalar e Rodar o Ambiente Local

1. **Ative a Ferramenta SQL Server Local e Suba o Banco:**
   Execute o `SQL Server Management Studio`. Crie um banco livre com o nome que desejar. Adicione o Kaggle `Movies.csv` como a sua fonte original gerando uma nova tabela nativa `Movies`. Marque seu `id` como _Primary Key_!
2. **Conecte com o Web.config:**
   No explorador do seu Windows, localize a raiz e edite a connection string em seu `Web.config`, apontando para onde sua máquina se abriga.
   _(Padrão Clássico: `Data Source=.\SQLEXPRESS;Initial Catalog=[SEUBANCO];Integrated Security=True`)_
3. **Compile em IIS:** Instancie as DLLs no seu ambiente Visual Studio ou rodelas em um `Local IIS`. A sua tela master é a `Projeto.aspx`.

---

_Status: `[✓] Completo e Aprovado`_
