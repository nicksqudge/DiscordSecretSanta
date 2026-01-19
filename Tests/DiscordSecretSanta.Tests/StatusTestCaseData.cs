namespace DiscordSecretSanta.Tests;

public class StatusTestCaseData
{
    public static IEnumerable<TestCaseData> AllStatuses
    {
        get
        {
            return Enum.GetValues<Status>().Select(x => new TestCaseData(x));
        }
    }

    public static IEnumerable<TestCaseData> StatusThatAreNotOpen
    {
        get
        {
            yield return new TestCaseData(Status.NotConfigured);
            yield return new TestCaseData(Status.Drawn);
            yield return new TestCaseData(Status.Ready);
        }
    }
}