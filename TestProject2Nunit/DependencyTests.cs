using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication1;

namespace TestProject2Nunit;

public class DependencyTests
{
    [TestCase]
    public void HasUnresolvedDependencyTest()
    {
        var targetAssembly = typeof(IAssemblyMarker).Assembly;
        var dependencies = GetControllersDependencies(targetAssembly);

        var app = new WebApplicationFactory<IAssemblyMarker>();
        var missedParameters = GetMissedParameters(dependencies, app.Services);

        if (missedParameters.Count > 0)
        {
            Assert.Fail(ParameterInfoHelper.CollectionToString(missedParameters));
        }
    }
    
    private static IEnumerable<ParameterInfo> GetControllersDependencies(Assembly targetAssembly)
    {
        var types = targetAssembly.GetTypes();
        var controllers = types.Where(t => t.IsAssignableTo(typeof(ControllerBase)));
        var dependencies = controllers.SelectMany(s => s.GetConstructors().First().GetParameters());
        return dependencies;
    }

    private static List<ParameterInfo> GetMissedParameters(IEnumerable<ParameterInfo> dependencyParameterInfos,
        IServiceProvider serviceProvider)
    {
        var missedParameters = new List<ParameterInfo>();
        foreach (var parameterInfo in dependencyParameterInfos)
        {
            var service = serviceProvider.GetService(parameterInfo.ParameterType);
            if (service == null)
            {
                missedParameters.Add(parameterInfo);
            }
        }

        return missedParameters;
    }

    public class ParameterInfoHelper
    {
        public static string CollectionToString(List<ParameterInfo> types)
        {
            var builder = new StringBuilder();
            foreach (var type in types)
            {
                builder.Append(ToString(type));
            }
            return builder.ToString();
        }
        
        private static string ToString(ParameterInfo parameterInfo)
        {
            return $"{parameterInfo} inside {parameterInfo.Member.DeclaringType}";
        }
    }
}