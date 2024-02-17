// <copyright file="DataModel.cs" company="CitrusLime Ltd">
// Copyright (c) CitrusLime Ltd. All rights reserved.
// </copyright>

namespace TwelveData.Services.Models;

using Newtonsoft.Json;

public class DataModel<T>
{
    [JsonProperty("data")]
    public T Data { get; set; }
}