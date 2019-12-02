﻿using System;
using System.Collections.Generic;

namespace SAEON.Logs
{
#pragma warning disable CA2237 // Mark ISerializable types with serializable
    public class MethodCallParameters : Dictionary<string, object> { }
#pragma warning restore CA2237 // Mark ISerializable types with serializable

    public static class MethodCalls
    {
        public static bool UseFullName { get; set; } = true;

        private static string GetTypeName(Type type, bool onlyName = false)
        {
            //return UseFullName && !onlyName ? type.FullName : type.Name;
            var typeName = type.IsGenericType ? type.Name.Split('`')[0] : type.Name;
            return UseFullName && !onlyName ? $"{type.Namespace}.{typeName}".TrimStart('.') : typeName;
        }

        private static string GetParameters(MethodCallParameters parameters)
        {
            string result = string.Empty;
            if (parameters != null)
            {
                bool isFirst = true;
                foreach (var kvPair in parameters)
                {
                    if (!isFirst)
                    {
                        result += ", ";
                    }

                    isFirst = false;
                    result += kvPair.Key + "=";
                    if (kvPair.Value == null)
                    {
                        result += "Null";
                    }
                    else if (kvPair.Value is string)
                    {
                        result += string.Format("'{0}'", kvPair.Value);
                    }
                    else if (kvPair.Value is Guid)
                    {
                        result += string.Format("{0}", kvPair.Value);
                    }
                    else
                    {
                        result += kvPair.Value.ToString();
                    }
                }
            }
            return result;
        }

        public static string MethodSignature(Type type, string methodName, MethodCallParameters parameters = null)
        {
            return $"{GetTypeName(type)}.{methodName}({GetParameters(parameters)})".Replace("..", ".");
        }

        public static string MethodSignature(Type type, Type entityType, string methodName, MethodCallParameters parameters = null)
        {
            return $"{GetTypeName(type)}<{GetTypeName(entityType)}>.{methodName}({GetParameters(parameters)})".Replace("..", ".");
        }

        public static string MethodSignature(Type type, Type entityType, Type relatedEntityType, string methodName, MethodCallParameters parameters = null)
        {
            return $"{GetTypeName(type)}<{GetTypeName(entityType)},{GetTypeName(relatedEntityType)}>.{methodName}({GetParameters(parameters)})".Replace("..", ".");
        }

    }

}