namespace BookBuddy.Models.DataModels
{
    public class InfoSectionModel
    {
        public string InfoTitle { get; set; }
        public string InfoFooter { get; set; }
        public IList<BulletPointItems> BulletPoints { get; set; } = new List<BulletPointItems>();
    }
}
