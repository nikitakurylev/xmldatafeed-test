﻿using xmldatafeed.Abstractions.Core;

namespace xmldatafeed.Core.Providers;

public class UrlProvider : IUrlProvider
{
    private readonly string _path;

    public UrlProvider(string path)
    {
        _path = path;
    }

    public string[] GetUrls()
    {
        return File.ReadAllLines(_path);
    }
}