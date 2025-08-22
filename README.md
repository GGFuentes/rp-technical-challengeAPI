# rp-tecnical-challenge

Este repositorio contiene el desarrollo de un **challenge técnico** diseñado para evaluar habilidades en programación, arquitectura de software y buenas prácticas de desarrollo.  

## 🚀 Tecnologías utilizadas
- C# .NET 8
- ASP.NET Core Web API
- Entity Framework Core o Dapper
- xUnit / NUnit / MSTest para pruebas unitarias
- Swagger / OpenAPI para documentación de endpoints

## 📂 Estructura del proyecto
```bash
rp-tecnical-challenge/
│── src/                # Código fuente principal
│   ├── Application/    # Casos de uso, servicios de aplicación
│   ├── Domain/         # Entidades de dominio y lógica de negocio
│   ├── Infrastructure/ # Persistencia, acceso a datos, servicios externos
│   └── API/            # Controladores y capa de exposición (Web API)
│
│── tests/              # Pruebas unitarias y de integración
│
│── docs/               # Documentación adicional
│── README.md           # Documentación principal
```

## ⚙️ Instalación y configuración

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/GGFuentes/rp-tecnical-challenge.git
   cd rp-tecnical-challenge
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Configurar variables de entorno** (por ejemplo, conexión a base de datos en `appsettings.json`):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ChallengeDB;User Id=sa;Password=your_password;"
     }
   }
   ```

4. **Ejecutar migraciones (si aplica)**
   ```bash
   dotnet ef database update
   ```

5. **Levantar el proyecto**
   ```bash
   dotnet run --project src/API
   ```

6. **Acceder a la API**
   - Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)  
   - API base: [http://localhost:5000/api](http://localhost:5000/api)  

## 🧪 Pruebas

Para ejecutar las pruebas:
```bash
dotnet test
```

## ✅ Funcionalidades implementadas
- CRUD de entidades principales (ejemplo: Usuario, Producto, etc.)  
- Validaciones con FluentValidation  
- Autenticación con JWT  
- Logging y manejo de errores centralizado  
- Pruebas unitarias con TDD  

## 📖 Convenciones y buenas prácticas
- Arquitectura basada en **Clean Architecture**  
- Principios **SOLID** y **DDD** aplicados  
- Desarrollo guiado por pruebas (**TDD**)  
- Uso de **Inyección de Dependencias (DI)**  

## 🤝 Contribuciones
Las contribuciones son bienvenidas. Por favor abre un **Pull Request** o un **Issue** si deseas sugerir cambios o mejoras.  

## 📜 Licencia
Este proyecto se distribuye bajo la licencia MIT.  
