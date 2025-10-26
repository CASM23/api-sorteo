# 🎯 Prueba Técnica – SYC Sorteo

Aplicación FullStack desarrollada como prueba técnica para **Sistemas y Computadores S.A. (SyC)**.  
Permite que usuarios se inscriban a un sorteo y que un **administrador** gestione dichas inscripciones (ver, aceptar, rechazar y notificar por correo electrónico).

---

## 🧩 Tecnologías utilizadas

### 🔹 Backend (.NET 8 - C#)
- ASP.NET Core Web API  
- Entity Framework Core (Code First)  
- SQL Server  
- JWT Authentication  
- FluentValidation  
- MailKit / SMTP para envío de correos  
- Arquitectura limpia con **Domain / Application / Infrastructure / API Layers**

### 🔹 Frontend (React + TypeScript)
- React 18 con TypeScript  
- React Router DOM  
- React Query  
- Axios  
- TailwindCSS (paleta pastel/azul)  
- React Hook Form + Yup (validaciones)  
- Context API (manejo de sesión JWT)

---

## 🏗️ Arquitectura General

```bash
Syc-Sorteo/
├── backend/
│   ├── SyC.Sorteo.Api/             # API REST (controladores, configuración)
│   ├── SyC.Sorteo.Application/     # Casos de uso, servicios, validaciones
│   ├── SyC.Sorteo.Domain/          # Entidades y contratos (interfaces)
│   ├── SyC.Sorteo.Infrastructure/  # Persistencia, Repositorios, EmailService
│   └── SyC.Sorteo.sln
│
└── frontend/
    ├── src/
    │   ├── api.ts                  # Axios + endpoints
    │   ├── notification/           # Componente para notificaciones
    │   ├── provider/               # AuthProvider
    │   ├── hooks/                  # Custom hooks (useAuth, 
    │   ├── pages/                  # Login, Inscripción, Admin, Detalle
    │   ├── types/                  # Tipos TS
    │   └── App.tsx, main.tsx
    └── tailwind.config.js
```

### 🧠 Flujo de Datos (Backend)

1. **Usuario o Admin** realiza una solicitud HTTP → `Controller`
2. El controller delega en un **Servicio de Aplicación**
3. El servicio valida los datos con **FluentValidation**
4. El servicio llama a un **Repositorio** que usa EF Core
5. EF Core persiste datos en **SQL Server**
6. En caso de cambio de estado, se invoca `EmailService` para enviar correo
7. Respuesta JSON devuelta al frontend

---

## 🚀 Instalación y ejecución

### 🖥️ Backend (.NET API)

#### 1️⃣ Configurar Base de Datos
- Crear base de datos en SQL Server:  
  ```sql
  CREATE DATABASE SyC_Sorteo;
  ```
- En el archivo `appsettings.json` de `SyC.Sorteo.Api` actualizar:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SyC_Sorteo;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Smtp": {
    "Host": "sandbox.smtp.mailtrap.io",
    "Port": 587,
    "User": "TU_USER_MAILTRAP",
    "Pass": "TU_PASS_MAILTRAP",
    "From": "no-reply@syc.com"
  },
  "Jwt": {
    "Key": "ClaveUltraSecreta123!",
    "Issuer": "syc.sorteo",
    "Audience": "syc.users"
  }
  ```

#### 2️⃣ Ejecutar migraciones y seed
```bash
cd backend/SyC.Sorteo.Api
dotnet ef database update
dotnet run
```

- La API se levanta en:  
  👉 `https://localhost:5184/swagger/index.html`

#### 3️⃣ Usuario administrador creado automáticamente:
```bash
Usuario: admin
Clave: Admin123!
```

---

### 💻 Frontend (React + TS)

#### 1️⃣ Instalación
```bash
cd frontend
npm install
```

#### 2️⃣ Variables de entorno
Crear archivo `.env`:
```env
VITE_API_URL=http://localhost:5184/api
```

#### 3️⃣ Ejecutar
```bash
npm run dev
```

- Acceder a: 👉 [http://localhost:5173](http://localhost:5173)

---

## 📸 Funcionalidades

### 👤 **1. Login (Admin)**
- Endpoint: `/api/auth/login`
- Autenticación con JWT
- El token se guarda en `localStorage`
- Redirección al panel admin

### 📝 **2. Formulario de Inscripción**
- Cualquier usuario puede inscribirse
- Campos requeridos:
  - Tipo y número de documento
  - Nombres, fecha de nacimiento
  - Dirección, teléfono, correo
  - Subida de archivo (PDF o imagen)
- Validación completa en frontend + backend
- Al enviar, se guarda en la BD y se sube el archivo

### 📋 **3. Panel del Administrador**
- Requiere login con token JWT
- Muestra todas las inscripciones con:
  - Fecha de registro
  - Nombres y apellidos
- Cada inscripción es clickeable para ver detalle

### 🔍 **4. Detalle de Inscripción**
- Muestra toda la información
- Permite **Aceptar** o **Rechazar**
- Al cambiar estado, se envía correo al usuario

### 📧 **5. Notificación por correo**
- Implementado con `SmtpClient` (Mailtrap)
- Mensajes HTML personalizados

---

## 🎨 Diseño de la Interfaz

- Colores **pasteles suaves** (azul, blanco, gris claro)
- Estilo limpio y profesional
- Componentes con TailwindCSS y tarjetas minimalistas
- Layouts responsivos (flex y grid)

---

## 🔒 Seguridad Implementada

- Autenticación por **JWT** (Bearer Token)
- Autorización por roles (`AdminOnly`)
- Validaciones servidor y cliente
- CORS configurado
- Sanitización de datos
- HTTPS habilitado

---

## 🧪 Endpoints principales

| Método | Endpoint | Descripción |
|--------|-----------|-------------|
| `POST` | `/api/auth/login` | Autentica usuario admin |
| `GET` | `/api/admin/inscripciones` | Lista todas las inscripciones (admin) |
| `GET` | `/api/admin/inscripciones/{id}` | Obtiene detalle de inscripción |
| `PUT` | `/api/admin/inscripciones/{id}/estado` | Cambia estado y envía correo |
| `POST` | `/api/inscripciones` | Registro público de usuarios |

---

## 🧰 Mejores prácticas aplicadas

- Principios **SOLID**
- Separación por capas (Clean Architecture)
- Inyección de dependencias (DI)
- Validación con FluentValidation
- DTOs y mapeo entre capas
- Código limpio, nombrado descriptivo
- Manejo de errores global
- UI reactiva con React Query

---

## 👨‍💻 Autor

**💼 Cristian Sánchez (Casm)**  
Desarrollador Fullstack (React + .NET)  
📍 Santander, Colombia  
📧 cristian11969@gmail.com  
🔗 [GitHub](https://github.com/CASM23) | [LinkedIn](https://www.linkedin.com/in/cristhian-sanchez-1a321424a/)

---

## 📚 Conclusión

Este proyecto demuestra el flujo completo de una aplicación **FullStack moderna**, aplicando:
- Arquitectura limpia en backend,
- Buenas prácticas en React + TypeScript,
- Autenticación JWT,
- Upload de archivos,
- Notificación por correo,
- UI agradable y funcional.

