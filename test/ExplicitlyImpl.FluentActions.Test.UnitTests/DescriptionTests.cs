using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System.Linq;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class DescriptionTests
    {
        [Fact(DisplayName = "Description inside single action")]
        public void FluentControllerBuilder_FluentActionWithDescription()
        {
            var action = new FluentAction("/route/url", HttpMethod.Get)
                .WithDescription("Custom Description")
                .To(() => "Hello");

            Assert.Equal("Custom Description", action.Definition.Description);
        }

        [Fact(DisplayName = "Description inside action collection config")]
        public void FluentControllerBuilder_FluentActionCollectionWithDescription()
        {
            var actionCollection = FluentActionCollection.DefineActions(actions => 
            {
                actions.Configure(config => 
                {
                    config.SetDescription(action => action.Id);
                });

                actions
                    .RouteGet("/users", "ListUsers")
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

            foreach (var action in actionCollection)
            {
                Assert.Equal(action.Definition.Id, action.Definition.Description);
            }
        }
    }
}
