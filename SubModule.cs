using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;

namespace MercFix
{
    class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            // Check that the game type is a campaign and return if it isn't.
            if (game.GameType as Campaign == null)
            {
                return;
            }
            // Add mod behavior.
            ((CampaignGameStarter)gameStarterObject).AddBehavior(new MercFix());
        }
    }
}
