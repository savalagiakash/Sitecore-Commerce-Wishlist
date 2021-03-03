using System.Threading.Tasks;
using SCPlugin.Commerce.Wishlist.Models;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using SCPlugin.Commerce.Wishlist.Entities;

namespace SCPlugin.Commerce.Wishlist
{
  public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
  {
    public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder arg, CommercePipelineExecutionContext context)
    {
      Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

      arg.AddEntityType(typeof(Wishlists));
      arg.EntitySet<Wishlists>(WishlistConstants.GETWISHLISTDATA);

      var addWishlist = arg.Action(WishlistConstants.ADDWISHLIST);
      addWishlist.Parameter<WishlistModel>(WishlistConstants.WISHLISTMODEL);
      addWishlist.ReturnsFromEntitySet<CommerceCommand>(WishlistConstants.COMMANDS);

      var getWishlist = arg.Action(WishlistConstants.GETWISHLIST);
      getWishlist.Parameter<string>(WishlistConstants.CUSTOMERID);
      getWishlist.Parameter<int>(WishlistConstants.TAKE);
      getWishlist.Parameter<int>(WishlistConstants.SKIP);
      getWishlist.ReturnsFromEntitySet<Wishlists>(WishlistConstants.GETWISHLISTDATA);

      var removeWishlist = arg.Action(WishlistConstants.REMOVEWISHLIST);
      removeWishlist.Parameter<string>(WishlistConstants.CUSTOMERID);
      removeWishlist.Parameter<string>(WishlistConstants.PRODUCTID);
      removeWishlist.ReturnsFromEntitySet<CommerceCommand>(WishlistConstants.COMMANDS);

      return Task.FromResult(arg);
    }
  }
}
