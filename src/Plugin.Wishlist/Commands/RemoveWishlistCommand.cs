using SCPlugin.Commerce.Wishlist.Components;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Customers;
using System;
using System.Threading.Tasks;

namespace SCPlugin.Commerce.Wishlist.Commands
{
  public class RemoveWishlistCommand : CommerceCommand
  {
    private readonly GetCustomerCommand _getCustomerCommand;
    private readonly PersistEntityPipeline _persistEntityPipeline;
    public RemoveWishlistCommand(PersistEntityPipeline persistEntityPipeline,
      IServiceProvider serviceProvider, GetCustomerCommand getCustomerCommand)
    {
      this._persistEntityPipeline = persistEntityPipeline;
      this._getCustomerCommand = getCustomerCommand;
    }

    public virtual async Task<CommerceCommand> Process(CommerceContext commerceContext, string customerId, string productId)
    {
      RemoveWishlistCommand removeWishlistCommand = this;
      try
      {
        var customerEntity = await _getCustomerCommand.Process(commerceContext, customerId);
        if (customerEntity != null)
        {
          if (customerEntity.HasComponent<WishlistComponent>())
          {
            WishlistComponent wishlistComponent = customerEntity.GetComponent<WishlistComponent>();
            if (wishlistComponent.WishlistCollection != null)
            {
              foreach (var item in wishlistComponent.WishlistCollection)
              {
                if (item.ProductId == productId)
                {
                  wishlistComponent.WishlistCollection.Remove(item);

                  if (wishlistComponent.WishlistCollection.Count == 0)
                    customerEntity.RemoveComponents(wishlistComponent);
                  else
                    customerEntity.SetComponent(wishlistComponent);
                  break;
                }
              }
            }
            else
              commerceContext.Logger.LogInformation($"start process of RemoveWishlistCommand has no Wishlist Component");
            
          }
          else
            commerceContext.Logger.LogDebug($"start process of RemoveWishlistCommand has no Wishlist Component");

          await this._persistEntityPipeline.Run(new PersistEntityArgument(customerEntity), commerceContext.PipelineContext);
        }
        else
          commerceContext.Logger.LogDebug($"start process of RemoveProductIdCommand has no Customer with this Id");
      }
      catch (Exception ex)
      {
        commerceContext.Logger.LogError(ex.Message, ex);
      }
      return removeWishlistCommand;
    }
  }
}
