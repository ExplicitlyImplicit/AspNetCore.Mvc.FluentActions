# Hello World Sample For Fluent Actions

This project demonstrates Fluent Actions for ASP.NET Core MVC in its most simple form. 
It contains a blank api project with two changes:

1. Added reference to `ExplicitlyImpl.AspNetCore.Mvc.FluentActions` in [project.json](project.json)
1. Replaced `AddMvc` and `UseMvc` in [Startup.cs](Startup.cs)

Making a **GET** request to `/` will return a plain text **Hello World!**.