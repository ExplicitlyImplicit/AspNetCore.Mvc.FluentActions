# Changelog

## 3.1.0 (2020-11-XX)

- Upgraded to .NET Core 3.1
- Changed the way a common config is applied to multiple actions (the Configure action)
  - Instead of providing a separate argument before the action definitions, the configure method is called inside the method that defines the fluent actions (see readme for more info)

## 2.2.0 (2019-05-19)

- Updated core dependencies
- Updated sample and test projects to .NET Core 2.2
- `AddFluentActions` can now be called multiple times (subsequent calls will not do anything)
- `UseFluentActions` can now be called multiple times (all actions will be built and included)
- `UseFluentActions` are now recommended to call before `UseMvc` (since it does not work on .NET Core 2.2)
- Added `ResponseCache` statement which adds a `ResponseCacheAttribute` to the action
- Added `IgnoreApi` statement which adds an `ApiExplorerSettingsAttribute` to the action
- The `GroupBy` statement now adds an `ApiExplorerSettingsAttribute` to the action

## 2.1.0 (2018-01-02)

- Upgraded to netstandard2.0
- Migrated xproj project files to the latest csproj xml format
- Updated core dependencies
- Made ToView, ToPartialView and ToViewComponent chainable
- Added `Append` statement to the config parameter
- Added new using statements:
  - UsingRequest
  - UsingResponse
  - UsingProperty

The `Append` statement can be used to append functionality to many actions at once, see the README for more info.

## 2.0.0 (2017-08-31)

This release comes with a new API for using fluent actions in your MVC project. New functionality in this release
focuses on the classes for the fluent actions.

- Instead of calling `AddMvcWithFluentActions()` call `AddMvc().AddFluentActions()`
- Instead of calling `UseMvcWithFluentActions()` call `UseMvc()` then `UseFluentActions()` separately
- Config is now added as a separate parameter in `UseFluentActions`
- Added `InheritingFrom` to define another class (deriving from Controller) for the fluent action
- Added `UsingParent` so the fluent action may use public methods from the derived class
- Added `WithCustomAttributeOnClass` to set attributes on the class for the fluent action
- Added `AuthorizeClass` to set the authorize attribute on the class for the fluent action
- Removed deprecated `UsingResultFromHandler` (renamed to `UsingResult` in 1.2.0)
- Removed deprecated `FluentActionDefinition.ValidateAntiForgeryToken` (Check custom attributes instead)

## 1.2.0 (2017-06-22)

The majority of this release is focused on the ability to add custom attributes to actions.

- Added `ToViewComponent` with a type parameter
- An AsyncStateMachine is no longer used if action only contains one async `To` statement
- `ToView`, `ToPartialView`, `ToViewComponent` can now be passed input from a `Using` statement
- Added `WithCustomAttribute` statement (also in config)
- Added `Authorize` statement (also in config)
- Added `AllowAnonymous` statement
- Renamed `UsingResultFromHandler` to `UsingResult`

## 1.1.0 (2017-02-21)

This release adds full support for async/await, any number of synchronous delegates can now be used together
with any number of asynchronous delegates. See below for an example of an async `To` statement piped to a
`ToView` statement.

```
app.UseMvcWithFluentActions(actions =>
{
    actions
        .RouteGet("/users/{userId}")
        .UsingService<IUserService>()
        .UsingRouteParameter<int>("userId")
        .To(async (userService, userId) => await userService.GetUserByIdAsync(userId))
        .ToView("~/Views/Users/DisplayUser.cshtml")
});
```

Side note: the test suite also now covers:

- `UsingHttpContext`
- `ToView`
- `ToPartialView`
- `ToViewComponent`

As well as async tests of all previously covered functionality.

## 1.0.3 (2016-11-15)

- Added new `Using` statements:
  - `UsingModelState`
  - `UsingFormFile`
  - `UsingFormFiles`
- Added statement for `ValidateAntiForgeryToken` attribute
- Renamed `ToController` to `ToMvcController`
- Renamed `ToAction` to `ToMvcAction`

## 1.0.2 (2016-09-18)

This release consists mostly of new functionality, both for additional use cases
and for better documentation.

- Added new `Using` statements:
  - `UsingTempData`
  - `UsingViewBag`
  - `UsingViewData`
- Fluent actions can now have metadata to help with documentation and maintainability.
  The following information can be added to every action: - Id (Optional parameter to the `Route` statements) - Group (`GroupBy` statement) - Title (`WithTitle` statement) - Description (`WithDescription` statement)
- The new `Configure` statement can be used to apply settings to multiple actions supporting
  the following statements: - `GroupBy` - `SetTitle` - `SetTitleFromResource` - `SetDescription` - `SetDescriptionFromResource`
- The new `Do` statement can be used instead of `To` when there is no return value.
- `ToView`, `ToPartialView` and `ToViewComponent` can now be used without a preceding `To`
  statement.
- `UsingModelBinder` now has a **Name** parameter.
- Added sample project **MvcTemplate**

## 1.0.1 (2016-08-06)

Core project has been moved into a separate nuget package.

## 1.0.0 (2016-08-05)

Initial release
