# Bulk-upload-csv-data-in-to-SQL-using-Azure-Functions
Bulk upload csv data in to SQL using Azure Functions

Using Azure Functions v1.x

## Pre-requisites and Guided Steps
- Create an Azure function with Blob trigger
- Create a connection string with this name "sqldb_con". If not be sure to change the connection name to your name in the code in line 15. [Reference Article here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scenario-database-table-cleanup)
- Make sure the SQL DB has tables with the same name as the csv files being dropped in the unzip folder this function picks them from
