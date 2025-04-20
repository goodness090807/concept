namespace Concept.Core.Services.Store.ViewModels
{
    public class StoreViewModel
    {
        public StoreViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; }
    }
}
