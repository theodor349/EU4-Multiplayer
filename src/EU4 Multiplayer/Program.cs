using EU4_Target_Selector.Models;

OldProvince.LoadProvinces("Provinces.csv");

Area.LoadAreas("area.txt");
Region.LoadRegions("region.txt");
SuperRegion.LoadSuperRegions("superregion.txt");

var areas = Area.Areas;
var regions = Region.Regions;
var superRegions = SuperRegion.SuperRegions;
var provinces = new List<Province>();

foreach (var a in areas)
{
    foreach (var p in a.Provinces)
    {
        provinces.Add(new Province(p, a.Name));
    }
}

Console.WriteLine();
