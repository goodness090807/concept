namespace Concept.Core.Services.User.ViewModels
{
    /// <summary>
    /// 使用者的商店列表 
    /// </summary>
    public class UserStoresViewModel
    {
        public UserStoresViewModel(IEnumerable<UserStoreViewModel> stores)
        {
            OwnedStores = stores.Where(x => x.IsOwner).ToList();
            SharedStores = stores.Where(x => !x.IsOwner).ToList();
        }

        /// <summary>
        /// 使用者擁有的商店
        /// </summary>
        public List<UserStoreViewModel> OwnedStores { get; }
        
        /// <summary>
        /// 分享給使用者的商店
        /// </summary>
        public List<UserStoreViewModel> SharedStores { get; }
    }
}
