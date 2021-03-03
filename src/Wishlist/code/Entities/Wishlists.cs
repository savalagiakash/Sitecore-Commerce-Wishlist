using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace SCPlugin.Commerce.Wishlist.Entities
{
  public class Wishlists : CommerceEntity
  {
    public List<WishlistEntity> Wishlist { get; set; }
    public int TotalCount { get; set; }
  }

  public class WishlistEntity
  {
    public string ProductId { get; set; }
    public string ProductTitle { get; set; }
    public string ProductPrice { get; set; }
  }
}
