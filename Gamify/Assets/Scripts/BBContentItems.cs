using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BbDateTime
{
    public string time { get; set; }
}
public class AdaptiveRelease
{
    public string start { get; set; }
    public string end { get; set; }
}

public class Availability
{
    public string available { get; set; }
    public bool allowGuests { get; set; }
    public bool allowObservers { get; set; }
    public AdaptiveRelease adaptiveRelease { get; set; }
}

public class ContentHandlerResult
{
    public string id { get; set; }
}

public class LinkResult
{
    public string href { get; set; }
    public string rel { get; set; }
    public string title { get; set; }
    public string type { get; set; }
}

public class Paging
{
    public string nextPage { get; set; }
}

public class Result
{
    public string id { get; set; }
    public string name { get; set; }
    public string parentId { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public string description { get; set; }
    public string created { get; set; }
    public string modified { get; set; }
    public int position { get; set; }
    public bool hasChildren { get; set; }
    public bool hasGradebookColumns { get; set; }
    public bool hasAssociatedGroups { get; set; }
    public bool launchInNewWindow { get; set; }
    public bool reviewable { get; set; }
    public Availability availability { get; set; }
    public ContentHandlerResult contentHandler { get; set; }
    public List<LinkResult> links { get; set; }
}

public class BbContentItems
{
    public List<Result> results { get; set; }
    public Paging paging { get; set; }
    public int Count { get; internal set; }
}