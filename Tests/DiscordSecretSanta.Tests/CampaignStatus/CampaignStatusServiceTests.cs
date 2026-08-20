namespace DiscordSecretSanta.Tests.CampaignStatus;

public class CampaignStatusServiceTests
{
    private ICampaignStatusService _sut = new DefaultCampaignStatusService();
    
    [TestCaseSource(typeof(TestData), nameof(TestData.AllStatuses))]
    public void CanDoArrived(CampaignStatusId statusId)
    {
        ShouldBeTrueFor(
            statusId,
            status => _sut.CanDoArrived(status),
            CampaignStatusId.Drawn
        );
    }

    private void ShouldBeTrueFor(CampaignStatusId id, Func<CampaignStatusId, bool> action, params CampaignStatusId[] allowed)
    {
        var result = action.Invoke(id);
        if (allowed.Contains(id))
            result.ShouldBeTrue();
        else 
            result.ShouldBeFalse();
    }
    
    private class TestData
    {
        public static IEnumerable<TestCaseData> AllStatuses
        {
            get
            {
                var keys = Enum.GetValues(typeof(CampaignStatusId)).Cast<CampaignStatusId>();
                foreach (var key in keys)
                    yield return new TestCaseData(key);
            }
        }
    }
}