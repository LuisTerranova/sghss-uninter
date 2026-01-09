
# 🏥 SGHSS - Sistema de Gestão Hospitalar e de Serviços de Saúde

![.NET 9](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)

O **SGHSS** é uma API RESTful desenvolvida como parte do Projeto Multidisciplinar da UNINTER. O sistema foca na gestão eficiente de unidades de saúde, implementando padrões modernos de segurança, performance e arquitetura limpa.

---

## 🚀 Diferenciais do Projeto

### 🔐 Autorização Granular e Proteção de Dados
O sistema implementa uma lógica de **Projeção de Dados Condicional** baseada em Roles (RBAC). Isso garante que dados sensíveis nunca saiam do banco de dados desnecessariamente.

- **Admin:** Acesso irrestrito às entidades completas para gestão.
- **Médico:** Acesso apenas a DTOs (Data Transfer Objects) com informações públicas de outros profissionais.

### ⚡ Performance (IQueryable & Paginação)
Utilização avançada do Entity Framework Core para realizar filtros, ordenação e paginação diretamente no banco de dados, resultando em:
- Menor tráfego de rede (Network payload reduzido).
- Menor uso de memória no servidor.
- Execução em uma única ida ao banco de dados (Single Database Round-trip).

---

## 🛠️ Stack Tecnológica

| Tecnologia | Descrição |
| :--- | :--- |
| **.NET 9** | Plataforma de desenvolvimento (Minimal APIs). |
| **EF Core** | ORM para acesso a dados. |
| **SQLITE** | Banco de dados relacional. |
| **ASP.NET Identity** | Gestão de usuários e Roles. |
| **Cookies** | Autenticação baseada em sessao |

---
