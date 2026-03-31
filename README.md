# MyWallpaperEngine

Gerenciador desktop de papéis de parede desenvolvido em C# (WPF). Construído com arquitetura MVVM para separar interface e lógica de forma limpa. Integra banco de dados SQLite via Entity Framework Core para persistência local e utiliza chamadas de baixo nível (P/Invoke) à API nativa do Windows para manipulação do sistema operacional.

## Funcionalidades Atuais (MVP)
- **Varredura de Diretórios:** Escaneia pastas locais importando imagens compatíveis.
- **Persistência de Dados:** Salva o histórico e o estado das imagens localmente para carregamento instantâneo.
- **Integração Nativa:** Altera o fundo de tela da Área de Trabalho do Windows em tempo real.

## Tecnologias e Arquitetura
- **Linguagem:** C# (.NET)
- **Interface:** WPF (Windows Presentation Foundation)
- **Padrão Estrutural:** MVVM (Model-View-ViewModel) usando `CommunityToolkit.Mvvm`
- **Banco de Dados:** SQLite com Entity Framework Core (Code-First)
- **Integração OS:** P/Invoke (Win32 API - `user32.dll`)

## Como Executar o Projeto
1. Clone o repositório na sua máquina.
2. Abra a solução (`MyWallpaperEngine.sln`) no Visual Studio.
3. Abra o Console do Gerenciador de Pacotes e rode o comando `Update-Database` para gerar o banco de dados SQLite local.
4. Compile e execute o projeto (F5).