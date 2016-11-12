using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Linq;
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

        [Fact(DisplayName = "2 form files, returns string")]
        public void FluentControllerBuilder_FluentActionUsing2FormFilesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Post)
                    .UsingFormFiles("files")
                    .To(files => $"Got {files.Count()} n.o. files!"),
                typeof(ControllerWith2FormFilesReturnsString));
        }
    }
}
