# GroceryItemLookup
ASP.NET Core MVC project that will allow users to search through a database of products, employees, departments, and sales/ordering records for a fictional grocery store.

## Project Inspiration and Summary
In my full time job as a grocery store crew member, we use kiosks around the store set up with a web application we call Quick Item Lookup, or QIL for short. At times, this app can be difficult or bothersome to use.  The item search function is unintuitive as search terms have to be exact matches of the names the products were given in the database, which often have inconsistent naming conventions.  For example, searching for "dark chocolate" would only serve the user items that have those two terms in the name, like "DARK CHOCOLATE ALMOND BARK", and omit ones that have names like "DK CHOC TRUFFLE".

This is just one issue with the program I intend to work around in this mock version, and I intend to add many of the same functionalities QIL has such as filtering searches by departments and department supervisors (who may have multiple assigned departments), and viewing ordering and sales records.  In this version, users will be able to interact with the database by reading, adding, editing, and deleting products, department leaders, departments, and employees.

## Planning Table
| Week | Concept                   | Feature                                                                               | Goal                                                         |  Test Plan     |
| ---  | -------                   | --------                                                                              | -------                                                      | ------- |
| 1    | Modeling                  | Create Product, Department, Employee, Supervisor entities                             | App can store these entities and relations to one another    | Run migration; check DB tables exist  |
| 2    | Separation of Concerns/DI | Add IProductService for search functions and CRUD                                     | Make app clean and modular by moving logic out of controller | Verify all product functions work through the service |
| 3    | CRUD                      | Allow users to create, read, update, and delete products, employees, and departments  | Users can interact with database                             | Confirm database updates |
| 4    | Diagnostics               | add healthz endpoint                                                                  | Reports if database is reachable                             | Stop DB and hit /healthz  |
| 5    | Logging                   | log all entity creation                                                               | Record logs of database changes                              | check log output  |
| 6    | Stored Procedures         | Call SP: Sort items by price asc/desc                                                 | Allow users to refine searches                               | Run SP in app and DB; compare results  |
| 7    | Deployment                | Deploy app to azure web service                                                       | Make app accessible                                          | Visit public URL; confirm health endpoint and one page load  |





## Week 10 Changes
I added Product, Department, Employee, and Supervisor models to the project and applied the initial migration to create a database table.  The Product class contains values for item SKUs (this being the primary key), item names, departments, prices, descriptions, and weights.  Each property is appropriately and explicitly typed.  The Departments class contains values for the ID of the department, the name, and a list of Products stored in the department, as well as any Supervisors assigned to the department. There is also a class for employees to store values for employee IDs, first names, last names, and hire dates.  The Supervisor class inherits these values, and also has a field for a list of Departments they may be assigned to.

The database structure of this app will remain relatively simple as there are few relationships between tables.  Some relationships include the Department class having a one-to-many relationship with products, where one department has many products but every product has one department. There also exists a many-to-many relationship between Departments and Supervisors, where one Department can have multiple assigned Supervisors and one Supervisor can be assigned many departments. 

In the finished version of the app, users will be able to search for products not just by name or department, but by ordering Supervisor, which will search items through filtering by a Supervisor’s assigned departments, then displaying the items contained in each assigned Department’s Product list.

## Week 11 Changes
I added the Product Service to the project and registered it to the Dependency Injection container with the a Scoped lifetime. I moved some logic from the Product model into the service to keep the project modular and to ensure that the model classes don't get bloated with unnessary code that can be placed elsewhere.

The ProductService class contains a number of useful methods for interacting with product data, such as:
* GetProducts()
  - returns an IEnumerable list of all products in the database
* GetProductBySKU(int sku)
  - returns the product with the given SKU
* GetDepartments()  (will later be moved to a DepartmentService class)
  - returns enumerable list of departments
* GetDepartmentByID(int id)
  - returns department of given id

as well as AddProduct(Product product), UpdateProduct(Product product), and DeleteProduct(int sku) which add placeholder CRUD functionality to the project.

Some of these methods were moved into the ProductController, such as the function for returning the product index view:

```
        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View(products);
        }
```
I still need to create service classes for the rest of the models and controllers in the project if I want to accurately replicate the funcitonality of Quick Item Lookup, but this gives me a good foundation to get started from. Next I'll refine the CRUD functionality and add async data access.

## Week 12 Changes

Considering that most of the foundation for CRUD functionality with Products was set in my last big push, there wasn't much to change this week. All I had to do was change the current functions to be called asynchronously as I already had some validation feedback on the create and edit form pages. EFCore supplies some methods like SaveChangesAsync() and FindAsync() which made this change easy, and left me with some time to think about the scope of this project as I think about my deadline.  I think it would be best to lessen the scope of this project and take the rest of my time only to flesh out the features relating to the Product and Department entities and their relationship rather than to go into how they would interact with Employee and Supervisor classes. It makes sense to me because I'd be able to meet all requirements of the project without having to worry about more moving parts of the app and rewriting a bunch of code with different class names. The core idea will also still be in the finished project as a searchable list of products with the ability to add, delete, or edit.

## Week 13 Changes
For this work session I worked on the Department controller as well as the Department service class, and created the necessary views for each CRUD function relating to Department info in the database. The requirements for this week were to add a /healthz endpoint, a dependency check, and enough detail to be returned in the health check without exposing secrets. I closely followed the documentation and tutorials provided by Microsoft to meet these requirements and kept the actual functionality light.  I even went ahead and created a separate class to keep the health check response writer separate from Program.cs. The final endpoint mapping looks like this:
```
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = DiagnosticsWriter.WriteResponse
});
```
I couldn't find anything in the documentation as to whether or not this is good practice, but it seemed like the right thing to do. With the endpoint added and the health checks mapped, the user can now reach a /healthz endpoint which gives safe information on the health of the app and whether or not connection with the database was successful upon running the app.

## Week 14 Changes
For this work session I added Logging features to the Product entity's Details endpoint. You can see the code here:
```
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductBySKU(id);
            var transactionId = Guid.NewGuid().ToString();
            using (_logger.BeginScope(new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("TransactionId", transactionId),
                }))
            {
                _logger.LogInformation(LogEvents.GetItem, "Details action, getting product {Id}", id);
                if (product == null)
                {
                    _logger.LogWarning(LogEvents.GetItemNotFound, "Product {Id} NOT FOUND", id);
                    return NotFound();
                }
                _logger.LogInformation(LogEvents.GetItem, "Successfully retrieved product {Id} details", id);
                return View(product);
            }
        }
```
The requirements for this week include logs on one success path and one error path, as well as useful fields (implemented here as TransactionId, the entity id, and the action of the endpoint) and verbage that makes the logs readable and actionable.
A logging scope is created so that all log entried share the given TransactionId, allowing for easier tracing if there was more logging going on throughout the app. A message is logged when the user reaches the details endpoint of a product with a given Id, then a check is ran to see if the product exists.  If it does not, a warning log is displayed to the console letting the user or developer know that no product with the given Id was found. If the product does exist, a success message relating to the endpoint and entity is logged.

## Week 15 Changes

For this session, I added a stored procedure and integrated it into the application using Entity Framework Core.  You can see the code that executes the stored procedure here:
```
        public async Task<List<Product>> GetProductsByDepartment(int departmentId)
        {
            return await _context.Product
                .FromSqlInterpolated($"EXECUTE dbo.GetProductsByDepartment {departmentId}")
                .ToListAsync();
        }
```
The GetProductsByDepartment method executes the stored procedure run by the code in the GetProductsByDepartment.sql script in the SQLScripts folder. This method is called in the Department controller, replacing the old code that populated the Details view with product information of a given DepartmentId. The SQL for creating the stored procedure can be seen here:
```
CREATE PROCEDURE dbo.GetProductsByDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.SKU, p.Name, p.Description, p.Price, p.Weight, p.DepartmentID
    FROM Product AS p
    WHERE p.DepartmentID = @DepartmentId;
END
```
Same as before, the returned product list is stored to a ViewBag, where the stored prodedure results are rendered in the Details view of the given department.

## Week 16 Changes - Deployment to Azure (Simulated)

Although my Azure credits were not exhausted, I was unable to deploy my project and felt I had no other option to submit other than submitting the DEPLOYMENT.md substitute assignment. My deployment guide mostly mirrors the [guide](https://learn.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vs?view=aspnetcore-10.0) provided by Microsoft, with a few extra steps to make deployment work with my app's unique setup. The most noteworthy instruction on my guide was the bit about implementing the stored procedure requirement from Week 15, seen in step 4.1.  This was necessary as the creation of the stored procedure would not transfer from the local development database to the production database.
