# Changelog

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
The following information can be added to every action:
	- Id (Optional parameter to the `Route` statements)
	- Group (`GroupBy` statement)
	- Title (`WithTitle` statement)
	- Description (`WithDescription` statement)
- The new `Configure` statement can be used to apply settings to multiple actions supporting
the following statements:
	- `GroupBy`
	- `SetTitle`
	- `SetTitleFromResource`
	- `SetDescription`
	- `SetDescriptionFromResource`
- The new `Do` statement can be used instead of `To` when there is no return value.
- `ToView`, `ToPartialView` and `ToViewComponent` can now be used without a preceding `To` 
statement.
- `UsingModelBinder` now has a **Name** parameter.
- Added sample project **MvcTemplate**

## 1.0.1 (2016-08-06)

Core project has been moved into a separate nuget package.

## 1.0.0 (2016-08-05)

Initial release