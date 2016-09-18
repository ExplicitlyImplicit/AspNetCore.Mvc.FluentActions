using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System.Linq;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class TitleTests
    {
        [Fact(DisplayName = "Title inside single action")]
        public void FluentControllerBuilder_FluentActionWithTitle()
        {
            var action = new FluentAction("/route/url", HttpMethod.Get)
                .WithTitle("Custom Title")
                .To(() => "Hello");

            Assert.Equal("Custom Title", action.Definition.Title);
        }

        [Fact(DisplayName = "Title inside action collection config")]
        public void FluentControllerBuilder_FluentActionCollectionWithTitle()
        {
            var actionCollection = FluentActionCollection.DefineActions(actions => 
            {
                actions.Configure(config => 
                {
                    config.UseTitle(action => action.Id);
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
                Assert.Equal(action.Definition.Id, action.Definition.Title);
            }
        }
    }
}
