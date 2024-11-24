using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Business.Factories
{
    public class PercentageSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "", Value = 0 },
                new SelectItem { Text = "50%", Value = 50 },
                new SelectItem { Text = "60%", Value = 60 },
                new SelectItem { Text = "70%", Value = 70 },
                new SelectItem { Text = "80%", Value = 80 },
                new SelectItem { Text = "90%", Value = 90 },
                new SelectItem { Text = "100%", Value = 100 }
            };
        }
    }
}
