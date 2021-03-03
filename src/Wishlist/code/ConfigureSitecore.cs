using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace SCPlugin.Commerce.Wishlist
{
  public class ConfigureSitecore : IConfigureSitecore
  {
    public void ConfigureServices(IServiceCollection services)
    {
      var assembly = Assembly.GetExecutingAssembly();
      services.RegisterAllPipelineBlocks(assembly);
      services.RegisterAllCommands(assembly);

      services.Sitecore().Pipelines(config => config          
          .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>()));
    }
  }
}
