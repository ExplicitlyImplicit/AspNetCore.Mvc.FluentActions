# [Fluent Actions for ASP.NET Core MVC](https://www.nuget.org/packages/ExplicitlyImpl.AspNetCore.Mvc.FluentActions)
Fluent actions are abstractions of regular MVC actions that are converted into MVC actions during startup.

Usage example:

```
app.UseFluentActions(actions =>
{
    actions.RouteGet("/").To(() => "Hello World!");
});
```

The above fluent action is converted into the following MVC action during startup:

```
[HttpGet]
[Route("/")]
public string Action()
{
    return "Hello World!";
}
```

This tool targets those of you who are already working on an existing MVC project or have previous experience with MVC
as you already know how it works one level deeper. As long as you can wrap your head around the mapping from fluent 
actions to MVC actions you should be all set.

Fluent actions do not limit regular use of ASP.NET Core MVC so you can gradually introduce fluent actions to your 
existing project and only use fluent actions for a subset of your applications functionality. 

This project does not have any third-party dependencies.

Another example:

```
app.UseFluentActions(actions =>
{
    actions
        .RouteGet("/users/{userId}")
        .UsingService<IUserService>()
        .UsingRouteParameter<int>("userId")
        .To((userService, userId) => userService.GetUserById(userId))
        .ToView("~/Views/Users/DisplayUser.cshtml")
});
```

See the **How to use** chapter for a better understanding of how fluent actions are mapped to MVC actions.

## Getting Started

Add this package to your project using the NuGet Package Manager Console:

```
Install-Package ExplicitlyImpl.AspNetCore.Mvc.FluentActions
```

Or using the dotnet CLI:

```
dotnet add package ExplicitlyImpl.AspNetCore.Mvc.FluentActions
```

You also need to add these two calls in your `Startup.cs` file:

<pre>
services.AddMvc()<b>.AddFluentActions();</b>
 .
 .
 .
app.UseMvc(routes);
<b>app.UseFluentActions(actions);</b> // see usage chapter below 
</pre>

There is also `UseFluentActions(config, actions)` to apply initial settings on all 
of the fluent actions you define, take a look at the `Config` parameter further down for more info. 

## How To Use

Fluent actions are added inside the `Startup.cs` file in the `UseFluentActions` statement:

```
app.UseFluentActions(actions => 
{
    actions.RouteGet("/helloWorld").To(() => "Hello World!");
});
```

The fluent action definitions can be placed in one or many other files though: 

```
app.UseFluentActions(actions => 
{
    actions.RouteGet("/helloWorld").To(() => "Hello World!");
    actions.Add(FluentActions.UserActions);
    actions.Add(FluentActions.ProfileActions);
});
```

Where `FluentActions.UserActions` could look like this:

```
public static FluentActionCollection UserActions => FluentActionCollection.DefineActions(actions =>
{
    actions
        .RouteGet("/users")
        .UsingService<IUserService>()
        .To(userService => userService.List());
    
    actions
        .RouteGet("/users/{userId}")
        .UsingService<IUserService>()
        .UsingRouteParameter<int>("userId")
        .To((userService, userId) => userService.Get(userId));
});
```

If we examine the first defined action in the `Startup.cs` above:

```
actions
    .RouteGet("/helloWorld")
    .To(() => "Hello World!");
```

- The first statement `RouteGet` defines the routing, any **GET** requests to **/helloWorld** will be handled by this action.
- The second statement `To` defines what will happen when someone makes a **GET** request to this url. In this case, a plain text "Hello World!" will be returned by the web app. 

How do we know how the web app writes our output to the HTTP response? The code 
above is equivalent to an action method in a controller looking like this:

```
[HttpGet]
[Route("/helloWorld")]
public string HelloWorldAction()
{
    return "Hello World!";
}
```

Fluent actions are only wrapping the tools that makes up the framework .NET MVC. 
We can still use MVC tools and concepts to implement our web app. 

### `Using` Statements

If you need to define any kind of input for your action, use a `Using` statement. 
Each `Using` statement will become a parameter to your delegate in the `To` 
statement (in the same order as you call them). 

```
actions
    .RouteGet("/hello")
    .UsingQueryStringParameter<string>("name")
    .To(name => $"Hello {name}!");
```

The `name` parameter in `To` is of type `string`, inferred from the generic type of `UsingQueryStringParameter`.

Above fluent action is equivalent to the following action method:

```
[HttpGet]
[Route("/hello")]
public string HelloAction([FromQuery]string name)
{
    return $"Hello {name}!";
}
```

Lets look at a previous example:

```
actions
    .RouteGet("/users/{userId}")
    .UsingService<IUserService>()
    .UsingRouteParameter<int>("userId")
    .To((userService, userId) => userService.GetUserById(userId))
    .ToView("~/Views/Users/DisplayUser.cshtml");
```

This is equivalent to the following action method:

```
[HttpGet]
[Route("/users/{userId}")]
public string GetUserAction([FromServices]IUserService userService, [FromRoute]int userId)
{
    var user = userService.GetUserById(userId);
    return View("~/Views/Users/DisplayUser.cshtml", user);
}
```

#### List of `using` Statements
Take a look at [Model Binding in ASP.NET Core MVC](https://docs.asp.net/en/latest/mvc/models/model-binding.html#customize-model-binding-behavior-with-attributes) for a better understanding of how the equivalent code of most of these `using` statements work.

- UsingBody
- UsingForm
- UsingFormFile
- UsingFormFiles
- UsingFormValue
- UsingHeader
- UsingHttpContext
- UsingModelBinder
- UsingModelState
- UsingParent
- UsingQueryStringParameter
- UsingResult (for piping multiple `To` statements)
- UsingRouteParameter
- UsingService
- UsingTempData
- UsingViewBag
- UsingViewData

#### UsingBody

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingBody<UserItem>()
    .To(user => $"Hello {user.Name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action([FromBody]UserItem user)
{
    return $"Hello {user.Name}!";
}
```

#### UsingForm

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingForm<UserItem>()
    .To(user => $"Hello {user.Name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action([FromForm]UserItem user)
{
    return $"Hello {user.Name}!";
}
```

#### UsingFormFile

This fluent action:

```
actions
    .RoutePost("/uploadFile")
    .UsingFormFile("file")
    .To(file => $"Got file with name {file.FileName}!");
```

Is equivalent to the following action method in a controller:

```
[HttpPost]
[Route("/uploadFile")]
public string Action(IFormFile file)
{
    return $"Got file with name {file.FileName}!";
}
```

#### UsingFormFiles

This fluent action:

```
actions
    .RoutePost("/uploadFiles")
    .UsingFormFiles("files")
    .To(files => $"Got {files.Count()} n.o. files!");
```

Is equivalent to the following action method in a controller:

```
[HttpPost]
[Route("/uploadFiles")]
public string Action(IEnumerable<IFormFile> files)
{
    return $"Got {files.Count()} n.o. files!";
}
```

#### UsingFormValue

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingFormValue<string>("name")
    .To(name => $"Hello {name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action([FromForm("name")string name)
{
    return $"Hello {name}!";
}
```

#### UsingHeader

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingHeader<string>("Content-Type")
    .To(contentType => $"Hello, your Content-Type is: {contentType}");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action([FromHeader("ContentType")string contentType)
{
    return $"Hello, your Content-Type is: {contentType}";
}
```

#### UsingHttpContext

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingHttpContext()
    .To(httpContext => $"Hello, your request path is: {httpContext.Request.Path}");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action()
{
    return $"Hello, your request path is: {HttpContext.Request.Path}";
}
```

#### UsingModelBinder

This fluent action:

```
actions
    .RouteGet("/hello")
    .UsingModelBinder<UserItem>(typeof(MyModelBinder))
    .To(user => $"Hello {user.Name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action([FromModelBinder(typeof(MyModelBinder))UserItem user])
{
    return $"Hello {user.Name}!";
}
```

#### UsingModelState

This fluent action:

```
actions
    .RoutePost("/submit")
    .UsingForm<MyModel>()
    .UsingModelState()
    .To((myModel, modelState) => modelState.IsValid ? "Model is valid! :)" : "Model is invalid :(");
```

Is equivalent to the following action method in a controller:

```
[HttpPost]
[Route("/submit")]
public string Action([FromForm]MyModel myModel)
{
    return ModelState.IsValid ? "Model is valid! :)" : "Model is invalid :(";
}
```

#### UsingParent

If there exists a sub-class to Controller, called HelloController, that has a method 
called `Hello`, then this fluent action:

```
actions
    .RouteGet("/hello")
    .InheritingFrom<HelloController>()
    .UsingParent()
    .To(parent => parent.Hello());
```

Is equivalent to the following fluent action in the following controller:

```
public class FluentActionController : HelloController 
{
    [HttpGet]
    [Route("/hello")]
    public string Action()
    {
        return Hello();
    }    
}
```

If the parent type is set in the config, the following statement can be used instead:

```
actions
    .RouteGet("/hello")
    .UsingParent<HelloController>()
    .To(parent => parent.Hello());
```

#### UsingQueryStringParameter

This fluent action:

```
actions
    .RouteGet("/hello/{name}")
    .UsingQueryStringParameter<string>("name")
    .To(name => $"Hello {name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello/{name}")]
public string Action([FromQuery]string name)
{
    return $"Hello {name}!";
}
```

#### UsingResult

This fluent action:

```
actions
    .RouteGet("/hello")
    .To(() => "Hello"))
    .UsingResult()
    .To(hello => hello + " World!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello")]
public string Action()
{
    return $"Hello World!";
}
```

#### UsingRouteParameter

This fluent action:

```
actions
    .RouteGet("/hello/{name}")
    .UsingRouteParameter<string>("name")
    .To(name => $"Hello {name}!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello/{name}")]
public string Action([FromRoute]string name)
{
    return $"Hello {name}!";
}
```

#### UsingService

This fluent action:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List());
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/users")]
public IList<UserItem> Action([FromServices]IUserService userService)
{
    return userService.List();
}
```

This assumes you have defined a dependency injection mapping for a `IUserService` in `StartUp.cs`.

#### UsingTempData

This fluent action:

```
actions
    .RouteGet("/users")
    .UsingTempData()
    .Do(tempData => tempData["Title"] = "List of Users")
    .ToView("~/Views/Users/ListUsers.cshtml");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/users")]
public ViewResult Action()
{
    TempData["Title"] = "List of Users";
    return View("~/Views/Users/ListUsers.cshtml");
}
```

#### UsingViewBag

This fluent action:

```
actions
    .RouteGet("/users")
    .UsingViewBag()
    .Do(viewBag => viewBag["Title"] = "List of Users")
    .ToView("~/Views/Users/ListUsers.cshtml");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/users")]
public ViewResult Action()
{
    ViewBag["Title"] = "List of Users";
    return View("~/Views/Users/ListUsers.cshtml");
}
```

#### UsingViewData

This fluent action:

```
actions
    .RouteGet("/users")
    .UsingViewData()
    .Do(viewData => viewData["Title"] = "List of Users")
    .ToView("~/Views/Users/ListUsers.cshtml");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/users")]
public ViewResult Action()
{
    ViewData["Title"] = "List of Users";
    return View("~/Views/Users/ListUsers.cshtml");
}
```

### Default Values in `Using` Statements

The following `Using` statements supports default values:

- UsingBody
- UsingForm
- UsingFormValue
- UsingHeader
- UsingModelBinder
- UsingQueryStringParameter
- UsingRouteParameter
- UsingService

Example:

```
actions
    .RouteGet("/hello/{name}")
    .UsingQueryStringParameter<string>("name", defaultValue: "John Doe")
    .To(name => $"Hello {name}!");
```

This is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/hello/{name}")]
public string Action([FromQuery]string name = "John Doe")
{
    return $"Hello {name}!";
}
```

### Specifying HTTP Method

There is a `Route` statement for each HTTP method, for example:

```
actions
    .RoutePost("/users")
    .UsingService<IUserService>()
    .UsingBody<UserItem>()
    .To((userService, user) => userService.Add(user));
```

You can also specify which HTTP method to use for an action using:

```
actions
    .Route("/users", HttpMethod.Post)
    .UsingService<IUserService>()
    .UsingBody<UserItem>()
    .To((userService, user) => userService.Add(user));
```

### `ToView`

To pipe your output from a `To` statement to an MVC view, you can use `ToView`:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List()))
    .ToView("~/Views/Users/ListUsers.cshtml");
```

This is equivalent to:

```
[HttpGet]
[Route("/users")]
public ActionResult Action([FromServices]IUserService userService)
{
    var users = userService.List();
    return View("~/Views/Users/ListUsers.cshtml", users);
}
```

You can also skip the `To` statement and only use a `Using` statement with the `ToView` statement to
pipe input directly to a view (if multiple `Using` statements are used, only the last one will be piped
to the view). 

### `ToPartialView`

To pipe your output from a `To` statement to an MVC partial view, you can use `ToPartialView`:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List()))
    .ToPartialView("~/Views/Users/ListUsers.cshtml");
```

This is equivalent to:

```
[HttpGet]
[Route("/users")]
public ActionResult Action([FromServices]IUserService userService)
{
    var users = userService.List();
    return PartialView("~/Views/Users/ListUsers.cshtml", users);
}
```

You can also skip the `To` statement and only use a `Using` statement with the `ToPartialView` statement to
pipe input directly to a partial view (if multiple `Using` statements are used, only the last one will be piped
to the partial view). 

### `ToViewComponent`

To pipe your output from a `To` statement to an MVC view component, you can use `ToViewComponent`:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List()))
    .ToViewComponent("~/Views/Users/ListUsers.cshtml");
```

This is equivalent to:

```
[HttpGet]
[Route("/users")]
public ActionResult Action([FromServices]IUserService userService)
{
    var users = userService.List();
    return ViewComponent("~/Views/Users/ListUsers.cshtml", users);
}
```

`ToViewComponent` can also take a `Type` as input:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List()))
    .ToViewComponent(typeof(ListUsersViewComponent));
```

This is equivalent to:

```
[HttpGet]
[Route("/users")]
public ActionResult Action([FromServices]IUserService userService)
{
    var users = userService.List();
    return ViewComponent(typeof(ListUsersViewComponent), users);
}
```

You can also skip the `To` statement and only use a `Using` statement with the `ToViewComponent` statement to
pipe input directly to a view component(if multiple `Using` statements are used, only the last one will be piped
to the view component). 

#### `Do` Statement

If you want to perform some logic but are not interested in outputting a result, 
you can use a `Do` statement. A `Do` statement must be accompanied by something 
that does not need an input such as `ToView`, `ToPartialView` or `ToViewComponent`.

The following fluent action:

```
actions
    .RouteGet("/users")
    .UsingViewData()
    .Do(viewData => viewData["Title"] = "List of Users")
    .ToView("~/Views/Users/ListUsers.cshtml");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/users")]
public ViewResult Action()
{
    ViewData["Title"] = "List of Users";
    return View("~/Views/Users/ListUsers.cshtml");
}
```

### Routing to an MVC Controller 

You can also use fluent actions for routing only:

```
actions
    .RouteGet("/hello")
    .UsingQueryStringParameter("name")
    .ToMvcController<HelloController>()
    .ToMvcAction((name, controller) => controller.Hello(name));
```

This will only add a route to the specified MVC controller and action - it will not create any 
additional controllers or actions.

Note that the lambda expression in the `ToMvcAction` statement must be a single method call to a 
controller method.

### Code Block in `To`

If you want to you can also write code like this:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .UsingQueryStringParameter<int>("userId")
    .To((userService, userId) => 
    {
      var user = userService.GetById(userId);
      return $"Hello {user.Name}!";
    });
```

### InheritingFrom

If you want to use a sub class of `Controller` for your fluent action, use `InheritingFrom`, for example:

```
actions
    .RouteGet("/hello")
    .InheritingFrom<MyBaseController>()
    .To(() => "Hello!");
```

The above fluent action is equivalent to the following action method in a controller:

```
public class FluentActionController : MyBaseController 
{
    [HttpGet]
    [Route("/hello")]
    public ActionResult FluentAction()
    {
        return "Hello!");
    }
}
```

Take a look at the `Config` parameter to set a parent type for multiple actions.

### Asynchronous Delegates

You can use async/await delegates:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(async userService => await userService.ListAsync());
```

### Piping multiple `To` Statements

You can use multiple `To` statements for an action:

```
actions
    .RouteGet("/users")
    .UsingService<IUserService>()
    .To(userService => userService.List())
    .UsingQueryStringParameter<string>("name")
    .UsingResult()
    .To((name, users) => $"Hello {name}! We got {users.Count} users!");
```

Why would you pipe `To` statements? Well, we are currently using some extension 
methods on top of our explicitly defined actions that are specific to our project 
business logic. Those extensions can be implemented using this concept.

### Validate Anti-Forgery Token

The following fluent action:

```
actions
    .RoutePost("/submit")
    .ValidateAntiForgeryToken()
    .To(() => "Anti-forgery token validated!");
```

Is equivalent to the following action method in a controller:

```
[HttpPost]
[Route("/submit")]
[ValidateAntiForgeryToken]
public ActionResult Action()
{
    return "Anti-forgery token validated!");
}
```

### Authorize

The following fluent action:

```
actions
    .RoutePost("/submit")
    .Authorize()
    .To(() => "You must be logged in first!");
```

Is equivalent to the following action method in a controller:

```
[HttpPost]
[Route("/submit")]
[Authorize]
public ActionResult Action()
{
    return "You must be logged in first!");
}
```

The `Authorize` statement has optional parameters for policy, roles and 
activeAuthenticationSchemes.

The `Authorize` statement can also be added in the `Config` parameter
which can be used to apply `Authorize` to multiple actions at once.

### AllowAnonymous

The following fluent action:

```
actions
    .RouteGet("/Hello")
    .AllowAnonymous()
    .To(() => "Hello!");
```

Is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/Hello")]
[AllowAnonymous]
public ActionResult Action()
{
    return "Hello!");
}
```

### Custom Attributes

Fluent actions support adding custom attributes:

```
actions
    .RouteGet("/url")
    .WithAttribute<MyCustomAttribute>()
    .To(() => "Hello!");
```

Above fluent action is equivalent to the following action method in a controller:

```
[HttpGet]
[Route("/url")]
[MyCustomAttribute]
public ActionResult Action()
{
    return "Hello!");
}
```

The following syntaxes are supported:

```
WithAttribute<T>()
WithAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs)
WithAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs, string[] namedProperties, object[] propertyValues)
WithAttribute<T>(ConstructorInfo con, object[] constructorArgs)
WithAttribute<T>(ConstructorInfo con, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
WithAttribute<T>(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
WithAttribute<T>(ConstructorInfo con, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
```

### Id of Fluent Action

You can set an id of a fluent action using an optional parameter of any of the `Route` 
statements. Example:

```
actions
    .RouteGet("/users", "ListUsers")
    .UsingService<IUserService>()
    .To(userService => userService.List());
```

This may increase the maintainability of your project as it might be easier 
to understand and debug your fluent actions. It can also be used for generating 
different kinds of documentation for your project.

### Title of Fluent Action

You can set a title of a fluent action using the statement `WithTitle`. Example:

```
actions
    .RouteGet("/users")
    .WithTitle("List Users")
    .UsingService<IUserService>()
    .To(userService => userService.List());
```

The title can be used for generating different kinds of documentation for your project.

Take a look at the `Config` parameter to set title of multiple actions.

### Description of Fluent Action

You can set a description of a fluent action using the statement `WithDescription`. Example:

```
actions
    .RouteGet("/users")
    .WithDescription("Description of an endpoint that list users.")
    .UsingService<IUserService>()
    .To(userService => userService.List());
```

The description can be used for generating different kinds of documentation for your project.

Take a look at the `Config` parameter to set description of multiple actions.

### Grouping Fluent Actions

You can group multiple actions by setting a common group name on each action:

```
actions
    .RouteGet("/users")
    .GroupBy("UserActions")
    .UsingService<IUserService>()
    .To(userService => userService.List());
```

The group can be used for generating different kinds of documentation for your project.

Take a look at the `Config` parameter to group multiple actions all at once.

### `Config` parameter

The `Config` parameter is an optional parameter that can be used to apply settings among multiple actions.

```
app.UseFluentActions(
    config => 
    {
        config.InheritingFrom<BaseController>();
    }, 
    actions =>
    {
        actions.RouteGet("/").To(() => "Hello World!");
    }
);
```

Above config, defined in the first parameter, will add the group name to all actions defined in the second parameter. 

The following settings are available with the `Config` parameter:

- GroupBy
- InheritingFrom
- SetTitle
- SetTitleFromResource
- SetDescription
- SetDescriptionFromResource
- Authorize
- WithCustomAttribute

## License

This software is [MIT licensed](LICENSE).
