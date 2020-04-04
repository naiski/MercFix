using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

namespace MercFix
{
    public class MercFix : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
        }

        private void RenewMercContract(MenuCallbackArgs args)
        {
            Clan.PlayerClan.LastFactionChangeTime = CampaignTime.Now;
            GameMenu.ExitToLast();
            InformationManager.DisplayMessage(new InformationMessage("You have renewed your mercenary contract."));
        }

        private void EndMercContract(MenuCallbackArgs args)
        {
            Clan.PlayerClan.ClanLeaveKingdom(true);
            GameMenu.ExitToLast();
            InformationManager.DisplayMessage(new InformationMessage("You have ended your mercenary contract."));
        }

        private void OnSessionLaunched(CampaignGameStarter obj)
        {
            obj.AddGameMenu(
                "merc_contract_expiration",
                "Your mercenary contract has expired. You may choose to renew the contract for an additional 30 days or end the contract and surrender any fiefs.",
                null,
                TaleWorlds.CampaignSystem.Overlay.GameOverlays.MenuOverlayType.None,
                GameMenu.MenuFlags.none,
                null);
            obj.AddGameMenuOption(
                "merc_contract_expiration",
                "merc_contract_renew",
                "Renew the contract.",
                null,
                this.RenewMercContract,
                true,
                0,
                false);
            obj.AddGameMenuOption(
                "merc_contract_expiration",
                "merc_contract_end",
                "End the contract.",
                null,
                this.EndMercContract,
                true,
                1,
                false);
        }

        private void OnDailyTick()
        {
            if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow >= 30)
            {
                GameMenu.ActivateGameMenu("merc_contract_expiration");
                GameMenu.SwitchToMenu("merc_contract_expiration");
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            // Required to inhereit CampaignBehaviorBase.
        }
    }
}
