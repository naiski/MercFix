using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;

namespace MercFix
{
    public class MercFix : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            // Add a listener on daily tick to check if we should trigger the contract expiration game menu.
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
        }

        private void RenewMercContract()
        {
            // Reset the time since the player's last faction change to track the "new contract".  Hopefully there aren't any side-effects of this.
            Clan.PlayerClan.LastFactionChangeTime = CampaignTime.Now;
            InformationManager.DisplayMessage(new InformationMessage("You have renewed your mercenary contract."));
        }

        private void EndMercContract()
        {
            // Remove the player clan from their associated kingdom.  Note that the player's banner and potentially other factors do not update until a menu is open and closed.  Maybe try to fix this in the future?
            Clan.PlayerClan.ClanLeaveKingdom(true);
            InformationManager.DisplayMessage(new InformationMessage("You have ended your mercenary contract."));
        }

        private void OnDailyTick()
        {
            // Check if the player is a mercenary and if they have been in the faction for 30+ days.
            if (Clan.PlayerClan.IsUnderMercenaryService && Clan.PlayerClan.LastFactionChangeTime.ElapsedDaysUntilNow >= 30)
            {
                // Display an inquiry pop up.
                InformationManager.ShowInquiry(
                    new InquiryData(
                        "Mercenary Contract Expired",
                        "Your mercenary contract has expired. You may choose to renew the contract for an additional 30 days or end the contract and surrender any fiefs.",
                        true,
                        true,
                        "Renew",
                        "Expire",
                        RenewMercContract,
                        EndMercContract),
                    true);
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            // This override is necessary in order to inhereit CampaignBehaviorBase.
        }
    }
}
