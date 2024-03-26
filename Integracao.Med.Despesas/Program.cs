using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Integracao.Med.Despesas.Domain.Configuration;
using Integracao.Med.Despesas.Services;
using Integracao.Med.Despesas.Services;
using Integracao.Med.Despesas.Domain.Configuration;
using Integracao.Med.Despesas.Core.Interfaces;
using Integracao.Med.Despesas.Core.Adapters;
using Integracao.Med.Despesas.Infra.Interfaces;
using Integracao.Med.Despesas.Infra.Repositories;
using Integracao.Med.Despesas.Services.Mapper.Profiles;
using Integracao.Med.Despesas.Services.Services_New;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var devCorsPolicy = "devCorsPolicy";
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

builder.Host.ConfigureDefaults(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Integracao VExepenses";
    })
    .ConfigureServices(services =>
    {
        #region [ Configuration ]
        services.Configure<Configuration>(configuration.GetSection("AppConfig"));
        #endregion

      

        #region [ Services ]       
        services.AddScoped<ISap_Service_New, Sap_Service_New>();
        services.AddScoped<IVexpenses_Service_New, Vexpenses_Service_New>();


        #endregion

        #region [ Adapters ]
        services.AddScoped<IHttpAdapter, HttpAdapter>();
        services.AddScoped<IServiceLayerAdapter, ServiceLayerAdapter>();
        services.AddScoped<IHanaAdapter, HanaAdapter>();
        #endregion

        #region [ Repositories ]
        //services.AddScoped<ILoggerRepository, LoggerRepository>();
       // services.AddScoped<ILogger, LoggerConfiguration>();
        #endregion

        #region [ Hangfire ]
        services.AddControllers();
      
        services.AddHangfire((provider, configuration) =>
        {
            configuration.UseMemoryStorage();
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            configuration.UseSimpleAssemblyNameTypeSerializer();
            configuration.UseRecommendedSerializerSettings();
        });

        services.AddHangfireServer();
        #endregion

        #region [ Cors ]
        services.AddCors(services =>
        {
            services.AddPolicy(devCorsPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
        #endregion

        #region [ ConfigMap ]
        var config = new AutoMapper.MapperConfiguration(cfg =>
        {

            cfg.AddProfile<CreateBusinessPartnerProfile>();
        });

        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        #endregion

    });

builder.Logging.AddSerilog();

using (var app = builder.Build())
{
    app.UseStaticFiles();
    Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\osvaldo\\Log.txt", rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

  

    var Sap_Service_New = new BackgroundJobServerOptions
    {
        ServerName = string.Format("{0}:Sap_Service_New", Environment.MachineName),
        Queues = new[] { "sapservicequeue" },
        WorkerCount = 1
    };


    app.UseHangfireDashboard("/scheduler", new DashboardOptions
    {
        //Authorization = new[] { new HangFireAuthorization() },
        AppPath = "/"
    });


    //app.UseHangfireServer(CancPedCompraSAPToStage);
    app.UseHangfireServer(Sap_Service_New);

    app.UseHangfireDashboard();




    #region [ Scheduler ]

    #region [ DEBUG ]

#if DEBUG

    var cronServiceTeste = Cron.Minutely();
    var cronServiceDebug = Cron.Never();
   
    RecurringJob.AddOrUpdate<Sap_Service_New>("Sap_Service_New", job => job.Processar(), cronServiceDebug);


#endif

    #endregion

    #region [ RELEASE ]

#if DEBUG == false
    var cronServiceTest = Cron.MinuteInterval(10);    
    var cronService = Cron.Minutely();
     
    RecurringJob.AddOrUpdate<Sap_Service_New>("Sap_Service_New", job => job.Processar(), cronServiceTest, null,"sapservicequeue");

#endif

    #endregion

    #endregion

    await app.RunAsync();
}

