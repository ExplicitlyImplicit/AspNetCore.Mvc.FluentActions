using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderOrderTestsSize2
    {
        private readonly TestLogger TestLogger;

        public BuilderOrderTestsSize2(ITestOutputHelper testOutputHelper)
        {
            TestLogger = new TestLogger(testOutputHelper);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [Do-Do] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoDoThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .Do(() => { /* woop woop */ })
                        .Do(() => { /* woop woop */ }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [Do-DoA] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoDoAThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .Do(() => { /* woop woop */ })
                        .DoAsync(async () => { await Task.Delay(1); }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [Do-To] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestDoToReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { text += "Do"; })
                    .To(() => { text += "To"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsString),
                null,
                TestLogger);

            Assert.Equal("DoTo", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [Do-ToA] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestDoToAReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { text += "Do"; })
                    .To(async () => { await Task.Delay(1); text += "ToA"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("DoToA", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [DoA-Do] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoADoThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .DoAsync(async () => { await Task.Delay(1); })
                        .Do(() => { /* woop woop */ }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [DoA-DoA] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestDoADoAThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .DoAsync(async () => { await Task.Delay(1); })
                        .DoAsync(async () => { await Task.Delay(1); }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [DoA-To] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestDoAToReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); text += "DoA"; })
                    .To(() => { text += "To"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("DoATo", text);
        }


        [Fact(DisplayName = "OrderTest, Size 2: [DoA-ToA] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestDoAToAReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); text += "DoA"; })
                    .To(async () => { await Task.Delay(1); text += "ToA"; return "Hello"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("DoAToA", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [To-Do] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestToDoThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .To(() => "Unused")
                        .Do(() => { /* woop woop */ }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [To-DoA] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestToDoAThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .To(() => "Unused")
                        .DoAsync(async () => { await Task.Delay(1); }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [To-To] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToToReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => { text += "To"; return "He"; })
                    .UsingResultFromHandler()
                    .To(he => { text += "To"; return $"{he}llo"; }),
                typeof(ControllerForOrderTestsReturnsString),
                null,
                TestLogger);

            Assert.Equal("ToTo", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [To-ToA] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToToAReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => { text += "To"; return "He"; })
                    .UsingResultFromHandler()
                    .To(async he => { await Task.Delay(1); text += "ToA"; return $"{he}llo"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("ToToA", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [ToA-Do] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestToADoThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .To(async () => { await Task.Delay(1); return "Unused"; })
                        .Do(() => { /* woop woop */ }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [ToA-DoA] throws")]
        public void FluentControllerBuilder_FluentActionOrderTestToADoAThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
            {
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .To(async () => { await Task.Delay(1); return "Unused"; })
                        .DoAsync(async () => { await Task.Delay(1); }),
                    TestLogger);
            });
        }

        [Fact(DisplayName = "OrderTest, Size 2: [ToA-To] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToAToReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); text += "ToA"; return "He"; })
                    .UsingResultFromHandler()
                    .To(he => { text += "To"; return $"{he}llo"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("ToATo", text);
        }

        [Fact(DisplayName = "OrderTest, Size 2: [ToA-ToA] returns string")]
        public void FluentControllerBuilder_FluentActionOrderTestToAToAReturnsString()
        {
            var text = "";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); text += "ToA"; return "He"; })
                    .UsingResultFromHandler()
                    .To(async he => { await Task.Delay(1); text += "ToA"; return $"{he}llo"; }),
                typeof(ControllerForOrderTestsReturnsStringAsync),
                null,
                TestLogger);

            Assert.Equal("ToAToA", text);
        }
    }
}
