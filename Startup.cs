using bbelt_exam.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace bbelt_exam
{
    public class Startup
    {
		public Startup(IHostingEnvironment environment)
        {   //used for building key/value based configuration like in json
            var builder = new ConfigurationBuilder()
                //sets the root path for the app to be this one
                .SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
			Configuration = builder.Build();
        }

		public IConfiguration Configuration { get; private set; }


        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<DojoActivityContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));
			services.Configure<MySqlOptions>(Configuration.GetSection("DBInfo"));
            services.AddScoped<DBConnector>();
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
				var context = serviceScope.ServiceProvider.GetRequiredService<DojoActivityContext>();
                context.Database.EnsureCreated();
            }
			loggerFactory.AddConsole();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvcWithDefaultRoute();

        }
    }
}
