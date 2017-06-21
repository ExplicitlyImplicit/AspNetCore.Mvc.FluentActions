using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Linq;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithCustomAttributesInConfig
    {
        [Fact(DisplayName = "Custom attributes (empty) in config")]
        public void FluentControllerBuilder_FluentActionWith1EmptyCustomAttributeReturnsString()
        {
            var actionCollection = FluentActionCollection.DefineActions(actions =>
            {
                actions.Configure(config =>
                {
                    config.WithCustomAttribute<MyCustomAttribute>();
                });

                actions
                    .RouteGet("/users", "ListUsers")
                    .WithCustomAttribute<MySecondCustomAttribute>()
                    .UsingService<IUserService>()
                    .To(userService => userService.ListUsers());

                actions
                    .RoutePost("/users", "AddUser")
                    .UsingService<IUserService>()
                    .UsingBody<UserItem>()
                    .To((userService, user) => userService.AddUser(user));

                actions
                    .RouteGet("/users/{userId}", "GetUser")
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .To((userService, userId) => userService.GetUserById(userId));

                actions
                    .RoutePut("/users/{userId}", "UpdateUser")
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .UsingBody<UserItem>()
                    .To((userService, userId, user) => userService.UpdateUser(userId, user));

                actions
                    .RouteDelete("/users/{userId}", "RemoveUser")
                    .UsingService<IUserService>()
                    .UsingRouteParameter<int>("userId")
                    .To((userService, userId) => userService.RemoveUser(userId));
            });

            foreach (var action in actionCollection.Where(action => action.Id != "ListUsers"))
            {
                Assert.Equal(1, action.Definition.CustomAttributes.Count);
            }

            Assert.Equal(2, actionCollection.Single(action => action.Id == "ListUsers").Definition.CustomAttributes.Count);
        }
    }
}
