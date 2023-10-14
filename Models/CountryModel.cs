
using System.Collections.Generic;

public class CountryModel
{
    public Name? name { get; set; }
    public string[]? tld { get; set; }
    public string? cca2 { get; set; }
    public string? ccn3 { get; set; }
    public string? cca3 { get; set; }
    public string? cioc { get; set; }
    public bool independent { get; set; }
    public string? status { get; set; }
    public bool? unMember { get; set; }
    public Dictionary<string, Currency>? currencies { get; set; }
    public Idd? idd { get; set; }
    public string[]? capital { get; set; }
    public string[]? altSpellings { get; set; }
    public string? region { get; set; }
    public string? subregion { get; set; }
    public Dictionary<string, string>? languages { get; set; }
    public Dictionary<string, Translation>? translations { get; set; }
    public float[]? latlng { get; set; }
    public bool? landlocked { get; set; }
    public float? area { get; set; }
    public Dictionary<string, Demonym>? demonyms { get; set; }
    public string? flag { get; set; }
    public Maps? maps { get; set; }
    public int? population { get; set; }
    public string? fifa { get; set; }
    public Car? car { get; set; }
    public string[]? timezones { get; set; }
    public string[]? continents { get; set; }
    public Flags? flags { get; set; }
    public Coatofarms? coatOfArms { get; set; }
    public string? startOfWeek { get; set; }
    public Capitalinfo? capitalInfo { get; set; }
    public Postalcode? postalCode { get; set; }
    public string[]? borders { get; set; }
    public Dictionary<string, float>? gini { get; set; }
}

public class Name
{
    public string? common { get; set; }
    public string? official { get; set; }
    public Dictionary<string, Translation>? nativename { get; set; }
}

public class Idd
{
    public string? root { get; set; }
    public string[]? suffixes { get; set; }
}

public class Maps
{
    public string? googleMaps { get; set; }
    public string? openStreetMaps { get; set; }
}

public class Car
{
    public string[]? signs { get; set; }
    public string? side { get; set; }
}

public class Flags
{
    public string? png { get; set; }
    public string? svg { get; set; }
    public string? alt { get; set; }
}

public class Coatofarms
{
    public string? png { get; set; }
    public string? svg { get; set; }
}

public class Capitalinfo
{
    public float[]? latlng { get; set; }
}

public class Postalcode
{
    public string? format { get; set; }
    public string? regex { get; set; }
}

public class Currency
{
    public string? name { get; set; }
    public string? symbol { get; set; }
}

public class Translation
{
    public string? official { get; set; }
    public string? common { get; set; }
}

public class Demonym
{
    public string? m { get; set;}
    public string? f { get; set; }
}
