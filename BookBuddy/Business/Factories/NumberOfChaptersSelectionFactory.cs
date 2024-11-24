using EPiServer.Shell.ObjectEditing;

namespace BookBuddy.Business.Factories
{
    public class NumberOfChaptersSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "", Value = 0 },
                new SelectItem { Text = "1", Value = 1 },
                new SelectItem { Text = "2", Value = 2 },
                new SelectItem { Text = "3", Value = 3 },
                new SelectItem { Text = "4", Value = 4 },
                new SelectItem { Text = "5", Value = 5 },
                new SelectItem { Text = "6", Value = 6 },
                new SelectItem { Text = "7", Value = 7 },
                new SelectItem { Text = "8", Value = 8 },
                new SelectItem { Text = "9", Value = 9 },
                new SelectItem { Text = "10", Value = 10 },
                new SelectItem { Text = "11", Value = 11 },
                new SelectItem { Text = "12", Value = 12 },
                new SelectItem { Text = "13", Value = 13 },
                new SelectItem { Text = "14", Value = 14 },
                new SelectItem { Text = "15", Value = 15 },
                new SelectItem { Text = "16", Value = 16 },
                new SelectItem { Text = "17", Value = 17 },
                new SelectItem { Text = "18", Value = 18 },
                new SelectItem { Text = "19", Value = 19 },
                new SelectItem { Text = "20", Value = 20 }
            };
        }
    }
}
