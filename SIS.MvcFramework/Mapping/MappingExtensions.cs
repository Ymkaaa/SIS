﻿using System.Linq;
using System.Collections.Generic;

namespace SIS.MvcFramework.Mapping
{
    public static class MappingExtensions
    {
        public static IEnumerable<TDestination> To<TDestination>(this IEnumerable<object> collection)
        {
            return collection.Select(ModelMapper.ProjectTo<TDestination>).ToList();
        }

        public static TDestination To<TDestination>(this object obj)
        {
            return ModelMapper.ProjectTo<TDestination>(obj);
        }
    }
}
