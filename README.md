# farm-dashboard-api (Backend) 🌾

[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/download)
[![Database](https://img.shields.io/badge/Database-Supabase-3ecf8e?logo=supabase)](https://supabase.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

ระบบ API หลังบ้านสำหรับโครงการ **Farm Dashboard** พัฒนาด้วย .NET 10 เพื่อจัดการข้อมูลธุรกรรม (Transactions) ของฟาร์ม เชื่อมต่อกับฐานข้อมูลระบบคลาวด์ Supabase 

## ✨ Key Features
- **Transaction Management:** ดึงข้อมูลรายรับ-รายจ่ายของฟาร์ม
- **Secure Configuration:** ใช้ระบบ Environment Variables และ GitHub Secrets เพื่อป้องกันรหัสผ่านรั่วไหล
- **Interactive Documentation:** มาพร้อม Swagger UI สำหรับทดสอบ API ได้ทันทีผ่าน Browser
- **Modern Architecture:** ออกแบบโครงสร้างแบบแยกส่วน (Separation of Concerns) ด้วย Controllers และ Models

---

## 🛠 Tech Stack & Tools
- **Framework:** .NET 10.0 (ASP.NET Core Web API)
- **Database:** Supabase (PostgreSQL)
- **Documentation:** Swagger UI 
- **Environment:** GitHub Codespaces

---

## 🚀 Getting Started

### 1. Database Schema (Supabase)
Create SQL Editor:

```sql
-- Database Schema for Supabase
CREATE TABLE farm_dashboard_transaction_db (
    transaction_id UUID PRIMARY KEY DEFAULT gen_random_uuid(), 
    transaction_date DATE NOT NULL,
    type_code VARCHAR(10),       --  'INCOME', 'EXPENSE'
    category_code VARCHAR(10),   --  'FEED', 'EGG', 'TOOLS'
    amount NUMERIC(15, 2),
    description VARCHAR(100),
    created_at TIMESTAMPTZ DEFAULT NOW(),
    created_by VARCHAR(30),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    updated_by VARCHAR(30)
);

-- แนะนำ: เปิดใช้งาน RLS เพื่อให้ API ดึงข้อมูลได้
ALTER TABLE farm_dashboard_transaction_db ENABLE ROW LEVEL SECURITY;
CREATE POLICY "Allow Public Read" ON farm_dashboard_transaction_db FOR SELECT USING (true);

