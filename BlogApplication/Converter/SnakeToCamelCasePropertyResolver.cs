using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BlogApplication.Converter
{
    public class SnakeToCamelCasePropertyResolver:DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {

            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = getPropertyName(property.PropertyName);
            return property;
        }
        private string getPropertyName(string propertyName)
        {
            String temp = "";
            if (propertyName.Contains("_"))
            {
                String[] arr = propertyName.Split("_".ToCharArray());
                temp = format(arr[0], false, true);
                for (int i = 1; i < arr.Length; i++)
                {
                    temp += format(arr[i], true, true);
                }
            }
            else
            {
                temp = format(propertyName, false, isUppercase(propertyName));
            }

            return temp;
        }
        private String format(String input, Boolean firstToUpper, Boolean bodyToLower)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            return (firstToUpper ? input.First().ToString().ToUpper() : input.First().ToString().ToLower()) + (bodyToLower ? input.Substring(1).ToLower() : input.Substring(1));
        }
        private Boolean isUppercase(String input)
        {
            foreach (char c in input)
            {
                if (char.IsLetter(c) && char.IsLower(c))
                    return false;
            }
            return true;
        }
    }
}