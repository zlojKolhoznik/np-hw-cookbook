using CookBookApi;
using CookBookApi.Data;
using Microsoft.EntityFrameworkCore;

// This was supposed to be a dependency injection container, but it didn't work out
var builder = new DbContextOptionsBuilder<CookBookContext>();
builder.UseSqlServer(Configuration.ConnectionString);   
var context = new CookBookContext(builder.Options);
var handler = new RequestHandler(context);
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();
await handler.StartAsync(cts.Token);
while (!cts.IsCancellationRequested){}