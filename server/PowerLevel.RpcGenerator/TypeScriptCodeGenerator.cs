namespace PowerLevel.RpcGenerator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Infrastructure;
using Xdxd.DotNet.Rpc;
using Xdxd.DotNet.Shared;

public static class TypeScriptCodeGenerator
{
    public static string Generate(List<RpcRequestMetadata> metadata)
    {
        string fileHeader = @"// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { BaseRpcClient, ApiResult } from './BaseRpcClient';

";

        string interfaces = GenerateInterfaces(metadata);
        string resultClient = GenerateResultClient(metadata);

        return fileHeader + interfaces + resultClient;
    }

    private static Type GetResultErrorType(Type methodReturnType)
    {
        if (methodReturnType.IsGenericType && methodReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            methodReturnType = methodReturnType.GenericTypeArguments.First();
        }

        Type GetPayloadType(Type possibleResultType)
        {
            if (possibleResultType == null)
            {
                return null;
            }

            if (possibleResultType.IsGenericType && possibleResultType.GetGenericTypeDefinition() == typeof(Result<,>))
            {
                return possibleResultType.GenericTypeArguments[1];
            }

            return GetPayloadType(possibleResultType.BaseType);
        }

        return GetPayloadType(methodReturnType);
    }

    /// <summary>
    /// Returns a TypeScript type name for a given CLR type.
    /// </summary>
    private static string GetTypeScriptTypeName(Type type)
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

            return GetTypeScriptTypeName(t) + "[]";
        }

        if (type.IsGenericType)
        {
            var genericDefinition = type.GetGenericTypeDefinition();

            if (genericDefinition == typeof(List<>))
            {
                var genericArguments = type.GetGenericArguments();
                var t = genericArguments[0];

                return GetTypeScriptTypeName(t) + "[]";
            }

            if (genericDefinition == typeof(Dictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();

                var tKey = genericArguments[0];
                var tValue = genericArguments[1];

                return "{ [key: " + GetTypeScriptTypeName(tKey) + "]: " + GetTypeScriptTypeName(tValue) + " }";
            }

            if (genericDefinition == typeof(Nullable<>))
            {
                var genericArguments = type.GetGenericArguments();

                var tKey = genericArguments[0];

                return GetTypeScriptTypeName(tKey);
            }

            throw new Exception($"Generic type not supported: {genericDefinition.Name}");
        }

        return type.Name;
    }

    /// <summary>
    /// Returns a TypeScript type definition for a given CLR type.
    /// </summary>
    private static string GetTypeScriptDefinition(Type targetType)
    {
        var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (props.Length == 0)
        {
            return null;
        }

        var scriptedProperties = props.Select(x =>
        {
            bool isNullableType = (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ||
                                  x.GetCustomAttribute<RpcNullableAttribute>() != null;
            return $"  {JsonNamingPolicy.CamelCase.ConvertName(x.Name)}{(isNullableType ? "?" : "")}: {GetTypeScriptTypeName(x.PropertyType)};";
        }).ToList();

        return $"export interface {targetType.Name} {{\n" + string.Join("\n", scriptedProperties) + "\n}";
    }

    /// <summary>
    /// Returns a list of nested CLR types for a given list of CLR types. The result includes the input list.
    /// </summary>
    private static List<Type> ResolveNestedDtoTypes(List<Type> types)
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

    /// <summary>
    /// Generates a class with method for each endpoints that returns Promise{ApiResult{TPayload, TError}}.
    /// </summary>
    private static string GenerateResultClient(List<RpcRequestMetadata> metadataList)
    {
        string GetMethodName(RpcRequestMetadata metadata)
        {
            string name = metadata.RequestType.Name[..^"Request".Length];
            return name[0].ToString().ToLower() + name[1..];
        }

        string GetRequestType(RpcRequestMetadata metadata)
        {
            return metadata.RequestType == null || GetTypeScriptDefinition(metadata.RequestType) == null ? null : metadata.RequestType.Name;
        }

        string GetResponseType(RpcRequestMetadata metadata)
        {
            var errorType = GetResultErrorType(metadata.HandlerMethod.ReturnType);

            string errorTypeArgument = errorType != null && errorType.Name != nameof(DefaultApiError) ? $", {errorType.Name}" : "";

            return metadata.ResponseType == null || GetTypeScriptDefinition(metadata.ResponseType) == null
                ? "ApiResult"
                : $"ApiResult<{metadata.ResponseType.Name}{errorTypeArgument}>";
        }

        string GetDirectResponseType(RpcRequestMetadata metadata)
        {
            return metadata.ResponseType == null || GetTypeScriptDefinition(metadata.ResponseType) == null
                ? "void"
                : metadata.ResponseType.Name;
        }

        var resultFunctions = metadataList.Select(metadata =>
        {
            var requestArgument = "";
            var requestParameter = "";
            string requestType = GetRequestType(metadata);
            if (requestType != null)
            {
                requestArgument = $"request: {requestType}";
                requestParameter = ", request";
            }

            string result = $"  {GetMethodName(metadata)}Result({requestArgument}): Promise<{GetResponseType(metadata)}> {{\n";
            result += $"    return this.baseClient.sendResult('{metadata.RequestType.Name}'{requestParameter});\n";
            result += "  }";

            return result;
        }).ToList();

        var directFunctions = metadataList.Select(metadata =>
        {
            var requestArgument = "";
            var requestParameter = "";
            string requestType = GetRequestType(metadata);
            if (requestType != null)
            {
                requestArgument = $"request: {requestType}";
                requestParameter = ", request";
            }

            string result = $"  {GetMethodName(metadata)}({requestArgument}): Promise<{GetDirectResponseType(metadata)}> {{\n";
            result += $"    return this.baseClient.send('{metadata.RequestType.Name}'{requestParameter});\n";
            result += "  }";

            return result;
        }).ToList();

        string functions = string.Join("\n\n", resultFunctions.Concat(directFunctions).OrderBy(x => x));

        return @$"export class RpcClient {{
  private baseClient: BaseRpcClient;

  constructor(baseClient: BaseRpcClient) {{
    this.baseClient = baseClient;
  }}

{functions}
}}
";
    }

    /// <summary>
    /// Generates TypeScript interfaces for request, response and error types.
    /// </summary>
    private static string GenerateInterfaces(List<RpcRequestMetadata> metadata)
    {
        var types = metadata.Select(x => x.RequestType)
            .Concat(metadata.Select(x => x.ResponseType))
            .Concat(metadata.Select(x => GetResultErrorType(x.HandlerMethod.ReturnType)))
            .Where(x => x != null)
            .ToList();

        return string.Join("\n\n", ResolveNestedDtoTypes(types).Select(GetTypeScriptDefinition).Where(x => x != null)) + "\n\n";
    }
}
