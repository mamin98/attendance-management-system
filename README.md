# 🚀 Attendance Management System

> A full-stack Attendance Requests Management System built with **.NET 8 Clean Architecture + Angular + JWT Authentication + SQL Server**

This project demonstrates **real-world backend engineering practices**, scalable architecture, and clean code principles.

---

## 📌 Overview

The system allows employees to submit attendance-related requests (late, early leave, remote work, permission) and enables admins to review, approve, or reject them.

It focuses on:
- Clean Architecture
- Separation of concerns
- Scalability
- Maintainability
- Real-world HR workflow simulation

---

## 🏗️ Architecture


### 📦 Layer Responsibilities

| Layer | Responsibility |
|------|----------------|
| Domain | Core entities & business rules |
| Application | Use cases, DTOs, interfaces |
| Infrastructure | Database, EF Core, external services |
| API | Controllers, middleware, authentication |

---

## ⚙️ Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Clean Architecture
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger

### Frontend
- Angular 17+
- Angular Material
- SCSS
- RxJS

---

## 👤 Roles

### Employee
- Create attendance request
- View own requests
- Edit pending requests
- Cancel request

### Admin
- View all requests
- Filter & search
- Approve / Reject requests

---

## 📌 Request Types

- Late Arrival
- Early Leave
- Remote Work
- Permission

---

## 🔁 Workflow

---

## 🧱 Design Principles

- Clean Code & SOLID principles
- Repository & Service Pattern
- DTO-based architecture
- Soft Delete implementation
- Base Entity inheritance

---

## 🧩 Base Entity

All entities inherit from:

- Id
- CreatedAt
- CreatedBy
- LastModifiedAt
- LastModifiedBy
- IsDeleted

---

## 🔐 Security

- JWT Authentication
- Role-based Authorization
- Secure endpoints

---

## ⚡ Getting Started

### Backend
