using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace SCPlugin.Commerce.Wishlist.Components
{
  public class WishlistComponent : Component
  {
    public List<WishlistEntity> WishlistCollection { get; set; }
  }

  public class WishlistEntity
  {
    public string ProductId { get; set; }
    public string ProductTitle { get; set; }
    public string ProductPrice { get; set; }
  }
}
