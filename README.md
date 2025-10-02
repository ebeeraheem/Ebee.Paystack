# Ebee.Paystack

A modern, comprehensive .NET SDK for integrating with the Paystack payment platform. Built with .NET 8 and following modern C# patterns.

## Features

- ? **Banks API**: List banks, resolve account numbers
- ? **Transactions API**: Initialize, verify, list, and fetch transactions  
- ? **Modern C# 12**: Uses latest language features and patterns
- ? **Dependency Injection**: First-class DI support with validation
- ? **Configuration Validation**: Validates configuration at startup
- ? **Strongly Typed**: Comprehensive request/response models
- ? **Error Handling**: Robust error handling with custom exceptions
- ? **Async/Await**: Full async support with cancellation tokens
- ? **Logging Support**: Optional request/response logging

## Installation

```bash
dotnet add package Ebee.Paystack
```

## Quick Start

### 1. Configuration

Add your Paystack configuration to `appsettings.json`:

```json
{
  "Paystack": {
    "SecretKey": "sk_test_your_secret_key_here",
    "BaseUrl": "https://api.paystack.co",
    "TimeoutInSeconds": 30,
    "EnableLogging": false
  }
}
```

### 2. Service Registration

In your `Program.cs` (.NET 8):

```csharp
using Ebee.Paystack.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register Paystack services
builder.Services.AddPaystack(builder.Configuration.GetSection("Paystack"));

var app = builder.Build();
```

### 3. Usage

Inject `IPaystackClient` into your services:

```csharp
public class PaymentService
{
    private readonly IPaystackClient _paystackClient;

    public PaymentService(IPaystackClient paystackClient)
    {
        _paystackClient = paystackClient;
    }

    public async Task<string> InitializePaymentAsync()
    {
        var request = new InitializeTransactionRequest
        {
            Email = "customer@example.com",
            Amount = 50000, // 500.00 NGN in kobo
            Currency = "NGN",
            CallbackUrl = "https://yoursite.com/payment/callback"
        };

        var response = await _paystackClient.Transactions.InitializeTransactionAsync(request);
        
        return response.Status ? response.Data!.AuthorizationUrl : throw new InvalidOperationException(response.Message);
    }
}
```

## Configuration Options

### Using appsettings.json

```json
{
  "Paystack": {
    "SecretKey": "sk_test_...",           // Required: Your Paystack secret key
    "BaseUrl": "https://api.paystack.co", // Optional: API base URL (defaults to production)
    "TimeoutInSeconds": 30,               // Optional: Request timeout (default: 30)
    "EnableLogging": false                // Optional: Enable HTTP logging (default: false)
  }
}
```

### Using Configuration Action

```csharp
builder.Services.AddPaystack(options =>
{
    options.SecretKey = "sk_test_your_secret_key_here";
    options.EnableLogging = true; // Enable for debugging
    options.TimeoutInSeconds = 60; // Increase timeout if needed
});
```

## API Usage Examples

### Banks API

#### List Available Banks

```csharp
// Get all Nigerian banks
var banksResponse = await _paystackClient.Banks.ListBanksAsync(country: "nigeria");

if (banksResponse.Status)
{
    foreach (var bank in banksResponse.Data!)
    {
        Console.WriteLine($"{bank.Name} - {bank.Code}");
    }
}
```

#### Resolve Account Number

```csharp
var request = new ResolveAccountRequest
{
    AccountNumber = "0022728151",
    BankCode = "063" // Diamond Bank
};

var response = await _paystackClient.Banks.ResolveAccountAsync(request);

if (response.Status)
{
    var account = response.Data!;
    Console.WriteLine($"Account Name: {account.AccountName}");
    Console.WriteLine($"Account Number: {account.AccountNumber}");
}
```

### Transactions API

#### Initialize Transaction

```csharp
var request = new InitializeTransactionRequest
{
    Email = "customer@email.com",
    Amount = 20000, // 200.00 NGN in kobo
    Currency = "NGN",
    CallbackUrl = "https://yourwebsite.com/payment/verify",
    Metadata = new Dictionary<string, object>
    {
        ["order_id"] = "12345",
        ["custom_field"] = "custom_value"
    }
};

var response = await _paystackClient.Transactions.InitializeTransactionAsync(request);

if (response.Status)
{
    // Redirect customer to response.Data!.AuthorizationUrl
    var authUrl = response.Data!.AuthorizationUrl;
    var reference = response.Data.Reference;
}
```

#### Verify Transaction

```csharp
var response = await _paystackClient.Transactions.VerifyTransactionAsync("T123456789");

if (response.Status && response.Data!.Status == "success")
{
    var transaction = response.Data;
    Console.WriteLine($"Payment successful!");
    Console.WriteLine($"Amount: {transaction.Amount / 100:C}"); // Convert from kobo
    Console.WriteLine($"Reference: {transaction.Reference}");
    Console.WriteLine($"Customer: {transaction.Customer?.Email}");
}
```

#### List Transactions with Filtering

```csharp
var request = new ListTransactionsRequest
{
    PerPage = 20,
    Status = "success",
    From = DateTime.Now.AddDays(-30),
    To = DateTime.Now,
    Customer = "customer@email.com"
};

var response = await _paystackClient.Transactions.ListTransactionsAsync(request);

if (response.Status)
{
    foreach (var transaction in response.Data!)
    {
        Console.WriteLine($"{transaction.Reference} - {transaction.Amount / 100:C} - {transaction.Status}");
    }
}
```

#### Fetch Specific Transaction

```csharp
var response = await _paystackClient.Transactions.FetchTransactionAsync(123456);

if (response.Status)
{
    var transaction = response.Data!;
    Console.WriteLine($"Status: {transaction.Status}");
    Console.WriteLine($"Gateway Response: {transaction.GatewayResponse}");
}
```

## Error Handling

The SDK uses custom exceptions for error scenarios:

```csharp
try
{
    var response = await _paystackClient.Transactions.VerifyTransactionAsync(reference);
    // Handle successful response
}
catch (PaystackException ex)
{
    // Handle Paystack-specific errors
    Console.WriteLine($"Paystack Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Response: {ex.ResponseContent}");
}
catch (ArgumentException ex)
{
    // Handle validation errors
    Console.WriteLine($"Validation Error: {ex.Message}");
}
```

## ASP.NET Core Integration

### Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaystackClient _paystackClient;

    public PaymentsController(IPaystackClient paystackClient)
    {
        _paystackClient = paystackClient;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromBody] InitializeTransactionRequest request)
    {
        try
        {
            var response = await _paystackClient.Transactions.InitializeTransactionAsync(request);
            
            if (response.Status)
            {
                return Ok(new 
                { 
                    AuthorizationUrl = response.Data!.AuthorizationUrl,
                    Reference = response.Data.Reference
                });
            }

            return BadRequest(new { Error = response.Message });
        }
        catch (PaystackException ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("verify/{reference}")]
    public async Task<IActionResult> Verify(string reference)
    {
        try
        {
            var response = await _paystackClient.Transactions.VerifyTransactionAsync(reference);
            
            if (response.Status)
            {
                return Ok(response.Data);
            }

            return BadRequest(new { Error = response.Message });
        }
        catch (PaystackException ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}
```

## Testing

The SDK is designed with testability in mind. You can easily mock the interfaces:

```csharp
// Mock the main client
var mockPaystackClient = new Mock<IPaystackClient>();
var mockTransactions = new Mock<ITransactionsClient>();

mockPaystackClient.Setup(x => x.Transactions).Returns(mockTransactions.Object);

// Setup transaction initialization
mockTransactions
    .Setup(x => x.InitializeTransactionAsync(It.IsAny<InitializeTransactionRequest>(), default))
    .ReturnsAsync(new PaystackResponse<InitializedTransaction>
    {
        Status = true,
        Data = new InitializedTransaction
        {
            AuthorizationUrl = "https://checkout.paystack.com/test123",
            Reference = "test_ref_123"
        }
    });
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Support

For support, please create an issue on the [GitHub repository](https://github.com/ebeeraheem/Ebee.Paystack).

## Acknowledgments

- Built for the [Paystack](https://paystack.com) payment platform
- Follows modern .NET development practices
- Inspired by the official Paystack API documentation