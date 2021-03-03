using SCPlugin.Commerce.Wishlist.Commands;
using SCPlugin.Commerce.Wishlist.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using System;
using System.Threading.Tasks;
using System.Web.Http.OData;

namespace SCPlugin.Commerce.Wishlist.Controllers
{
  public class CommandsController : CommerceController
  {
    public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
        : base(serviceProvider, globalEnvironment)
    {
    }

    [HttpPut]
    [Route("AddWishlist()")]
    public async Task<IActionResult> AddWishlist([FromBody] ODataActionParameters value)
    {
      var inputArgs = JsonConvert.DeserializeObject<WishlistModel>(value["WishlistModel"].ToString());
      var addWishlistCommand = this.Command<AddWishlistCommand>();
      await addWishlistCommand.Process(this.CurrentContext, inputArgs);
      return new ObjectResult(addWishlistCommand);
    }

    [HttpPut]
    [Route("RemoveWishlist()")]
    public async Task<IActionResult> RemoveWishlist([FromBody] ODataActionParameters value)
    {
      if (!value.ContainsKey(WishlistConstants.CUSTOMERID) || string.IsNullOrEmpty(value[WishlistConstants.CUSTOMERID]?.ToString()))
      {
        return new BadRequestObjectResult(value);
      }

      var customerId = value[WishlistConstants.CUSTOMERID].ToString();
      string productId = value[WishlistConstants.PRODUCTID].ToString();
      var removeWishlistCommand = this.Command<RemoveWishlistCommand>();
      await removeWishlistCommand.Process(this.CurrentContext, customerId, productId);
      return new ObjectResult(removeWishlistCommand);
    }
  }
}
