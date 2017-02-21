using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithFormFiles
    {
        [Fact(DisplayName = "1 form file, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormFileReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingFormFile("file")
                    .To(file => $"Got file with name {file.FileName}!"),
                typeof(ControllerWithFormFileReturnsString));
        }

        [Fact(DisplayName = "1 form file, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingFormFileReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingFormFile("file")
                    .To(async file => { await Task.Delay(1); return $"Got file with name {file.FileName}!"; }),
                typeof(ControllerWithFormFileReturnsStringAsync));
        }

        [Fact(DisplayName = "2 form files, returns string")]
        public void FluentControllerBuilder_FluentActionUsing2FormFilesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingFormFiles("files")
                    .To(files => $"Got {files.Count()} n.o. files!"),
                typeof(ControllerWith2FormFilesReturnsString));
        }

        [Fact(DisplayName = "2 form files, returns string async")]
        public void FluentControllerBuilder_FluentActionUsing2FormFilesReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingFormFiles("files")
                    .To(async files => { await Task.Delay(1); return $"Got {files.Count()} n.o. files!"; }),
                typeof(ControllerWith2FormFilesReturnsStringAsync));
        }
    }
}
