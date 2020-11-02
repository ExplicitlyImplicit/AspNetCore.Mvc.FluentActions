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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

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
            services
                .AddMvc()
                .AddFluentActions();

            services.AddAuthentication(o => {
                o.DefaultScheme = "AuthScheme";
            }).AddScheme<AuthHandlerOptions, AuthHandler>("AuthScheme", (options) => { });

            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseFluentActions(
                actions =>
                {
                    actions.Configure(config =>
                    {
                        config.InheritingFrom<HelloWorldController>();
                        config.Append(action => action
                            .UsingResult()
                            .UsingResponse()
                            .To(async (result, response) =>
                            {
                                await Task.Delay(1);
                                response.StatusCode = 418;
                                return result is string ?
                                    $">> {result}" :
                                    result;
                            })
                        );
                    });

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
                        .WithCustomAttribute<AuthorizeAttribute>(new Type[0], new object[0], new string[] { "AuthenticationSchemes" }, new object[] { "AuthScheme" })
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
                        .UsingHttpContext()
                        .To((httpContext) => new ErrorViewModel { RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier })
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

                    actions
                        .RouteGet("/Home/Error")
                        .WithCustomAttribute<ResponseCacheAttribute>(
                            new Type[0],
                            new object[0],
                            new string[] { "Duration", "Location", "NoStore" },
                            new object[] { 0, ResponseCacheLocation.None, true })
                        .UsingHttpContext()
                        .To(httpContext => new ErrorViewModel { RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier })
                        .ToView("~/Views/Shared/Error.cshtml");

                    actions
                        .RouteGet("/Home/Error2")
                        .ResponseCache(
                            duration: 0,
                            location: ResponseCacheLocation.None,
                            noStore: false
                        )
                        .UsingHttpContext()
                        .To(httpContext => new ErrorViewModel { RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier })
                        .ToView("~/Views/Shared/Error.cshtml");
                }
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class AuthHandlerOptions : AuthenticationSchemeOptions
    {

    }

    public class AuthHandler : AuthenticationHandler<AuthHandlerOptions>
    {
        public AuthHandler(IOptionsMonitor<AuthHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Access denied."));
        }
    }
}
