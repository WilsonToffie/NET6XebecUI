using Blazored.Localisation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MatBlazor;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Services;
using XebecPortal.UI.Services.Models;
using XebecPortal.UI.Utils;
using Smart.Blazor;
using XebecPortal.UI.Service_Interfaces;
using MudBlazor.Services;
using XebecPortal.UI.Pages.HR;
using XebecPortal.UI.Shared;
using Blazored.LocalStorage;
using XebecPortal.UI.Utils.Handlers;

namespace XebecPortal.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddSingleton<State>();
            builder.Services.AddSingleton<HrJobState>();
            builder.Services.AddSingleton<UserState>();
            builder.Services.AddSingleton<JobState>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddScoped<IApplicantDataService, ApplicantDataService>();
            builder.Services.AddScoped<IApplicationDataService, ApplicationDataService>();
            builder.Services.AddScoped<IJobDataService, JobDataService>();
            builder.Services.AddScoped<IApplicationPhaseHelperDataService, ApplicationPhaseHelperDataService>();
            
            builder.Services.AddScoped<IPersonalInformationDataService, PersonalInformationDataService>();
            builder.Services.AddScoped<IWorkHistoryDataService, WorkHistoryDataService>();
            builder.Services.AddScoped<IEducationDataService, EducationDataService>();
            builder.Services.AddScoped<IEducationDataService, EducationDataService>();
            builder.Services.AddScoped<IAdditionalInformationDataService, AdditionalInformationDataService>();
            builder.Services.AddScoped<IStatusDataService, StatusDataService>();
            builder.Services.AddScoped<IPhaseDataService, PhaseDataService>();            
            //Testing
            builder.Services.AddScoped<IMyJobListDataService, MyJobListDataService>();

            // builder.Services.AddSmart();
            // builder.Services.AddMatBlazor();

            builder.Services.AddMudServices();

            builder.Services.AddBlazoredLocalStorage();

            //builder.Services.AddTransient<CustomHandler>();


            await builder.Build().RunAsync();            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalisation(); // This adds the IBrowserDateTimeProvider to the DI container
            services.AddBlazoredLocalStorage(); // This adds the ability to store the JWT key locally in the browsers storage, it is used on the sign in page.            
            
        }
    }
}
