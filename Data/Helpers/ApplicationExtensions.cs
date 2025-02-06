﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Pizza_Star.Data.Helpers
{
    public static  class ApplicationExtensions
    {
        public static string GetDescription(this Enum myEnum)
        {
            return myEnum.GetType()
                .GetMember(myEnum.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>().Name;

        }
    }
}
