using Sitecore.Commerce.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCPlugin.Commerce.Wishlist.Commands;
using System.Web.Http.OData;

namespace SCPlugin.Commerce.Wishlist.Controllers
{
  public class GetWishlistDataController : CommerceController
  {
    public GetWishlistDataController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
        : base(serviceProvider, globalEnvironment)
    {
    }

    [HttpGet]
    [Route("GetWishlist")]
    public async Task<IActionResult> GetWishlist([FromBody] ODataActionParameters value)
    {
      GetWishlistDataController optionsController = this;
      if (!optionsController.ModelState.IsValid)
        return (IActionResult)new BadRequestObjectResult(optionsController.ModelState);
      return string.IsNullOrEmpty(value["customerId"].ToString()) ? (IActionResult)new BadRequestObjectResult((object)value["customerId"]) : 
        (IActionResult)new ObjectResult((object)await optionsController.Command<GetWishlistCommand>().Process(
          optionsController.CurrentContext, value["customerId"].ToString(), Convert.ToInt32(value["take"].ToString()), Convert.ToInt32(value["skip"].ToString())).ConfigureAwait(false));
    }
  }
}
