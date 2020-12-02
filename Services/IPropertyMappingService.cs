using System.Collections.Generic;

namespace BandAPI.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMapingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExist<TSource, TDestination>(string fields);
    }
}