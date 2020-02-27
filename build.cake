///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Build")
    .Does(() =>
{    
    DotNetCoreBuild("./SimplePaymentGateway.sln");
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./PaymentGateway/test/PaymentGateway.UnitTests/PaymentGateway.UnitTests.csproj");
});

Task("Functional-Tests")
    .IsDependentOn("Unit-Tests")
    .Does(() =>
{
    DotNetCoreTest("./PaymentGateway/test/PaymentGateway.FunctionalTests/PaymentGateway.FunctionalTests.csproj");
});

Task("Default")
    .IsDependentOn("Functional-Tests");

RunTarget(target);