using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using SimpleMvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using SimpleMvc.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace SimpleMvc
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
            services.AddMvc().AddFluentActions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFluentActions(
                config =>
                {
                    config.InheritingFrom<HelloWorldController>();
                },
                actions =>
                {
                    actions
                        .Route("/helloWorld", HttpMethod.Get)
                        .To(() => "Hello World!");

                    actions
                        .Route("/helloWorldAsync", HttpMethod.Get)
                        .To(async () => { await Task.Delay(2000); return "Hello World Async!"; });

                    actions
                        .RouteGet("/helloWorldAsyncWithDo")
                        .DoAsync(async () => { await Task.Delay(2000); })
                        .To(() => "Hello World Async (with Do)!");

                    actions
                        .RouteGet("/helloWorldFromParent")
                        .UsingParent<HelloWorldController>()
                        .To(parent => parent.HelloWorld());

                    actions
                        .RouteGet("/helloWorldFromParentAsync")
                        .InheritingFrom<HelloWorldController>()
                        .UsingParent()
                        .To(async parent => await parent.HelloWorldAsync());

                    actions
                        .RouteGet("/hello")
                        .UsingQueryStringParameter<string>("name")
                        .ToView("~/Views/Plain/Hello.cshtml");

                    actions
                        .Route("/helloWithAttributes", HttpMethod.Get)
                        .WithCustomAttribute<AuthorizeAttribute>()
                        .To(() => "Hello With Attributes!");

                    actions
                        .Route("/toHome", HttpMethod.Get)
                        .ToMvcController<HomeController>()
                        .ToMvcAction(homeController => homeController.Index());

                    actions
                        .Route("/toAbout", HttpMethod.Get)
                        .UsingViewBag()
                        .UsingViewData()
                        .UsingTempData()
                        .Do((viewBag, viewData, tempData) =>
                        {
                            viewData["ViewDataMessage"] = "ViewData message from fluent action.";
                            tempData["TempDataMessage"] = "TempData message from fluent action.";
                            viewBag.ViewBagMessage = "ViewBag message from fluent action.";
                        })
                        .ToView("~/Views/Home/About.cshtml");

                    actions
                        .Route("/toError", HttpMethod.Get)
                        .ToView("~/Views/Shared/Error.cshtml");

                    actions
                        .Route("/users", HttpMethod.Get, "List users.")
                        .UsingService<IUserService>()
                        .To(userService => userService.List())
                        .ToView("~/views/users/list.cshtml");

                    actions
                        .Route("/api/users", HttpMethod.Get, "List users.")
                        .UsingService<IUserService>()
                        .To(userService => userService.List());

                    actions
                        .RouteGet("/uploadFile", "Form to upload file.")
                        .ToView("~/Views/Form/UploadFile.cshtml");

                    actions
                        .RoutePost("/uploadFile", "Upload file.")
                        .UsingFormFile("file")
                        .To(file => $"Got {file.FileName}!");

                    actions
                        .RouteGet("/uploadFiles", "Form to upload files.")
                        .ToView("~/Views/Form/UploadFiles.cshtml");

                    actions
                        .RoutePost("/uploadFiles", "Upload files.")
                        .UsingFormFiles("files")
                        .To(files => $"Got {files.Count()} n.o. files!");

                    actions
                        .RouteGet("/submitWithAntiForgeryToken", "Form to submit with anti forgery token.")
                        .ToView("~/Views/Form/SubmitWithAntiForgeryToken.cshtml");

                    actions
                        .RoutePost("/submitWithAntiForgeryToken", "Submit with anti forgery token.")
                        .ValidateAntiForgeryToken()
                        .To(() => "Got submission!");

                    actions
                        .RouteGet("/submitWithModelState", "Form to submit with model state.")
                        .ToView("~/Views/Form/SubmitWithModelState.cshtml");

                    actions
                        .RoutePost("/submitWithModelState", "Submit with model state.")
                        .UsingModelState()
                        .UsingForm<ModelStateFormModel>()
                        .To((modelState, model) => modelState.IsValid ? "Model valid! :)" : "Model invalid :(");

                    actions
                        .RouteGet("/201")
                        .UsingResponse()
                        .To(response => { response.StatusCode = 201; return "Hello from 201!"; });

                    actions
                        .RouteGet("/203")
                        .UsingProperty<HttpResponse>("Response")
                        .To(response => { response.StatusCode = 203; return "Hello from 203!"; });

                    actions
                        .RouteGet("/request")
                        .UsingRequest()
                        .To(request => $"Hello from {request.Path}!");

                    actions
                        .RouteGet("/helloProp")
                        .UsingProperty<string>("HelloProp")
                        .To(helloProp => helloProp);
                }
            );
        }
    }
}
