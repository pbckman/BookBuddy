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

            [Display(
                Name = "Header",
                Order = 30
            )]
            public const string Header = "Header";

            [Display(
                Name = "Footer",
                Order = 40
            )]
            public const string Footer = "Footer";

            [Display(
                Name = "Navbar",
                Order = 50
            )]
            public const string Navbar = "Navbar";
        
        }
    }
}
