using Microsoft.AspNetCore.Routing;
using System;
using System.Text.RegularExpressions;

namespace CarDiaryX.Web.Common
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value is null) 
            { 
                return null; 
            }

            return Regex.Replace(
                value.ToString(),
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
        }
    }
}
