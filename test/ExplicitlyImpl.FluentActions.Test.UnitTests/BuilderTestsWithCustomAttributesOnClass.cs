using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithCustomAttributesOnClass
    {
        [Fact(DisplayName = "1 custom attribute on class (empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith1EmptyCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>()
                    .To(() => "hello"),
                typeof(ControllerWith1EmptyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (const), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(new Type[] { typeof(int) }, new object[] { 10 })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (const with Type[]), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorWithInfoCustomOnClassAttributeReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (prop with Type[]), returns string")]
        public void FluentControllerBuilder_FluentActionWith1PropertyCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(new Type[0], new object[0], new string[] { "Property" }, new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1PropertyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1PropertyWithInfoCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]),
                        new object[0],
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1PropertyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (field), returns string")]
        public void FluentControllerBuilder_FluentActionWith1FieldCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]), 
                        new object[0], 
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") }, 
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1FieldCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (const - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorPropertyWithInfoCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorPropertyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (const - field), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorFieldWithInfoCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorFieldCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (field - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1FieldPropertyWithInfoCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[0]),
                        new object[0],
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1FieldPropertyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (const - field - prop), returns string")]
        public void FluentControllerBuilder_FluentActionWith1ConstructorFieldPropertyWithInfoCustomAttributeOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>(
                        typeof(MyCustomAttribute).GetConstructor(new Type[] { typeof(int) }),
                        new object[] { 10 },
                        new PropertyInfo[] { typeof(MyCustomAttribute).GetProperty("Property") },
                        new object[] { "prop" },
                        new FieldInfo[] { typeof(MyCustomAttribute).GetField("Field") },
                        new object[] { "field" })
                    .To(() => "hello"),
                typeof(ControllerWith1ConstructorFieldPropertyCustomAttributeOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "2 custom attributes on class (empty, empty), returns string")]
        public void FluentControllerBuilder_FluentActionWith2EmptyCustomAttributesOnClassReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>()
                    .WithCustomAttributeOnClass<MySecondCustomAttribute>()
                    .To(() => "hello"),
                typeof(ControllerWith2EmptyCustomAttributesOnClassReturnsString),
                null);
        }

        [Fact(DisplayName = "1 custom attribute on class (empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith1EmptyCustomAttributeOnClassReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1EmptyCustomAttributeOnClassReturnsViewResultAsync),
                null);
        }

        [Fact(DisplayName = "2 custom attributes on class (empty, empty), returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionWith2EmptyCustomAttributesOnClassReturnsViewResultAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .WithCustomAttributeOnClass<MyCustomAttribute>()
                    .WithCustomAttributeOnClass<MySecondCustomAttribute>()
                    .To(async () => { await Task.Delay(1); return "hello"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith2EmptyCustomAttributesOnClassReturnsViewResultAsync),
                null);
        }
    }
}
