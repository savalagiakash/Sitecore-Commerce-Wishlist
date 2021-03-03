using SCPlugin.Commerce.Wishlist.Components;
using SCPlugin.Commerce.Wishlist.Models;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCPlugin.Commerce.Wishlist.Commands
{
  public class AddWishlistCommand : CommerceCommand
  {
    private readonly GetCustomerCommand _getCustomerCommand;
    private readonly PersistEntityPipeline _persistEntityPipeline;

    public AddWishlistCommand(
       PersistEntityPipeline persistEntityPipeline,
       IServiceProvider serviceProvider,
       GetCustomerCommand getCustomerCommand)
    {
      this._getCustomerCommand = getCustomerCommand;
      this._persistEntityPipeline = persistEntityPipeline;
    }

    public virtual async Task<CommerceCommand> Process(CommerceContext commerceContext, WishlistModel wishlistModel)
    {
      AddWishlistCommand addWishlistCommand = this;
      try
      {
        var customerEntity = await _getCustomerCommand.Process(commerceContext, wishlistModel.CustomerId);
        if (customerEntity != null)
        {
          if (customerEntity.HasComponent<WishlistComponent>())
          {
            var component = customerEntity.GetComponent<WishlistComponent>();
            // Checking if product is already added to list or not
            bool isProductAdded = false;
            if (component.WishlistCollection == null)
            {
              component.WishlistCollection = new List<WishlistEntity>();
            }
            else
            {
              isProductAdded = component.WishlistCollection.Any(x => x.ProductId == wishlistModel.ProductId);
            }

            if (!isProductAdded)
            {
              WishlistEntity wishlistEntity = new WishlistEntity()
              {
                ProductId = wishlistModel.ProductId,
                ProductTitle = wishlistModel.ProductTitle,
                ProductPrice = wishlistModel.ProductPrice
              };
              component.WishlistCollection.Add(wishlistEntity);
              customerEntity.SetComponent(component);
              commerceContext.Logger.LogInformation($"AddWishlistCommand for customer id: {wishlistModel.CustomerId} added product Id: {wishlistModel.ProductId}");
            }
            else
              commerceContext.Logger.LogInformation($"AddWishlistCommand for customer id: {wishlistModel.CustomerId} NOT ADDED product Id: {wishlistModel.ProductId} as it already exists");
          }
          else
          {
            WishlistComponent wishlistComponent = new WishlistComponent();
            wishlistComponent.WishlistCollection = new List<WishlistEntity>();
            WishlistEntity wishlistEntity = new WishlistEntity()
            {
              ProductId = wishlistModel.ProductId,
              ProductTitle = wishlistModel.ProductTitle,
              ProductPrice = wishlistModel.ProductPrice
            };
            wishlistComponent.WishlistCollection.Add(wishlistEntity);
            customerEntity.SetComponent(wishlistComponent);
          }
          await this._persistEntityPipeline.Run(new PersistEntityArgument(customerEntity), commerceContext.PipelineContext);
        }
      }
      catch (Exception ex)
      {
        commerceContext.Logger.LogError($"Exception occured in getting customer { ex.StackTrace} and id is {ex.Message}");
      }
      return addWishlistCommand;
    }
  }
}
