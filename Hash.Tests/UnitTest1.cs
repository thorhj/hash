using Xunit;

namespace Hash.Tests
{
    public class RunnerTest
    {
        [Fact]
        public void Test1()
        {
            var runner = new Runner();
            runner.Run(new[] { "-s", "1235" });
        }
    }
}
