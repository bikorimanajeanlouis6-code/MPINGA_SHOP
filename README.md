# MPINGA Shop - Enterprise Shop & Inventory Management System

![.NET 10.0](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue?style=flat-square)
![UI Ecosystem](https://img.shields.io/badge/UI-MudBlazor-7E57C2?style=flat-square)
![Database](https://img.shields.io/badge/Database-MS%20SQL%20Server-CC292B?style=flat-square&logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

A robust, enterprise-grade Shop and Inventory Management System designed for small-to-medium retail and agricultural businesses. Built with **ASP.NET Core Blazor Server** on **.NET 10.0** using **Clean Architecture** principles, the platform provides real-time stock tracking, automated valuation, sequential transaction counters, and financial reporting.

---

## 🌟 Key Features

### 📦 Inventory & Stock Control
* **Real-Time Stock Tracking:** Monitor stock quantities across custom units (e.g., `kg`, `units`) with automated low-stock and out-of-stock indicators.
* **Automated Stock Valuation:** Compute current inventory valuation based on available quantity and unit purchase price.
* **Sequential Reference Identifiers:** Auto-generated structured references (e.g., `ST2026001`) tracking items by transaction date and index.
* **Category & Master-Detail Management:** Organize items by business categories (e.g., `goat meat`, `inkoko`, `piza`).

### 💰 Sales & Financial Analytics
* **Sales Counter & POS Tracking:** Log line-item customer purchases seamlessly while syncing available stock quantities.
* **Profit & Loss Metrics:** Real-time visibility into selling price vs. purchase price margins.
* **Audit Trail & Date Logs:** Comprehensive timestamping (`Date Added`, modification history) for full operational traceability.

### 🖥️ Modern Enterprise UI
* **MudBlazor Integration:** Interactive, responsive dashboard featuring data grids, customizable dialog popups, date pickers, and dynamic color themes.
* **Cross-Device Ready:** Optimized UI layout for desktop counters, tablets, and POS devices.

---

## 🏗️Architecture & Technology Stack

The solution strictly adheres to **Clean Architecture** patterns to ensure testability, maintainability, and domain isolation.

* **Presentation Layer:** ASP.NET Core Blazor Server (.NET 10.0), MudBlazor UI Components
* **Application Layer:** CQRS / Service Contracts, DTO mapping, business validation logic
* **Domain Layer:** Entities, Value Objects, Domain Exceptions, Enum Status states
* **Infrastructure Layer:** Entity Framework Core (EF Core), Repository Pattern, Migrations
* **Database:** Microsoft SQL Server (Normalized relational schema with master-detail constraints)

---

 Getting Started

Prerequisites

Ensure you have the following installed on your developer workstation:
* [.NET 10.0 SDK](https://dotnet.microsoft.com/)
* [Visual Studio 2022 / 2026](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/) (Express or Developer Edition)
* [Git](https://git-scm.com/)

---
STRUCTURE OF SYSTEM
===================
MPINGA_SHOP/
├── src/
│   ├── Domain/                 # Core entities, Enums, Domain Logic
│   ├── Application/            # DTOs, Interfaces, Service Contracts
│   ├── Infrastructure/         # EF Core DbContext, Migrations, Repositories
│   └── Web/                    # Blazor Server UI Components, Pages, MudBlazor Setup
│       ├── Components/         # Dialogs, DataGrids, Custom Input Modals
│       ├── Pages/              # Inventory, Sales, Stock Ledger Views
│       ├── wwwroot/            # Static assets, CSS, images
│       └── appsettings.json    # Application configurations
└── MPINGA_SHOP.sln
