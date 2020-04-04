using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;

namespace MercFix
{
    class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            Campaign campaign = game.GameType as Campaign;
            if (campaign == null) return;
            CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
            gameInitializer.AddBehavior(new MercFix());
        }
    }
}
