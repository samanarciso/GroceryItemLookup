# Deployment Tutorial: GroceryItemLookup

This document provides a step-by-step guide to deploying and publishing the GroceryItemLookup application to Azure using Visual Studio Publish based on Microsoft's documentation.

## 2. Create the App Service and Resource Group

1. In Visual Studio, right-click the GroceryItemLookup project and select Publish… .
2. In the Publish dialog:
   - Select Azure
   - Select Next
   - Specific target: Azure App Service (Windows)
   - Select Next
3. In App Service, click Create new.
4. In the Create App Service dialog:
   - The Name, Resource Group, and Hosting Plan entry fields are automatically populated and do not need to be changed for the purposes of this deployment.
5. Click Create. Visual Studio will create the App Service and a publish profile. The new instance is automatically selected in the Publish dialog. Click Finish.

## 3. Creating the Azure SQL Database

1. In the Publish Profile summary page, click the ellipses next to SQL Server Database.
2. Select Azure SQl Database, and proceed.
3. Click Create new in the new dialogue window, and the Create Azure SQL Database window will appear, pre-populated with default values.

Enter Database admin username and password, and select Create.

The new database instance will be automatically selected in the Connect to Azure SQL Database dialogue, and you can click Next.

You'll be prompted to enter a username and password. Enter something different than the admin credentials and click Finish.

## 4. Apply Migrations to the Azure SQL Database

After creating the Azure SQL Database, Visual Studio automatically detects the existing Entity Framework Core migrations in the project. 
In the Publish profile, under the Entity Framework Migrations section, you can select Apply this migration on publish. Select Save.

Click Publish and Visual Studio takes care of publishing the app to Azure.

## 5. Externalizing Secrets and Production Configuration

After publishing, it's important to externalize sensitive information such as connection strings and production configurations.
In the Azure Portal, navigate to the App Service that was created, open Configuration, and add the following application setting:

- ASPNETCORE_ENVIRONMENT = Production

Next, in the Connection strings section, add an entry named GroceryItemLookupContext and paste the Azure SQL connection string that was
generated when the database was created. Set the type to SQLAzure. A matching entry can appear in appsettings.Production.json, but it should remain empty so that no secrets are accidentally committed.

## 6. Verify Deployment

Once the app is published, Visual Studio will open the site in a browser using the newly created URL. To confirm everything deployed correctly,
navigate to the /healthz endpoint. This endpoint should return the application's health information, including the status of the database connection.
Now, you can test the functionality of the GroceryItemLookup application by navigating to the Product and Department pages to test CRUD operations.
