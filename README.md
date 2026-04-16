# farm-dashboard-api (Backend) 🌾

![Status](https://img.shields.io/badge/Status-Active--Development-green?style=for-the-badge)
[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/download)
[![Supabase](https://img.shields.io/badge/Database-Supabase-3ecf8e?logo=supabase)](https://supabase.com/)
[![Deployment](https://img.shields.io/badge/Deployment-Railway-0b0d0e?logo=railway)](https://railway.app/)

---

## 🇹🇭 Introduction
โปรเจกต์นี้เป็นการพัฒนา Full-Stack Application สำหรับจัดการข้อมูลฟาร์ม (Farm Dashboard) โดยแบ่งการทำงานออกเป็นระบบ API หลังบ้านด้วย **.NET 10** และหน้าบ้านด้วย **Next.js** เชื่อมต่อกับฐานข้อมูล **Supabase** 

---

## 🇺🇸 Project Overview
This is a Full-Stack Farm Dashboard application. The project features a robust **.NET 10 Web API** backend and a modern **Next.js** frontend, integrated with **Supabase (PostgreSQL)**.

---

## 🚧 Development Roadmap (แผนการดำเนินงาน)

### Phase 1: The "First Connection" (Current)
- [x] **Database:** Initialize Supabase with UUID-based schema.
- [x] **Backend:** Setup .NET 10 API and Swagger documentation.
- [x] **ORM Integration:** Implement Entity Framework Core with Npgsql.
- [ ] **Data Access:** Successfully fetched real-time data using EF Core.
- [ ] **End-to-End Test:** Verified connectivity via Swagger UI.

### Phase 2: Cloud & Public Access
- [ ] **Railway Deployment:** Host the .NET API on Railway for public access.
- [ ] **Public Testing:** Confirm the live API is reachable.

### Phase 3: Frontend Integration
- [ ] **Next.js Setup:** Initialize project with TypeScript and Tailwind CSS.
- [ ] **UI Implementation:** Integrate **Shadcn/UI** components.
- [ ] **Data Consumption:** Fetch and display the first transaction record on the dashboard.
- [ ] **Vercel Deployment:** Host the frontend on Vercel.

### Phase 4: Full Feature Parity
- [ ] Implement remaining API endpoints (GET ,POST , PUT, DELETE).
- [ ] Build comprehensive dashboard UI components.

---

## 📊 Database Schema (PostgreSQL)
*Designed for high security and performance.*

```sql
CREATE TABLE farm_dashboard_transaction_db (
    transaction_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    transaction_date DATE NOT NULL,
    type_code VARCHAR(10),       -- 'INCOME' / 'EXPENSE'
    category_code VARCHAR(10),   -- 'FEED', 'EQUIPMENT', 'EGG'
    amount NUMERIC(15, 2),
    description VARCHAR(100),
    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by VARCHAR(30)
);
