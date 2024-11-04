namespace BookBuddy.Models.DDS
{
    using EPiServer.Data;
    using EPiServer.Data.Dynamic;

    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class Book
    {
        public Identity Id { get; set; } = Identity.NewIdentity();
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

    }
}
