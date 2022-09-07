# Swagger Documentation and API Versioning

## What is Swagger ?

The evolution of API’s functionality is inevitable, but the headache of maintaining API docs doesn’t have to be. Swagger tools takes the hard work out of generating and maintaining API docs, ensuring documentation stays up-to-date as API evolves.

Swagger UI allows anyone — be it development team or end consumers — to visualize and interact with the API’s resources without having any of the implementation logic in place. It’s automatically generated from OpenAPI (formerly known as Swagger) Specification, with the visual documentation making it easy for back end implementation and client side consumption


![swagger_ui](/readme_assets/swagger_ui.png)


---------------

## About this exercise

In this lab we will be working on **Backend codebase** .

### **Backend Codebase:**

Previously we developed a base structure of an api solution in asp.net core that have just two api functions **GetLast12MonthBalances** & **GetLast12MonthBalances/{userId}** which returns data of the last 12 months total balances.

![apimethods](/readme_assets/apimethods.jpg)


There are 4 Projects in the solution. 

*	**Entities** : This project contains DB models like *User* where each User has one *Account* and each Account can have one or many *Transaction*. There is also a Response Model of *LineGraphData* that will be returned as API Response. 

*	**Infrastructure**: This project contains *BBBankContext* that serves as fake DBContext that populates one User with its corresponding Account that has some Transactions dated of last twelve months with hardcoded data. 

* **Services**: This project contains *TransactionService* with the logic of converting Transactions into LineGraphData after fetching them from BBBankContext.

* **BBBankAPI**: This project contains *TransactionController* with two GET methods *GetLast12MonthBalances* & *GetLast12MonthBalances/{userId}* to call the *TransactionService*.

![apiStructure](/readme_assets/apistructure.png)

For more details about this base project see: [Service Oriented Architecture Lab](https://github.com/PatternsTechGit/PT_ServiceOrientedArchitecture)

---------------
## In this exercise

* Package installation
* Add and configure swagger middleware
* API info and description
* Adding comments into swagger documentation
* API versioning

Here are the steps to begin with 

 ## Step 1: Install nuget packages 

Install following nuget package in API project *BBBankAPI* through nuget package manager to add swagger into application 

- Swashbuckle.AspNetCore 


You may install using package manager console as well, make sure to select *BBBankAPI* project in package manager console:

```cs
Install-Package Swashbuckle.AspNetCore
```

 ## Step 2: Add and configure Swagger middleware

 Add the Swagger generator to the services collection in `Program.cs`.

 ```csharp
builder.Services.AddSwaggerGen();
```

Enable the middleware for serving the generated JSON document and the Swagger UI, also in `Program.cs`.

 ```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
The above code adds the swagger middleware only if the current environment is set to development in `launchSettings.json`.

Launch the app and navigate to swagger UI by using this API URL http://localhost:5070/swagger

 ## Step 3: API info and description

The configuration action passed to the `AddSwaggerGen` method adds information such as the author, license, and description.

In `Program.cs`, import the following namespace to use the *OpenApiInfo* class:


```csharp
using Microsoft.OpenApi.Models;
```


Using the *OpenApiInfo* class, modify the information displayed in the UI by modifying the method added in **step 2**:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BBBank API",
        Description = "An ASP.NET Core Web API for a fictitious bank application",

    });
});
```
The Swagger UI displays the custom title and description.

![swagger-info](/readme_assets/swagger-info.png)

## Step 4: Adding comments into swagger documentation

C# source(code) files can have structured comments that produce API documentation for the types(classes) defined in those files. The C# compiler produces an XML file that contains structured data representing the comments and the API signatures. Tools like swagger can process that XML output to create human-readable documentation .

To create documentation for code by writing special comment fields indicated by triple slashes `///`. Visual Studio automatically inserts the `<summary>` and `</summary>` tags and positions your cursor within these tags after you type the `///` delimiter in the code editor then you can define methods or class functionality that would eventually be appear in swagger documentation.

Recommended XML tags for C# documentation comments can be found [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags)


### Step 4.1 Setup API project to show XML comments

To setup the API project to show XML comments in swagger UI we will do following

* Right-click the **BBBankAPI** project in Solution Explorer and select *Edit Project File*.
* Add **GenerateDocumentationFile** setting to the API project *BBBankAPI* .csproj file.

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```


### Step 4.2 Configure Swagger to use the XML file

Configure Swagger to use the XML file that's generated with the preceding instructions, modify code done in step 3 as:
Reflection is used to build an XML file name matching that of the web API project. The *AppContext.BaseDirectory* property is used to construct a path to the XML file.

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BBBank API",
        Description = "An ASP.NET Core Web API for a fictitious bank application",

    });


    // this code is required to add code comments in swagger.
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

});
``` 


Adding triple-slash comments to an action enhances the Swagger UI by adding the description to the section header. Add a `<summary>` element above the both **GetLast12MonthBalances** actions in **TransactionController**


```cs
/// <summary>
/// Get the last 12 month balances
/// </summary>
/// <returns>Returns last 12 months data</returns>
[HttpGet]
[Route("GetLast12MonthBalances")]
public async Task<ActionResult> GetLast12MonthBalances()
{

}


/// <summary>
/// Get the last 12 month balances for specific user
/// </summary>
/// <param name="userId">Id of the user</param>
/// <returns>Returns last 12 months data</returns>
[HttpGet]
[Route("GetLast12MonthBalances/{userId}")]
public async Task<ActionResult> GetLast12MonthBalances(string userId)
{

}
```

The Swagger UI displays the inner text of the preceding code's **summary** element:

![swagger-delete-summary.png](/readme_assets/swagger-delete-summary.png)


Adding a `<remarks>` element to the action method documentation, supplements information specified in the `<summary>` element and provides a more robust Swagger UI. The `<remarks>` element content can consist of text, JSON, or XML.

```cs
/// <summary>
/// Get the last 12 month balances for specific user
/// </summary>
/// <param name="userId">Id of the user</param>
/// <returns>Returns last 12 months data</returns>
/// <remarks>
/// Sample request:
///
///     Get /GetLast12MonthBalances
///     {
///        "userId": "0ea07fd2-e971-4240-b280-2b1865f7cce8"
///   
///     }
///
/// </remarks>
[HttpGet]
[Route("GetLast12MonthBalances/{userId}")]
public async Task<ApiResponse> GetLast12MonthBalances(string userId)
{
    return new ApiResponse("Last 12 Month Balances Returned", await _transactionService.GetLast12MonthBalances(userId));

}
```

Notice the UI enhancements with these additional comments:

![swagger-post-remarks](/readme_assets/swagger-post-remarks.png)


## Step 5 API versioning

Imagine a scenario where you need to make changes to your API in its structure or in its functionality, but at the same time ensure that the existing API clients shouldn’t face any issues. In other words, you would want to upgrade your API, but ensure that it’s still backward compatible. How’d you solve this problem? The solution is something we use very extensively in software applications – versioning. You can simply mark your existing APIs as the current (older version), while all the changes you intend to do on the APIs move to the next (or the latest) version.

This solves two problems for us – one, you no longer need to worry about backward compatibility, as the current state of the APIs still exist (or say, co-exist) so your clients aren’t going to face any issues and two, you get the capability of giving optional functionalities to your clients in the form for versions, where a client can subscribe to a particular version for new or improved functionalities or features without having to break the existing. Sounds like an application of the Open Closed Principle right? But it is often easier said than done.

API versioning can be done in one of ways:
* Via **URL Segment** mean URL will contain api version in it like *api/v1/controller/action*
* Via **Query parameter** mean URL will contain version information in URL query parameter *api/v1/controller/action?apiverion=1.0*
* Via **Header** mean api version information would be sent in request headers

We will be using **URL segment** for api versioning.

### Step 5.1 Install packages for API versioning

To get the information on versions and endpoints, we add the `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer` nuget package which provides the metadata for the APIs based on how they are decorated. In our case, it returns us the version information of each action.

Install using package manager console as well, make sure to select *BBBankAPI* project in package manager console:

```cs
Install-Package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

Add The APIVersioning service to the **IServiceCollection** in `program.cs`. This adds the necessary capabilities to detect and branch out the requests based on their version.

```cs
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    setup.GroupNameFormat = "'v'VVV";
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    setup.SubstituteApiVersionInUrl = true;
});
```

* Here we’re setting the DefaultApiVersion to specify to which the requests fallback to.
The AssumeDefaultVersionWhenUnspecified lets the router fallback to the default version (specified by the DefaultApiVersion setting) in cases where the router is unable to determine the requested API version.
 * The ReportApiVersions setting sends out the available versions for the API in all the responses in the form of "api-supported-versions" header.

 ### 5.2 Decorate API Controller

Decorate the API Controller with the version numbers that it’d be called when a request is made for that specific version.

Now **TransactionController** would be decorated as below, this will enable transaction controller to work for version **v1** as well as for **v2**. if no version mentioned in URL segment then default version **v1** would be called.

```
namespace BBBankAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
	{
		// controller operations
	}
}
```
 ### 5.3 Get version information

Now we would use api version in **TransactionService** and based on version we will calculate **average** for *TransactionController* action **GetLast12MonthBalances** for *v2* and will return **average** with value *-1* for *v1*

Install following nuget packages using package manager console , make sure to select *Services* project in package manager console:

```cs
Install-Package Microsoft.AspNetCore.Http

Install-Package Microsoft.AspNetCore.Routing
```

Now add The **IHttpContextAccessor** service to the **IServiceCollection** in `program.cs`. This will help to detect api version.

```cs
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
```

 ### 5.4 Version usage

 Now we will get api version in **TransactionService** method **GetLast12MonthBalances**, for this dependency inject **IHttpContextAccessor** in *TransactionService* constructor.

```cs
private readonly IHttpContextAccessor _httpContextAccessor;
public TransactionService(BBBankContext BBBankContext, IHttpContextAccessor httpContextAccessor)
{
    _bbBankContext = BBBankContext;
    _httpContextAccessor = httpContextAccessor;
}
```

Modify **LineGraphData** model in **Entities** project and add new **Average** property .

```cs
 public class LineGraphData
{
    public decimal TotalBalance { get; set; }
    public ICollection<string> Labels { get; set; }
    public ICollection<decimal> Figures { get; set; }
    public LineGraphData()
    {
        Labels = new List<string>();
        Figures = new List<decimal>();
    }

    public decimal Average { get; set; } = -1;
}

```


In **TransactionService** method **GetLast12MonthBalances** get api information and based on information add the **average** in response .Now method would be as below, this line *_httpContextAccessor.HttpContext.GetRouteData().Values;* would give us api version and based on version we are adding **average**

```cs
        public async Task<LineGraphData> GetLast12MonthBalances(string? userId)
        {
            // Object to contain the line graph data
            var lineGraphData = new LineGraphData();

            // Object to contain the transactions data
            var allTransactions = new List<Transaction>();
            if (userId == null)
            {
                // if account id is NULL then fetch all transactions
                allTransactions = _bbBankContext.Transactions.ToList();
            }
            else
            {
                // if account id is not NULL then fetch all transactions for specific account id
                allTransactions = _bbBankContext.Transactions.Where(x => x.Account.User.Id == userId).ToList();
            }
            if (allTransactions.Count() > 0)
            {
                // Calculate the total balance till now
                var totalBalance = allTransactions.Sum(x => x.TransactionAmount);
                lineGraphData.TotalBalance = totalBalance;

                decimal lastMonthTotal = 0;

                // looping through last three months starting from the current
                for (int i = 12; i > 0; i--)
                {
                    // Calculate the running total balance
                    var runningTotal = allTransactions.Where(x => x.TransactionDate >= DateTime.Now.AddMonths(-i) &&
                       x.TransactionDate < DateTime.Now.AddMonths(-i + 1)).Sum(y => y.TransactionAmount) + lastMonthTotal;

                    // adding labels to line graph data for current month and year
                    lineGraphData.Labels.Add(DateTime.Now.AddMonths(-i + 1).ToString("MMM yyyy"));

                    // adding data to line graph data for current month and year
                    lineGraphData.Figures.Add(runningTotal);

                    // saving the running total for this month
                    lastMonthTotal = runningTotal;
                }


                var routeValues = _httpContextAccessor.HttpContext.GetRouteData().Values;
                if (routeValues["version"] != null && routeValues["version"].ToString() == "2")
                {
                    lineGraphData.Average = Math.Round(lineGraphData.Figures.Sum() / lineGraphData.Figures.Count(),2);
                }
            }
            // returning the line graph data object
            return lineGraphData;
        }
```

 ### 5.5 Map API version for swagger UI

Modify step 4.2 code as below to get the api versions comments information for both versions in **swagger** UI .

```csharp
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BBBank API",
        Description = "This is the first version of the API",
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "BBBank API",
        Description = "This is the Second version with some extra properties returned.",

    });

    // this code is required to add code comments in swagger.
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

});
``` 

Now to build a swagger endpoint for each API version, modify **UseSwaggerUI** added in step 2,this will add both versions in swagger UI top right dropdown.

```cs
app.UseSwaggerUI(c =>
{
    // build a swagger endpoint for each API version
    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "BBBank API V1");
    c.SwaggerEndpoint($"/swagger/v2/swagger.json", "BBBank API V2");
});
```

----------------------------

## API versioning output
Execute the application and access swagger UI using URL http://localhost:5070/swagger

Now you should be able to see both versions in top right dropdown of swagger UI

![api-versions](/readme_assets/api-versions.png)

Now access **TransactionController** action **GetLast12MonthBalances** for **v1** version using swagger or api url http://localhost:5070/api/v1/Transaction/GetLast12MonthBalances and you will notice **average** is added with *-1* mean no value calculated.

![v1-result](/readme_assets/v1-result.png)

Now access **TransactionController** action **GetLast12MonthBalances** for **v2** version using swagger or api url http://localhost:5070/api/v2/Transaction/GetLast12MonthBalances and you will notice **average** is being calculated.

![v1-result](/readme_assets/v2-result.png)
