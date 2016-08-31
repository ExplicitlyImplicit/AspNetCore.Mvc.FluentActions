using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public interface IStringTestService
    {
        string GetTestString();

        Task<string> GetTestStringAsync();
    }

    public class StringTestService : IStringTestService
    {
        public string GetTestString()
        {
            return "ThisIsAString";
        }

        public async Task<string> GetTestStringAsync()
        {
            await Task.Delay(1);
            return "ThisIsAString";
        }
    }

    public class StringTestService2 : IStringTestService
    {
        public string GetTestString()
        {
            return "ThisIsAlsoAString";
        }
        public async Task<string> GetTestStringAsync()
        {
            await Task.Delay(1);
            return "ThisIsAlsoAString";
        }
    }
}
