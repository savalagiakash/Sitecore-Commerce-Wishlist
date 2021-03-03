using Microsoft.Extensions.Logging;
using SCPlugin.Commerce.Wishlist.Components;
using SCPlugin.Commerce.Wishlist.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPlugin.Commerce.Wishlist.Commands
{
  public class GetWishlistCommand : CommerceCommand
  {
    private readonly GetCustomerCommand _getCustomerCommand;

    public GetWishlistCommand(IServiceProvider serviceProvider, GetCustomerCommand getCustomerCommand)
    {
      this._getCustomerCommand = getCustomerCommand;
    }

    public virtual async Task<Wishlists> Process(CommerceContext commerceContext, string customerId, int take = 10, int skip = 0)
    {
      var customerEntity = await _getCustomerCommand.Process(commerceContext, customerId);
      Wishlists wishlists = new Wishlists();
      wishlists.Wishlist = new List<Entities.WishlistEntity>();
      try
      {
        if (customerEntity != null)
        {
          if (customerEntity.HasComponent<WishlistComponent>())
          {
            var component = customerEntity.GetComponent<WishlistComponent>();
            if (component?.WishlistCollection != null && component.WishlistCollection.Any())
            {
              wishlists.TotalCount = component.WishlistCollection.Count;
              var customerWishlist = (skip == 0 && take == 0) ? component?.WishlistCollection :
                                      component?.WishlistCollection.Skip(skip).Take(take);

              foreach (var item in customerWishlist)
              {
                Entities.WishlistEntity wishlistEntity = new Entities.WishlistEntity()
                {
                  ProductId = item.ProductId,
                  ProductPrice = item.ProductPrice,
                  ProductTitle = item.ProductTitle
                };
                wishlists.Wishlist.Add(wishlistEntity);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        commerceContext.Logger.LogError(ex.StackTrace, ex.Message, ex);
      }
      return wishlists;
    }
  }
}
