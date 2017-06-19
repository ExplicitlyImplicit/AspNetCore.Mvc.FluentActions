using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithCustomAttributes
    {
        [Fact(DisplayName = "1 custom attribute (empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith1EmptyCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>()
                    .To(() => "hello"),
                typeof(ControllerWith1EmptyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (const), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(new Type[] { typeof(int) }, new object[] { 10 })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (const with Type[]), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (prop with Type[]), returns string")]
        public void FluentControllerBuilder_FluentActionWith1PropertyCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(new Type[0], new object[0], new string[] { "Property" }, new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1PropertyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1PropertyWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]),
                        new object[0],
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1PropertyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (field), returns string")]
        public void FluentControllerBuilder_FluentActionWith1FieldCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]), 
                        new object[0], 
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") }, 
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1FieldCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (const - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorPropertyWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorPropertyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (const - field), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorFieldWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorFieldCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (field - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1FieldPropertyWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]),
                        new object[0],
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1FieldPropertyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (const - field - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorFieldPropertyWithInfoCustomAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorFieldPropertyCustomAttributeReturnsString),
                null);
        }

        [Fact(DisplayName = "2 custom attributes (empty, empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith2EmptyCustomAttributesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>()
                    .WithCustomAttribute<MySecondCustomAttribute>()
                    .To(() => "hello"),
                typeof(ControllerWith2EmptyCustomAttributesReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute (empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith1EmptyCustomAttributeReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1EmptyCustomAttributeReturnsViewResultAsync),
                null);
        }

        [Fact(DisplayName = "2 custom attributes (empty, empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith2EmptyCustomAttributesReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttribute<MyCustomAttribute>()
                    .WithCustomAttribute<MySecondCustomAttribute>()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith2EmptyCustomAttributesReturnsViewResultAsync),
                null);
        }
    }
}
