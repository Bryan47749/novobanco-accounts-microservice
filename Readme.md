# NovoBanco API: Gestión de Cuentas y Transacciones

Solución backend desarrollada en .NET 8, diseñada para garantizar consistencia, trazabilidad y correctitud en operaciones financieras, siguiendo los lineamientos de modernización tecnológica de NovoBanco.

---

## Arquitectura

El sistema sigue los principios de Clean Architecture, permitiendo desacoplar la lógica de dominio de los detalles de infraestructura.

* API: Exposición de endpoints REST
* Application: Casos de uso y reglas de negocio
* Domain: Entidades (Account, Transaction) y validaciones críticas
* Infrastructure: Persistencia con PostgreSQL, ORM EF Core y manejo de transacciones

---

## Resolución de Escenarios Críticos

Cumpliendo con los escenarios de negocio definidos, se han tomado las siguientes decisiones:

| Escenario             | Estrategia de Solución                                   | Justificación                                                                                          |
| --------------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| Saldo Negativo        | CHECK constraint (DB) + validación en capa de aplicación | Se garantiza la integridad a nivel de base de datos y se mejora la experiencia con errores controlados |
| Cuenta Inactiva       | Validación en el caso de uso                             | Se evita operar sobre cuentas no activas con rechazo explícito                                         |
| Transferencia Parcial | Transacciones ACID                                       | Si alguna operación falla, se revierte completamente, asegurando consistencia                          |
| Concurrencia          | Bloqueo pesimista (SELECT FOR UPDATE)                    | Previene condiciones de carrera en operaciones simultáneas                                             |
| Idempotencia          | Restricción UNIQUE en columna reference                  | Evita duplicados ante reintentos de red                                                                |

---

## Arquitectura Decision Records (ADR)

### ADR 001: Selección de Bloqueo para Concurrencia

Contexto: Necesidad de prevenir saldo negativo ante retiros concurrentes.

Opciones consideradas:

* Bloqueo optimista (versionado)
* Bloqueo pesimista (SELECT FOR UPDATE)

Decisión: Bloqueo pesimista.

Consecuencias:

* Garantiza consistencia total en el saldo
* Evita condiciones de carrera
* Alineado a prácticas del sector financiero

---

### ADR 002: Atomicidad en Transferencias

Contexto: Garantizar que no exista pérdida de dinero en transferencias.

Opciones consideradas:

* Patrón Saga
* Transacción local en base de datos

Decisión: Transacción local (ACID).

Consecuencias:

* Operaciones all-or-nothing
* Mayor eficiencia al no requerir orquestación distribuida
* Suficiente para el alcance del sistema

---

## Guía de Ejecución

### 1. Requisitos

* Docker y Docker Compose
* .NET 8 SDK

---

### 2. Levantar entorno

```bash
cd docker
docker-compose up --build
```

Esto levantará:

* PostgreSQL
* API en http://localhost:5000

---

### 3. Ejecutar pruebas

```bash
dotnet test
```

---

## Datos iniciales para pruebas

### Customer

```
a7450ace-24fe-4979-ab7c-33fa53880b57
```

---

### Cuenta disponible

* AccountNumber: ACC-0001
* AccountId: 3e442e25-17f7-4b36-b5da-81f36786f49a

Ejemplo de creación:

```csharp
var account = new Account
{
    Id = Guid.NewGuid(),
    CustomerId = customer.Id,
    AccountNumber = "ACC-0001",
    Balance = 1000m,
    Currency = "USD",
    Type = AccountType.SAVINGS,
    Status = AccountStatus.ACTIVE,
    CreatedAt = DateTime.UtcNow
};
```

Esta cuenta puede utilizarse para pruebas de depósitos, retiros y transferencias.

---

## Endpoints principales

### Transferencia

```
POST /api/transactions/transfer
```

---

### Historial paginado

```
GET /api/transactions/account/{id}/transactions?page=1&pageSize=20
```

---

### Consultar saldo

```
GET /api/transactions/account/{id}/balance
```

---

### Depósito

```
POST /api/transactions/deposit
```

---

### Retiro

```
POST /api/transactions/withdraw
```

---

## Manejo de errores

La API utiliza un middleware global para manejo de errores.

| Escenario     | Código HTTP |
| ------------- | ----------- |
| Validación    | 400         |
| Duplicados    | 409         |
| No encontrado | 404         |
| Error interno | 500         |

---

## Cumplimiento de requerimientos

* SGBD: PostgreSQL 16

  * Soporte ACID
  * Control de concurrencia MVCC
  * Integridad referencial

* Esquema:

  * Índices en account_number, reference y created_at
  * Optimización para consultas de historial y validación de duplicados

* CI/CD:

  * Pipeline configurado en `workflows/ci.yml`
  * Ejecuta build, tests y validación de contenedor

---

## Conclusión

La solución está diseñada para priorizar:

* consistencia de datos
* integridad transaccional
* trazabilidad de operaciones
* claridad arquitectónica

---

## Autor

Bryan Toalumbo Rodriguez
Enfoque: Calidad, consistencia y trazabilidad
