# Dependency Inversion Principle (DIP) - Architecture Discussion

**Date:** January 30, 2026  
**Context:** Hexagonal Architecture Template Review

---

## Initial Assessment

### Current Structure Analysis

The hexarch solution implements hexagonal architecture with the following structure:

```
API/
├── API.Ports/ - Interfaces for driving the application (ISakService)
└── API.Adapters/ - HTTP controllers

Domain/
├── Domain.Data/ - Pure domain models (Sak, SakStatus)
└── Domain.Logic/ - Business logic (SakService)

Infrastructure/
├── Infrastructure.Ports/ - Interfaces for infrastructure (ISakRepository)
└── Infrastructure.Adapters/ - Database implementations
```

**Dependency Flow (Current):**

```
API.Adapters → Domain.Logic → Infrastructure.Ports
                           ↓
                Infrastructure.Adapters
```

---

## 🚨 Key Discovery: DIP Violation

### The Problem

**Question:** Does having Infrastructure.Ports adhere to DIP?

**Answer:** **NO - This violates DIP**

**Why it's wrong:**

- `Domain.Logic` depends on `Infrastructure.Ports` to get `ISakRepository`
- This means the **domain (high-level) depends on infrastructure (low-level)**
- Even though it's "just the ports," the **ownership is backwards**

### The DIP Principle

The Dependency Inversion Principle states:

1. High-level modules should not depend on low-level modules
2. **Abstractions should be owned by the client (consumer), not by the implementer**

**Current violation:**

```
Domain.Logic → Infrastructure.Ports (defines ISakRepository)
                      ↑
        Infrastructure.Adapters (implements ISakRepository)
```

The infrastructure layer "owns" the interface that the domain needs. This is backwards!

### The Correct DIP Structure

The interface should be **owned by the domain**:

```
Domain (defines ISakRepository)
   ↑
   └─── Infrastructure.Adapters (depends on Domain to implement ISakRepository)
```

**All dependency arrows should point toward the domain.**

---

## 🎯 Understanding APIs in Hexagonal Architecture

### The Dual Nature of APIs

APIs can be on **both sides** of hexagonal architecture:

#### **Driving Side (Primary/Left Side)**

- External actors that **invoke** your application
- Examples:
  - REST API endpoints you expose
  - GraphQL endpoints
  - Message queue consumers (listening)
  - CLI interfaces
  - WebSocket servers

#### **Driven Side (Secondary/Right Side)**

- External systems that your application **uses**
- Examples:
  - External REST APIs you call (HTTP clients)
  - Database repositories
  - Message queue producers (publishing)
  - Third-party services (Stripe, SendGrid)
  - File system operations

---

## ✅ Correct Organization with DIP

### Recommended Structure

```
Domain/ (OWNS ALL PORTS)
├── Domain.Data/ - Pure domain models (Sak, SakStatus)
├── Domain.Ports/ - ALL interfaces
│   ├── Driving/
│   │   └── ISakService.cs - "How to use the domain"
│   └── Driven/
│       ├── ISakRepository.cs - "What the domain needs"
│       └── IOrganizationApiClient.cs - "External API the domain needs"
└── Domain.Logic/
    └── SakService.cs - Implements ISakService, depends on ISakRepository

API.Adapters/ (Driving Adapters)
├── References: Domain.Ports, Domain.Logic
├── Controllers call ISakService
└── Translates HTTP ↔ Domain

Infrastructure.Adapters/ (Driven Adapters)
├── References: Domain.Ports
├── Implements ISakRepository, IOrganizationApiClient
└── Translates Domain ↔ Database/External APIs
```

**All dependency arrows point toward Domain:**

```
API.Adapters ────→ Domain ←──── Infrastructure.Adapters
```

### Key Principles

1. **Domain owns ALL ports**
   - Driving ports: "This is what I offer" (ISakService)
   - Driven ports: "This is what I need" (ISakRepository, IOrganizationApiClient)

2. **"API" can be either side**
   - Driving: HTTP REST API your app exposes → API.Adapters
   - Driven: External REST API your app calls → Infrastructure.Adapters

3. **Port naming reflects purpose, not technology**
   - ❌ Bad: `IPostgresRepository`, `IHttpExternalService`
   - ✅ Good: `ISakRepository`, `IOrganizationLookup`

4. **Adapters are technology-specific**
   - API.Adapters: ASP.NET Core specifics
   - Infrastructure.Adapters: EF Core, HttpClient, etc.
   - Domain: No technology dependencies

---

## 📋 Implementation Options

### Option 1: Move Infrastructure.Ports into Domain.Logic

- Move `ISakRepository` into the `Domain.Logic` project
- Infrastructure.Adapters depends on Domain.Logic to implement interfaces
- The domain fully owns its required abstractions
- Simpler structure

### Option 2: Create Domain.Ports Layer (Recommended)

- Create new project: `Domain.Ports`
- Organize as `Domain.Ports/Driving/` and `Domain.Ports/Driven/`
- Move `ISakService` from API.Ports → Domain.Ports/Driving/
- Move `ISakRepository` from Infrastructure.Ports → Domain.Ports/Driven/
- Keeps organizational separation while fixing dependency direction

---

## 🔑 Mental Model

### The Domain as the Center

```
    [HTTP Client]                [REST Controller]
    [DB Client]                  [gRPC Server]
    [Queue Publisher]            [Queue Consumer]
           ↓                            ↓
    Infrastructure.Adapters      API.Adapters
           ↓                            ↓
         IMPLEMENTS                   CALLS
           ↓                            ↓
    ┌─────────────────────────────────────┐
    │         DOMAIN (owns ports)         │
    │  ┌───────────────────────────────┐  │
    │  │ Driven Ports  │ Driving Ports │  │
    │  │ (what I need) │ (what I offer)│  │
    │  └───────────────────────────────┘  │
    │          Domain Logic                │
    └─────────────────────────────────────┘
```

**Everything points inward. Domain owns all interfaces. That's DIP.**

---

## 📊 Comparison Table

| Aspect               | Driving Side                        | Driven Side                                 |
| -------------------- | ----------------------------------- | ------------------------------------------- |
| **Direction**        | Calls INTO your app                 | Your app calls OUT                          |
| **Port Owner**       | Domain (ISakService)                | Domain (ISakRepository, IExternalApiClient) |
| **Port Location**    | Domain.Ports/Driving/               | Domain.Ports/Driven/                        |
| **Adapter Location** | API.Adapters, Messaging.Adapters    | Infrastructure.Adapters                     |
| **Adapter Role**     | Calls domain ports                  | Implements domain ports                     |
| **HTTP Example**     | REST controller (exposes endpoints) | HTTP client (calls external API)            |
| **Dependency**       | Adapter → Domain                    | Adapter → Domain                            |
| **Domain Knowledge** | None (doesn't know how it's called) | None (doesn't know implementations)         |

---

## 💡 Real-World Examples

### Example 1: External API Call (Driven)

```csharp
// Domain.Ports/Driven/IOrganizationApiClient.cs (Domain owns this)
public interface IOrganizationApiClient
{
    Task<OrganizationDetails> GetOrganization(string orgNumber);
}

// Domain.Logic/SakService.cs (Domain uses this)
internal class SakService(
    ISakRepository sakRepository,
    IOrganizationApiClient orgClient) : ISakService
{
    public async Task<Sak> CreateNewSak(CreateSakDto dto)
    {
        var org = await orgClient.GetOrganization(dto.OrgNumber);
        // Use org details...
    }
}

// Infrastructure.Adapters/ExternalApis/OrganizationApiClient.cs (Adapter implements)
internal class OrganizationApiClient(HttpClient httpClient) : IOrganizationApiClient
{
    public async Task<OrganizationDetails> GetOrganization(string orgNumber)
    {
        var response = await httpClient.GetAsync($"/api/org/{orgNumber}");
        // Parse and return
    }
}
```

**This is driven/secondary even though it's an "API" - your app is USING it, not EXPOSING it.**

### Example 2: Message Queue (Both Sides)

**Consuming messages (Driving):**

```
QueueMessage → MessageAdapter → ISakService → Domain Logic
```

**Publishing messages (Driven):**

```
Domain Logic → IEventPublisher → QueueAdapter → Message Queue
```

---

## 🎯 Action Plan for Refactoring

### Phase 1: Restructure Port Ownership

1. Create `Domain.Ports` project
2. Create `Domain.Ports/Driving/` and `Domain.Ports/Driven/` folders
3. Move `ISakService` from `API.Ports` → `Domain.Ports/Driving/`
4. Move `ISakRepository`, `IDatabaseMigrationService` from `Infrastructure.Ports` → `Domain.Ports/Driven/`
5. Delete old `API.Ports` and `Infrastructure.Ports` projects

### Phase 2: Update Project References

1. Update `Domain.Logic.csproj` to reference `Domain.Ports`
2. Update `API.Adapters.csproj` to reference `Domain.Ports`
3. Update `Infrastructure.Adapters.csproj` to reference `Domain.Ports`
4. Remove references to old port projects

### Phase 3: Update Namespaces

1. Update all port interfaces to use `Domain.Ports.Driving` or `Domain.Ports.Driven` namespace
2. Update using statements in all projects
3. Update ArchUnit tests to reflect new structure

### Phase 4: Update ArchUnit Tests

1. Add explicit DIP validation tests
2. Verify all adapters depend on domain, not vice versa
3. Ensure port interfaces don't reference adapter types

### Phase 5: Documentation

1. Update README.md with DIP explanation
2. Add inline documentation to port interfaces
3. Document the dependency flow in architecture diagrams

---

## 📝 Key Takeaways

1. ✅ **Current structure is close but has a critical flaw**: Infrastructure.Ports violates DIP
2. ✅ **The fix is conceptually simple**: Move port ownership to the domain
3. ✅ **"API" is ambiguous**: Can be driving (exposing) or driven (calling)
4. ✅ **Domain must own all abstractions**: Both what it offers and what it needs
5. ✅ **All dependencies point inward**: Adapters depend on domain, never the reverse

---

---

## 🎭 Mock Roles, Not Objects

### The Principle (Freeman & Pryce)

From "Growing Object-Oriented Software, Guided by Tests":

**"Mock Roles, Not Objects"** means that interfaces should be designed around the **role** a collaborator plays in the system, not the object's type or implementation.

**Key insight:** Name interfaces using **verbs that describe behavior**, not nouns that describe objects.

### Role-Based vs Object-Based Naming

#### ❌ Object-Based (Current)

```csharp
// Focuses on WHAT the object IS (noun)
public interface ISakRepository
{
    Task<Sak> PersistSak(string organisajonsnummer);
    Task<Sak?> GetSak(Guid id);
    Task<IEnumerable<Sak>> GetSaker();
    Task<Sak?> UpdateSakStatus(Guid id, SakStatus status);
}

public interface IOrganizationApiClient
{
    Task<OrganizationDetails> GetOrganization(string orgNumber);
}
```

**Problems:**

- Name reveals implementation detail ("Repository", "ApiClient")
- Doesn't communicate the role in the domain conversation
- Focuses on the object type rather than the responsibility

#### ✅ Role-Based (Improved)

```csharp
// Focuses on WHAT the object DOES (verb/role)
public interface IStoreSaker
{
    Task<Sak> Store(string organisajonsnummer);
}

public interface IRetrieveSaker
{
    Task<Sak?> FindById(Guid id);
    Task<IEnumerable<Sak>> FindAll();
}

public interface IUpdateSakStatus
{
    Task<Sak?> UpdateStatus(Guid id, SakStatus status);
}

public interface ILookupOrganization
{
    Task<OrganizationDetails> Lookup(string orgNumber);
}
```

**Benefits:**

- **Communicates intent** - "I need something that can lookup organizations"
- **Technology-agnostic** - No mention of "database", "API", "HTTP"
- **Single Responsibility** - Each interface has one clear role
- **Testable** - Easy to understand what behavior to mock
- **Business-aligned** - Uses domain vocabulary, not technical jargon

### How It Works with Hexagonal Architecture

Ports should express **roles in the domain conversation**:

```csharp
// Domain.Logic/SakService.cs
internal class SakService(
    IStoreSaker sakStorer,
    IRetrieveSaker sakRetriever,
    IUpdateSakStatus statusUpdater,
    ILookupOrganization orgLookup) : ICoordinateSakOperations
{
    public async Task<Sak> CreateNewSak(CreateSakDto dto)
    {
        // Read this as a conversation:
        // "I need to LOOKUP an organization..."
        var org = await orgLookup.Lookup(dto.Organisajonsnummer);

        // "...then STORE a new sak"
        return await sakStorer.Store(dto.Organisajonsnummer);
    }

    public async Task<Sak> ArchiveSak(Guid sakId)
    {
        // "I need to UPDATE the status of a sak"
        return await statusUpdater.UpdateStatus(sakId, SakStatus.Archived)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> GetSakById(Guid sakId)
    {
        // "I need to RETRIEVE a sak"
        return await sakRetriever.FindById(sakId)
            ?? throw new SakNotFoundException(sakId);
    }
}
```

**Notice:** Reading the code tells you about the **business operations**, not technical details.

### Naming Guidelines for Ports

#### Driven Ports (What Domain Needs)

Use **verbs** that describe the action from the domain's perspective:

| ❌ Object-Based          | ✅ Role-Based                           |
| ------------------------ | --------------------------------------- |
| `ISakRepository`         | `IStoreSaker`, `IRetrieveSaker`         |
| `IEmailService`          | `ISendNotifications`                    |
| `IOrganizationApiClient` | `ILookupOrganization`                   |
| `IFileStorage`           | `IStoreDocuments`, `IRetrieveDocuments` |
| `IEventPublisher`        | `IPublishDomainEvents`                  |
| `IPaymentGateway`        | `IProcessPayments`                      |

#### Driving Ports (What Domain Offers)

Can use **nouns** for business capabilities, or **verbs** for coordinators:

| ❌ Generic      | ✅ Role-Based                                  |
| --------------- | ---------------------------------------------- |
| `ISakService`   | `ICoordinateSakOperations` or `ISakOperations` |
| `IOrderService` | `IProcessOrders` or `IOrderProcessing`         |
| `IUserService`  | `IManageUsers` or `IUserManagement`            |

### Example: Refactoring Current Design

#### Current Design

```csharp
// Domain.Ports/Driven/ISakRepository.cs
public interface ISakRepository
{
    Task<Sak> PersistSak(string organisajonsnummer);
    Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus);
    Task<Sak?> GetSak(Guid? id);
    Task<IEnumerable<Sak>> GetSaker();
}
```

**Issues:**

- "Repository" is a technical pattern name
- Interface does multiple things (CRUD)
- Doesn't express domain roles

#### Role-Based Design

```csharp
// Domain.Ports/Driven/IStoreSaker.cs
/// <summary>
/// Role: Persistently store new saker in the system
/// </summary>
public interface IStoreSaker
{
    Task<Sak> Store(string organisajonsnummer);
}

// Domain.Ports/Driven/IRetrieveSaker.cs
/// <summary>
/// Role: Find and retrieve existing saker
/// </summary>
public interface IRetrieveSaker
{
    Task<Sak?> FindById(Guid id);
    Task<IEnumerable<Sak>> FindAll();
}

// Domain.Ports/Driven/IUpdateSakStatus.cs
/// <summary>
/// Role: Change the status of an existing sak
/// </summary>
public interface IUpdateSakStatus
{
    Task<Sak?> UpdateStatus(Guid id, SakStatus newStatus);
}
```

**Adapter can implement multiple roles:**

```csharp
// Infrastructure.Adapters/Db/SakRepository.cs
internal class SakRepository(SakDbContext context)
    : IStoreSaker,
      IRetrieveSaker,
      IUpdateSakStatus
{
    public async Task<Sak> Store(string organisajonsnummer)
    {
        // EF Core implementation
    }

    public async Task<Sak?> FindById(Guid id)
    {
        // EF Core implementation
    }

    public async Task<IEnumerable<Sak>> FindAll()
    {
        // EF Core implementation
    }

    public async Task<Sak?> UpdateStatus(Guid id, SakStatus newStatus)
    {
        // EF Core implementation
    }
}
```

### Benefits for Testing

When mocking, you mock the **behavior role**, not the object:

```csharp
// ❌ Object-based mocking
var mockRepository = new Mock<ISakRepository>();
// What does this mock? Storage? Retrieval? Both?

// ✅ Role-based mocking
var mockStorer = new Mock<IStoreSaker>();
var mockRetriever = new Mock<IRetrieveSaker>();
// Clear: This mock STORES, that mock RETRIEVES
```

**Test reads like a specification:**

```csharp
[Fact]
public async Task CreateNewSak_UsesOrganizationLookup_AndStoresSak()
{
    // Arrange - Set up the ROLES
    var orgLookup = new Mock<ILookupOrganization>();
    var sakStorer = new Mock<IStoreSaker>();

    orgLookup
        .Setup(x => x.Lookup("123456789"))
        .ReturnsAsync(new OrganizationDetails { Name = "Test Org" });

    sakStorer
        .Setup(x => x.Store("123456789"))
        .ReturnsAsync(new Sak { Id = Guid.NewGuid() });

    var service = new SakService(sakStorer.Object, ..., orgLookup.Object);

    // Act & Assert
    // Test communicates: "Service needs to LOOKUP org and STORE sak"
}
```

### Impact on Architecture

**Role-based ports clarify dependencies:**

```
SakService needs:
  - Something that can LOOKUP organizations (ILookupOrganization)
  - Something that can STORE saker (IStoreSaker)
  - Something that can UPDATE statuses (IUpdateSakStatus)

Infrastructure provides implementations:
  - OrganizationApiClient plays the role of ILookupOrganization
  - SakRepository plays the roles of IStoreSaker, IRetrieveSaker, IUpdateSakStatus
```

**This makes the domain conversation explicit and technology-agnostic.**

### When to Use Role-Based Naming

**Always for:**

- Driven ports (domain dependencies)
- Interfaces that cross architectural boundaries
- Collaborators that represent business capabilities

**Optional for:**

- Internal domain services
- Pure data structures
- Framework/library integrations where conventional names are clearer

---

## 🔗 References

- **Dependency Inversion Principle** - Robert C. Martin (SOLID Principles)
- **Hexagonal Architecture** - Alistair Cockburn
- **Ports and Adapters** - Pattern for architectural boundaries
- **Growing Object-Oriented Software, Guided by Tests** - Steve Freeman & Nat Pryce (Mock Roles, Not Objects)

---

## Next Steps

- [ ] Decide on implementation approach (Option 1 or 2)
- [ ] Create Domain.Ports structure
- [ ] Migrate interfaces
- [ ] Update project references
- [ ] Update ArchUnit tests
- [ ] Update documentation
- [ ] Verify all tests pass

---

**Status:** Architecture discussion complete. Ready for implementation.
