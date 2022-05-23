using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

var builder = WebApplication.CreateBuilder(args);


// Add the Yarp reverse proxy to Services
var proxyBuilder = builder.Services.AddReverseProxy();
proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

var env = app.Services.GetService<IWebHostEnvironment>();
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
}

// Enable endpoint routing, required for the reverse proxy
app.UseRouting();
// Register the reverse proxy routes
app.UseEndpoints(endpoints => 
{
    endpoints.MapReverseProxy(pipeline =>
    {
        pipeline.UseLoadBalancing();
    }); 
}); 

app.Run();