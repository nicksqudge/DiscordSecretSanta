using DiscordSecretSanta.Commands;
using DiscordSecretSanta.Tests.TestHelpers;

namespace DiscordSecretSanta.Tests.Commands;

// public class CloseCommandTests : AbstractCommandTest<CloseCommand>
// {
//     protected override CloseCommand InitCommand()
//         => new(DataStore, Messages);
//     
//     [TestCase(CampaignStatusId.Open)]
//     [TestCase(CampaignStatusId.NotConfigured)]
//     [TestCase(CampaignStatusId.Ready)]
//     [TestCase(CampaignStatusId.Closed)]
//     public async Task NotRightCampaignStatus(CampaignStatusId status)
//     {
//         // ARRANGE
//         ArrangeGetStatusReturns(status);
//         
//         // ACT
//         var response = await Command.Handle(TestFactory.DiscordUserId(), CancellationToken.None);
//         
//         // ASSERT
//         response.ToString().ShouldBe(Messages.StatusNotValidForSent());
//         
//     }
// }