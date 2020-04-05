using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

namespace MercFix
{
    public class MercFix : CampaignBehaviorBase
    {
        string lastMenu = null;

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
        }

        private void RenewMercContract(MenuCallbackArgs args)
        {
            Clan.PlayerClan.LastFactionChangeTime = CampaignTime.Now;
            InformationManager.DisplayMessage(new InformationMessage("You have renewed your mercenary contract."));
            if (null == lastMenu)
            {
                GameMenu.ExitToLast();
            }
            else
            {
                GameMenu.SwitchToMenu(lastMenu);
            }
        }

        private void EndMercContract(MenuCallbackArgs args)
        {
            Clan.PlayerClan.ClanLeaveKingdom(true);
            InformationManager.DisplayMessage(new InformationMessage("You have ended your mercenary contract."));
            if (null == lastMenu)
            {
                GameMenu.ExitToLast();
            }
            else
            {
                GameMenu.SwitchToMenu(lastMenu);
            }
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
                false,
                0,
                false);
            obj.AddGameMenuOption(
                "merc_contract_expiration",
                "merc_contract_end",
                "End the contract.",
                null,
                this.EndMercContract,
                false,
                1,
                false);
        }

        private void OnDailyTick()
        {
            if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow >= 30)
            {
                if (null == Campaign.Current.CurrentMenuContext)
                {
                    lastMenu = null;
                    GameMenu.ActivateGameMenu("merc_contract_expiration");
                }
                else
                {
                    lastMenu = Campaign.Current.CurrentMenuContext.GameMenu.StringId;
                }
                GameMenu.SwitchToMenu("merc_contract_expiration");
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            // Required to inhereit CampaignBehaviorBase.
        }
    }
}
