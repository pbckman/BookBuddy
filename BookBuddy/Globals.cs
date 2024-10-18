using System.ComponentModel.DataAnnotations;

namespace BookBuddy
{
    public class Globals
    {
        [GroupDefinitions]

        public static class GroupNames
        {
            [Display(
                Name = "MetaData",
                Order = 10
            )]
            public const string MetaData = "MetaData";

            [Display(
                Name = "Specialized",
                Order = 20
            )]
            public const string Specialized = "Specialized";

        }
    }
}
