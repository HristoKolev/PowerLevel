namespace PowerLevel.RpcGenerator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Xdxd.DotNet.Rpc;

public static class TypeScriptCodeGenerator
{
    public static string Generate(List<RpcRequestMetadata> metadata)
    {
        var types = metadata.Select(x => x.RequestType)
            .Concat(metadata.Select(x => x.ResponseType))
            .ToList();

        var allTypes = GetAllTypes(types);

        string dtoContents = string.Join("\n\n", allTypes.Select(ScriptType).Where(x => x != null));

        string rpcClientContents = BuildServerApiClient(metadata);

        string contents = "import { RpcClientHelper } from '~infrastructure/RpcClientHelper';\n" +
                          "import { Result } from '~infrastructure/helpers';\n\n"
                          + dtoContents + "\n" + rpcClientContents;

        return contents;
    }

    private static string ScriptType(Type targetType)
    {
        var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var scriptedProperties = props.Select(x =>
        {
            bool isNullableType = x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return $"  {CamelCase(x.Name)}{(isNullableType ? "?" : "")}: {ResolveType(x.PropertyType)};";
        }).ToList();

        if (scriptedProperties.Count == 0)
        {
            return null;
        }

        return $"export interface {targetType.Name} {{\n" + string.Join("\n", scriptedProperties) + "\n}";
    }

    private static string ResolveType(Type type)
    {
        var typeMap = new Dictionary<Type, string>
        {
            { typeof(int), "number" },
            { typeof(long), "number" },
            { typeof(string), "string" },
            { typeof(bool), "boolean" },
            { typeof(decimal), "number" },
            { typeof(DateTime), "Date" },
        };

        if (typeMap.ContainsKey(type))
        {
            return typeMap[type];
        }

        if (type.IsArray)
        {
            if (type.GetArrayRank() != 1)
            {
                throw new Exception("Multidimensional arrays are not supported.");
            }

            var t = type.GetElementType();

            return ResolveType(t) + "[]";
        }

        if (type.IsGenericType)
        {
            var genericDefinition = type.GetGenericTypeDefinition();

            if (genericDefinition == typeof(List<>))
            {
                var genericArguments = type.GetGenericArguments();
                var t = genericArguments[0];

                return ResolveType(t) + "[]";
            }

            if (genericDefinition == typeof(Dictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();

                var tKey = genericArguments[0];
                var tValue = genericArguments[1];

                return "{ [key: " + ResolveType(tKey) + "]: " + ResolveType(tValue) + " }";
            }

            if (genericDefinition == typeof(Nullable<>))
            {
                var genericArguments = type.GetGenericArguments();

                var tKey = genericArguments[0];

                return ResolveType(tKey);
            }

            throw new Exception($"Generic type not supported: {genericDefinition.Name}");
        }

        return type.Name;
    }

    private static List<Type> GetAllTypes(List<Type> types)
    {
        var bannedTypes = new[]
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(float),
            typeof(char),
            typeof(double),
        };

        var simpleTypes = new[]
        {
            typeof(int),
            typeof(long),
            typeof(string),
            typeof(bool),
            typeof(decimal),
            typeof(DateTime),
        };

        var allTypes = new HashSet<Type>();

        void Recurse(Type targetType)
        {
            if (bannedTypes.Contains(targetType))
            {
                throw new Exception($"Banned type detected in one of the Dto classes: {targetType.Name}");
            }

            if (simpleTypes.Contains(targetType))
            {
                return;
            }

            if (targetType.IsArray)
            {
                if (targetType.GetArrayRank() != 1)
                {
                    throw new Exception("Multidimensional arrays are not supported.");
                }

                var t = targetType.GetElementType();

                Recurse(t);
                return;
            }

            if (targetType.IsGenericType)
            {
                var genericTypeDefinition = targetType.GetGenericTypeDefinition();

                var allowedGenericTypes = new[]
                {
                    typeof(List<>),
                    typeof(Dictionary<,>),
                    typeof(Nullable<>),
                };

                if (!allowedGenericTypes.Contains(genericTypeDefinition))
                {
                    throw new Exception($"Generic type not supported: {targetType}");
                }

                foreach (var t in targetType.GetGenericArguments())
                {
                    Recurse(t);
                }

                return;
            }

            if (allTypes.Add(targetType))
            {
                var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    Recurse(prop.PropertyType);
                }
            }
        }

        foreach (var type in types)
        {
            Recurse(type);
        }

        return allTypes.ToList();
    }

    private static string BuildServerApiClient(List<RpcRequestMetadata> metadata)
    {
        string GetMethodName(Type reqType)
        {
            string name = reqType.Name.Substring(0, reqType.Name.Length - "Request".Length);
            return name[0].ToString().ToLower() + name.Substring(1);
        }

        string GetResponseType(Type responseType)
        {
            return responseType == null || ScriptType(responseType) == null ? "Result" : $"Result<{responseType.Name}>";
        }

        string GetRequestType(Type requestType)
        {
            return requestType == null || ScriptType(requestType) == null ? null : requestType.Name;
        }

        string functions = string.Join("\n", metadata.Select((x, index) =>
        {
            var requestArgument = "";
            var requestParameter = "";
            string requestType = GetRequestType(x.RequestType);
            if (requestType != null)
            {
                requestArgument = $"request: {requestType}";
                requestParameter = ", request";
            }

            string result = $"  {GetMethodName(x.RequestType)}({requestArgument}): Promise<{GetResponseType(x.ResponseType)}> {{\n";
            result += $"    return this.helper.send('{x.RequestType.Name}'{requestParameter});\n";
            result += "  }";

            if (index != metadata.Count - 1)
            {
                result += "\n";
            }

            return result;
        }));

        return @$"
export class RpcClient {{
  helper: RpcClientHelper;

  constructor(helper: RpcClientHelper) {{
    this.helper = helper;
  }}

{functions}
}}
";
    }

    private static string CamelCase(string input)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(input);
    }
}
