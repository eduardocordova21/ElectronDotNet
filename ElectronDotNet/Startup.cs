using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace ElectronDotNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            if (HybridSupport.IsElectronActive)
            {
                ElectronStartup();
            }
        }

        private async void ElectronStartup()
        {
            CreateMenu();

            var window = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions { Width = 1152, Height = 940, Show = false });

            await window.WebContents.Session.ClearCacheAsync();

            window.OnReadyToShow += () => window.Show();
            window.SetTitle("Exemplo do Electron.NET");

            window.OnClosed += () =>
            {
                Electron.App.Quit();
            };
        }

        private void CreateMenu()
        {
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            MenuItem[] menu = null;

            MenuItem[] appMenu = new MenuItem[]
            {
                new MenuItem { Role = MenuRole.about },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Role = MenuRole.services },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Role = MenuRole.quit }
            };

            MenuItem[] fileMenu = new MenuItem[]
            {
                new MenuItem { Role = isMac ? MenuRole.close : MenuRole.quit }
            };

            MenuItem[] viewMenu = new MenuItem[]
            {
                new MenuItem { Role = MenuRole.reload },
                new MenuItem { Role = MenuRole.forcereload },
                new MenuItem { Role = MenuRole.toggledevtools },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Role = MenuRole.resetzoom },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Role = MenuRole.togglefullscreen }
            };

            if (isMac)
            {
                menu = new MenuItem[]
                {
                    new MenuItem {
                        Label = "Electron",
                        Type = MenuType.submenu,
                        Submenu = appMenu
                    },
                    new MenuItem {
                        Label = "File",
                        Type = MenuType.submenu,
                        Submenu = fileMenu
                    },
                    new MenuItem {
                        Label = "View",
                        Type = MenuType.submenu,
                        Submenu = viewMenu
                    }
                };
            }
            else
            {
                menu = new MenuItem[]
                {
                    new MenuItem {
                        Label = "File",
                        Type = MenuType.submenu,
                        Submenu = fileMenu
                    },
                    new MenuItem {
                        Label = "View",
                        Type = MenuType.submenu,
                        Submenu = viewMenu
                    }
                };
            }

            Electron.Menu.SetApplicationMenu(menu);
        }
    }
}
