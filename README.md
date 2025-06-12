# 🚬 FirstSmoke - API Smoke Testing Framework

<div align="center">

![FirstSmoke](FirstSmoke.ico)

*Your First Choice for API Smoke Testing*

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download)

</div>

---

## 📖 Overview

FirstSmoke is a powerful and flexible API smoke testing framework designed to automate the testing of REST APIs. It provides a YAML-based configuration system that makes it easy to define and execute API test scenarios without writing code.

### ✨ Key Features

- 🎯 **Zero-Code Testing**: Define all test scenarios in YAML
- 🔄 **HTTP Methods**: Support for GET, POST, PUT, DELETE
- 📤 **File Upload**: Built-in file upload handling
- 🔑 **Token Management**: Automatic authentication handling
- 💾 **Variable Persistence**: Store and reuse variables between requests
- 📝 **Detailed Logging**: Comprehensive test execution logs
- 🐳 **Docker Ready**: Containerized for consistent execution

---

## 🎯 What is FirstSmoke?

FirstSmoke is a .NET-based smoke testing tool that allows you to:
- 📋 Define API test scenarios in YAML configuration files
- 🌐 Execute HTTP requests (GET, POST, PUT, DELETE)
- 📤 Handle file uploads
- 🔑 Manage authentication tokens
- 💾 Store and reuse variables between requests
- 📝 Generate detailed test execution logs

---

## 💪 Why FirstSmoke is a Good Solution

1. **🎯 Configuration-Driven**
   - No coding required to create test scenarios
   - Easy to maintain and modify
   - Version control friendly

2. **🔄 Flexible**
   - Supports various HTTP methods
   - Multiple authentication mechanisms
   - Customizable request/response handling

3. **📝 Maintainable**
   - YAML-based configuration
   - Clear and readable test scenarios
   - Easy to document

4. **🔌 Extensible**
   - Strategy pattern architecture
   - Easy to add new functionality
   - Plugin-friendly design

5. **💾 Variable Management**
   - Dynamic variable substitution
   - Persistent storage between runs
   - Context-aware variable handling

---

## 📋 YAML Configuration Guide

### Structure Overview

The `apis.yaml` file consists of three main sections:

1. **Variables**: Global variables for reuse across API calls
2. **TokenGenerators**: Authentication configurations
3. **APIs**: FHIR API endpoints to test

### 1. Variables Section
```yaml
Variables:
  MRN: "NS1027"
  AccessionNumber: "NS202506027"
  NPI: "2284932251"
```
- Define reusable variables here
- Use `{{VariableName}}` syntax in other sections
- Common variables include MRN, AccessionNumber, and NPI

### 2. TokenGenerators Section
```yaml
TokenGenerators:
- Name: "AuthToken"
  Type: "POST"
  Endpoint: "https://api.example.com/auth/token"
  Payload: >
    {
      "clientId": "your-client-id",
      "clientSecret": "your-client-secret",
      "audience": "your-audience"
    }
  ResponseTokenKey: "access_token"
```
- `Name`: Unique identifier for the token generator
- `Type`: HTTP method (usually POST)
- `Endpoint`: Token generation endpoint
- `Payload`: JSON payload for token request
- `ResponseTokenKey`: JSON path to extract token from response

### 3. APIs Section
```yaml
APIs:
- Name: "CreateOrder"
  Type: "POST"
  Endpoint: "https://api.example.com/fhir/Orders"
  IsTokenNeeded: true
  TokenGenerationAPIName: "AuthToken"
  ContentType: "application/fhir+json"
  Payload: >
    {
      "resourceType": "Bundle",
      "type": "collection",
      "entry": [
        {
          "resource": {
            "resourceType": "Practitioner",
            "id": "Pra1",
            "identifier": [
              {
                "type": {
                  "text": "NPI"
                },
                "value": "{{NPI}}"
              }
            ]
          }
        }
      ]
    }
```

#### API Configuration Fields
- `Name`: Unique identifier for the API
- `Type`: HTTP method (GET, POST, PUT, DELETE)
- `Endpoint`: API endpoint URL
- `IsTokenNeeded`: Whether authentication is required
- `TokenGenerationAPIName`: Reference to a TokenGenerator
- `ContentType`: Request content type (e.g., "application/fhir+json")
- `Payload`: FHIR Bundle or resource payload

### FHIR Resource Structure
The payload typically contains a FHIR Bundle with multiple resources:

1. **Practitioner Resources**:
   - Primary and secondary practitioners
   - NPI identifiers
   - Contact information

2. **Organization Resources**:
   - Healthcare organizations
   - Organization identifiers

3. **PractitionerRole Resources**:
   - Links practitioners to organizations
   - Role assignments

4. **Coverage Resources**:
   - Insurance information
   - Policy details
   - Payment methods

5. **Patient Resources**:
   - Patient demographics
   - Identifiers (MRN, etc.)
   - Contact information

6. **Specimen Resources**:
   - Collection details
   - Specimen types

7. **ProcedureRequest Resources**:
   - Test orders
   - Accession numbers
   - Test codes

### Variable Usage Examples
```yaml
# In API payloads
"value": "{{MRN}}"
"value": "{{AccessionNumber}}"
"value": "{{NPI}}"

# In endpoints
Endpoint: "https://api.example.com/fhir/orders/{{OrderId}}/update"
```

### Best Practices
1. **FHIR Bundle Organization**:
   - Group related resources together
   - Maintain proper resource references
   - Use consistent resource IDs

2. **Variable Management**:
   - Use meaningful variable names
   - Keep sensitive data in variables
   - Document variable purposes

3. **Resource References**:
   - Ensure proper resource linking
   - Use consistent reference formats
   - Validate reference integrity

4. **Security**:
   - Never hardcode sensitive data
   - Use token generators for authentication
   - Keep credentials in variables

---

## ⚙️ Configuration Guide

### 👩‍💻 For Testers

1. **🚀 Basic Setup**:
   ```bash
   # Download the latest release from the releases folder
   # Extract the release zip file to your desired location
   
   # Create a Configs folder in the same directory as FirstSmoke.exe
   mkdir Configs
   
   # Place your apis.yaml file in the Configs folder
   # Run FirstSmoke.exe
   FirstSmoke.exe
   ```
   
2. **🔑 Key Features**:
   - Multiple API endpoint testing
   - Status code validation
   - Response body validation
   - Variable substitution
   - Token management
   - File upload handling

3. **📁 Required Directory Structure**:
   ```
   FirstSmoke/
   ├── FirstSmoke.exe
   ├── FirstSmoke.dll
   ├── Configs/
   │   └── apis.yaml
   └── [other runtime files]
   ```

### 👨‍💻 For Developers

1. **📁 Project Structure**:
   ```
   FirstSmoke/
   ├── Services/          # Core functionality
   ├── Models/           # Data structures
   ├── Configs/          # YAML configurations
   ├── Decorators/       # Additional features
   └── Properties/       # Project properties
   ```

2. **🔌 Extending FirstSmoke**:
   ```csharp
   // Example: Custom HTTP Strategy
   public class CustomStrategy : IHttpStrategy
   {
       public async Task<HttpResponseMessage> Execute(ApiDefinition api)
       {
           // Your custom implementation
       }
   }
   ```

3. **🔄 Integration Example**:
   ```csharp
   var config = new DeserializerBuilder()
       .WithNamingConvention(PascalCaseNamingConvention.Instance)
       .Build()
       .Deserialize<AppConfig>(yaml);
   
   var executor = new ApiExecutor(tokenSvc, config.TokenGenerators, logger, varSvc);
   await executor.Execute(api);
   ```

---

## 🚀 Getting Started

1. **📋 Prerequisites**:
   - .NET 6.0 or later
   - Git

2. **⚙️ Installation**:
   ```bash
   # Clone the repository
   git clone [repository-url]
   
   # Build the project
   dotnet build
   
   # Run the tests
   dotnet run
   ```

---

## 📚 Best Practices

1. **⚙️ Configuration**:
   - 📁 Keep configurations modular
   - 🔤 Use meaningful variable names
   - 📝 Document complex scenarios
   - 🔄 Version control your configs

2. **🧪 Testing**:
   - 🎯 Start with basic smoke tests
   - 📈 Gradually add complexity
   - 🔄 Use variables for dynamic data
   - ✅ Validate success and error cases

3. **🔧 Maintenance**:
   - 📅 Regular configuration updates
   - 🧹 Clean up unused variables
   - 📊 Monitor execution logs
   - 🔄 Keep dependencies updated

---

## 🤝 Contributing

We welcome contributions to FirstSmoke! Here's how you can help:

1. 🐛 Report bugs and issues
2. 💡 Suggest new features
3. 🔧 Submit pull requests
4. 📝 Improve documentation
5. 🎯 Add test cases
6. 🔄 Review code

---

<div align="center">

Made with ❤️ by Aniruddha and Vedanshu

</div> 
